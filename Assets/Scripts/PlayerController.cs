using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GridSystem gridSystem;

    [SerializeField] private StackObjectController stackObjectController;

    [SerializeField] private Transform playerVisualTransform;

    [Header("Context")]

    [SerializeField] private Vector3 offsetPositionFromGrid;

    [SerializeField] private BlockState targetBlockState;

    [SerializeField] private Direct direct;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool isMoving = false;

    [SerializeField] private Vector3 targetPosition;
    public bool IsMoving => isMoving;

    #region Init
    public void OnEnable()
    {
        EventBus<OnChangeDirect>.Subcribe(ChangeDirect);

        EventBus<OnChangeStackAmount>.Subcribe(Jump);
        EventBus<OnMoveOnBridge>.Subcribe(MoveOnBridge);
    }

    public void OnDisable()
    {
        EventBus<OnChangeDirect>.UnSubcribe(ChangeDirect);
        EventBus<OnChangeStackAmount>.UnSubcribe(Jump);
        EventBus<OnMoveOnBridge>.UnSubcribe(MoveOnBridge);
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

        direct = onChangeDirect.direct;
        Vector3 directionVector = new Vector3(CalculateDirect2D.ChangeDirectToVector2Int(direct).x, 0f, CalculateDirect2D.ChangeDirectToVector2Int(direct).y);
        RaycastHit hit;
        Debug.DrawRay(transform.position, directionVector);
        if (Physics.Raycast(transform.position, directionVector, out hit, GameConfig.MAX_DISTANCE_RAYCAST, GameConfig.LAYER_WALL))
        {
            if (hit.collider.TryGetComponent<BrickBase>(out BrickBase brick))
            {
                targetPosition = brick.GetWorldPosition() - directionVector + offsetPositionFromGrid;
                
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

            // Đổi góc tự động khi gặp góc nảy
            if (isMoving && (targetPosition - transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;

                if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out RaycastHit hit, 20f, GameConfig.LAYER_BRICK))
                {
                    Debug.Log(hit);
                    if (hit.collider.TryGetComponent<BrickBase>(out BrickBase brick))
                    {
                        Debug.Log(brick.GetBlockState());
                        Direct nxtDirect = CalculateDirect2D.ChangeCornerToDirect(brick.GetBlockState(),direct);

                        ChangeDirect(new OnChangeDirect
                        {
                            direct = nxtDirect
                        });
                       
                    }
                }



                //RaycastHit[] hit = Physics.Raycast(transform.position, new Vector3(0, -1, 0), 20f);

            }
            if ((targetPosition - transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;
            }




        }
    }

    public void MoveOnBridge(OnMoveOnBridge data)
    {
        if (isMoving)
        {
            return;
        }


        targetPosition = data.target;

        if ((targetPosition - transform.position).sqrMagnitude >= 0.01)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void Jump(OnChangeStackAmount onChangeStackAmount)
    {
        playerVisualTransform.localPosition = new Vector3(0f, onChangeStackAmount.numberStack * stackObjectController.OffsetY, 0f);
    }

    void Start()
    {
        OnInit();
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Move();
    }



}
