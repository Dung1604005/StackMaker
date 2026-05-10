using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private StackObjectController stackObjectController;

    [SerializeField] private Transform playerVisualTransform;

    [SerializeField] private Animator anim;

    [Header("Context")]

    [SerializeField] private Vector3 offsetPositionFromGrid;

    [SerializeField] private Direct direct;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool isMoving = false;

    [SerializeField] private Vector3 targetPosition;

    public StackObjectController StackObjectController => stackObjectController;

    #region Init
    public void OnEnable()
    {
        EventBus<OnChangeDirect>.Subcribe(ChangeDirect);
        EventBus<OnWinEvent>.Subcribe(OnWin);
        EventBus<OnPause>.Subcribe(OnPause);
        EventBus<OnContinue>.Subcribe(OnContinue);
        EventBus<OnChangeLevel>.Subcribe(OnChangeLevel);
    }

    public void OnDisable()
    {
        EventBus<OnChangeDirect>.UnSubcribe(ChangeDirect);
        EventBus<OnWinEvent>.UnSubcribe(OnWin);
        EventBus<OnPause>.UnSubcribe(OnPause);
        EventBus<OnContinue>.UnSubcribe(OnContinue);
        EventBus<OnChangeLevel>.UnSubcribe(OnChangeLevel);
    }

    public void OnInit()
    {
        LevelDataSO levelDataSO = LevelManager.Instance.CurrentLevelData;
        this.transform.position = GridHelper.ConvertGridToWorldPosition(levelDataSO.startPosition.x, levelDataSO.startPosition.y, GameConfig.OriginPos) + offsetPositionFromGrid;
        isMoving = false;
        targetPosition = transform.position;
        stackObjectController.OnInit();
        anim.SetInteger("renwu", 0);


    }

    #endregion

    #region Get and Set

    public void ChangeDirect(OnChangeDirect onChangeDirect)
    {
        if (isMoving)
        {
            return;
        }
        //When player change direct, detech the furthest block can reach

        direct = onChangeDirect.direct;
        StartCoroutine(IERotate(direct));
        Vector3 directionVector = new Vector3(CalculateDirect2D.ChangeDirectToVector2Int(direct).x, 0f, CalculateDirect2D.ChangeDirectToVector2Int(direct).y);
        RaycastHit hit;
        // Detech the nearest wall
        if (Physics.Raycast(transform.position, directionVector, out hit, GameConfig.MAX_DISTANCE_RAYCAST, GameConfig.LAYER_WALL))
        {
            Collider hitCollider = hit.collider;
            BrickBase brick = ColliderCache<BrickBase>.GetComponent(hitCollider);

            if (brick == null)
            {
                ColliderCache<BrickBase>.AddComponent(hitCollider, hitCollider.GetComponent<BrickBase>());
                brick = ColliderCache<BrickBase>.GetComponent(hitCollider);

            }

            if (brick != null)
            {
                // Back 1 cell from the wall to get the last cell player can reach
                Vector3 directBack = new Vector3(directionVector.x * GameConfig.CellSize.x, directionVector.y * GameConfig.CellSize.y,
                directionVector.z * GameConfig.CellSize.z);
                targetPosition = brick.GetWorldPosition() - directBack + offsetPositionFromGrid;
            }
            else
            {
                Debug.LogError("Brick dont have brickBase component");
            }

        }
        if (isMoving && (targetPosition - transform.position).sqrMagnitude <= 0.1f)
        {
            isMoving = false;

        }
        else
        {
            isMoving = true;
        }
    }

    #endregion
    public void Move()
    {

        // If player havent reached the target then move to it
        if ((targetPosition - transform.position).sqrMagnitude >= 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {


            if (isMoving && (targetPosition - transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;
                // Automatic change direct if find special corner
                if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out RaycastHit hit, 20f, GameConfig.LAYER_BRICK))
                {
                    Collider hitCollider = hit.collider;
                    BrickBase brick = ColliderCache<BrickBase>.GetComponent(hitCollider);

                    if (brick == null)
                    {
                        brick = hitCollider.GetComponent<BrickBase>();
                        ColliderCache<BrickBase>.AddComponent(hitCollider, brick);

                    }
                    if (brick != null)
                    {
                        Direct nxtDirect = CalculateDirect2D.ChangeCornerToDirect(brick.GetBrickState(), direct);
                        if (nxtDirect == Direct.NULL)
                        {
                            //Place player to the middle of brick 
                            this.transform.position = brick.GetWorldPosition() + offsetPositionFromGrid;
                            return;
                        }
                        else
                        {

                            StartCoroutine(IERotate(nxtDirect));
                            // Automatic change direct if find special corner
                            ChangeDirect(new OnChangeDirect
                            {
                                direct = nxtDirect
                            });
                        }
                    }
                    else
                    {
                        Debug.LogError("Brick dont have brickBase component");
                    }
                }

            }
            if ((targetPosition - transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void StopMove()
    {
        isMoving = false;
    }

    public void OnPause(OnPause onPause)
    {
        isMoving = false;
    }

    public void OnContinue(OnContinue onContinue)
    {
        isMoving = true;
    }

    public void OnChangeLevel(OnChangeLevel onChangeLevel)
    {
        OnInit();
    }

    public void OnWin(OnWinEvent onWinEvent)
    {
        StopMove();
        anim.SetInteger("renwu", 2);
    }

    IEnumerator IERotate(Direct direct)
    {
        Vector3 targetRotation = CalculateDirect2D.ChangeDirectToEulerQuaternion(direct);

        targetRotation = new Vector3(playerVisualTransform.eulerAngles.x, targetRotation.y, playerVisualTransform.eulerAngles.z);

        
        for(int i = 0; i < 100 && (targetRotation - playerVisualTransform.eulerAngles).sqrMagnitude > 0.1f; i++)
        {
            playerVisualTransform.eulerAngles = Vector3.Lerp(playerVisualTransform.eulerAngles, targetRotation, 0.5f);

            Debug.Log(playerVisualTransform.eulerAngles);
            yield return null;
        }

    }


    public void Jump(int stackAmount)
    {
        anim.SetTrigger("jump");
        playerVisualTransform.localPosition = new Vector3(0f, stackAmount * stackObjectController.OffsetY, 0f);

    }
    public void Fall(int stackAmount)
    {

        playerVisualTransform.localPosition = new Vector3(0f, stackAmount * stackObjectController.OffsetY, 0f);
    }



    void Start()
    {
        OnInit();
    }

    void Update()
    {
        if (!isMoving) return;

        Move();
    }



}
