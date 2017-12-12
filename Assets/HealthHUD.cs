using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour {
    Image image;
	public Vector2 offset;
	public DragonMain target;

	void Awake () {
        image = GetComponent<Image>();
	}
	

	void Update () {
		if (target != null)
			image.fillAmount = (float)target.currentHealth / (float)target.maxHealth;
		else
			Destroy (gameObject);
	}

	void LateUpdate(){
		transform.position = Camera.main.WorldToScreenPoint (target.lifeAnchor.transform.position + (Vector3)offset);
	}
}
