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

    private ARTrackedImageManager m_TrackedImageManager;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);
     
       
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();

        // setup all game objects in dictionary
        foreach (GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, scaleFactor, Quaternion.identity);
            newARObject.name = arObject.name;
            arObjects.Add(arObject.name, newARObject);
        }
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }





private void Dismiss() => welcomePanel.SetActive(false);

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        imageTrackedText.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if(arObjectsToPlace != null)
        {

            GameObject goARObject = arObjects[name];
            
            goARObject.transform.position= newPosition;
            if(goARObject.GetComponentInChildren<VideoPlayer>())
            {
                goARObject.transform.rotation =new Quaternion(0,0,180,0);
            }

            goARObject.transform.localScale = scaleFactor;
            goARObject.SetActive(true);
            playButton.onClick.AddListener(goARObject.GetComponentInChildren<VideoPlayer>().Play);
          pauseButton.onClick.AddListener(goARObject.GetComponentInChildren<VideoPlayer>().Pause);
            foreach(GameObject go in arObjects.Values)
            {
                if(go.name != name)
                {
                    go.SetActive(false);
                }
              
            } 
        }
    }
}