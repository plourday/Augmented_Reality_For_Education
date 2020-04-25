using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using Debug = System.Diagnostics.Debug;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageAndVideoManager : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private Button dismissButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Text imageTrackedText;
    [SerializeField] private GameObject[] arObjectsToPlace;
    [SerializeField] private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);
    private ARTrackedImageManager _mTrackedImageManager;
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);
        
        _mTrackedImageManager = GetComponent<ARTrackedImageManager>();

        // setup all game objects in dictionary
        foreach (GameObject arObject in arObjectsToPlace)
        {
            GameObject newArObject = Instantiate(arObject, scaleFactor, Quaternion.identity);
            newArObject.name = arObject.name;
            arObjects.Add(arObject.name, newArObject);
        }
    }
    void OnEnable()
    {
        _mTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    void OnDisable()
    {
        _mTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void Dismiss() => welcomePanel.SetActive(false);
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateArImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateArImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateArImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        imageTrackedText.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
    }

    void AssignGameObject(string objName, Vector3 newPosition)
    {
        if(arObjectsToPlace != null)
        {

            GameObject instantiatedArObject = arObjects[objName];
            
            instantiatedArObject.transform.position= newPosition;
            
            if(instantiatedArObject.GetComponentInChildren<VideoPlayer>())
            {
                instantiatedArObject.transform.rotation =new Quaternion(0,0,180,0);
            }

            instantiatedArObject.transform.localScale = scaleFactor;
            instantiatedArObject.SetActive(true);
            //For demo purposes
            /* if(goARObject.name == "demoSphere")
             {
                 goARObject.transform.localScale = new Vector3(.1f,.1f,.1f);
                 goARObject.SetActive(true);
             }
             else
             {
                 goARObject.transform.localScale = scaleFactor;
                 goARObject.SetActive(true);
             }
             */
            playButton.onClick.AddListener(instantiatedArObject.GetComponentInChildren<VideoPlayer>().Play);
          pauseButton.onClick.AddListener(instantiatedArObject.GetComponentInChildren<VideoPlayer>().Pause);
            foreach(GameObject obj in arObjects.Values)
            {
                if(obj.name != name)
                {
                    obj.SetActive(false);
                }
              
            } 
        }
    }
}