using System.Numerics;

public class GridHelper
{
    public Vector3 ConvertGridToWorldPosition(int x, int z, Vector3 originPos)
    {
        
        Vector2 cellSize = new Vector2(1f, 1f);
        Vector3 worldPos = new Vector3(originPos.X - x * GameConfig.CellSize.x - GameConfig.CellSize.x / 2f, 0f, originPos.Z - z * GameConfig.CellSize.y - GameConfig.CellSize.y / 2f);

        return worldPos;
    }

}