using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Attack : MonoBehaviour, IPunObservable {
    public GameObject FireballPref;
    public List<FireballScript> fireballList = new List<FireballScript>();
    private int fireballSpeed = 3;
    public float fireballCd = 1;
    private bool readyToFire = true;
    public float fireballLifespan = 1.5f;
    private DragonMain dragon;
    // todo: wrong value when looking to left side
    private Vector3 mouthPos = new Vector3(-2, 0.3f, 0);
    private Animator anim;
    private FireballScript currentFireball;
    public bool shouldFire;
    public bool shouldRelease;
    

    void Awake () {
        dragon = GetComponent<DragonMain>();
        anim = GetComponent<Animator>();
	}
    
    #region ShootFireball

    private void Fire(bool fireImmediate=false)
    {
        if (!readyToFire || dragon.fireballInMouth) return;
        readyToFire = false;
        GameObject newFireball = Instantiate(FireballPref, transform);
        //GameObject newFireball = new GameObject(transform.name + "_fireball");
        newFireball.transform.position = transform.position;
        newFireball.transform.localRotation = Quaternion.identity;
        //currentFireball = Instantiate(FireballPref, newFireball.transform).GetComponent<FireballScript>();
        currentFireball = newFireball.GetComponent<FireballScript>();
        fireballList.Add(currentFireball);
        currentFireball.transform.localPosition = mouthPos;
        /*if (transform.localScale.y >= 0)
            currentFireball.transform.localPosition = mouthPos;
        else
            currentFireball.transform.localPosition = new Vector3(mouthPos.x, mouthPos.y, 0);*/
        //currentFireball.transform.localScale = Vector3.one * 0.3f;
        //FireballScript newFireballScript = currentFireball;
        currentFireball.lifespan = fireballLifespan;
        currentFireball.damage = dragon.damage;
        currentFireball.owner = gameObject;
        currentFireball.growingTimer = currentFireball.maxGrowingTime;
        dragon.fireballInMouth = true;
        anim.Play("Dragon_Armature|Head_Prepare_Fireball");
        for (int i = 1; i < newFireball.transform.childCount; i++)
            newFireball.transform.GetChild(i).transform.localScale = Vector3.one * 0.05f;
        if (fireImmediate)
            shouldRelease = true;
        currentFireball.transform.localScale = Vector3.one * 1.5f;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown(){
        yield return new WaitForSeconds(fireballCd);
        readyToFire = true;
    }

    private void Update()
    {
        //if (!UIMainMenu.isPaused)
        //{
        if (dragon.isPlayer && Input.GetButtonDown("Fire1") && readyToFire)
            shouldFire = true;
        if (Input.GetButtonUp("Fire1") && currentFireball != null)
        {
            shouldRelease = true;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            HandleInput();
        }
        //}
    }

    private void ReleaseFireball()
    {
        dragon.fireballInMouth = false;
        anim.Play("Dragon_Armature|Head_Shoot_Fireball");
        Vector3 currentDirection = gameObject.GetComponent<Rigidbody2D>().velocity;
        //currentFireball.transform.eulerAngles = currentDirection;
        currentFireball.speed = fireballSpeed * currentDirection.magnitude;
        currentFireball = null;
    }

    private void HandleInput()
    {
        if (shouldFire)
        {
            Fire();
            shouldFire = false;
        }

        if (shouldRelease)
        {
            if (currentFireball == null) return;
            currentFireball.ReleaseFireball();
            ReleaseFireball();
            shouldRelease = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //if (shouldFire || shouldRelease)
            //{
                //stream.SendNext(player.currentHealth);
                stream.SendNext(shouldFire);
                stream.SendNext(shouldRelease);
            //}
        }
        else
        {
            shouldFire = (bool)stream.ReceiveNext();
            shouldRelease = (bool)stream.ReceiveNext();
        }

        HandleInput();
    }
    
    #endregion

    #region ReceiveDamage

    public void GetHit(int incDamage, DragonMain attacker)
    {
        print(gameObject.name + " got hit and took " + incDamage + " damage!");
        anim.Play("Dragon_Armature|Head_Get_Hit");
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0.5f);
        //StartCoroutine(Recover());
        if (currentFireball != null)
            currentFireball.DestroyFireball();
        //print(fireballInMouth);
        dragon.fireballInMouth = false;
        dragon.currentHealth -= incDamage;
        if (dragon.isPlayer && dragon.photonView.IsMine)
            CameraShaker.Shake(12);
        else
            Game.controller.target = dragon;
        if (dragon.currentHealth <= 0)
            Die(attacker);
    }

    private void Die(DragonMain attacker)
    {
        attacker.AbsorbAttributes(dragon);
        if (dragon.isPlayer && dragon.photonView.IsMine)
        {
            Game.controller.Lose();
            print("Leaving the room because the player died");
            PhotonNetwork.Destroy(FireballScript.FireballParent);
            MultiplayerManager.inst.LeaveRoom();
        }
        else
        {
            if (dragon.isPlayer) return;
            //todo: might be PhotonNetwork.Destroy
            Destroy(gameObject);
        }
    }

    #endregion

    /*IEnumerator Recover()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }*/
}
