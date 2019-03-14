using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedPropertyToken : MonoBehaviour {

    public Sprite[] sprites;
    Image sprite;

    void Awake(){
        sprite = GetComponent<Image>();
    }

	void Update () {
        CollectedPropertiesStack stack = GetComponentInParent<CollectedPropertiesStack>();
        if (stack.property == Game.Properties.horns)
            sprite.sprite = sprites[(int)Game.player.storedHorns];
        if (stack.property == Game.Properties.wings)
            sprite.sprite = sprites[(int)Game.player.storedWings];
        if (stack.property == Game.Properties.tail)
            sprite.sprite = sprites[(int)Game.player.storedTail];
        if (stack.property == Game.Properties.colour)
        {
            sprite.sprite = sprites[4];
            sprite.color = Game.controller.dragonColours[(int)Game.player.storedColour];
        }
        else
            sprite.color = Color.white;


        }
		
	}

