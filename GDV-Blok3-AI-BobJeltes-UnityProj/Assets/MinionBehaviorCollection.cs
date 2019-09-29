using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class MinionBehaviorCollection : MonoBehaviour
{
    public Transform Player;
    public int StartingHealth = 25;
    public float AttackRange = 2f;
    public float HealingRange = 2f;

    public int HealStrength = 1;

    private int health;
    [SerializeField] private Boss boss;

    private void Awake() {
        health = StartingHealth;
        if (Player == null) {
            Player = FindObjectOfType<Player>().transform;
        }
        if (boss == null) {
            boss = FindObjectOfType<Boss>();
        }
    }

    [Task] private bool IsHealthBelowZero() {
        if (health <= 0) {
            return true;
        }
        return false;
    }

    [Task] private bool IsPlayerInRange() {
        if (Vector3.Distance(transform.position, Player.position) < AttackRange) {
            return true;
        }
        return false;
    }

    [Task] private bool IsBossHealthBelowThreshhold() {
        
        return false;
    }

    [Task] private bool IsBossClose() {
        if (Vector3.Distance(transform.position, boss.transform.position) < HealingRange) {
            return true;
        }
        return false;
    }

    [Task] private bool HasPathToBoss() {
        return false;
    }

    //[Task] private Vector3[] FindPathToBoss() {

    //    //return new Vector3[];
    //}

    [Task] private void HealBoss() {
        Debug.Log("Healing boss");
    }

    [Task] private Vector3 GetNewDestination() {
        return new Vector3();
    }

    [Task] private void Die() {
        gameObject.SetActive(false);
    }
}
