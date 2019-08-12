using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {


    void Start() {

    }

    public void Die() {
        LevelManager.levelManager.WinGame();
    }

    public void SpawnMinions() {

    }

    void Update() {

    }
}
