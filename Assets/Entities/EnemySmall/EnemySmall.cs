using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmall : MonoBehaviour {

    public int hp = 1;
    public int scoreValue = 100;
    private int dmg;
    private Vector2 spriteSize;
    public GameObject enemyshot;
    public float shotspeed;
    public GameObject explosion;

    public AudioClip enemyshoot;
    public AudioClip enemydeath;

    private ScoreKeeper death;

    public float secondsPerShot = 2;
    // Turns out I don't need a time counter. Gotta use probability instead.
    //private float timePassed;
    
    void Start() {
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        death = GameObject.Find("Score").GetComponent<ScoreKeeper>();
        //timePassed = 0 + Random.value;
    }

    void Update() {
        // conversion for easier settings at inspector
        float shotsPerSecond = 1 / secondsPerShot;
        // use probability to control firing
        if (Random.value < shotsPerSecond * Time.deltaTime) {
            PewPew();
        }

        /*
        timePassed += Time.deltaTime;
        if (timePassed >= secondsPerShot) {
            PewPew();
            timePassed = 0 + Random.value;
        }
        */
                
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Projectile hit = collision.GetComponent<Projectile>();
        if (hit) {
            dmg = hit.GetDamage();
            DamageShip();
        }
        
    }
    
    void DamageShip() {
        hp -= dmg;
        print("damaged enemy for " + dmg + ". HP Left:" + hp);
        if (hp <= 0) {
            EnemyDeath();
        }
    }

    void PewPew() {
        // Set position of laser
        Vector2 pewposition = new Vector2(transform.position.x, transform.position.y - (spriteSize.y / 2));
        // Spawn laser
        GameObject pewpew = Instantiate(enemyshot, pewposition, Quaternion.identity) as GameObject;
        pewpew.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shotspeed);
        // play shoot sound (at camera position so that sound volume isn't fucked)
        AudioSource.PlayClipAtPoint(enemyshoot, Camera.main.transform.position);
    }

    void EnemyDeath() {
        death.UpdateScore(scoreValue);
        print("Enemy Destroyed");
        Destroy(gameObject);
        // play enemy death sound (at camera position so that sound volume isn't fucked)
        AudioSource.PlayClipAtPoint(enemydeath, Camera.main.transform.position, 1);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
