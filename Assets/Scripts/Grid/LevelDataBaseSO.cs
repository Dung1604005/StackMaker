using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewMapLevel", menuName = "Game Data/LevelDataBase")]
public class LevelDataBaseSO : ScriptableObject
{
    
    [SerializeField]private List<LevelDataSO> levelDataBase = new List<LevelDataSO>();

    public LevelDataSO GetLevel(int index)
    {
        if(index >= 0 && index < levelDataBase.Count)
        {
            return levelDataBase[index];
        }

        return null;
    }

    public List<string> GetAllNameLevel()
    {
        List<string> result = new List<string>();

        foreach(LevelDataSO levelDataSO in levelDataBase)
        {
            result.Add(levelDataSO.nameLevel);
        }
        return result;
    }

    public int GetCountLevel()
    {
        return levelDataBase.Count;
    }

    public void AddLevelData(LevelDataSO levelDataSO)
    {
        levelDataBase.Add(levelDataSO);
    }

}