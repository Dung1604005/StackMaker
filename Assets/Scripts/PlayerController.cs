using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GridSystem gridSystem;

    [Header("Context")]

    [SerializeField] private Vector2Int playerGridPosition;

    [SerializeField] private Vector3 offsetPositionFromGrid;

    [SerializeField] private BlockState targetBlockState;
    
    [SerializeField] private Direct direct;

    [SerializeField] private float speed = 5f;

    [SerializeField]private bool isMoving = false;

    [SerializeField]private Vector3 targetPosition;
    
    public Vector2Int PlayerGridPosition => playerGridPosition;

    public bool IsMoving => isMoving;

    #region Init
    public void OnEnable()
    {
        EventBus<OnChangeDirect>.Subcribe(ChangeDirect);
        EventBus<OnChangeTargetPositionPlayer>.Subcribe(ChangeTarget);
    }

    public void OnDisable()
    {
        EventBus<OnChangeDirect>.UnSubcribe(ChangeDirect);
        EventBus<OnChangeTargetPositionPlayer>.UnSubcribe(ChangeTarget);
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
        
        if((targetPosition-transform.position).sqrMagnitude >= 0.01)
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
        if((targetPosition-transform.position).sqrMagnitude >= 0.01)
        {
            transform.position= Vector3.MoveTowards(transform.position, targetPosition, speed*Time.deltaTime);
        }
        else
        {
            // Đổi góc tự động khi gặp góc nảy
            if (isMoving && (targetPosition-transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;
                if(targetBlockState == BlockState.LeftTopCorner ||targetBlockState == BlockState.RightTopCorner||
                targetBlockState == BlockState.LeftBottomCorner || targetBlockState == BlockState.RightBottomCorner)
                {
                    Direct nextDirect = CalculateDirect2D.ChangeCornerToDirect(targetBlockState, direct);
                    Debug.Log(nextDirect);
                    if(nextDirect != Direct.NULL)
                    {
                        EventBus<OnChangeDirect>.Raise(new OnChangeDirect
                        {
                            playerGridPosition = playerGridPosition,
                            direct = nextDirect
                        });
                    }

                }
                
            }
            if((targetPosition-transform.position).sqrMagnitude <= 0.01f)
            {
                isMoving = false;
            }
            
        }
    }

    void Start()
    {
        OnInit();
    }

    void Update()
    {
        if(!isMoving)return;
        Move();
    }



}
