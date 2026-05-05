using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour, IUIPanel
{
    public void SetActive(bool active)
    {
        this.transform.gameObject.SetActive(active);
    }
    public void StartGame()
    {
        SetActive(false);
        LevelManager.Instance.ChangeLevel(LevelManager.Instance.CurrentLevel.levelId);
        EventBus<OnGameStart>.Raise(new OnGameStart{});
    }

    public void OpenSetting()
    {
        
    }

    public void ExitGame()
    {
        PlayerPrefs.SetInt("currentLevel", LevelManager.Instance.CurrentLevel.levelId);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
           
           Application.Quit();
        #endif
    }
}