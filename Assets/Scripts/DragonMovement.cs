using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour {

    public float thrust,boost, boostSpeed, terminalVelocity, boostCost, boostCooldown, boostCharge, speed, decelleration, rotationSpeed, upward, lift, gravity = -9.81f;
	public bool boosting;
	public float velocityMag;
    Rigidbody2D rigidBody2D;
	Vector2 velocity;
    DragonMain dragon;
    public Vector2 target;
    public float targetAngle;
    public float direction;
    public bool pathCorrection;
    public float correctionMargin;
    private Animator anim;
    public bool movementEnabled = true;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        dragon = GetComponent<DragonMain>();
        anim = GetComponent<Animator>();
    }



    public void Boost(bool active){
        if (active && boost > boostCooldown)
        {
            boosting = true;
            anim.SetBool("Headbut",true);
            speed = boostSpeed * dragon.relativeBoostSpeed;
            boost = Mathf.Clamp(boost - (boostCost * Time.fixedDeltaTime), 0, dragon.boostEnergy);

            if (boost == 0)
            {
                boostCooldown = dragon.boostEnergy / 10;
                boosting = false;
                anim.SetBool("Headbut", false);
            }
        }
        else if (boosting == true){
            boosting = false; 
            anim.SetBool("Headbut", false);
        }
    }

    public void Move(float direction){
        if (!boosting)
            rigidBody2D.angularVelocity = (direction * rotationSpeed * dragon.dexterity);
    }

	void FixedUpdate () {
        //if (UIMainMenu.isPaused && GetComponent<DragonMain>().isPlayer)
        //    return;
        if (dragon.isPlayer)
        {
            if (Input.GetButton("Boost"))
                Boost(true);
            else
                Boost(false);
        }
        //Boost!
        if (!boosting)
        {
            speed = thrust*dragon.relativeSpeed;
            if (rigidBody2D.velocity.magnitude > thrust*dragon.relativeSpeed)
                rigidBody2D.drag = 0.3f;
            else
                rigidBody2D.drag = 0;
            if (boost > boostCooldown)
                boostCooldown = 0;
            boost = Mathf.Clamp(boost + boostCharge * Time.fixedDeltaTime, 0, dragon.boostEnergy);
        }

        //Core Movement
        if(dragon.isPlayer && !Game.controller.mouseControl && !pathCorrection && movementEnabled)
            Move(Input.GetAxis ("Vertical"));
        if (dragon.isPlayer && Game.controller.mouseControl && !pathCorrection && movementEnabled)
            target = Game.controller.ScreenToZ(new Vector2(Input.mousePosition.x,Input.mousePosition.y));

        //Move to point
        if ((dragon.isPlayer && Game.controller.mouseControl) || !dragon.isPlayer || pathCorrection)
        {
            float xDiff = target.x - transform.position.x;
            float yDiff = target.y - transform.position.y;
            targetAngle = UnsignedToSignedAngle(Mathf.Atan2(yDiff, xDiff) * 180 / Mathf.PI);
            //transform.rotation = Quaternion.Euler(0,0, targetAngle);
            direction = (Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle) / 180) * rotationSpeed * dragon.dexterity * Time.fixedDeltaTime;//TargetAngleToDirection(transform.eulerAngles.z, targetAngle);
            Move(direction);
        }

        //Gliding Physics
		lift = AngleToAirResistance(transform.rotation.eulerAngles.z, 0.05f);
		Vector2 forwards = transform.TransformDirection (Vector3.right);
        rigidBody2D.velocity = forwards * Mathf.Clamp(rigidBody2D.velocity.magnitude,speed,terminalVelocity*dragon.relativeSpeed) + (new Vector2(0,lift*gravity));
		velocityMag = rigidBody2D.velocity.magnitude;

        //Path Correction
        if(transform.position.y < -Game.controller.worldSize.y || transform.position.y > Game.controller.worldSize.y || transform.position.x< -Game.controller.worldSize.x|| transform.position.x > Game.controller.worldSize.x){
            if (!pathCorrection){
                pathCorrection = true;
                target = new Vector3(Mathf.Clamp(transform.position.x, -Game.controller.worldSize.x + correctionMargin, Game.controller.worldSize.x - correctionMargin),
                                     Mathf.Clamp(transform.position.y, -Game.controller.worldSize.y + correctionMargin, Game.controller.worldSize.y - correctionMargin),0);
            }
        } else
        pathCorrection = false;

        //Flip Model
        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
        {
            if (transform.localScale.y>0)
                transform.localScale = new Vector3(transform.localScale.x,-transform.localScale.y,-transform.localScale.z);
        }
        else if (transform.localScale.y <0)
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, -transform.localScale.z);

        // Show the Animator how fast the dragon is
        anim.SetFloat("Speed", velocityMag / terminalVelocity);
        bool isNeg = transform.eulerAngles.z > 180;
        float dir = transform.eulerAngles.z % 180;
        if (dir > 90)
            dir = 180 - dir;
        if (isNeg)
            dir *= -1;
        dir /= 60;
        anim.SetFloat("Direction", dir);
    }

	float AngleToAirResistance(float angle, float upwardsGravity){
		if (angle > 180 && angle < 360)
			upwardsGravity = 1;
		angle = (angle / 180) * Mathf.PI;
		return Mathf.Abs(Mathf.Sin(angle) *  upwardsGravity);
        
    }

    float UnsignedToSignedAngle(float input){
        if (input < 0)
            input = 360 + input;
        return input;
    }

    float TargetAngleToDirection(float currentAngle,float targetAngle){
        if (Mathf.Abs(currentAngle - targetAngle) > Mathf.Abs(currentAngle - targetAngle -360))
            return -1;
        else
            return 1;
    }
}
