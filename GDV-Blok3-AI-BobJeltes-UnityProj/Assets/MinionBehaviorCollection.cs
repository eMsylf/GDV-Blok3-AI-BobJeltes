using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Lague;
using UnityEditor;

public class MinionBehaviorCollection : MonoBehaviour
{
    public Character CharacterScript;
    public Transform Player;
    public Shooting Shooting;
    public float StartingHealth = 25;
    [Range(0f, 10f)]
    public float AttackRange = 2f;
    [Range(0f, 10f)]
    public float HealingRange = 4f;

    public int HealStrength = 1;

    private float health;
    [SerializeField] private Boss boss;
    private float distanceToBoss;

    private Pathfinding pathfinding;
    private Vector3[] currentPath;
    private int currentWaypointIndex = 0;
    public bool hasActivePath = false;

    [Range(.1f, 10f)]
    [SerializeField] private float speed = 1f;
    [SerializeField] Vector3 currentDestination;

    private void Awake() {
        health = StartingHealth;
        if (Player == null) {
            Player = FindObjectOfType<Player>().transform;
        }
        if (boss == null) {
            boss = FindObjectOfType<Boss>();
        }
        pathfinding = FindObjectOfType<Pathfinding>();
        Shooting = GetComponent<Shooting>();

        CharacterScript = GetComponent<Character>();
        StartingHealth = CharacterScript.MaxHealth;
        health = CharacterScript.Health;
    }

    private void Update() {
        health = CharacterScript.Health;
    }

    [Task]
    private bool IsHealthOverZero {
        get {
            if (health >= 0) {
                return true;
            }
            return false;
        }
    }

    [Task]
    private bool IsPlayerInRange {
        get {
            if (Vector3.Distance(transform.position, Player.position) < AttackRange) {
                return true;
            }
            return false;
        }
    }

    [Task]
    private bool IsBossHealthBelowFiftyPercent => boss.IsHealthBelowFiftyPercent;

    [Task]
    private bool IsBossClose {
        get {
            if (Vector3.Distance(transform.position, boss.transform.position) < HealingRange) {
                Debug.Log("Boss is in range");
                return true;
            } else {
                return false;
            }
        }
    }

    [Task]
    public bool HasActivePath {
        get {
            if (currentPath == null) {
                Debug.Log("No active path found");
                return false;
            } else if (transform.position == currentDestination) {
                Debug.Log("Reached destination");
                return false;
            } else {
                Debug.Log("Has active path");
                return true;
            }
        }
    }

    [Task]
    private void Attack() {
        transform.LookAt(Player);
        Shooting.FireBullet();
        Debug.Log("Attack");
        Task.current.Succeed();
    }

    [Task]
    private void HealBoss() {
        boss.CharacterScript.Health++;
        Debug.Log("Healing boss");
        Task.current.Succeed();
    }

    [Task] void GetNewPathToPlayer() {
        Debug.Log("Get new path to player");
        Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        currentPath = pathfinding.FindPath(transform.position, Player.position + randomOffset);
        currentDestination = currentPath[currentPath.Length -1];
        currentWaypointIndex = 0;
        Task.current.Succeed();
    }

    [Task]
    private void GetNewPathToBoss() {
        Debug.Log("Get new path to boss");
        Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        currentPath = pathfinding.FindPath(transform.position, boss.transform.position + randomOffset);
        currentDestination = currentPath[currentPath.Length -1];
        currentWaypointIndex = 0;
        Task.current.Succeed();
    }

    [Task]
    private void FollowPath() {
        if (transform.position == currentDestination) {
            Debug.Log("Done following path");
        } else if (transform.position == currentPath[currentWaypointIndex]){
            currentWaypointIndex++;
        } else {
            Debug.Log("Follow path");
            transform.position = Vector3.MoveTowards(transform.position, currentPath[currentWaypointIndex], speed * Time.deltaTime);
            //transform.position += transform.forward * speed * Time.deltaTime;
            transform.LookAt(currentPath[currentWaypointIndex]);
        }
        Task.current.Succeed();
    }

    [Task]
    private void Die() {
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    #region Gizmos
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, AttackRange);
    }
    #endregion
#endif
}
