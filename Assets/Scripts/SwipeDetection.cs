using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;

    [SerializeField] private Vector2 endPos;

    private bool canDetect;

    public void OnEnable()
    {
        EventBus<OnWinEvent>.Subcribe(OnWin);
    }
    public void OnDisable()
    {
        EventBus<OnWinEvent>.UnSubcribe(OnWin);
    }
    public void OnInit()
    {
        canDetect = true;
    }

    public void OnWin(OnWinEvent onWinEvent)
    {
        // block input when win
        canDetect = false;
    }
    void Awake()
    {
        OnInit();
    }

    void Update()
    {
        if(!canDetect)return;
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Calculate to first touch and the last to get the direct player want to move
            endPos = Input.mousePosition;
            Direct swipeDirect = CalculateDirect2D.CalculateDirect(startPos, endPos);
            EventBus<OnChangeDirect>.Raise(new OnChangeDirect
            {
                direct = swipeDirect,
            });

        }
    }



}
