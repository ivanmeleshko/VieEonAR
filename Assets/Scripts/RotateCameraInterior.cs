using UnityEngine;


public class RotateCameraInterior : MonoBehaviour
{

    [SerializeField]
    Camera cam;
    [SerializeField]
    float speedH = 3.0f;
    [SerializeField]
    float speedV = 3.0f;
    [SerializeField]
    float damping = 5f;

    private float yaw = 30.0f;
    private float pitch = 10.0f;
    private bool rotationDisabled = true;
    private float minRotX = -90, maxRotX = 90;
    private float xAngle = -22.0f;
    private float yAngle = 10.0F;
    private float xAngTemp = 0.0f;
    private float yAngTemp = 0.0f;
    private Vector3 firstpoint = new Vector3(0f, 1f, -10f);
    private Vector3 secondpoint = new Vector3(0f, 1f, -10f);


    void Start()
    {
        switch (Settings.instance.currentScene)
        {
            case "Reception1Scene":
                switch (Settings.instance.previousScene)
                {
                    case "Reception2Scene":
                        SetAngles(120, 30);
                        break;
                    case "Roof1Scene":
                        SetAngles(0, 10);                    
                        break;
                }
                break;
            case "Reception2Scene":
                switch (Settings.instance.previousScene)
                {
                    case "VestibulScene":
                        SetAngles(-90, 10);
                        break;
                    case "Reception1Scene":
                        SetAngles(30, 0);
                        break;
                    case "Reception3Scene":
                        SetAngles(90, 10);
                        break;
                }
                break;
            
            case "Reception3Scene":
                SetAngles(180, 10);
                break;
            case "VestibulScene":
                switch (Settings.instance.previousScene)
                {
                    case "ExtFront1Scene":
                        SetAngles(0, 10);
                        break;
                    case "Reception2Scene":
                        SetAngles(180, 10);                     
                        break;
                }
                break;
            case "RoofLivingScene":
                switch (Settings.instance.previousScene)
                {
                    case "Reception1Scene":
                    case "Reception2Scene":
                        SetAngles(0, 10);                    
                        break;
                    case "Roof1Scene":
                        SetAngles(180, 10);
                        break;
                }
                break;
            case "Roof1Scene":
                SetAngles(-90, 10);           
                break;
            case "KitcheningScene":
                SetAngles(0, 10);             
                break;
            case "ExtFront1Scene":
                SetAngles(0, 10);   
                break;
            case "Garden2Scene":
                SetAngles(-90, 10);     
                break;
            case "StreetViewScene":
                SetAngles(0, 10);         
                break;
            case "Garden1Scene":
                SetAngles(0, 10);         
               break;
        }  
    }


    void LateUpdate()
    {
        //Check count touches
        if (Input.touchCount == 1)
        {
            //Touch began, save position
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                yAngle = Mathf.Clamp(yAngTemp + (secondpoint.y - firstpoint.y) * 90.0f / Screen.height, minRotX, maxRotX);
                //Rotate camera
                //transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
                transform.eulerAngles = new Vector3(yAngle, xAngle, 0.0f);
            }
        }
    }


    private void SetAngles(float x, float y)
    {
        xAngle = x;
        yAngle = y;
        cam.transform.eulerAngles = new Vector3(yAngle, xAngle, 0.0f);
    }

}