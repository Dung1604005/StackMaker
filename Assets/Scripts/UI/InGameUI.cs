using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI: MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI levelText;

    public void OnEnable()
    {
        EventBus<OnChangeLevel>.Subcribe(SetLevel);
    }

    public void OnDisable()
    {
        EventBus<OnChangeLevel>.UnSubcribe(SetLevel);
    }
    public void SetLevel(OnChangeLevel onChangeLevel)
    {
        levelText.text = "Level " + (onChangeLevel.LevelId + 1).ToString();


    }

    public void OpenPauseUI()
    {
        EventBus<OnPause>.Raise(new OnPause{});

        EventBus<OnCanInteract>.Raise(new OnCanInteract{canInteract = false});
    }
}