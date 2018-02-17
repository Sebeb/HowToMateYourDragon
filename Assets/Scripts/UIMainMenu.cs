using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour {
    public GameObject[] mainMenuButtons;
    public GameObject creditsImg;
    public GameObject howToPlayImg;
    public GameObject backButton;
    public GameObject[] objectsToMove;

    public static bool isPaused = true;
    private float sequenceDur = 6;

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    public Animator anim;

// Use this for initialization
/*void Start()
{
    myLoadedAssetBundle = Resources.LoadAll<Scene>("Scenes");// AssetBundle.LoadFromFile("Assets/Resources/Scenes");
    scenePaths = myLoadedAssetBundle.GetAllScenePaths();
}*/

    public void PlayBtn()
    {

        switchMenu();
        anim.SetBool("shouldStart", true);
        
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(sequenceDur);
        MoveObjectToNewScene.LoadScene("Basic Game", objectsToMove, 0);
        /*
        SceneManager.LoadScene("Basic Game");
        Scene mainGameScene = SceneManager.GetSceneByName("Basic Game");
        // Object.DontDestroyOnLoad(player.gameObject);
        foreach (GameObject go in objectsToMove)
            SceneManager.MoveGameObjectToScene(go, mainGameScene);
            */
    }

    public void HowToPlayBtn()
    {
        switchMenu();
        backButton.SetActive(true);
        howToPlayImg.SetActive(true);
    }

    public void CreditsBtn()
    {
        switchMenu();
        backButton.SetActive(true);
        creditsImg.SetActive(true);
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }

    public void BackBtn()
    {
        switchMenu(true);
        backButton.SetActive(false);
        creditsImg.SetActive(false);
        howToPlayImg.SetActive(false);
    }

    private void switchMenu(bool toMainMenu=false)
    {
        foreach (GameObject btn in mainMenuButtons)
            btn.SetActive(toMainMenu);
    }

    private void StartSequence()
    {

    }
}
