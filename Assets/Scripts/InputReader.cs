using System.Collections;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;

    [SerializeField] private Vector2 endPos;

    [SerializeField]private bool canDetect;

    public void OnEnable()
    {
        EventBus<OnCanInteract>.Subcribe(OnSetInteract);
        
    }
    public void OnDisable()
    {
        EventBus<OnCanInteract>.UnSubcribe(OnSetInteract);
    }

    public void OnSetInteract(OnCanInteract onCanInteract)
    {
        if(canDetect == true)
        {
            StartCoroutine(DelayEnableDetection());
        }
        else
        {
            canDetect = onCanInteract.canInteract;
        }
        
    }
    
    void Awake()
    {
        canDetect = false;
    }

    void Update()
    {
        if(!canDetect)return;
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Calculate to first touch and the last to get the direct player want to move
            endPos = Input.mousePosition;

            //Player have to swipe
            if((endPos - startPos).sqrMagnitude <= 36)
            {
                return;
            }
            
            Direct swipeDirect = CalculateDirect2D.CalculateDirect(startPos, endPos);
            Debug.Log(swipeDirect);
            EventBus<OnChangeDirect>.Raise(new OnChangeDirect
            {
                direct = swipeDirect,
            });

        }
    }
    private IEnumerator DelayEnableDetection()
    {
        
        yield return new WaitForEndOfFrame(); 
        
        canDetect = true;
    }



}
