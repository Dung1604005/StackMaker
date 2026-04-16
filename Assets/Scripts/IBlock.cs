using UnityEngine;

public interface IBlock
{
    public Vector3 GetWorldPosition();

    public Vector2 GetGridPosition();

    public BlockState GetBlockState();


}

public enum BlockState
{
    Empty,
    Fullfill,

    HaveStack,
    Blocked
}
public enum Direct
{
     Forward, Back, Right, Left
}