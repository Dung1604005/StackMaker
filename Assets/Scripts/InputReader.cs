using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;

    [SerializeField] private Vector2 endPos;

    [SerializeField]private bool canDetect;

    public void OnEnable()
    {
        EventBus<OnWinEvent>.Subcribe(OnWin);
        EventBus<OnPause>.Subcribe(OnPause);
        EventBus<OnContinue>.Subcribe(OnContinue);
        EventBus<OnGameStart>.Subcribe(OnGameStart);
        EventBus<OnBackHome>.Subcribe(OnBackHome);
        EventBus<OnChangeLevel>.Subcribe(OnChangeLevel);
    }
    public void OnDisable()
    {
        EventBus<OnWinEvent>.UnSubcribe(OnWin);
        EventBus<OnGameStart>.UnSubcribe(OnGameStart);
        EventBus<OnPause>.UnSubcribe(OnPause);
        EventBus<OnContinue>.UnSubcribe(OnContinue);
        EventBus<OnBackHome>.UnSubcribe(OnBackHome);
        EventBus<OnChangeLevel>.UnSubcribe(OnChangeLevel);
    }

    public void OnGameStart(OnGameStart onGameStart)
    {
        canDetect = true;
    }

    public void OnPause(OnPause onPause)
    {
        canDetect = false;
    }
    public void OnContinue(OnContinue onContinue)
    {
        canDetect = true;
    }
    public void OnBackHome(OnBackHome onBackHome)
    {
        canDetect = false;
    }

    public void OnChangeLevel(OnChangeLevel onChangeLevel)
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
        canDetect = false;
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

            //Player have to swipe
            if((endPos - startPos).sqrMagnitude <= 3)
            {
                return;
            }
            Direct swipeDirect = CalculateDirect2D.CalculateDirect(startPos, endPos);
            EventBus<OnChangeDirect>.Raise(new OnChangeDirect
            {
                direct = swipeDirect,
            });

        }
    }



}
