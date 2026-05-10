using TMPro;
using UnityEngine;


public class WinPanel: MonoBehaviour, IUIPanel
{
    [SerializeField] private TextMeshProUGUI scoredText;
    public void SetActive(bool active)
    {
        SetScore(LevelManager.Instance.CollectedStack);
        this.transform.gameObject.SetActive(active);
    }
    public void NextLevel()
    {
        SetActive(false);
        UIManagement.Instance.PlayLevelTransition();
    }

    public void SetScore(int score)
    {
        scoredText.text = score + " Stack Collected!";
    }

    public void Retry()
    {
        LevelManager.Instance.ChangeLevel(LevelManager.Instance.CurrentLevelNumber);
        SetActive(false);

       
        
    }
}