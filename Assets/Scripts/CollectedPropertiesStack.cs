using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedPropertiesStack : MonoBehaviour {

    public Game.Properties property;


    public GameObject token;


    void AddLife()
    {
        GameObject newToken = Instantiate(token, transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
        if (property == Game.Properties.colour)
        {
            if (transform.childCount < Game.player.colourCount)
                AddLife();
            if (transform.childCount > Game.player.colourCount)
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }

        if (property == Game.Properties.horns) {
            if (transform.childCount < Game.player.hornsCount)
                AddLife();
            if (transform.childCount > Game.player.hornsCount)
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }

        if (property == Game.Properties.wings) {
            if (transform.childCount < Game.player.wingsCount)
                AddLife();
            if (transform.childCount > Game.player.wingsCount)
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }

        if (property == Game.Properties.tail) {
            if (transform.childCount < Game.player.tailCount)
                AddLife();
            if (transform.childCount > Game.player.tailCount)
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }

    }

}
