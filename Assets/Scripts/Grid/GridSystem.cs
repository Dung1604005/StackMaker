using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public class GridSystem : MonoBehaviour
{

    [SerializeField] private BrickPrefabDataBase brickPrefabDataBase;

    private List<BrickBase> listBrick = new List<BrickBase>();

    public void OnEnable()
    {
        EventBus<OnChangeLevel>.Subcribe(OnLoadLevel);
    }
    public void OnDisable()
    {
        EventBus<OnChangeLevel>.UnSubcribe(OnLoadLevel);
    }

    

    public void OnInit()
    {
        
    }
    public void GenerateGrid(LevelDataSO levelDataSO)
    {
        foreach(BrickSaveData brickSaveData in levelDataSO.brickSaveDatas)
        {

                Vector3 worldPos = GridHelper.ConvertGridToWorldPosition(brickSaveData.x, brickSaveData.y, GameConfig.OriginPos);
                int indexPrefab = brickSaveData.IdBrick;
                Vector3 rotateEuler = brickSaveData.eulerRotate;

                BrickBase brickBase = brickPrefabDataBase.GetBrickPrefab(indexPrefab);

                BrickBase ob = PoolManager.Instance.Spawn<BrickBase>(brickBase, worldPos, Quaternion.Euler(rotateEuler));
                ob.SetEulerRotation(rotateEuler);
                ob.transform.SetParent(this.transform);

                listBrick.Add(ob);
        }
        
    }
    public void ClearGrid()
    {
        foreach(BrickBase brick in listBrick)
        {
            PoolManager.Instance.DeSpawn<BrickBase>(brick);
        }
        listBrick.Clear();
    }

    public void OnLoadLevel(OnChangeLevel onChangeLevel)
    {
        LevelDataSO levelData = LevelManager.Instance.CurrentLevelData;
        ClearGrid();
        GenerateGrid(levelData);

        
    }
    void Start()
    {
        OnInit();
    }


}
