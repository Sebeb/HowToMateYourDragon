using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {
    public float speed;
    public float lifespan;
    public float damage;
    public bool shouldGrow = true;
    // Public string owner;
    public GameObject owner;
    public float growingTimer;
    public float maxGrowingTime = 1.5f;

	// Use this for initialization
	void Start () {
        
	}
	
    IEnumerator Lifetime(){
        yield return new WaitForSeconds(lifespan);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }

	void FixedUpdate () {
        if (!shouldGrow)
        {
            transform.localPosition += Vector3.right * speed * Time.deltaTime;
            StartCoroutine(Lifetime());
            transform.parent.parent = null;
        }
        else
        {
            if (growingTimer > 0)
            {
                growingTimer -= Time.deltaTime;
                //transform.localScale += 5 * Vector3.one * Time.deltaTime / 6;
                //for (int i = 0; i < transform.childCount; i++)
                //    transform.GetChild(i).transform.localScale += Vector3.one * Time.deltaTime / 2000;
                //for (int i = 0; i < transform.GetComponentsInChildren<Particle>().Length; i++)
                //    transform.GetComponentsInChildren<Particle>()[i].size += 5 * Time.deltaTime / 6;
            }
            shouldGrow = owner.GetComponent<Attack>().ReleaseFireball();
            if (!shouldGrow)
            {
                damage *= 0.5f + ((maxGrowingTime - growingTimer) / maxGrowingTime) / 2;
                DragonMovement DG = owner.GetComponent<DragonMovement>();
                damage *=  DG.velocityMag / DG.terminalVelocity;
                transform.GetChild(0).gameObject.SetActive(true);
                if (owner.GetComponent<DragonMain>().isPlayer)
                    CameraShaker.Shake(3);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (shouldGrow)
            return;
        if (other.gameObject.name == owner.name)
            return;
        if (other.gameObject.GetComponent<Attack>() != null)
        {
            other.gameObject.GetComponent<Attack>().GetHit((int)damage);
            Destroy(gameObject);
        }
        if (other.gameObject.GetComponent<FireballScript>() != null)
        {
            print("Hit another fireball!");
            Destroy(other.gameObject);
            Destroy(gameObject);
            // should cause an explosion
        }
    }
}
