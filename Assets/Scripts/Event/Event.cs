
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

public struct OnAddStack: IEvent
{
    
}

public struct OnRemoveStack: IEvent
{
    public int IdBrick;
}

public struct OnRemoveStackSucceed : IEvent
{
    public int IdBrick;
}

public struct OnWinEvent: IEvent
{
    
}

public struct OnLoseEvent: IEvent
{
    
}