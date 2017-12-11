using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour {
    private Sprite[] CloudPrefabs;
    public GameObject CloudPref;
    private GameObject[] clouds;
    int cloudAmount = 99;

	// Use this for initialization
	void Start () {
        clouds = new GameObject[cloudAmount];
        CloudPrefabs = Resources.LoadAll<Sprite>("Clouds");
        print(CloudPrefabs.Length);
        for (int i = 0; i < cloudAmount; i++)
        {
            clouds[i] = Instantiate(CloudPref);
            clouds[i].transform.position = new Vector3(Random.Range(-1000, Game.controller.worldSize[0] + 900)
                , Random.Range(-500, Game.controller.worldSize[1] + 500), -10 + 200 * Random.Range(0, 3));
            clouds[i].GetComponent<SpriteRenderer>().sprite = CloudPrefabs[Random.Range(0, CloudPrefabs.Length - 1)];
            clouds[i].transform.parent = transform;
            clouds[i].transform.localScale = Vector3.one * Random.Range(10, 40);
        }
    }
}
