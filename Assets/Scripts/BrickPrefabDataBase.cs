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
}
