using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    private Sprite[] CloudPrefabs;
    public GameObject CloudPref;
    private GameObject[] clouds;
    public float[] spawnArea;
    public int layers;
    public int firstLayerZ;
    public int layerDist = 200;
    int cloudAmount = 99;
    int spriteOrder = 10;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 2; i++)
            if (spawnArea[i] == -1)
                spawnArea[i] = Game.controller.worldSize[i];

        clouds = new GameObject[cloudAmount];
        CloudPrefabs = Resources.LoadAll<Sprite>("Clouds");
        for (int i = 0; i < cloudAmount; i++)
        {
            clouds[i] = Instantiate(CloudPref);
            clouds[i].transform.position = new Vector3(Random.Range(-1000, spawnArea[0] + 900)
                , Random.Range(-500, spawnArea[1] + 500), firstLayerZ + layerDist * Random.Range(0, layers));
            clouds[i].GetComponent<SpriteRenderer>().sprite = CloudPrefabs[Random.Range(0, CloudPrefabs.Length - 1)];
            clouds[i].GetComponent<SpriteRenderer>().sortingOrder = spriteOrder;
            //clouds[i].GetComponent<SpriteRenderer>().ord
            clouds[i].transform.parent = transform;
            clouds[i].transform.localScale = Vector3.one * Random.Range(10, 40);
        }
    }
}

