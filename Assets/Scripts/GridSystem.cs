using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public class GridSystem : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private GameObject winpos;

    [SerializeField] private List<BrickBase> listBrickPrefab;

    [SerializeField] private List<AreaController> listArea;

    [SerializeField] private List<string> listJsonPath;
    [SerializeField] private AreaController currentArea;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private StackObjectController stackObjectController;

    public AreaController CurrentArea => currentArea;





    public BrickBase GetBrickPrefab(int index)
    {
        if (listBrickPrefab == null || index >= listBrickPrefab.Count || index < 0)
        {
            return null;
        }
        return listBrickPrefab[index];
    }

    public void OnInit()
    {
        listArea[0].LoadLevel(listJsonPath[0]);
        listArea[0].GenerateGrid();

        //Generate bridge between every area
        for (int i = 1; i <= listArea.Count && i <= listJsonPath.Count; i++)
        {
             Vector3 lastEndBlockPosition = listArea[i - 1].ConvertGridToWorldPosition(listArea[i - 1].EndGridPosition.x, listArea[i - 1].EndGridPosition.y);
             Vector3 bridgeDirect = new Vector3(listArea[i - 1].AreaCtx.BridgeDirect[0], 0f, listArea[i - 1].AreaCtx.BridgeDirect[1]);
            Vector3 worldPositionStartBlock = lastEndBlockPosition
                + bridgeDirect * (listArea[i - 1].AreaCtx.StackToPass + 1);
            if (i == listArea.Count)
            {
                //Generate bridge between last area and win area
                 

                Instantiate(winpos, worldPositionStartBlock,Quaternion.identity, this.transform);

                GenerateBridge(lastEndBlockPosition + bridgeDirect, bridgeDirect, listArea[i - 1].AreaCtx.StackToPass);
            }
            else
            {
                listArea[i].LoadLevel(listJsonPath[i]);
                
                listArea[i].CalculateOriginWorldPosition(listArea[i].StartGridPosition, worldPositionStartBlock);

                listArea[i].GenerateGrid();

                GenerateBridge(lastEndBlockPosition + bridgeDirect, bridgeDirect, listArea[i - 1].AreaCtx.StackToPass);
            }
           

        }



    }

    public void GenerateBridge(Vector3 startPosition, Vector3 direct, int amount)
    {
        Vector3 currentPosition = startPosition;

        for (int i = 0; i < amount; i++)
        {
            //Spawn bridge and 2 block aside
            Bridge bridgeBase = (Bridge)listBrickPrefab[GameConfig.ID_PREFAB_BRIDGE];

            BrickBase block = listBrickPrefab[GameConfig.ID_PREFAB_BLOCK];
            Bridge bridgePrefab = PoolManager.Instance.Spawn<Bridge>(bridgeBase, currentPosition, Quaternion.identity);
            //Check if direct is horizontal or vertical
            if(direct.z == 0)
            {
                PoolManager.Instance.Spawn<BrickBase>(block, currentPosition + new Vector3(0, 0, 1), Quaternion.identity);

                PoolManager.Instance.Spawn<BrickBase>(block, currentPosition + new Vector3(0, 0, -1), Quaternion.identity);
            }
            else if (direct.x == 0)
            {
                 PoolManager.Instance.Spawn<BrickBase>(block, currentPosition + new Vector3(1, 0, 0), Quaternion.identity);

                PoolManager.Instance.Spawn<BrickBase>(block, currentPosition + new Vector3(-1, 0, -0), Quaternion.identity);
            }
            
            bridgePrefab.SetInfo(BlockState.Bridge, Vector3.zero);
            
            currentPosition += direct;
        }

    }
    void Start()
    {
        OnInit();
    }


}
