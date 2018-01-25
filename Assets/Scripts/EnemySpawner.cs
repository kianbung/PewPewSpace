using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemySmall;
    public float width;
    public float height;
    public float spawnDelay = 0.5f;

    // for screen bounds
    public float movespeed;
    float xmin;
    float xmax;
    float ymin;
    float ymax;

    // set starting movement
    private bool moveRight = false;

    // Use this for initialization
    void Start () {
        SetBounds();
        SpawnUntilFull();
	}
	
	// Update is called once per frame
	void Update () {
        MoveEnemy();
        if (AllMembersDead()) {
            print("All enemies dead");
            // Invoke gets called a million times instead of only triggering once, so second wave appears together.
            // How do I solve this?
            // Invoke ("SpawnUntilFull", spawnDelay);
            SpawnUntilFull();
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }

    /* no longer valid
    void SpawnEnemies() {
        foreach (Transform child in transform) {
            // Spawn enemy under each spawn point (child of Spawner)
            GameObject enemy = Instantiate(enemySmall, child.transform.position, Quaternion.identity) as GameObject;
            // Keeping spawned enemies nested under EnemySpawner GameObject
            enemy.transform.parent = child;
        }

    }
    */

    //
    // there is still a bug where enemies constantly respawn if they are destroyed while spawning
    void SpawnUntilFull() {
        Transform freePosition = NextFreePosition();
        if (freePosition) {
            // Spawn enemy under each spawn point (child of Spawner)
            GameObject enemy = Instantiate(enemySmall, freePosition.transform.position, Quaternion.identity) as GameObject;
            // Keeping spawned enemies nested under EnemySpawner GameObject
            enemy.transform.parent = freePosition;
        }
        // check if there is another full position, if so, spawn next
        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    // Look for next spawnpoint under GameObject
    Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in transform) {
            // check if any child empty, and if so, return their transform
            if (childPositionGameObject.childCount == 0) {
                return childPositionGameObject.transform;
            }
        }
        return null;
    }

    void MoveEnemy() {
        // Jedi code below: Ternary Operator
        transform.position += (moveRight ? Vector3.right : Vector3.left) * movespeed * Time.deltaTime;
        /* Trying out Liono's solution above ^^, Ternary Operator
        if (!moveRight) {
            transform.position += Vector3.left * movespeed * Time.deltaTime;
            /* original solution; below is solution from lecture
            if (transform.position.x <= xmin) {
                moveRight = true;
            }
            
        } else if (moveRight) {
            transform.position += Vector3.right * movespeed * Time.deltaTime;
            /* original solution; below is solution from lecture
 
            if (transform.position.x >= xmax) {
                moveRight = false;
            }
            
        }
        */

        // define formation edges
        float formationLeftEdge = transform.position.x - (width / 2);
        float formationRightEdge = transform.position.x + (width / 2);
        // (don't) use boolean flipper for shorter code (teacher sohai teach us wrong thing)
        // edge detection
        if (formationLeftEdge <= xmin) {
            moveRight = true;
        } else if (formationRightEdge >= xmax) {
            moveRight = false;
        }

       // EnemyClamp(formationLeftEdge, formationRightEdge);

    }

    void SetBounds() {
        // find camera distance from player (unnecessary in 2D, but ref for when make 3D)
        float distance = transform.position.z - Camera.main.transform.position.z;
        // define leftmost and rightmost corners of camera 
        Vector3 bottomleft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topright = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));
        xmin = bottomleft.x;
        xmax = topright.x;
        ymin = bottomleft.y;
        ymax = topright.y;
    }

    /*
    void EnemyClamp(float enemyxmin, float enemyxmax) {
        float newX = Mathf.Clamp(transform.position.x, enemyxmin, enemyxmax);
        // float newY = Mathf.Clamp(transform.position.y, enemyymin, enemyymax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    */

    


    bool AllMembersDead() {
        // i see, Transform can be used to return enums (arrays)
        foreach (Transform childPositionGameObject in transform) {
            // count children of children to see if any left alive
            if (childPositionGameObject.childCount > 0) {
                return false;
            }
        }
        return true;
    }
}
