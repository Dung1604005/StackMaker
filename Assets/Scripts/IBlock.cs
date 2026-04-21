using System;
using UnityEngine;

public interface IBlock
{
    public Vector3 GetWorldPosition();

    public Vector2 GetGridPosition();

    public BlockState GetBlockState();


}

[Serializable]
public enum BlockState
{
    Null = -1,
   Blocked = 0,
    Empty = 1,
    LeftTopCorner = 2,
    HaveStack = 3,
    Fullfill = 4,
    StartBlock = 5,
    EndBlock = 6,
    RightTopCorner = 7,
    LeftBottomCorner = 8,
    RightBottomCorner = 9,
}
public enum Direct
{
     Forward, Back, Right, Left, NULL
}