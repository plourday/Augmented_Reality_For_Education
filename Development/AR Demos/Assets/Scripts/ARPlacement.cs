using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARPlacement : MonoBehaviour
{
   public GameObject ObjectToSpawn;
   public GameObject placementIndicator;
   private GameObject SpawnedObject;
   private Pose PlacementPose;
   private ARRaycastManager raycastManager;
   private bool placementPoseIsValid = false; 
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        
    }
    void Update()
    {
        if(SpawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceArObject();
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacementPose()
    {
            var ScreenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f,0.5f));
            var hits = new List<ARRaycastHit>();
            raycastManager.Raycast(ScreenCenter, hits, TrackableType.Planes);

            placementPoseIsValid = hits.Count > 0;

            if(placementPoseIsValid)
            {
                PlacementPose = hits[0].pose;
            }
    }
    

    private void UpdatePlacementIndicator()
    {
         if(SpawnedObject == null && placementPoseIsValid)
         {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position,PlacementPose.rotation);
         }
         else
         {
            placementIndicator.SetActive(false);
         }
    }

    void PlaceArObject()
    {
        SpawnedObject = Instantiate(ObjectToSpawn, PlacementPose.position,PlacementPose.rotation);
    }
}
