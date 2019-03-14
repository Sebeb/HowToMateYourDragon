using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;

public class HealthHUD : MonoBehaviour {
    Image image;
    private Image imageBorder;
	public Vector2 offset;

    public bool player;
	// Use this for initialization
	void Awake () {
        image = GetComponent<Image>();
        foreach (Image img in transform.GetComponentsInChildren<Image>())
        {
	        if (img != image)
	        {
		        imageBorder = img;
	        }
        }
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		if (player)
			image.fillAmount = Game.player.currentHealth / (float) Game.player.maxHealth;
		else if (Game.controller.target != null)
		{
			image.enabled = true;
			imageBorder.enabled = true;
			image.fillAmount = (float) Game.controller.target.currentHealth / (float) Game.controller.target.maxHealth;
			transform.position =
				Camera.main.WorldToScreenPoint(Game.controller.target.transform.position + (Vector3) offset);
		}
		else
		{
			image.enabled = false;
			imageBorder.enabled = false;
			image.fillAmount = 0;
		}

	//transform.position = Camera.main.WorldToScreenPoint (Game.player.transform.position + (Vector3)offset);
	}
}
