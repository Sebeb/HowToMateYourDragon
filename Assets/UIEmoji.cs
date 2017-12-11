using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEmoji : MonoBehaviour {

    Image sprite;
    public Sprite[] sprites;
    public Game.Properties property;

    void Awake(){
        sprite = GetComponent<Image>();
    }

	void Update () {
        if(property == Game.Properties.colour){
            if (Game.controller.idealColour == Game.player.currentColour)
                sprite.sprite = sprites[2];
            else if (Game.controller.hatesColour == Game.player.currentColour)
                sprite.sprite = sprites[0];
            else
                sprite.sprite = sprites[1];
        }
        if (property == Game.Properties.horns) {
            if (Game.controller.idealHorns == Game.player.currentHorns)
                sprite.sprite = sprites[2];
            else if (Game.controller.hatesHorns == Game.player.currentHorns)
                sprite.sprite = sprites[0];
            else
                sprite.sprite = sprites[1];
        }
        if (property == Game.Properties.wings) {
            if (Game.controller.idealWings == Game.player.currentWings)
                sprite.sprite = sprites[2];
            else if (Game.controller.hatesWings == Game.player.currentWings)
                sprite.sprite = sprites[0];
            else
                sprite.sprite = sprites[1];
        }
        if (property == Game.Properties.tail) {
            if (Game.controller.idealTail == Game.player.currentTail)
                sprite.sprite = sprites[2];
            else if (Game.controller.hatesTail == Game.player.currentTail)
                sprite.sprite = sprites[0];
            else
                sprite.sprite = sprites[1];
        }
    }
}
