using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceMultipleObjectsOnPlane : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    [SerializeField]
    List<GameObject> Popups;

    public GameObject SpawnedObject { get; private set; }
    public static event Action OnPlacedObject;
    ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    Vector3 firstpoint;    //change type on Vector3
    Vector3 secondpoint;
    float xAngle = 0.0f;   //angle for axes x for rotation
    float xAngTemp = 0.0f; //temp variable for angle
    

    public GameObject PlacedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }


    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (SpawnedObject == null && m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;

                    SpawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                    OnPlacedObject?.Invoke();
                }
            }
        }
    }


    void LateUpdate()
    {
        if (SpawnedObject != null)
        {
            if (!UI.IsPointerOverUIObject())
            {
                xAngle = SpawnedObject.transform.eulerAngles.y;

                //Check count touches
                if (Input.touchCount == 1)
                {
                    //Touch began, save position
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        firstpoint = Input.GetTouch(0).position;
                        xAngTemp = xAngle;
                    }
                    //Move finger
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        secondpoint = Input.GetTouch(0).position;
                        xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                        SpawnedObject.transform.localRotation = Quaternion.AngleAxis(xAngle, Vector3.up);
                    }
                }

                //Pinch zoom
                if (Input.touchCount == 2 && !PopupActive())
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
                    float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                    float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

                    if (deltaMagDiff > 0)
                    {
                        if (SpawnedObject.transform.localScale.x > 0.0029f)
                            SpawnedObject.transform.localScale = SpawnedObject.transform.localScale / 1.05f;
                    }
                    else
                    {
                        if (SpawnedObject.transform.localScale.x < 0.12f)
                            SpawnedObject.transform.localScale = SpawnedObject.transform.localScale * 1.05f;
                    }
                }
            }
        }
    }


    private bool PopupActive()
    {
        foreach (GameObject g in Popups)
        {
            if (g.active)
            {
                return true;
            }
        }
        return false;
    }


    public void RemoveAllAnchors()
    {
        if (SpawnedObject != null)
        {
            Destroy(SpawnedObject);
            SpawnedObject = null;
        }   
    }
}
