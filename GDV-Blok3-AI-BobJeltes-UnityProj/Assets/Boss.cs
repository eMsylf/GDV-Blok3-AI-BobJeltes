using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {


    void Start() {

    }

    public void Die() {
        LevelManager.levelManager.RestartLevel();
    }

    public void SpawnMinions() {

    }

    void Update() {

    }
}
