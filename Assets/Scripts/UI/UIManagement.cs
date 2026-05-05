using UnityEngine;

public class UIManagement : MonoBehaviour
{
    
    [SerializeField] private MainMenuUI mainMenuUI;

    [SerializeField] private PauseUI pauseUI;

    public void OnEnable()
    {
        EventBus<OnBackHome>.Subcribe(OpenMainMenu);
        EventBus<OnPause>.Subcribe(OpenPauseUI);
    }
    public void OnDisable()
    {
        EventBus<OnBackHome>.UnSubcribe(OpenMainMenu);
        EventBus<OnPause>.UnSubcribe(OpenPauseUI);
    }
    public void OpenMainMenu(OnBackHome onBackHome)
    {
        mainMenuUI.SetActive(true);
    }
    public void OpenPauseUI(OnPause onPause)
    {
        pauseUI.SetActive(true);
    }
}
