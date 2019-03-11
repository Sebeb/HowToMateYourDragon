using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour {
    Image image;
	public Vector2 offset;

    public bool player;
	// Use this for initialization
	void Awake () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (player)
            image.fillAmount = (float)Game.player.currentHealth / (float)Game.player.maxHealth;
        else if (Game.controller.target != null)
            image.fillAmount = (float)Game.controller.target.currentHealth / (float)Game.controller.target.maxHealth;
        else
            image.fillAmount = 0;
		//transform.position = Camera.main.WorldToScreenPoint (Game.player.transform.position + (Vector3)offset);
	}
}
