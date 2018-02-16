using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMain : MonoBehaviour {

    [Header("Stats")]
    [HideInInspector]

    public int maxHealth;
    public int currentHealth = 100;
    public int damage = 40;
    public float boostEnergy = 100;
    public float relativeBoostSpeed = 1;
    public float relativeSpeed = 1;
    public float dexterity = 1;
    public float baseHornDamage = 1;

    public bool isPlayer;

    public bool randomise;

    public Game.Elements currentHorns;
    public Game.Elements storedHorns;
    public int hornsCount;
    public Game.Elements currentWings;
    public Game.Elements storedWings;
    public int wingsCount;
    public Game.Elements currentTail;
    public Game.Elements storedTail;
    public int tailCount;

    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer hornsMesh;
    public SkinnedMeshRenderer wingsMesh;
    public SkinnedMeshRenderer tailMesh;

    public Game.Colour currentColour, storedColour;
    public int colourCount;
    public Texture[] blueTextures;
    public Texture[] greenTextures;
    public Texture[] redTextures;
    public Texture[] yellowTextures;

    Animator anim;


    void Start(){
        anim = GetComponent<Animator>();
        maxHealth = currentHealth;
        if (randomise)
        {
            currentHorns = (Game.Elements)Random.Range(0, 3);
            currentWings = (Game.Elements)Random.Range(0, 3);
            currentTail = (Game.Elements)Random.Range(0, 3);
            currentColour = (Game.Colour)Random.Range(0, 3);
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
                if (hornsCount == 3)
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

    }

    void UpdateAttributes()
    {
        switch (currentHorns)
        {
            case (Game.Elements.air):
                SetTexture(hornsMesh, Game.Elements.air);
                break;
            case (Game.Elements.fire):
                SetTexture(hornsMesh, Game.Elements.fire);
                break;
            case (Game.Elements.rock):
                SetTexture(hornsMesh, Game.Elements.rock);
                break;
            case (Game.Elements.water):
                SetTexture(hornsMesh, Game.Elements.water);
                break;
        }

        switch (currentWings)
        {
            case (Game.Elements.air):
                SetTexture(wingsMesh, Game.Elements.air);
                break;
            case (Game.Elements.fire):
                SetTexture(wingsMesh, Game.Elements.fire);
                break;
            case (Game.Elements.rock):
                SetTexture(wingsMesh, Game.Elements.rock);
                break;
            case (Game.Elements.water):
                SetTexture(wingsMesh, Game.Elements.water);
                break;
        }
        switch (currentTail)
        {
            case (Game.Elements.air):
                SetTexture(tailMesh, Game.Elements.air);
                break;
            case (Game.Elements.fire):
                SetTexture(tailMesh, Game.Elements.fire);
                break;
            case (Game.Elements.rock):
                SetTexture(tailMesh, Game.Elements.rock);
                break;
            case (Game.Elements.water):
                SetTexture(tailMesh, Game.Elements.water);
                break;
        }
        SetTexture(bodyMesh, 0);
        GetComponent<TrailRenderer>().startColor = Game.controller.dragonColours[(int)currentColour];
    }

    void SetTexture(SkinnedMeshRenderer mesh, Game.Elements element){
        if (currentColour == Game.Colour.blue)
            mesh.material.SetTexture("_MainTex",blueTextures[(int)element]);
        if (currentColour == Game.Colour.green)
            mesh.material.SetTexture("_MainTex", greenTextures[(int)element]);
        if (currentColour == Game.Colour.red)
            mesh.material.SetTexture("_MainTex", redTextures[(int)element]);
        if (currentColour == Game.Colour.yellow)
            mesh.material.SetTexture("_MainTex", yellowTextures[(int)element]);

    }
}

