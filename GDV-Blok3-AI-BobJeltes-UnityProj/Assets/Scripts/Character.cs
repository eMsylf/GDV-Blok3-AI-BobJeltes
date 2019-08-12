using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public bool IsPlayer;
    public float MaxHealth = 100;
    public float Health;

    void Start() {
        
    }

    public void TakeDamage(int amount) {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, 150);
        Debug.Log(name + " takes " + amount + " damage");
        if (Health <= 0) {
            if (GetComponent<Boss>() != null) {
                GetComponent<Boss>().Die();
            } else if (IsPlayer) {
                LevelManager.levelManager.LoseGame();
            }
            gameObject.SetActive(false);
        }
    }

    

    void Update() {
    }
}
