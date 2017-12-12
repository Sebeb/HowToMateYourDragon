using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHealth : MonoBehaviour {

    public GameObject anchor;
    public Vector2 offset;


	void Start () {

	}
	

	void Update () {
        transform.position = anchor.transform.position + (Vector3)offset;
	}
}
