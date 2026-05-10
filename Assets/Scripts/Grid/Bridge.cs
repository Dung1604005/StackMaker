using System.Data.Common;
using UnityEngine;

public class Bridge : BrickBase
{
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
            PlayerController player = ColliderCache<PlayerController>.GetComponent(collider);

            if(player == null)
            {
                player = collider.GetComponent<PlayerController>();
                ColliderCache<PlayerController>.AddComponent(collider, player);
            }
            if (player != null)
            {
                if (player.StackObjectController.RemoveStackObject())
                {
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
            else
            {
                Debug.LogError("Collider of player dont have component PlayerController");
            }

        }
    }
   
}
