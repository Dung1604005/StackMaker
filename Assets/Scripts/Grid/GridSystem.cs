using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public class GridSystem : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private LevelDataSO levelData;

    [SerializeField] private BrickPrefabDataBase brickPrefabDataBase;

    public LevelDataSO GetLevelData()
    {
        return levelData;
    }

    public void OnInit()
    {
        GenerateGrid(levelData);
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
        }
        
    }
    void Start()
    {
        OnInit();
    }


}
