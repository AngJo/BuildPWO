using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour {

    [SerializeField]
    private int scene;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object Entered the trigger");

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
    }


    
}
