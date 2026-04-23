using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GridSystem gridSystem;

    [SerializeField] private StackObjectController stackObjectController;

    [SerializeField] private Transform playerVisualTransform;

    [Header("Context")]

    [SerializeField] private Vector3 offsetPositionFromGrid;

    [SerializeField] private Direct direct;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool isMoving = false;

    [SerializeField] private Vector3 targetPosition;
    public bool IsMoving => isMoving;

    #region Init
    public void OnEnable()
    {
        EventBus<OnChangeDirect>.Subcribe(ChangeDirect);
        EventBus<OnWinEvent>.Subcribe(OnWin);
    }

    public void OnDisable()
    {
        EventBus<OnChangeDirect>.UnSubcribe(ChangeDirect);
        EventBus<OnWinEvent>.UnSubcribe(OnWin);
    }

    public void OnInit()
    {

        this.transform.position = gridSystem.CurrentArea.ConvertGridToWorldPosition(gridSystem.CurrentArea.StartGridPosition.x, gridSystem.CurrentArea.StartGridPosition.y) + offsetPositionFromGrid;
        isMoving = false;
        targetPosition = transform.position;
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
        Vector3 directionVector = new Vector3(CalculateDirect2D.ChangeDirectToVector2Int(direct).x, 0f, CalculateDirect2D.ChangeDirectToVector2Int(direct).y);
        RaycastHit hit;
        // Detech the nearest wall
        if (Physics.Raycast(transform.position, directionVector, out hit, GameConfig.MAX_DISTANCE_RAYCAST, GameConfig.LAYER_WALL))
        {
            if (hit.collider.TryGetComponent<BrickBase>(out BrickBase brick))
            {
                // Back 1 cell from the wall to get the last cell player can reach
                Vector3 directBack = new Vector3(directionVector.x * GameConfig.CellSize.x, directionVector.y * GameConfig.CellSize.y,
                directionVector.z * GameConfig.CellSize.z);
                targetPosition = brick.GetWorldPosition() - directBack + offsetPositionFromGrid;

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
                    if (hit.collider.TryGetComponent<BrickBase>(out BrickBase brick))
                    {
                        Direct nxtDirect = CalculateDirect2D.ChangeCornerToDirect(brick.GetBlockState(), direct);
                        if (nxtDirect == Direct.NULL)
                        {
                            //Place player to the middle of brick 
                            this.transform.position = brick.GetWorldPosition() + offsetPositionFromGrid;
                            return;
                        }
                        else
                        {
                            // Automatic change direct if find special corner
                            ChangeDirect(new OnChangeDirect
                            {
                                direct = nxtDirect
                            });
                        }
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
        targetPosition = this.transform.position;
    }

    public void OnWin(OnWinEvent onWinEvent)
    {
        StopMove();
    }


    public void Jump(int stackAmount)
    {
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
