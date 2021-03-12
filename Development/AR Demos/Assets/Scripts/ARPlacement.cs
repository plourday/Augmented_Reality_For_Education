using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
public class ARPlacement : MonoBehaviour
{
    [SerializeField] public GameObject objectToSpawn;
    [SerializeField] public GameObject placementIndicator;
    [SerializeField] public Text objectIdentifierName;
    [SerializeField] private GameObject spawnedObject;
    [SerializeField] private Pose placementPose;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private bool placementPoseIsValid = false;
    [SerializeField] private Camera arCamera;

    //Sanity Checks that there is indeed a ARRaycastManager object in the scene.
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();

    }
    //Called every frame
    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (spawnedObject == null)
        {
            updatePlacementPose(hits);
            updatePlacementIndicator(hits);
        }


        //Instantitate computer Object in Real World space if the placementPose Is Valid and the screen has been touched
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            placeArObject();
        }
        displayObjectIdentifierText();
    }

    //Determines whether or not the camera is looking at a horizontal plane. If so, placement pose is valid.
    private void updatePlacementPose(List<ARRaycastHit> hits)
    {
        Vector3 screenCenter = getScreenCenter();

        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }

    //sets the placement indicator to active if the placement pose is valid.
    private void updatePlacementIndicator(List<ARRaycastHit> hits)
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    //Displays the name of the object that the camera is looking at.
    private void displayObjectIdentifierText()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    PlacedObject placedObject = hitObject.transform.GetComponent<PlacedObject>();
                    if(placedObject != null)
                    {
                        objectIdentifierName.text = placedObject.name.ToString();
                    }
                }
            }
        }
    }
    public Vector3 getScreenCenter()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        return screenCenter;
    }
    //Instantiates the object to be spawned in world space.
    void placeArObject()
    {
        spawnedObject = Instantiate(objectToSpawn, placementPose.position, placementPose.rotation);
    }
}