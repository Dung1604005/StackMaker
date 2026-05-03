using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class LevelData
{
    
    public string name;

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

}