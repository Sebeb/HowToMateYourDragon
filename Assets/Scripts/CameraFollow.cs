using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public Vector2 bounds;
	public int zoomMin, zoomMax;

	public float targetZoom, smoothedZoom, zoomSpeed, smoothSpeed;

	void Start () {
		targetZoom = transform.position.z;
		smoothedZoom = targetZoom;
	}
	
	// Update is called once per frame
	void Update () {
		targetZoom = Mathf.Clamp (targetZoom + (Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime), zoomMin, zoomMax);
		smoothedZoom = Mathf.MoveTowards (smoothedZoom, targetZoom, Time.deltaTime*smoothSpeed);

		Vector3 goal = new Vector3 (Mathf.Clamp (target.transform.position.x, -Game.controller.worldSize.x + bounds.x, Game.controller.worldSize.x - bounds.x), Mathf.Clamp (target.transform.position.y, -Game.controller.worldSize.y + bounds.y, Game.controller.worldSize.y - bounds.y),smoothedZoom);

        transform.position = goal;
		
	}
}
