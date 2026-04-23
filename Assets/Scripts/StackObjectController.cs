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

    [SerializeField]private Stack<StackObject> currentStackObjects;

    private ObjectPool<StackObject> objectPool;

    public float OffsetY => offsetY;

    public void OnEnable()
    {
        EventBus<OnAddStack>.Subcribe(AddStackObject);
        EventBus<OnRemoveStack>.Subcribe(RemoveStackObject);
    }
    public void OnDisable()
    {
        EventBus<OnAddStack>.UnSubcribe(AddStackObject);
        EventBus<OnRemoveStack>.UnSubcribe(RemoveStackObject);
    }
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

    public void AddStackObject(OnAddStack onAddStack)
    {
       
        StackObject stackObject = objectPool.Get();
        
        stackObject.transform.SetParent(this.transform);
        stackObject.transform.localPosition = new Vector3(0, ((currentStackObjects.Count-2)*offsetY), 0);
        currentStackObjects.Push(stackObject);
        playerController.Jump(currentStackObjects.Count);
        
        
    }

    
    public void RemoveStackObject(OnRemoveStack onRemoveStack)
    {
        if(currentStackObjects.Count == 0)
        {
            playerController.StopMove();
            //Handle logic when player lose
            return;
        }
        StackObject stackObject = currentStackObjects.Pop();

        objectPool.ReturnToPool(stackObject);
        playerController.Fall(currentStackObjects.Count);

        EventBus<OnRemoveStackSucceed>.Raise(new OnRemoveStackSucceed
        {
            IdBrick = onRemoveStack.IdBrick
        });
    }
    



    
}
