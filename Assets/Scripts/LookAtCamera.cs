using UnityEngine;


public class LookAtCamera : MonoBehaviour
{
    Camera cam;


    private void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        transform.LookAt(cam.transform);
    }
}
