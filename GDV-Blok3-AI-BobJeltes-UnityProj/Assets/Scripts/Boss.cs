using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEditor;

public class Boss : MonoBehaviour {

    public Transform Player;
    public int StartingHealth = 100;
    [Range(0f, 10f)]
    public float ChaseRange = 6f;
    [Range(0f, 10f)]
    public float AttackRange = 2f;

    [SerializeField] private int currentHealth;
    [SerializeField] private Shooting shooting;

    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 destination;

    private void Awake() {
        currentHealth = StartingHealth;
        Player = FindObjectOfType<Player>().transform;
        shooting = GetComponent<Shooting>();
    }

    #region Tasks
    [Task]
    public bool IsHealthOverZero() {
        if (currentHealth >= 0) {
            Task.current.Succeed();
            return true;
        }
        Task.current.Fail();
        return false;
    }

    [Task]
    public bool IsHealthOverFiftyPercent() {
        if (currentHealth >= StartingHealth / 2) {
            Task.current.Succeed();
            return true;
        }
        Task.current.Fail();
        return false;
    }

    [Task]
    public bool IsPlayerInChaseRange() {
        if (Vector3.Distance(transform.position, Player.position) < ChaseRange) {
            return true;
        }
        return false;
    }

    [Task]
    public bool IsPlayerInAttackRange() {
        if (Vector3.Distance(transform.position, Player.position) < AttackRange) {
            Task.current.Succeed();
            return true;
        }
        Task.current.Fail();
        return false;
    }

    [Task]
    public void Chase() {
        Debug.Log("Chase");
        transform.Translate(Player.position);
    }

    [Task]
    public void Attack() {
        Debug.Log("Attack");
        shooting.FireBullet();
    }

    [Task]
    public void Wander() {
        Debug.Log("Wandering");
        if (destination == null || destination == transform.position || destination == Vector3.zero) {
            // Get new destination
            GetNewDestination();
        }
        transform.position = Vector3.Lerp(start, destination, .5f);
        Task.current.Succeed();
    }

    [Task]
    public void Panic() {
        Debug.Log("Panic");
    }

    [Task]
    public void Die() {
        LevelManager.levelManager.WinGame();
        gameObject.SetActive(false);
    }
    #endregion

    #region Functions
    void GetNewDestination() {
        start = transform.position;
        destination = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        Debug.Log("Getting new destination: " + destination);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected() {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, ChaseRange);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, AttackRange + 1);
    }
    #endregion
}
