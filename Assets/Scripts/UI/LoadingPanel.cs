
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LoadingPanel: MonoBehaviour
{
    
    [SerializeField] private Slider loadingBar;

    [SerializeField] private float loadingTime;

    private float loadingTimer = 0f;

    public void Loading()
    {
        loadingTimer += Time.deltaTime;
        loadingBar.value = loadingTimer/loadingTime;

        if(loadingTimer >= loadingTime)
        {
            
            EventBus<OnBackHome>.Raise(new OnBackHome{});
            this.transform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Loading();
        
    }




}