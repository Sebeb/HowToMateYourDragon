using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FireballScript : MonoBehaviour {
    public static GameObject FireballParent;
    public float speed;
    public float lifespan;
    public float damage;
    public bool hasBeenReleased;

    private bool destroyInitiated;
    // Public string owner;
    public GameObject owner;
    private DragonMain ownerScript;
    public float growingTimer;
    public float maxGrowingTime = 1.5f;

    private void Awake()
    {
        
        if (FireballParent == null)
        {
            FireballParent = PhotonNetwork.Instantiate("Fireballs", Vector3.zero, Quaternion.identity);
        }
    }

    // Use this for initialization
	void Start ()
    {
        ownerScript = owner.GetComponent<DragonMain>();
    }
	
    IEnumerator Lifetime(){
        yield return new WaitForSeconds(lifespan);
        DestroyFireball();
        //Photon.Destroy(gameObject);
    }

    private bool IsFireballManager()
    {
        if (owner.name.Contains("NPC")) {
            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            {
                return false;
            }
        }
        else
        {
            //print(ownerScript.gameObject.name + " " + ownerScript.photonView);
            if (ownerScript == null || !ownerScript.photonView.IsMine && PhotonNetwork.IsConnected)
            {
                return false;
            }
        }

        return true;
    }

	void FixedUpdate ()
    {
        if (hasBeenReleased)
        {
            float zRot = (float)(transform.eulerAngles.z / 360f * 2f * Math.PI);
            transform.localPosition += speed * Time.deltaTime * (Vector3.right * (float)Math.Cos(zRot)
                                       + Vector3.up * (float)Math.Sin(zRot));
        } 
        else
        {
            if (!IsFireballManager()) return;
            if (growingTimer > 0)
                {
                growingTimer -= Time.deltaTime;
                //transform.localScale += 5 * Vector3.one * Time.deltaTime / 6;
                //for (int i = 0; i < transform.childCount; i++)
                //    transform.GetChild(i).transform.localScale += Vector3.one * Time.deltaTime / 2000;
                //for (int i = 0; i < transform.GetComponentsInChildren<Particle>().Length; i++)
                //    transform.GetComponentsInChildren<Particle>()[i].size += 5 * Time.deltaTime / 6;
            }
        }
    }

    public void ReleaseFireball()
    {
        StartCoroutine(Lifetime());
        transform.parent = FireballParent.transform;
        hasBeenReleased = true;
        damage *= 0.5f + ((maxGrowingTime - growingTimer) / maxGrowingTime) / 2;
        DragonMovement dm = owner.GetComponent<DragonMovement>();
        //damage *=  dm.velocityMag / dm.terminalVelocity;
        damage *=  owner.GetComponent<Rigidbody2D>().velocity.magnitude / dm.terminalVelocity;
        transform.GetChild(0).gameObject.SetActive(true);
        if (owner.GetComponent<DragonMain>().isPlayer)
            CameraShaker.Shake(3);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (!IsFireballManager()) return;
        if (!hasBeenReleased || other.gameObject == owner) return;
        Attack oAttack = other.gameObject.GetComponent<Attack>();
        if (oAttack != null)
        {
            oAttack.GetHit((int)damage, ownerScript);
            //DestroyFireball();
        }

        
        // todo: 2 fireballs colliding should cause an explosion
        if (other.gameObject.GetComponent<FireballScript>() != null)
        {
            // big explosion
        }
        //Destroy(other.gameObject);
        DestroyFireball();
    }

    public void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
