
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T:  Component, IPoolable
{
    
    private Queue<T> pool = new Queue<T>();

    private int minSize;

    private int maxSize;

    private T prefab;

    private Transform parent;

    public void ReturnToPool(T ob)
    {
        if(pool.Count >= maxSize)
        {
            Object.Destroy(ob.gameObject);
        }
        else
        {
            ob.transform.SetParent(parent);
            ob.gameObject.SetActive(false);

            ob.OnDeSpawn();

            pool.Enqueue(ob);
        }
    }

    public T Get()
    {
        T ob;
        if(pool.Count > 0)
        {
            ob = pool.Dequeue();
        }
        else
        {
            ob = CreateNewObject();
        }

        ob.gameObject.SetActive(true);

        ob.OnInit();
        ob.transform.SetParent(null);
        return ob;
    }

    public T CreateNewObject()
    {
        T ob = Object.Instantiate(prefab,parent);
        ob.gameObject.SetActive(false);
        return ob;
    }

    public void PreWarm()
    {
        pool.Clear();
        for(int i = 0; i < minSize; i++)
        {
            pool.Enqueue(CreateNewObject());
        }
        
    }

    public ObjectPool(T _prefab, int _minSize, int _maxSize, Transform _parent)
    {
        this.prefab = _prefab;
        this.minSize = _minSize;
        this.maxSize = _maxSize;
        this.parent = _parent;
    }

}


public interface IPoolable
{
    public void OnInit();

    public void OnDeSpawn();
}