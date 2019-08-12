// Made with the help of this tutorial from Code Monkey https://www.youtube.com/watch?v=Gtw7VyuMdDc&t=11s&ab_channel=CodeMonkey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public Transform bar;
    public Character HealthScript;
    private float health;


    void Start() {
        bar.localScale = new Vector3(.4f, 1f);
        health = HealthScript.Health / HealthScript.MaxHealth;
        SetSize(health);
    }

    void Update() {
        if (health != HealthScript.Health) {
            health = HealthScript.Health / HealthScript.MaxHealth;
            SetSize(health);
        }
    }
    public void SetSize(float sizeNormalized) {
        Debug.Log(HealthScript.Health + " / " + HealthScript.MaxHealth + " = " + health);
        Debug.Log(sizeNormalized);
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
