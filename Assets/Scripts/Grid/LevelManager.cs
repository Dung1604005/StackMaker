using Unity.VisualScripting;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelDataBaseSO levelDataBase;

    [SerializeField] private LevelDataSO currentLevel;

    [SerializeField] private int collectedStack;

    public LevelDataSO CurrentLevel => currentLevel;

    public int CollectedStack => collectedStack;

    public void OnEnable()
    {
        EventBus<OnGameStart>.Subcribe(OnGameStart);
        EventBus<OnAddStack>.Subcribe(UpdateStackAmount);
    }

    public void OnDisable()
    {
        EventBus<OnGameStart>.UnSubcribe(OnGameStart);
        EventBus<OnAddStack>.UnSubcribe(UpdateStackAmount);
    }
    public void ChangeLevel(int levelId)
    {
        if (levelDataBase.GetLevel(levelId) != null)
        {
            currentLevel = levelDataBase.GetLevel(levelId);
            collectedStack = 0;
            PlayerPrefs.SetInt("currentLevel", levelId);
            EventBus<OnChangeLevel>.Raise(new OnChangeLevel { LevelId = levelId });

            
        }
        else
        {
            Debug.Log("Level is invalid");
        }
    }

    public void OnGameStart(OnGameStart onGameStart)
    {
        OnInit();
    }

    public void UpdateStackAmount(OnAddStack onAddStack)
    {
        collectedStack += 1;

    }

    public void OnInit()
    {
        collectedStack = 0;
        ChangeLevel(currentLevel.levelId);       
    }
    public void LoadSaveData()
    {
         if (PlayerPrefs.HasKey("currentLevel"))
        {
            int levelId = PlayerPrefs.GetInt("currentLevel");
            ChangeLevel(levelId);
        }
        else
        {
            PlayerPrefs.SetInt("currentLevel", 0);
            ChangeLevel(0);
        }
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        LoadSaveData();
        OnInit();
       
    }

    #region HELPER METHOD

    [ContextMenu("Clear save data")]
    public void ClearSaveData()
    {
        PlayerPrefs.SetInt("currentLevel", 0);
    }

    #endregion


}