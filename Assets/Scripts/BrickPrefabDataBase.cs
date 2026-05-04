using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BrickPrefabDataBase", menuName = "Game Data/BrickPrefabDataBase")]
public class BrickPrefabDataBase : ScriptableObject
{
    [SerializeField] private List<BrickBase> brickBases  = new List<BrickBase>();

    public BrickBase GetBrickPrefab(int index)
    {
        if(index >= 0 && index < brickBases.Count)
        {
            return brickBases[index];
        }
        return null;
    }

    public int Count()
    {
        return brickBases.Count;
    }

    public List<string> GetAllNamePrefab()
    {
        List<string> result = new List<string>();

        foreach(BrickBase brickBase in brickBases)
        {
            result.Add(brickBase.GetName());
        }
        return result;
    }
}
