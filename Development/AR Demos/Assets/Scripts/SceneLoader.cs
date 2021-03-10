using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{ 
    [SerializeField] private Button NewSessionButton;
    [SerializeField] private Button SignInButton;

    public void Start()
    {
        NewSessionButton.onClick.AddListener(SceneSwitchARSession);
    }

    public void SceneSwitchARSession()
    {
         SceneManager.LoadScene("ARSession");
    }

    public void SceneSwitchSignIn()
    {
        SceneManager.LoadScene("SignInMenu");
    }

    
   

}
