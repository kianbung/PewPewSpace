using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject lasershot;
    public float laserspeed;
    public float movespeed;
    public float dps;
    float xmin;
    float xmax;
    float ymin;
    float ymax;
    private Vector2 playerSpriteSize;

    public AudioClip playerShootSound;

    // Use this for initialization
    void Start () {
        SetBounds();
    }

    // Update is called once per frame
    void Update () {
        KbControls();
        Clamping();
	}

    // Keyboard Controls (Time.deltaTime makes it framerate independent)
    void KbControls() {
        if (Input.GetKey(KeyCode.LeftArrow)) {           
            // original code + clamp:
            // transform.position = new Vector2(Mathf.Clamp(transform.position.x - movespeed * Time.deltaTime, 0f, 15.5f), transform.position.y);
            transform.position += Vector3.left * movespeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            // transform.position = new Vector2(Mathf.Clamp(transform.position.x + movespeed * Time.deltaTime, 0f, 15.5f), transform.position.y);
            transform.position += Vector3.right * movespeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            // transform.position = new Vector2(Mathf.Clamp(transform.position.x + movespeed * Time.deltaTime, 0f, 15.5f), transform.position.y);
            transform.position += Vector3.up * movespeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            // transform.position = new Vector2(Mathf.Clamp(transform.position.x + movespeed * Time.deltaTime, 0f, 15.5f), transform.position.y);
            transform.position += Vector3.down * movespeed * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            float shotsPerSecond = 1 / dps;
            InvokeRepeating("PewPew", 0.1f, shotsPerSecond);
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke();
        }
    }

    void PewPew() {
        // Set position of laser
        Vector2 pewposition = new Vector2(transform.position.x, transform.position.y + (playerSpriteSize.y / 2));
        // Spawn laser
        GameObject pewpew = Instantiate(lasershot, pewposition, Quaternion.identity) as GameObject;
        pewpew.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserspeed);
        // play pew pew sound
        //gameObject.GetComponent<AudioSource>().Play();
        AudioSource.PlayClipAtPoint(playerShootSound, Camera.main.transform.position);
    }

    void Clamping() {
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        float newY = Mathf.Clamp(transform.position.y, ymin, ymax);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void SetBounds() {
        // find camera distance from player (unnecessary in 2D, but ref for when make 3D)
        float distance = transform.position.z - Camera.main.transform.position.z;
        // define leftmost and rightmost corners of camera 
        Vector3 bottomleft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topright = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

        //get player sprite size
        playerSpriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;

        // adjust playerbounds to sprite size
        xmin = bottomleft.x + playerSpriteSize.x / 2;
        xmax = topright.x - playerSpriteSize.x / 2;
        ymin = bottomleft.y + playerSpriteSize.y / 2;
        ymax = topright.y - playerSpriteSize.y / 2;
    }
}
