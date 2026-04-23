using UnityEngine;

public class Brick : BrickBase
{
    public override void OnTriggerEnter(Collider collider)
    {
        if (interacted)
        {
            return;
        }
        if (collider.CompareTag(GameConfig.PLAYER_TAG))
        {
            // If brick collder with player then add stack and turn off the stack piece
            EventBus<OnAddStack>.Raise(new OnAddStack
            {
                
            });
            interacted = true;
            foreach(Transform child in this.transform)
            {
                if (child.CompareTag(GameConfig.STACK_TAG))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    
}
