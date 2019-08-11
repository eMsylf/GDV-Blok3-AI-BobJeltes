using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public bool IsPlayer;
    public int Health;

    void Start() {
        
    }

    public void TakeDamage(int amount) {
        Health -= amount;
        Debug.Log(name + " takes " + amount + " damage");
        if (Health <= 0) {
            if (GetComponent<Boss>() != null) {
                GetComponent<Boss>().Die();
            } else if (IsPlayer) {
                LevelManager.levelManager.RestartLevel();
            }
            gameObject.SetActive(false);

        }
    }

    void Update() {
    }
}
