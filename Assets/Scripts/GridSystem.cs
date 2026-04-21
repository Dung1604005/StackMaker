using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
public class GridSystem: MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GameObject bridgeBlockPrefab;

    [SerializeField] private List<AreaController> listArea;

    [SerializeField] private List<string> listJsonPath;
    [SerializeField] private AreaController currentArea;

    public AreaController CurrentArea => currentArea;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private StackObjectController stackObjectController;


    /// <summary>
    /// This func get direct from swipe detect then raise event OnChangeDirect
    /// </summary>
    /// <param name="direct"></param>
    public void ActiveMove(Direct direct)
    {
        // Dont let player change direction when moving
        if (playerController.IsMoving)
        {
            
            return;
        }
        // Nếu player đang đứng ở end block thì check xem hướng di chuyển có phải đi qua bridge ko, nếu có thì đi
        Vector2Int bridgeDirect = new Vector2Int(currentArea.AreaCtx.BridgeDirect[0], currentArea.AreaCtx.BridgeDirect[1]);
        if(currentArea == null)
        {
            
        }
        else if(playerController.PlayerGridPosition == currentArea.EndGridPosition)
        {

            if(CalculateDirect2D.ChangeDirectToVector2Int(direct) == bridgeDirect)
            {
                int currentStackObjects = stackObjectController.CountStackObject();
                
                currentArea = null;
            }
        }
        else if (true)
        {
            
        }

        EventBus<OnChangeDirect>.Raise(new OnChangeDirect
        {
            areaId = currentArea.AreaCtx.areaId,
                direct = direct,
                playerGridPosition = playerController.PlayerGridPosition
        });
    }

    public void OnInit()
    {
        listArea[0].LoadLevel(listJsonPath[0]);
        listArea[0].GenerateGrid();
        
        for(int i = 1; i < listArea.Count && i < listJsonPath.Count; i++)
        {
            
            listArea[i].LoadLevel(listJsonPath[i]);
            Vector3 lastEndBlockPosition = listArea[i-1].ConvertGridToWorldPosition(listArea[i-1].EndGridPosition.x, listArea[i-1].EndGridPosition.y);
            Vector3 bridgeDirect = new Vector3(listArea[i-1].AreaCtx.BridgeDirect[0],0f,  listArea[i-1].AreaCtx.BridgeDirect[1]);
            Vector3 worldPositionStartBlock = lastEndBlockPosition
            + bridgeDirect*(listArea[i-1].AreaCtx.StackToPass);

            
            listArea[i].CalculateOriginWorldPosition(listArea[i].StartGridPosition, worldPositionStartBlock);

            listArea[i].GenerateGrid();
            
            GenerateBridge(lastEndBlockPosition + bridgeDirect, bridgeDirect,  listArea[i-1].AreaCtx.StackToPass);

        }
    }

    public void GenerateBridge(Vector3 startPosition, Vector3 direct, int amount)
    {
        Vector3 currentPosition = startPosition ;

        for(int i = 0; i < amount; i++)
        {

            Instantiate(bridgeBlockPrefab, this.transform).transform.position = currentPosition;
            currentPosition += direct;
        }

    }
    void Start()
    {
        OnInit();
    }


}
