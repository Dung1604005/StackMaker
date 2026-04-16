using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem
{
    
}

public class AreaSystem
{
    private AreaContext areaCtx;

    private IBlock[,] graph;

    #region GETTER


    public IBlock[,] Graph => graph;

    #endregion

    #region CONSTRUCTOR

    public AreaSystem(AreaContext _areaCtx)
    {
        areaCtx = _areaCtx;
    }

    public void GenerateGraph()
    {
        graph = new IBlock[areaCtx.Height, areaCtx.Width];


    }

    #endregion
}

[System.Serializable]
public class GridContext
{
    public int TotalArea;
}

[System.Serializable]

public class AreaContext
{
    public int Width;

    public int Height;

    public int MinContinuousBlock;

    public int MaxContinuousBlock;

    public int TotalStack;

    public Vector3 OriginWorldPos;

    public Vector2 StartGridPos;
}