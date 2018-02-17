using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    public bool mouseControl;
    public Plane zPlane;
    public Vector2 worldSize;
    public int gameStartTime;
    public int secondsRemaining;
    public int gameLengthSeconds;
    public static Game controller;
    public static DragonMain player;
    public DragonMain thePlayer;
    public Color[] dragonColours;
    public DragonMain target;

    public enum Elements { air, fire, rock, water };
    public enum Properties { horns, wings, tail, colour };
    public Elements idealHorns, idealWings, idealTail;
    public Elements hatesHorns, hatesWings, hatesTail;
    public enum Colour { blue, green, red, yellow };
    public Colour idealColour, hatesColour;
    public List<GameObject> spawnedDragons;
    public int dragonsPerLevel;
    public GameObject enemy;

    public List<int> highscores;
    public static float newSceneTimer = -20;

	void Awake () {
        controller = this;
        zPlane = new Plane(new Vector3(1, 0, 0), new Vector3(0, 1, 0), Vector3.zero);
        player = thePlayer;
        player.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

    }

    void Start()
    {
        InitialiseGame();
        gameStartTime = (int)Time.unscaledTime;
    }

    public void InitialiseGame(){
        RandomiseIdealDragon();
        for (int i = 0; i < dragonsPerLevel; i++)
        {
            SpawnDragon(false,Properties.tail);
        }
    }

    void RandomiseIdealDragon(){
        idealHorns = (Elements)Random.Range(0, 3);
        idealWings = (Elements)Random.Range(0, 3);
        idealTail = (Elements)Random.Range(0, 3);
        idealColour = (Colour)Random.Range(0, 3);
        hatesHorns = (Elements)Random.Range(0, 3);
        while(hatesHorns == idealHorns) {
            hatesHorns = (Elements)Random.Range(0, 3);
        }
        hatesWings = (Elements)Random.Range(0, 3);
        while (hatesWings == idealWings) {
            hatesWings = (Elements)Random.Range(0, 3);
        }
        hatesTail = (Elements)Random.Range(0, 3);
        while (hatesTail == idealHorns) {
            hatesTail = (Elements)Random.Range(0, 3);
        }
        hatesColour = (Colour)Random.Range(0, 3);
        while (hatesColour == idealColour) {
            hatesColour = (Colour)Random.Range(0, 3);
        }
    }

    void SpawnDragon(bool setProperty, Properties set){
        Vector2 spawnPosition = new Vector2(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y));
        int spawnSide = Random.Range(1, 5);
        switch (spawnSide){
            case 1: spawnPosition.y = worldSize.y;
                break;
            case 2:
                spawnPosition.y = -worldSize.y;
                break;
            case 3:
                spawnPosition.x = worldSize.x;
                break;
            case 4:
                spawnPosition.x = -worldSize.x;
                break;

            }
        GameObject newDragon = Instantiate(enemy, spawnPosition, Quaternion.identity);
        spawnedDragons.Add(newDragon);
        if (setProperty){
            switch (set)
            {
                case Properties.horns:
                    newDragon.GetComponent<DragonMain>().currentHorns = idealHorns;
                    break;
                case Properties.wings:
                    newDragon.GetComponent<DragonMain>().currentWings = idealWings;
                    break;
                case Properties.tail:
                    newDragon.GetComponent<DragonMain>().currentTail = idealTail;
                    break;
                case Properties.colour:
                    newDragon.GetComponent<DragonMain>().currentColour = idealColour;
                    break;
            }
        }
    }

    public void StartGame(){
        
    }

    //public void EndGame(){
    //    player.
    //}

    public void Lose(){
        SceneManager.LoadScene(3);
    }

    public void Win(){
        SceneManager.LoadScene(2);
    }

    void Update(){
        if (Input.mousePosition.x > 0 && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height && Input.mousePosition.x < Screen.width)
        {
            if (Input.GetMouseButtonDown(0))
                mouseControl = true;
        }
        else
            mouseControl = false;
        secondsRemaining = Mathf.Clamp(gameLengthSeconds - ((int)Time.unscaledTime) - gameStartTime,0,gameLengthSeconds);
        //if (secondsRemaining == 0)
        //    EndGame();
        if (newSceneTimer > 0)
        {
            newSceneTimer -= Time.deltaTime;
        }
        if (newSceneTimer <= 0 && newSceneTimer > -10)
        {
            newSceneTimer = -20;
            MoveObjectToNewScene.GoToLoadedScene();
            Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
            playerRb.isKinematic = false;
            player.transform.position = Vector3.zero;
            player.transform.eulerAngles.Set(0, 0, 0);
        }

    }

    public Vector3 ScreenToZ(Vector3 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        float distance;
        zPlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

}
