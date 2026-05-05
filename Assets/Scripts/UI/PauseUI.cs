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
        EventBus<OnCanInteract>.Raise(new OnCanInteract{canInteract = true});
    }

    public void BackHome()
    {
        SetActive(false);
        EventBus<OnBackHome>.Raise(new OnBackHome{});
        EventBus<OnCanInteract>.Raise(new OnCanInteract{canInteract = false});
    }

    public void Retry()
    {
        SetActive(false);
        LevelManager.Instance.ChangeLevel(LevelManager.Instance.CurrentLevel.levelId);
        EventBus<OnCanInteract>.Raise(new OnCanInteract{canInteract = true});
        
        
    }

}