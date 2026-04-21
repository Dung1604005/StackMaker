using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GridSystem gridSystem;

    [SerializeField] private StackObjectController stackObjectController;

    [SerializeField] private Transform playerVisualTransform;

    [Header("Context")]

    [SerializeField] private Vector2Int playerGridPosition;

    [SerializeField] private Vector3 offsetPositionFromGrid;

    [SerializeField] private BlockState targetBlockState;

    [SerializeField] private Direct direct;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool isMoving = false;

    [SerializeField] private Vector3 targetPosition;

    private bool canHitStack =  true;

    public Vector2Int PlayerGridPosition => playerGridPosition;

    public bool IsMoving => isMoving;

    #region Init
    public void OnEnable()
    {
        EventBus<OnChangeDirect>.Subcribe(ChangeDirect);
        EventBus<OnChangeTargetPositionPlayer>.Subcribe(ChangeTarget);
        EventBus<OnChangeStackAmount>.Subcribe(Jump);
        EventBus<OnMoveOnBridge>.Subcribe(MoveOnBridge);
    }

    public void OnDisable()
    {
        EventBus<OnChangeDirect>.UnSubcribe(ChangeDirect);
        EventBus<OnChangeTargetPositionPlayer>.UnSubcribe(ChangeTarget);
        EventBus<OnChangeStackAmount>.UnSubcribe(Jump);
        EventBus<OnMoveOnBridge>.UnSubcribe(MoveOnBridge);
    }

    public void OnInit()
    {
        playerGridPosition = gridSystem.CurrentArea.StartGridPosition;
        this.transform.position = gridSystem.CurrentArea.ConvertGridToWorldPosition(playerGridPosition.x, playerGridPosition.y) + offsetPositionFromGrid;
        isMoving = false;
        targetPosition = transform.position;
    }

    #endregion

    #region Get and Set

    public void ChangeDirect(OnChangeDirect onChangeDirect)
    {
        direct = onChangeDirect.direct;

    }

    public void ChangeTarget(OnChangeTargetPositionPlayer data)
    {
        playerGridPosition = data.GridPosition;
        targetBlockState = data.TargetBlockState;
        targetPosition = data.TargetPosition + offsetPositionFromGrid;

        if ((targetPosition - transform.position).sqrMagnitude >= 0.01)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
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
                if (targetBlockState == BlockState.LeftTopCorner || targetBlockState == BlockState.RightTopCorner ||
                targetBlockState == BlockState.LeftBottomCorner || targetBlockState == BlockState.RightBottomCorner)
                {
                    Direct nextDirect = CalculateDirect2D.ChangeCornerToDirect(targetBlockState, direct);
                    
                    if (nextDirect != Direct.NULL)
                    {
                        gridSystem.ActiveMove(nextDirect);
                        
                    }

                }

            }
            if ((targetPosition - transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;
            }

        }
    }

    public void MoveOnBridge(OnMoveOnBridge data)
    {
        
        Vector3 moveDirect = new Vector3(CalculateDirect2D.ChangeDirectToVector2Int(data.direct).x, 0f,
         CalculateDirect2D.ChangeDirectToVector2Int(data.direct).y)*data.LengthMove;
        targetPosition = transform.position + moveDirect;

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
       playerVisualTransform.localPosition = new Vector3(0f, onChangeStackAmount.numberStack*stackObjectController.OffsetY, 0f);
    }

    #region Collision
    public void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.CompareTag("stack"))
        {
            //Nếu gặp stack thì phá hủy stack ở đất và thêm vào khối stack player giữ
            
            // Sử dụng 1 biến bool để ngăn va chạm 2 lần do destroy chỉ được thực hiện vào frame sau
            Destroy(collider.gameObject);
            if (canHitStack)
            {
                stackObjectController.AddStackObject();
                canHitStack = false;
            }
            

        }
    }
    

    #endregion

    void Start()
    {
        OnInit();
    }

    void FixedUpdate()
    {
        if (!isMoving) return;
        if(!canHitStack) canHitStack = true;
        Move();
    }



}
