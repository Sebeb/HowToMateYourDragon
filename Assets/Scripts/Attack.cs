using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject FireballPref;
    private List<GameObject> fireballList = new List<GameObject>();
    public int fireballSpeed;
    public float fireballCd = 1;
    public bool readyToFire;
    public float fireballLifespan = 2;
    DragonMain dragon;
    private bool fireballIsReleased = false;
    private bool fireballInMouth = false;
    public Vector3 mouthPos;
    private Animator anim;
    GameObject currentFireball;

    void Awake () {
        dragon = GetComponent<DragonMain>();
        anim = GetComponent<Animator>();
	}

    public void Fire(bool fireImmediate=false)
    {
        if (readyToFire && !fireballInMouth)
        {
            readyToFire = false;
            GameObject newFireball = new GameObject(transform.name + "_fireball");
            newFireball.transform.parent = transform;
            newFireball.transform.position = transform.position;
            newFireball.transform.rotation = transform.rotation;
            currentFireball = Instantiate(FireballPref, newFireball.transform);
            fireballList.Add(newFireball);
            if (transform.localScale.y >= 0)
                currentFireball.transform.localPosition = mouthPos;
            else
                currentFireball.transform.localPosition = new Vector3(mouthPos.x, - mouthPos.y, 0);
            //currentFireball.transform.localScale = Vector3.one * 0.3f;
            FireballScript newFireballScript = currentFireball.GetComponent<FireballScript>();
            newFireballScript.speed = fireballSpeed
                + 2 * gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
            newFireballScript.lifespan = fireballLifespan;
            newFireballScript.damage = dragon.damage;
            newFireballScript.owner = gameObject;
            newFireballScript.shouldGrow = true;
            newFireballScript.growingTimer = newFireballScript.maxGrowingTime;
            fireballInMouth = true;
            anim.Play("Dragon_Armature|Head_Prepare_Fireball");
            for (int i = 0; i < newFireball.transform.childCount; i++)
                newFireball.transform.GetChild(i).transform.localScale = Vector3.one * 0.05f;
            if (fireImmediate)
                fireballIsReleased = true;
            currentFireball.transform.localScale = Vector3.one * 1.5f;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown(){
        yield return new WaitForSeconds(fireballCd);
        readyToFire = true;
    }

    void Update()
    {
        //if (!UIMainMenu.isPaused)
        //{
            if (dragon.isPlayer && Input.GetButton("Fire1") && readyToFire)
                Fire();
            if (Input.GetButtonUp("Fire1"))
            {
                fireballIsReleased = true;
            }
        //}
    }

    public void GetHit(int incDamage)
    {
        print(gameObject.name + " got hit and took " + incDamage + " damage!");
        anim.Play("Dragon_Armature|Head_Get_Hit");
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0.5f);
        //StartCoroutine(Recover());
        if (currentFireball != null)
            Destroy(currentFireball.transform.parent.gameObject);
        //print(fireballInMouth);
        fireballIsReleased = false;
        fireballInMouth = false;
        dragon.currentHealth -= incDamage;
        if (dragon.isPlayer)
            CameraShaker.Shake(12);
        else
            Game.controller.target = dragon;
        if (dragon.currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (dragon.isPlayer)
            Game.controller.Lose();
        else
            Game.player.AbsorbAttributes(dragon);
        Destroy(gameObject);
    }

    /*IEnumerator Recover()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }*/

    public bool ReleaseFireball()
    {
        if (fireballIsReleased)
        {
            fireballIsReleased = false;
            fireballInMouth = false;
            anim.Play("Dragon_Armature|Head_Shoot_Fireball");
            return false;
        }
        return true;
    }
}
