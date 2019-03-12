using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{   
    public static Game controller;
    public static DragonMain player;
    public static Rigidbody2D rb;
    public static Animator anim;
    public static GameObject FireballParent;
    public GameObject PlayerPref;
    private static string playerDataPath = "Assets/GameData/";
    private static string playerDataName = "PlayerData.txt";
    
    public bool mouseControl;
    public Plane zPlane;
    public Vector2 worldSize;
    public int gameStartTime;
    public int secondsRemaining;
    public int gameLengthSeconds;
    //public DragonMain thePlayer;
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
    public static string status = "MainMenu";
    public GameObject enemies;

	void Awake () {
        print("awake");
        controller = this;
        
        zPlane = new Plane(new Vector3(1, 0, 0), new Vector3(0, 1, 0), Vector3.zero);

        // todo: just call when in game and if you are the master
        RandomiseIdealDragon(); // should be in InitialiseGamel, but Im not sure if that works
        
        // make sure the player gets Instantiated by MonoBehaviour when of a room or else by Photon.Instantiate
        if (PhotonNetwork.InRoom)
        {
            player = PhotonNetwork.Instantiate(PlayerPref.name, Vector2.zero, Quaternion.identity)
                .GetComponent<DragonMain>();
        }
        else
        {
            player = Instantiate(PlayerPref, Vector2.zero, Quaternion.identity).GetComponent<DragonMain>();
        }

        string playerData;
        if (!Directory.Exists(playerDataPath))
        {
            Directory.CreateDirectory(playerDataPath);
        }
        if (!File.Exists(playerDataPath + playerDataName))
        {
            print("Randomizing player");
            player.GetAttributes();
            playerData = JsonUtility.ToJson(player);
            StreamWriter writer = new StreamWriter(playerDataPath + playerDataName, false);
            writer.WriteLine(playerData);
            writer.Close();
        }
        else
        {
            int maxHealth = player.maxHealth;
            StreamReader reader = new StreamReader(playerDataPath + playerDataName);
            playerData = reader.ReadToEnd();
            reader.Close();
            print("Applying jason data");
            JsonUtility.FromJsonOverwrite(playerData, player);
            player.maxHealth = maxHealth;
            player.currentHealth = maxHealth;
            player.UpdateAttributes();
        }
        
        rb = player.gameObject.GetComponent<Rigidbody2D>();
        anim = player.gameObject.GetComponent<Animator>();
        
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ActivatePlayer(false);
        } else if (SceneManager.GetActiveScene().name == "Basic Game")
        {
            if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
            {
                // Load Dragon Settings for Basic Game
                for (int i = 0; i < dragonsPerLevel; i++)
                {
                    SpawnDragon(false, Properties.tail, true);
                }
            }
            ActivatePlayer(true);
        }
    }

    void Start()
    {
        print("start");
        rb.velocity = Vector2.zero;
        rb.rotation = 0;
        rb.angularVelocity = 0;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Load Dragon Settings for MainMenu
            rb.isKinematic = true;
            anim.SetBool("gameStarted", false);
            anim.SetBool("shouldStart", false);
            for (int i = 0; i < 4; i++)
            {
                anim.Play("Runestone_Idle", i);
            }

            player.transform.position = new Vector3(-400, 77, 2.4f);
            player.transform.eulerAngles = Vector3.up * 180;
            player.transform.localScale = Vector3.one * 100;
            status = "MainMenu";
        } else if (SceneManager.GetActiveScene().name == "Basic Game")
        {
            InitialiseGame();
        }
        else
        {
            print("Warning: There might be an error, e.g. because a scene has been renamed or a new scene has " +
                  "been created with this script. Scene Name: " + SceneManager.GetActiveScene().name);
        }
    }

    private void ActivatePlayer(bool active)
    {
        player.GetComponent<TrailRenderer>().enabled = active;
        player.GetComponent<Attack>().enabled = active;
        player.GetComponent<DragonMovement>().enabled = active;
        //player.GetComponent<DragonMain>().enabled = active;
        player.GetComponent<CapsuleCollider2D>().enabled = active;
        player.GetComponent<PhotonTransformView>().enabled = active;
        player.GetComponent<PhotonAnimatorView>().enabled = active;
    }

    public void InitialiseGame(){
        rb.isKinematic = false;
        anim.SetBool("gameStarted", true);
        player.transform.position = Vector3.zero;
        player.transform.eulerAngles = Vector3.zero;
        player.transform.localScale = new Vector3(-4, 4, -4); // Vector3.one * 4;
        status = "Basic Game";
        foreach (GameObject go in spawnedDragons)
        {
            //go.SetActive(true);
            //go.GetComponent<Animator>().SetBool("gameStarted", true);
        }
        gameStartTime = (int)Time.unscaledTime;
        if (FireballParent == null)
        {
            FireballParent = new GameObject("Fireballs");
        }
    }

    void RandomiseIdealDragon(){
        idealHorns = (Elements)Random.Range(0, 4);
        idealWings = (Elements)Random.Range(0, 4);
        idealTail = (Elements)Random.Range(0, 4);
        idealColour = (Colour)Random.Range(0, 4);
        hatesHorns = (Elements)Random.Range(0, 4);
        while(hatesHorns == idealHorns) {
            hatesHorns = (Elements)Random.Range(0, 4);
        }
        hatesWings = (Elements)Random.Range(0, 4);
        while (hatesWings == idealWings) {
            hatesWings = (Elements)Random.Range(0, 4);
        }
        hatesTail = (Elements)Random.Range(0, 4);
        while (hatesTail == idealHorns) {
            hatesTail = (Elements)Random.Range(0, 4);
        }
        hatesColour = (Colour)Random.Range(0, 4);
        while (hatesColour == idealColour) {
            hatesColour = (Colour)Random.Range(0, 4);
        }
    }

    void SpawnDragon(bool setProperty, Properties set, bool activate){
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
        GameObject newDragon = PhotonNetwork.Instantiate("Enemy", spawnPosition, Quaternion.identity);
        spawnedDragons.Add(newDragon);
        newDragon.transform.parent = enemies.transform;
        newDragon.name = "EnemyNPC";
        newDragon.GetComponent<DragonMain>().GetAttributes();
        newDragon.SetActive(activate);
        newDragon.GetComponent<Animator>().SetBool("gameStarted", true);
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
        
        /*if (MoveObjectToNewScene.shouldLoadNewScene) {
            if (MoveObjectToNewScene.targetSceneName.Equals("Basic Game"))
            {
                InitialiseGame();
            }
        }*/

    }

    public Vector3 ScreenToZ(Vector3 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        float distance;
        zPlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    
    /*static void CreatePrefab(GameObject go)
    {
        //Set the path as within the Assets folder, and name it as the GameObject's name with the .prefab format
        string prefPath = "Assets/Resources/" + prefabName + ".prefab";
        
        //Create a new Prefab at the path given
        Object prefab = PrefabUtility.SaveAsPrefabAsset(go, prefPath);
        //PrefabUtility.SaveAsPrefabAssetAndConnect(go, prefPath, InteractionMode.UserAction);
    }*/
}
