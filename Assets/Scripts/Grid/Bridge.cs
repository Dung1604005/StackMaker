using System.Data.Common;
using UnityEngine;

public class Bridge: BrickBase
{
    public void OnEnable()
    {
        EventBus<OnRemoveStackSucceed>.Subcribe(FillStack);
    }

    public void OnDisable()
    {
        EventBus<OnRemoveStackSucceed>.UnSubcribe(FillStack);
    }

    public override void OnDeSpawn()
    {
        base.OnDeSpawn();
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag(GameConfig.STACK_TAG))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public override void OnTriggerEnter(Collider collider)
    {
        if (interacted)
        {
            return;
        }
        if (collider.CompareTag(GameConfig.PLAYER_TAG))
        {
            // If bridge collder with player then add stack and turn on the stack piece
            EventBus<OnRemoveStack>.Raise(new OnRemoveStack
            {
                worldPositionStack = GetWorldPosition()
            });
            
        }
    }
     public void FillStack(OnRemoveStackSucceed onRemoveStackSucceed)
    {
        if(GetWorldPosition() != onRemoveStackSucceed.worldPositionStack)
        {
            return;
        }
        interacted = true;
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag(GameConfig.STACK_TAG))
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
