using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour {

    public int hp = 3;
    private int dmg;
    private Text playerHPText;

    private void Start() {
        playerHPText = GameObject.Find("PlayerHP").GetComponent<Text>();
        playerHPText.text = "HP: " + hp.ToString();

        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Projectile hit = collision.GetComponent<Projectile>();
        if (hit) {
            dmg = hit.GetDamage();
            DamageShip();
            playerHPText.text = "HP: " + hp.ToString();
        }
    }

    void DamageShip() {
        hp -= dmg;
        print("player damaged for " + dmg + ". HP Left:" + hp);
        if (hp <= 0) {
            PlayerDead();
        }
    }

    void PlayerDead() {
        print("Player Destroyed");
        Destroy(gameObject);
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("GameOver");

    }
}
