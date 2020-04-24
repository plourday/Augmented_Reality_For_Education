using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class leonardNimoy : MonoBehaviour
{
    [SerializeField] private GameObject[] placedPrefabs;
    
    [SerializeField] private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);
    private ARTrackedImageManager trackedImageManager;
    private String sceneName;
    void Start()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        trackedImageManager.trackedImagePrefab = placedPrefabs[0];
    }




    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        
        foreach (var var in eventArgs.added)
        {
            if (var.referenceImage.name == "playitsam")
            {
                sceneName = "playitsam";
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}