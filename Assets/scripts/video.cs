using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class video : MonoBehaviour
{
    public RawImage image;
 
    public VideoClip videoToPlay;
 
    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
 
    private AudioSource audioSource;
    // Start is
    // called before the first frame update
    
    void Start()
    { Application.runInBackground = true;
        StartCoroutine(playVideo());
        IEnumerator playVideo()
        {
            //Add VideoPlayer to the GameObject
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
 
            //Add AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();
 
            //Disable Play on Awake for both Video and Audio
            videoPlayer.playOnAwake = false;
            audioSource.playOnAwake = false;
            audioSource.Pause();
 
            //We want to play from video clip not from url
        
            videoPlayer.source = VideoSource.VideoClip;
 
            // Video clip from Url
            //videoPlayer.source = VideoSource.Url;
            videoPlayer.url = "Assets/images/videos/Casablanca - Rick's play it Sam.mp4";
 
 
            //Set Audio Output to AudioSource
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
 
            //Assign the Audio from Video to AudioSource to be played
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.SetTargetAudioSource(0, audioSource);
 
            //Set video To Play then prepare Audio to prevent Buffering
            videoPlayer.clip = videoToPlay;
            videoPlayer.Prepare();
            WaitForSeconds waitTime = new WaitForSeconds(1);
            while (!videoPlayer.isPrepared)
            {
                Debug.Log("Preparing Video");
                //Prepare/Wait for 5 sceonds only
                yield return waitTime;
                //Break out of the while loop after 5 seconds wait
                break;
            }
            image.texture = videoPlayer.texture;
            videoPlayer.Play();
            audioSource.Play();
            while (videoPlayer.isPlaying)
            {
                Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
