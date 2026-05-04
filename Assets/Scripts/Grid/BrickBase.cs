
using Unity.VisualScripting;
using UnityEngine;

public abstract class BrickBase : MonoBehaviour, IBrick, IPoolable
{

    [SerializeField] protected int idBrick;
    [SerializeField] protected BrickState brickState;

    [SerializeField] protected bool interacted;

    [SerializeField] protected Vector3 eulerRotation;

    public virtual void OnInit()
    {
        interacted = false;
        idBrick = this.gameObject.GetInstanceID();

    }

    public void OnDeSpawn()
    {
        foreach (Transform child in this.transform)
        {
            
            child.gameObject.SetActive(true);
        
        }
        interacted = false;
        brickState = BrickState.Null;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetInfo(BrickState _blockState, Vector3 EulerRotate)
    {

        brickState = _blockState;

        this.transform.Rotate(EulerRotate);

        eulerRotation = EulerRotate;
        
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public BrickState GetBrickState()
    {
        return brickState;
    }

    public virtual void RotateBrick(Vector3 eulerRotate)
    {
        this.eulerRotation += eulerRotate;
         this.transform.Rotate(eulerRotate);
    }

    public Vector3 GetEulerRotation()
    {
        return eulerRotation;
    }

    public void SetEulerRotation(Vector3 eulerRotation)
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
