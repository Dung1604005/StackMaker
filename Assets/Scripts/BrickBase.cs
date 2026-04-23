
using UnityEngine;

public class BrickBase : MonoBehaviour, IBlock, IPoolable
{

    [SerializeField] private BlockState blockState;

    [SerializeField] protected bool interacted;

    public void OnInit()
    {
        interacted = false;
    
    }

    public void OnDeSpawn()
    {
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
