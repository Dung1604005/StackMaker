
using Unity.VisualScripting;
using UnityEngine;

public class BrickBase : MonoBehaviour, IBlock, IPoolable
{

    [SerializeField] protected int idBrick;
    [SerializeField] private BlockState blockState;

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
        blockState = BlockState.Null;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetInfo(BlockState _blockState, Vector3 EulerRotate)
    {

        blockState = _blockState;

        this.transform.Rotate(EulerRotate);
        
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public BlockState GetBlockState()
    {
        return blockState;
    }

    public virtual void OnTriggerEnter(Collider collider)
    {


    }

}
