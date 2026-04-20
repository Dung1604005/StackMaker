using UnityEngine;

public class SwipeDetection: MonoBehaviour
{
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private Vector2 startPos;

    [SerializeField] private Vector2 endPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            Direct swipeDirect = CalculateDirect2D.CalculateDirect(startPos, endPos);
            gridSystem.ActiveMove(swipeDirect);
            
        }
    }



}
