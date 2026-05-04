using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapLevel", menuName = "Game Data/Map Level")]
public class LevelDataSO : ScriptableObject
{
    
    public string nameLevel;

    public Vector2Int mapSize;

    public int levelId;

    public Vector2Int startPosition;

    public List<BrickSaveData> brickSaveDatas = new List<BrickSaveData>();

}

[System.Serializable]

public struct BrickSaveData
{
    public int IdBrick;

    public int x;
    
    public int y;

    public Vector3 eulerRotate;

}