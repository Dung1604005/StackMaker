using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [SerializeField] private List<PoolContext> inputPoolContext;

    private Dictionary<string, PoolContext> dictPoolContext = new Dictionary<string, PoolContext>();
    private Dictionary<string, IPool> dictPool = new Dictionary<string, IPool>();

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
    public void OnInit()
    {
        dictPoolContext.Clear();
        dictPool.Clear();

        foreach(PoolContext poolContext in inputPoolContext)
        {
            dictPoolContext.Add(poolContext.PoolName, poolContext);
        }
    }
    public T Spawn<T>(T prefab, Vector3 position, Quaternion quaternion) where T: Component, IPoolable
    {
        string name = prefab.tag;

        ObjectPool<T> pool;

        if (!dictPool.ContainsKey(name))
        {
            PoolContext poolContext = new PoolContext();
            if (dictPoolContext.ContainsKey(name))
            {
                poolContext = dictPoolContext[name];
            }

            ObjectPool<T> objectPool = new ObjectPool<T>(prefab, poolContext.MaxSize, poolContext.MinSize, poolContext.Parent);
            poolContext.PoolName = name;

            if (!dictPoolContext.ContainsKey(name))
            {
                dictPoolContext.Add(name, poolContext);
            }

            dictPool.Add(name, objectPool);
            pool = objectPool;
        }
        else
        {
            pool = (ObjectPool<T>)dictPool[name];
        }

        T result = pool.Get();
        result.gameObject.tag = name;
        result.transform.position = position;
        result.transform.rotation = quaternion;

        return result;
    }

    public void DeSpawn<T>(T ob) where T: Component, IPoolable
    {
        if (dictPool.ContainsKey(ob.gameObject.tag))
        {
            ObjectPool<T> pool = (ObjectPool<T>)dictPool[name];

            pool.ReturnToPool(ob);
        }
        
    }





    
}


[Serializable]

public class PoolContext
{
    
    public string PoolName;

    public int MinSize = 10;

    public int MaxSize = 50;

    public Transform Parent = null;
}