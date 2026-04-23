
using UnityEngine;

public interface IEvent
{
    
}

public struct OnChangeDirect: IEvent
{
    public Direct direct;
}
public struct OnMoveOnBridge: IEvent
{
    public Direct direct;

    public Vector3 target;

}
public struct OnChangeTargetPositionPlayer: IEvent
{
    public Vector3 TargetPosition;

    public Vector2Int GridPosition;

    public BlockState TargetBlockState;


}

public struct OnChangeStackAmount: IEvent
{
    public int numberStack;
}
