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
    LeftTopCorner = 2,

    RightTopCorner = 7,

    LeftBottomCorner = 8,

    RightBottomCorner = 9,
    Empty = 1,
    Fullfill = 4,
    HaveStack = 3,
    Blocked = 0,
    StartBlock = 5,
    EndBlock = 6
}
public enum Direct
{
     Forward, Back, Right, Left, NULL
}