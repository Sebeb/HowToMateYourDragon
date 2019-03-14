using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSkip : MonoBehaviour {

    public VideoPlayer video;
    public bool goToNext;
    public int scene;
	// Use this for initialization
	void Start () {
        StartCoroutine(waitTillEnd());
	}
	
    public void Update(){
        if (Input.GetKeyDown(KeyCode.Space))
            Skip();
    }

    IEnumerator waitTillEnd(){
        yield return new WaitForSeconds((float)video.clip.length);
        Skip();
    }

    void Skip(){       
        if (goToNext)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
        SceneManager.LoadScene(scene);
        }

}
