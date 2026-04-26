
using UnityEngine;

public class GridHelper
{
    public Vector3 ConvertGridToWorldPosition(int x, int z, Vector3 originPos)
    {
        
        Vector3 worldPos = new Vector3(originPos.x - x * GameConfig.CellSize.x - GameConfig.CellSize.x / 2f, 0f, originPos.z - z * GameConfig.CellSize.y - GameConfig.CellSize.y / 2f);

        return worldPos;
    }

    public Vector2Int ConvertWorldPositionToGridPosition(Vector3 worldPosition, Vector3 originPos)
    {
        Vector2Int gridPosition = new Vector2Int((Mathf.FloorToInt((worldPosition.x - originPos.x + GameConfig.CellSize.x / 2f)/(- GameConfig.CellSize.x))),
        Mathf.FloorToInt((worldPosition.z - originPos.z + GameConfig.CellSize.y / 2f)/ (-GameConfig.CellSize.y)));

        return gridPosition;
    }

}