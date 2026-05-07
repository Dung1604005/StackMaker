using UnityEngine;

public class CameraEffect: MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float zoomSpeed;

    private bool isInEffect;

    [SerializeField]private float targetFieldValue;

    


    public void SetZoomField(float value)
    {
        targetFieldValue = value;
        isInEffect = true;
    }

    void Update()
    {
        if(!isInEffect)return;

        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFieldValue, zoomSpeed*Time.deltaTime);

        if(Mathf.Abs(_camera.fieldOfView - targetFieldValue) <= 0.01f)
        {
            isInEffect= false;
        }
        
    }
    
}
