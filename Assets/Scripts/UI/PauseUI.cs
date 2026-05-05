using UnityEngine;

public class PauseUI: MonoBehaviour, IUIPanel
{
    public void SetActive(bool active)
    {
        this.transform.gameObject.SetActive(active);
    }

    public void ContinueGame()
    {
        SetActive(false);
        EventBus<OnContinue>.Raise(new OnContinue{});
    }

    public void BackHome()
    {
        SetActive(false);
        EventBus<OnBackHome>.Raise(new OnBackHome{});
    }

    public void Retry()
    {
        SetActive(false);
        LevelManager.Instance.ChangeLevel(LevelManager.Instance.CurrentLevel.levelId);
      
    }

}