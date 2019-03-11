using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyScript : MonoBehaviour {

    DragonMain dragon;


	void Awake () {
        dragon = GetComponentInParent<DragonMain>();
	}


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Attack>()!=null){
            print("headbutt!");
            DragonMain otherDragon = other.gameObject.GetComponent<DragonMain>();
            if ((dragon.isPlayer && !otherDragon.isPlayer) || (!dragon.isPlayer && otherDragon.isPlayer))
                otherDragon.GetComponent<Attack>().GetHit((int)dragon.baseHornDamage, transform.name);
        }
    }
}
