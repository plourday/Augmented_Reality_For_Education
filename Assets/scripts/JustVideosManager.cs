using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class JustVideosManager : MonoBehaviour
{
    [SerializeField] private GameObject[] placedPrefabs;
    
    [SerializeField] private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);
    private ARTrackedImageManager trackedImageManager;
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
            
        }
    }
}

