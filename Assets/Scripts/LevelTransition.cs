using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelTransition : MonoBehaviour, IUIPanel
{

    [SerializeField] private RectTransform maskHoleRect;

    [SerializeField] private Vector2 maxSizeHole;

    [SerializeField] private float transitionSpeed;

    [SerializeField] private CameraEffect cameraEffect;

    

    private Vector2 targetSize;

    private bool isInTransition;

    


    public void SetActive(bool active)
    {
        this.transform.gameObject.SetActive(active);

        if (active)
        {
            EndLevelTransition();
        }
    }

    public void LoadLevelTransition()
    {
        cameraEffect.SetZoomField(60);
        targetSize = maxSizeHole;
        isInTransition = true;
        
    }

    public void EndLevelTransition()
    {

        cameraEffect.SetZoomField(55);
        targetSize = Vector2.zero;

        isInTransition = true;
    }


    void Update()
    {
        if(!isInTransition)return;


        maskHoleRect.sizeDelta = Vector2.Lerp(maskHoleRect.sizeDelta, targetSize, transitionSpeed*Time.deltaTime);

        if((maskHoleRect.sizeDelta - targetSize).sqrMagnitude <= 1)
        {
            if((targetSize - Vector2.zero).sqrMagnitude <= 1)
            {
                LevelManager.Instance.ChangeLevel(LevelManager.Instance.CurrentLevel.levelId + 1);
                StartCoroutine(IEDelayAction(0.2f, LoadLevelTransition));
            }
            else
            {
                isInTransition = false;
                SetActive(false);
            }
        }
    }

    IEnumerator IEDelayAction(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);

        action?.Invoke();
    }


}
