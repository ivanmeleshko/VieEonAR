using UnityEngine;


public class ZoomCameraInterior : MonoBehaviour
{

    Camera cam;
    float minFov = 30f;
    float maxFov = 100f;
    float sensitivity = 50f;
    float damping = 5f;
    float distance = 60f;
    float zoomOutMin = 1;
    float zoomOutMax = 8;
    float zoomSpeed = 0.22f;


    private void Start()
    {
        cam = Camera.main;
        distance = cam.fieldOfView;
    }


    private void LateUpdate()
    {  
        //Pinch zoom
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
            float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

            /*if (cam.orthographic)
            {
                cam.orthographicSize += deltaMagDiff * orthoZoomSpeed;
                cam.orthographicSize = Mathf.Max(cam.orthographicSize, .1f);
            }
            else*/
            cam.fieldOfView += deltaMagDiff * zoomSpeed;// * perspectiveZoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 30f, 100f);
        }
    }

}


