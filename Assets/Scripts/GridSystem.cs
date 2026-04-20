using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
public class GridSystem: MonoBehaviour
{
    [SerializeField] private AreaController currentArea;

    public AreaController CurrentArea => currentArea;

    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// This func get direct from swipe detect then raise event OnChangeDirect
    /// </summary>
    /// <param name="direct"></param>
    public void ActiveMove(Direct direct)
    {
        // Dont let player change direction when moving
        if (playerController.IsMoving)
        {
            
            return;
        }
        EventBus<OnChangeDirect>.Raise(new OnChangeDirect
        {
                direct = direct,
                playerGridPosition = playerController.PlayerGridPosition
        });
    }


}
