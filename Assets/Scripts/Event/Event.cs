
using UnityEngine;

public interface IEvent
{
    
}

public struct OnChangeDirect: IEvent
{
    public Direct direct;

    public Vector2Int playerGridPosition;
}

public struct OnChangeTargetPositionPlayer: IEvent
{
    public Vector3 TargetPosition;

    public Vector2Int GridPosition;

    public BlockState TargetBlockState;


}