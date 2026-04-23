using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public class GridSystem : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GameObject bridgeBlockPrefab;

    [SerializeField] private List<BrickBase> listBlockPrefab;

    [SerializeField] private List<AreaController> listArea;

    [SerializeField] private List<string> listJsonPath;
    [SerializeField] private AreaController currentArea;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private StackObjectController stackObjectController;

    public AreaController CurrentArea => currentArea;





    public BrickBase GetBlockPrefab(int index)
    {
        if(listBlockPrefab == null || index >= listBlockPrefab.Count || index < 0)
        {
            return null;
        }
        return listBlockPrefab[index];
    }
    public void RayCastToMoveOnBridge(int currentStackObjects, Vector2Int bridgeDirect, Direct direct)
    {
        Vector3 target = playerController.transform.position;
        RaycastHit[] raycast = Physics.RaycastAll(playerController.transform.position, new Vector3(bridgeDirect.x, 0f, bridgeDirect.y), 50f, GameConfig.LAYER_BRIDGE);
        foreach (RaycastHit hit in raycast)
        {
            float distance = hit.distance;
            // Ở đây mỗi cell có size (1f, 1f)
            if (distance <= currentStackObjects * 1f)
            {
                target = hit.transform.position;
            }
        }
        currentArea = null;
        EventBus<OnMoveOnBridge>.Raise(new OnMoveOnBridge
        {
            direct = direct,
            target = target
        });
        return;
    }

    public void OnInit()
    {
        listArea[0].LoadLevel(listJsonPath[0]);
        listArea[0].GenerateGrid();

        for (int i = 1; i < listArea.Count && i < listJsonPath.Count; i++)
        {

            listArea[i].LoadLevel(listJsonPath[i]);
            Vector3 lastEndBlockPosition = listArea[i - 1].ConvertGridToWorldPosition(listArea[i - 1].EndGridPosition.x, listArea[i - 1].EndGridPosition.y);
            Vector3 bridgeDirect = new Vector3(listArea[i - 1].AreaCtx.BridgeDirect[0], 0f, listArea[i - 1].AreaCtx.BridgeDirect[1]);
            Vector3 worldPositionStartBlock = lastEndBlockPosition
            + bridgeDirect * (listArea[i - 1].AreaCtx.StackToPass);


            listArea[i].CalculateOriginWorldPosition(listArea[i].StartGridPosition, worldPositionStartBlock);

            listArea[i].GenerateGrid();

            GenerateBridge(lastEndBlockPosition + bridgeDirect, bridgeDirect, listArea[i - 1].AreaCtx.StackToPass);

        }
    }

    public void GenerateBridge(Vector3 startPosition, Vector3 direct, int amount)
    {
        Vector3 currentPosition = startPosition;

        for (int i = 0; i < amount; i++)
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
