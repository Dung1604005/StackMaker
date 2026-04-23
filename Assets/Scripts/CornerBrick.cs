using UnityEngine;

public class CornerBrick : BrickBase
{
    [SerializeField] private Animator anim;

    void Awake()
    {

    }

    public void ChangeAnim(string nameAnim)
    {
        anim.SetTrigger(nameAnim);
    }
    public override void OnTriggerEnter(Collider collider)
    {

        //play anim when interact
        if (collider.CompareTag(GameConfig.PLAYER_TAG))
        {
            ChangeAnim("interact");
        }
        //same logic with normal brick
        if (interacted)
        {
            return;
        }
        EventBus<OnAddStack>.Raise(new OnAddStack
        {

        });
        interacted = true;
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag(GameConfig.STACK_TAG))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
