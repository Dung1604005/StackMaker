using UnityEngine;

[System.Serializable]

public class AreaContext
{
    public int areaId;
    public int Width;

    public int Height;

    public int StackToPass;

    public int[] BridgeDirect;

    public int[] StartPosition;

    public int[] EndPosition;

    public BlockState[][] Grid;

    
}