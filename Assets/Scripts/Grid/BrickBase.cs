
using Unity.VisualScripting;
using UnityEngine;

public class BrickBase : MonoBehaviour, IBrick, IPoolable
{

    [SerializeField] protected int idBrick;
    [SerializeField] private BrickState brickState;

    [SerializeField] protected bool interacted;


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
        
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public BrickState GetBrickState()
    {
        return brickState;
    }

    public virtual void OnTriggerEnter(Collider collider)
    {


    }

}
