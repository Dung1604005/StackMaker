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
            //stackObjectController.AddStackObject();
            interacted = false;
        }
    }
}
