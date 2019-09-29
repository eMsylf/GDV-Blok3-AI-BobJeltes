using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEditor;

public class Boss : MonoBehaviour {

    public Transform Player;
    public float StartingHealth = 100;
    [SerializeField] public float currentHealth;
    [Range(0f, 10f)]
    public float ChaseRange = 6f;
    [Range(0f, 10f)]
    public float AttackRange = 2f;
    [Range(.1f, 10f)]
    public float Speed = 1f;

    public LayerMask UnwalkableLayer;


    [SerializeField] private Shooting shooting;

    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 destination;

    public Character CharacterScript;

    private void Awake() {
        CharacterScript = GetComponent<Character>();
        StartingHealth = CharacterScript.MaxHealth;
        currentHealth = CharacterScript.Health;

        Player = FindObjectOfType<Player>().transform;
        shooting = GetComponent<Shooting>();
    }

    private void Update() {
        StartingHealth = CharacterScript.MaxHealth;
        currentHealth = CharacterScript.Health;
    }

    #region Tasks
    [Task]
    public bool IsHealthOverZero {
        get {
            if (currentHealth >= 0) {
                Task.current.Succeed();
                return true;
            }
            Task.current.Fail();
            return false;
        }
    }

    [Task]
    public bool IsHealthBelowFiftyPercent {
        get {
            if (currentHealth <= StartingHealth / 2) {
                return true;
            }
            return false;
        }
    }

    [Task]
    public bool IsPlayerInChaseRange {
        get {
            if (Vector3.Distance(transform.position, Player.position) < ChaseRange && !Physics.Linecast(transform.position, Player.position, UnwalkableLayer)) {
                return true;
            }
            return false;
        }
    }

    [Task]
    public bool IsPlayerInAttackRange {
        get {
            if (Vector3.Distance(transform.position, Player.position) < AttackRange) {
                return true;
            }
            return false;
        }
    }

    [Task]
    public bool IsDestinationReached {
        get {
            if (Vector3.Distance(transform.position, destination) < .5f) {
                return true;
            } else {
                return false;
            }
        }
    }

    [Task]
    public void Chase() {
        transform.LookAt(Player);
        transform.position += transform.forward * Speed * Time.deltaTime;
        Task.current.Succeed();
    }

    [Task]
    public void Attack() {
        transform.LookAt(Player);
        shooting.FireBullet();
        Task.current.Succeed();
    }

    [Task]
    public void Wander() {
        if (destination == null || IsDestinationReached|| destination == Vector3.zero) {
            // Get new destination
            GetNewDestination(20f);
        } else {
            transform.LookAt(destination);
            transform.position = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
        }
        Task.current.Succeed();
    }

    [Task]
    public void Panic() {
        if (destination == null || IsDestinationReached|| destination == Vector3.zero) {
            // Get new destination
            GetNewDestination(5f);
            shooting.FireBullet();
        } else {
            transform.LookAt(destination);
            transform.position = Vector3.MoveTowards(transform.position, destination, Speed * 2.0f * Time.deltaTime);
        }
        Task.current.Succeed();
    }

    [Task]
    public void Die() {
        LevelManager.levelManager.WinGame();
        gameObject.SetActive(false);
    }
    #endregion

    #region Functions
    void GetNewDestination(float destinationRadius) {

        for (int i = 0; i < 15; i++) {
            destination = transform.position + new Vector3(Random.Range(-destinationRadius, destinationRadius), 0f, Random.Range(-destinationRadius, destinationRadius));
            if (Physics.Linecast(transform.position, destination)) {
            } else {
                start = transform.position;
                return;
            }
        }
        destination = start; // fallback condition: move back to previous position if no new destination can be found in time
    }
    #endregion

#if UNITY_EDITOR

    #region Gizmos
    private void OnDrawGizmosSelected() {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, ChaseRange);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, AttackRange + 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, destination);
        Gizmos.DrawWireCube(destination, Vector3.one);
    }
    #endregion
#endif
}
