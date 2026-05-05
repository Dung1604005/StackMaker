using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelDataBaseSO levelDataBase;

    [SerializeField] private LevelDataSO currentLevel;

    public LevelDataSO CurrentLevel => currentLevel;

    public LevelDataBaseSO LevelDataBase => levelDataBase;


    public void ChangeLevel(int levelId)
    {
        if (levelDataBase.GetLevel(levelId) != null)
        {
            currentLevel = levelDataBase.GetLevel(levelId);


            EventBus<OnChangeLevel>.Raise(new OnChangeLevel { LevelId = levelId });
        }
        else
        {
            Debug.Log("Level is invalid");
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


}