using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs player tagged as Player
// Needs follow target to be true on enemy

public class DragonAI : MonoBehaviour {

    DragonMain dragon;
    DragonMovement movement;
    Attack attack;
    public float aggresssiveness = 1;
    public float cowardliness = 1;
    public float idleness = 1;
    public GameObject sees;
    public float eyesight;
    public bool fire;
    public bool boost;
    public float memorycooldown = 0;
    public enum moods { idle, aggressive, fleeing };
    public moods mood = moods.aggressive;
    public float moodcounter;
    public bool remember;
    public float moodreset = 15;
    public float memoryreset = 10;
    public float wandercutoff = 500;
    public float wanderreset = 10;
    public Vector2 wandergoal;
    public bool up;


	void Start () {
        dragon = GetComponent<DragonMain>();
        movement = GetComponent<DragonMovement>();
        attack = GetComponent<Attack>();
        updateWandering();
        StartCoroutine(wanderUpdater());
        
    }

	void Update () {
        Vector2 goal;

        if (Vector2.Distance(wandergoal, transform.position) < 100) {
            updateWandering();
        }

        // Raycast ahead
        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("UI");
        int layerToIgnore = 1 << gameObject.layer;
        layerToIgnore = ~layerToIgnore;
        Vector2 forwards = transform.TransformDirection(Vector3.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, forwards, eyesight, layerToIgnore);
        gameObject.layer = oldLayer;
        if (hit.collider) {
            sees = hit.collider.transform.gameObject;
        }
        else {
            sees = null;
        }

        // -------

        // Decide Mood
        if (moodcounter <= 0) {
            float state = Random.Range(0.0f, aggresssiveness + cowardliness + idleness);
            if (state <= aggresssiveness) {
                mood = moods.aggressive;
            }
            else if (state > aggresssiveness && state <= aggresssiveness+cowardliness) {
                mood = moods.fleeing;
            }
            else {
                mood = moods.idle;
            }
            moodcounter = moodreset;
        }
        else {
            moodcounter -= Time.deltaTime;
        }

        // -------

        if (memorycooldown > 0) { // Remember player
            remember = true;
        }
        else {
            remember = false;
        }

        if (sees != null) { // Sees some object
            if (sees == Game.player.gameObject) {  // Sees Player
                goal = seesPlayer();
            }

            else { // Sees something else
                goal = seesObject();
            }

        }

        else { // Sees nothing
            goal = seesNothing();
        }

        movement.target = goal;
        if (memorycooldown > 0) {
            memorycooldown -= Time.deltaTime;
        }
    }

    Vector2 seesPlayer() {
        // Update Memory
        memorycooldown = memoryreset;
        if (mood == moods.aggressive) {  // If aggressive and can see player, target them (horns/fire)

            if (fire)
            {
                attack.shouldFire = true;
                attack.shouldRelease = true;
                //attack.Fire(true);
            }

            /*if (boost) {
                movement.Boost(true);
            }*/

            return Game.player.gameObject.transform.position;
        }

        else if (mood == moods.fleeing) {
            return fleeFrom(Game.player.gameObject.transform.position);
        }

        else {
            return wandergoal;
        }
    }


    Vector2 seesObject() {
        DragonMain isdragon = (sees.GetComponent(typeof(DragonMain)) as DragonMain);
        if (isdragon != null) { // Sees Dragon
            return avoid(isdragon.gameObject.transform.position);
        }
        else { // Sees something else
            if(remember) {
                if (mood == moods.aggressive) {
                    // Can remember player, hunt them
                    return Game.player.gameObject.transform.position;
                }
                else if (mood == moods.fleeing) {
                    // Can remember player, flee from them
                    return fleeFrom(Game.player.gameObject.transform.position);
                }
            }
            return wandergoal;
        }
    }

    Vector2 seesNothing() {
        if (remember) {
            if (mood == moods.aggressive) { // Hunt Player
                return Game.player.gameObject.transform.position;
            }

            else if (mood == moods.fleeing) { // Flee from Player
                return fleeFrom(Game.player.gameObject.transform.position);
            }
        }
        return wandergoal;
    }

    Vector2 fleeFrom(Vector2 thing) {
        Vector2 vec = -(Game.player.gameObject.transform.position - dragon.gameObject.transform.position);
        while (vec.magnitude < 100) {
            vec *= 2;
        }
        if (vec.x > Game.controller.worldSize.x) {
            vec.x = Game.controller.worldSize.x - 100;
        }
        if (vec.x < -Game.controller.worldSize.x) {
            vec.x = -Game.controller.worldSize.x + 100;
        }
        if (vec.y > Game.controller.worldSize.y) {
            vec.y = Game.controller.worldSize.y - 100;
        }
        if (vec.y < -Game.controller.worldSize.y) {
            vec.y = -Game.controller.worldSize.y + 100;
        }
        return vec;
    }

    Vector2 avoid(Vector2 thing) {
        Vector2 upwards = transform.TransformDirection(Vector3.up);
        Vector2 vec = thing;
        if (up) {
            vec += upwards * 100;
        }
        else {
            vec -= upwards * 100;
        }
        return vec;
    }


    IEnumerator wanderUpdater() {
        yield return new WaitForSeconds(wanderreset);
        updateWandering();
        StartCoroutine(wanderUpdater());
    }

    void updateWandering() {
        if(Random.Range(0.0f, 1.0f) > 0.5) {
            up = true;
        }
        else {
            up = false;
        }
        Vector2 vec = getRandomSpot();
        float distance = Vector2.Distance(vec, dragon.gameObject.transform.position);
        while (distance < wandercutoff) {
            vec = getRandomSpot();
            distance = Vector2.Distance(vec, dragon.gameObject.transform.position);
        }
        wandergoal = vec;
    }

    Vector2 getRandomSpot() {
        float x = Random.Range(-Game.controller.worldSize.x + 100, Game.controller.worldSize.x-100);
        float y = Random.Range(-Game.controller.worldSize.y + 100, Game.controller.worldSize.y-100);
        Vector2 vec = new Vector2(x, y);
        return vec;
    }
}
