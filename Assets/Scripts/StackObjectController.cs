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

    [SerializeField] private Stack<StackObject> currentStackObjects = new Stack<StackObject>();

    private ObjectPool<StackObject> objectPool;

    public float OffsetY => offsetY;

    public void OnEnable()
    {
        EventBus<OnAddStack>.Subcribe(AddStackObject);
        
    }
    public void OnDisable()
    {
        EventBus<OnAddStack>.UnSubcribe(AddStackObject);
        
    }
    void Start()
    {
        
        objectPool = new ObjectPool<StackObject>(prefab, minSize, maxSize, parent);
        objectPool.PreWarm();
    }

    public void OnInit()
    {
        
        ClearAllStack();
    }

    public int CountStackObject()
    {
        return currentStackObjects.Count;
    }

    public void AddStackObject(OnAddStack onAddStack)
    {


        StackObject stackObject = objectPool.Get();

        stackObject.transform.SetParent(this.transform);
        stackObject.transform.localPosition = new Vector3(0, ((currentStackObjects.Count - 2) * offsetY), 0);
        currentStackObjects.Push(stackObject);
        playerController.Jump(currentStackObjects.Count);



    }

    public void ClearAllStack()
    {
        for (int time = 0; time < 100 && currentStackObjects.Count > 0; time++)
        {
            
            StackObject stackObject = currentStackObjects.Pop();

            objectPool.ReturnToPool(stackObject);
        }
    }


    public bool RemoveStackObject()
    {
        if (currentStackObjects.Count == 0)
        {
            playerController.StopMove();
            //Handle logic when player lose
            return false;
        }
        StackObject stackObject = currentStackObjects.Pop();

        objectPool.ReturnToPool(stackObject);
        playerController.Fall(currentStackObjects.Count);

        return true;

    }





}
