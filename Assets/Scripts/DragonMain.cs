using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable] public class DragonMain : MonoBehaviourPunCallbacks {

    [Header("Stats")]
    [HideInInspector]

    public int maxHealth;
    [NonSerialized]public int currentHealth = 100;
    [NonSerialized]public int damage = 40;
    [NonSerialized]public float boostEnergy = 100;
    [NonSerialized]public float relativeBoostSpeed = 1;
    [NonSerialized]public float relativeSpeed = 1f;
    [NonSerialized]public float dexterity = 1;
    [NonSerialized]public float baseHornDamage = 1;

    public bool isPlayer;

    public bool randomise;

    public Game.Elements currentHorns;
    [NonSerialized]public Game.Elements storedHorns;
    [NonSerialized]public int hornsCount;
    public Game.Elements currentWings;
    [NonSerialized]public Game.Elements storedWings;
    [NonSerialized]public int wingsCount;
    public Game.Elements currentTail;
    [NonSerialized]public Game.Elements storedTail;
    [NonSerialized]public int tailCount;

    [NonSerialized]public SkinnedMeshRenderer bodyMesh;
    [NonSerialized]public SkinnedMeshRenderer hornsMesh;
    [NonSerialized]public SkinnedMeshRenderer wingsMesh;
    [NonSerialized]public SkinnedMeshRenderer tailMesh;

    public Game.Colour currentColour;
    [NonSerialized]public Game.Colour storedColour;
    [NonSerialized]public int colourCount;
    [NonSerialized]private Dictionary<string, Texture> allTextures;

    [NonSerialized]Animator anim;

    private void LoadTextures()
    {
        allTextures = new Dictionary<string, Texture>();
        foreach (Texture tex in Resources.LoadAll<Texture>("Dragon Textures"))
        {
            allTextures.Add(tex.name, tex);
        }
    }

    void Start(){
        anim = GetComponent<Animator>();
        maxHealth = currentHealth;
    }

    public void GetAttributes()
    {
        if (randomise)
        {
            currentHorns = (Game.Elements)Random.Range(0, 4);
            currentWings = (Game.Elements)Random.Range(0, 4);
            currentTail = (Game.Elements)Random.Range(0, 4);
            currentColour = (Game.Colour)Random.Range(0, 4);
        }
        UpdateAttributes();
    }

    public void AbsorbAttributes(DragonMain other)
    {
        if (currentHorns != other.currentHorns)
        {
            if (storedHorns == other.currentHorns)
            {
                hornsCount++;
                if (hornsCount == 3){
                    currentHorns = storedHorns;
                    hornsCount = 0;
                }
            }
            else
            {
                storedHorns = other.currentHorns;
                hornsCount = 1;
            }
        }
        if (currentWings != other.currentWings)
        {
            if (storedWings == other.currentWings)
            {
                wingsCount++;
                if (wingsCount == 3)
                {
                    currentWings = storedWings;
                    wingsCount = 0;
                }
            }
            else
            {
                storedWings = other.currentWings;
                wingsCount = 1;
            }
        }
        if (currentTail != other.currentTail)
        {
            if (storedTail == other.currentTail)
            {
                tailCount++;
                if (tailCount == 3)
                {
                    currentTail = storedTail;
                    tailCount = 0;
                }
            }
            else
            {
                storedTail = other.currentTail;
                tailCount = 1;
            }
        }
        if (currentColour != other.currentColour)
        {
            if (storedColour == other.currentColour)
            {
                colourCount++;
                if (colourCount == 3)
                {
                    currentColour = storedColour;
                    colourCount = 0;
                }
            }
            else
            {
                storedColour = other.currentColour;
                colourCount = 1;
            }
        }
        UpdateAttributes();
    }

    private void GetMeshRenderers()
    {
        // get the mesh renderers, if not already known
        if (bodyMesh == null)
        {
            foreach (SkinnedMeshRenderer meshRenderer in transform.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                switch (meshRenderer.gameObject.name)
                {
                    case ("Body"):
                        bodyMesh = meshRenderer;
                        break;
                    case ("Horns"):
                        hornsMesh = meshRenderer;
                        break;
                    case ("Tail"):
                        tailMesh = meshRenderer;
                        break;
                    case ("Wings"):
                        wingsMesh = meshRenderer;
                        break;
                }
            }
        }
    }

    public void UpdateAttributes()
    {
        GetMeshRenderers();
        SetTexture(hornsMesh, currentHorns);
        SetTexture(wingsMesh, currentWings);
        SetTexture(tailMesh, currentTail);
        SetTexture(bodyMesh, 0);
        if (GetComponent<TrailRenderer>() != null && Game.controller != null)
            GetComponent<TrailRenderer>().startColor = Game.controller.dragonColours[(int)currentColour];
    }

    void SetTexture(SkinnedMeshRenderer mesh, Game.Elements element){
        if (allTextures == null)
        {
            LoadTextures();
        }
        mesh.material.SetTexture("_MainTex",allTextures["Dragon " + element + " " + currentColour]);
    }
}

