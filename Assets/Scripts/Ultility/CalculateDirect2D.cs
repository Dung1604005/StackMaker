
using UnityEngine;

public class CalculateDirect2D
{
    
    public static Direct CalculateDirect(Vector2 startPos, Vector2 endPos)
    {
        Vector2 yAxis = new Vector2(0, 1);
        Vector2 vectorA = (endPos - startPos).normalized;
        // Calculate the degree between the VectorA and Y-Axis
        float angle = Vector2.SignedAngle(yAxis, vectorA);

        if(Mathf.Abs(angle) <= 45f)
        {
            return Direct.Forward;
        }
        else if(Mathf.Abs(angle) >= 135f)
        {
            return Direct.Back;
        }
        else if(angle > 45f && angle < 135f)
        {
            return Direct.Left;
        }
        else
        {
            return Direct.Right;
        }
    }

    public static Vector2Int ChangeDirectToVector2Int(Direct direct)
    {
        switch (direct)
        {
            case Direct.Back:
                return new Vector2Int(1,0);
                
            case Direct.Forward:
                return  new Vector2Int(-1, 0);
                
            case Direct.Left:
                return  new Vector2Int(0,  -1);
                
            default:
                return new Vector2Int(0, 1);
                

        }
    }

    public static Direct ChangeCornerToDirect(BlockState blockState, Direct currentDirect)
    {
        switch (blockState)
        {
            case BlockState.LeftTopCorner:
               if(currentDirect == Direct.Forward)
                {
                    return Direct.Right;
                }
                else if(currentDirect == Direct.Left)
                {
                    return Direct.Back;
                }
                else
                {
                    return Direct.NULL;
                }
            case BlockState.RightTopCorner:
               if(currentDirect == Direct.Forward)
                {
                    return Direct.Left;
                }
                else if(currentDirect==Direct.Right)
                {
                    return Direct.Back;
                }
                else
                {
                    return Direct.NULL;
                }
            case BlockState.LeftBottomCorner:
               if(currentDirect == Direct.Left)
                {
                    return Direct.Forward;
                }
                else if (currentDirect == Direct.Back)
                {
                    return Direct.Right;
                }
                else
                {
                    return Direct.NULL;
                }
            case BlockState.RightBottomCorner:
                if(currentDirect == Direct.Right)
                {
                    return Direct.Forward;
                }
                else if (currentDirect == Direct.Back)
                {
                    return Direct.Left;
                }
                else
                {
                    return Direct.NULL;
                }
            default:
               return Direct.NULL;
        }
    }


}