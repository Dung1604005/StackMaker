using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI: MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI levelText;

    public void SetLevel(OnChangeLevel onChangeLevel)
    {
        levelText.text = "Level " + onChangeLevel.LevelId.ToString();

    }

    public void OpenPauseUI()
    {
        EventBus<OnPause>.Raise(new OnPause{});
    }
}