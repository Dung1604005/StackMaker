
using Unity.VisualScripting;
using UnityEngine;

public abstract class BrickBase : MonoBehaviour, IBrick, IPoolable
{
    [SerializeField] protected string nameBrick;
    [SerializeField] protected int idBrick;
    [SerializeField] protected BrickState brickState;

    [SerializeField] protected bool interacted;

    [SerializeField] protected Vector3 eulerRotation;

    public virtual void OnInit()
    {
        interacted = false;
    }

    public virtual void OnDeSpawn()
    {
        foreach (Transform child in this.transform)
        {
            
            child.gameObject.SetActive(true);
        
        }
        interacted = false;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public BrickState GetBrickState()
    {
        return brickState;
    }

    public string GetName()
    {
        return nameBrick;
    }

    public virtual void RotateBrick(Vector3 eulerRotate)
    {
        this.eulerRotation += eulerRotate;
         this.transform.Rotate(eulerRotate);
         Debug.Log(transform.eulerAngles);
    }

    public Vector3 GetEulerRotation()
    {
        return eulerRotation;
    }

    public virtual void SetEulerRotation(Vector3 eulerRotation)
    {
        this.eulerRotation = eulerRotation;
        this.transform.eulerAngles = eulerRotation;
    }

    public int GetBrickId()
    {
        return idBrick;
    }

    public virtual void OnTriggerEnter(Collider collider)
    {


    }

}
