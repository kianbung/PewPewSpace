using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int shotDamage = 1;

    // Get Damage and Destroy Shot
    public int GetDamage() {
        Destroy(gameObject);
        return shotDamage;
    }
}
