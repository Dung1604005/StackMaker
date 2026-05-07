using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public static UIManagement Instance;
    [SerializeField] private MainMenuUI mainMenuUI;

    [SerializeField] private PauseUI pauseUI;

    [SerializeField] private WinPanel winPanel;

    [SerializeField] private LevelTransition levelTransition;


    public void OnEnable()
    {
        EventBus<OnBackHome>.Subcribe(OpenMainMenu);
        EventBus<OnPause>.Subcribe(OpenPauseUI);
        EventBus<OnWinEvent>.Subcribe(OpenWinUI);
        
    }
    public void OnDisable()
    {
        EventBus<OnBackHome>.UnSubcribe(OpenMainMenu);
        EventBus<OnPause>.UnSubcribe(OpenPauseUI);
        EventBus<OnWinEvent>.UnSubcribe(OpenWinUI);
        
    }
    public void OpenMainMenu(OnBackHome onBackHome)
    {
        mainMenuUI.SetActive(true);
    }
    public void OpenPauseUI(OnPause onPause)
    {
        pauseUI.SetActive(true);
    }
    public void OpenWinUI(OnWinEvent onWinEvent)
    {
        winPanel.SetActive(true);
    }
    public void PlayLevelTransition()
    {
        levelTransition.SetActive(true);
    }

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    

    }
}
