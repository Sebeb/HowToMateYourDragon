using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour {
    Image image;

    public bool player;
	// Use this for initialization
	void Awake () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player)
            image.fillAmount = (float)Game.player.currentHealth / (float)Game.player.maxHealth;
        else if (Game.controller.target != null)
            image.fillAmount = (float)Game.controller.target.currentHealth / (float)Game.controller.target.maxHealth;
        else
            image.fillAmount = 0;
	}
}
