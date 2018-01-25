using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {

    static MusicPlayer instance = null;
    public AudioClip[] gameMusic;

    void Awake() {
        // Debug.Log("Music Awake: " + GetInstanceID());

        // Check if MusicPlayer exists. If it does, destroy new spawn. If new, make persistent.
        // also check to make sure we don't destroy ourselves
        if (instance != null && instance != this) {
            Destroy(gameObject);
            //Debug.Log("Dupe Music Destruct: " + GetInstanceID());
        } else {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    // Adding (and removing on disable) OnSceneLoaded() as delegate to SceneManager.sceneLoaded
    // alternatively: SceneManager.activeSceneChanged
    // have also confirmed that previous scenes are unloaded when the new one is loaded, so it doesn't really matter at this level
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //this triggers on scene change
    void OnSceneLoaded(Scene s1, LoadSceneMode s2) {
        // play audioclip based on array, using scene index
        GetComponent<AudioSource>().clip = gameMusic[SceneManager.GetActiveScene().buildIndex];
        GetComponent<AudioSource>().Play();
    }
}
