
using UnityEngine;

public class GridHelper
{
    public static Vector3 ConvertGridToWorldPosition(int x, int z, Vector3 originPos)
    {
        
        Vector3 worldPos = new Vector3(originPos.x - x * GameConfig.CellSize.x - GameConfig.CellSize.x / 2f, 0f, originPos.z - z * GameConfig.CellSize.y - GameConfig.CellSize.y / 2f);

        return worldPos;
    }

    public static Vector2Int ConvertWorldPositionToGridPosition(Vector3 worldPosition, Vector3 originPos)
    {
        Vector2Int gridPosition = new Vector2Int((Mathf.FloorToInt((worldPosition.x - originPos.x + GameConfig.CellSize.x / 2f)/(- GameConfig.CellSize.x))),
        Mathf.FloorToInt((worldPosition.z - originPos.z + GameConfig.CellSize.y / 2f)/ (-GameConfig.CellSize.y)));
        return gridPosition;
    }

     public static bool IsGridPositionValid(Vector2Int gridPosition, int height, int width)
    {
        if (gridPosition.x < 0 || gridPosition.y < 0 || gridPosition.x >= height || gridPosition.y >= width)
        {
            return false;
        }
        return true;
    }

}