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
            if (collider.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                if (playerController.StackObjectController.RemoveStackObject())
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

        }
    }
   
}
