
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

    public BrickState TargetBlockState;


}

public struct OnAddStack: IEvent
{
    
}

public struct OnRemoveStack: IEvent
{
    public Vector3 worldPositionStack;
}

public struct OnRemoveStackSucceed : IEvent
{
    public Vector3 worldPositionStack;
}

public struct OnGameStart : IEvent
{
    

}
public struct OnPause : IEvent
{
    
}
public struct OnContinue: IEvent
{
    
}

public struct OnBackHome: IEvent
{
    
}

public struct OnRetry: IEvent
{
    
}

public struct OnChangeLevel: IEvent
{
    public int LevelId;
}

public struct OnWinEvent: IEvent
{
    
}

public struct OnLoseEvent: IEvent
{
    
}