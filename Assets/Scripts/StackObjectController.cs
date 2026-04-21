using System.Collections.Generic;
using UnityEngine;

public class StackObjectController : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField] private PlayerController playerController;

    [SerializeField] private float offsetY;

    [Header("Info for pool")]

    [SerializeField] private int minSize;

    [SerializeField] private int maxSize;

    [SerializeField] private StackObject prefab;

    [SerializeField] private Transform parent;

    private ObjectPool<StackObject> objectPool;

    private Stack<StackObject> currentStackObjects;

    public float OffsetY => offsetY;


    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        objectPool = new ObjectPool<StackObject>(prefab, minSize, maxSize, parent);
        currentStackObjects = new Stack<StackObject>();
        objectPool.PreWarm();
    }

    public int CountStackObject()
    {
        return currentStackObjects.Count;
    }

    public void AddStackObject()
    {
        StackObject stackObject = objectPool.Get();
        
        stackObject.transform.SetParent(this.transform);
        stackObject.transform.localPosition = new Vector3(0, ((currentStackObjects.Count-2)*offsetY), 0);
        currentStackObjects.Push(stackObject);
        
        EventBus<OnChangeStackAmount>.Raise(new OnChangeStackAmount
        {
            numberStack = currentStackObjects.Count
        });
    }

    public void RemoveStackObject()
    {
        StackObject stackObject = currentStackObjects.Pop();

        objectPool.ReturnToPool(stackObject);
    }



    
}
