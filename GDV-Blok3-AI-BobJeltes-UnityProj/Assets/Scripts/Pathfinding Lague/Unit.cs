using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lague {
    public class Unit : MonoBehaviour {
        public Transform target;
        public float speed = 20;
        Vector3[] path;
        int targetIndex;
        public LayerMask PlayerLayer;
        public MinionState _minionState;

        public int ActionDelay = 180;
        float actionDelay;

        public Boss boss;
        Character bossCharacter;
        Shooting shooting;
        Transform player;
        float attackRange = 4f;

        bool isFollowingPath;

        [SerializeField] Character characterScript;

        void Start() {
            actionDelay = ActionDelay;

            player = target;
            bossCharacter = boss.GetComponent<Character>();
            
            shooting = GetComponent<Shooting>();
            characterScript = GetComponent<Character>();
        }

        private void Update() {
            if (LevelManager.levelManager.gameState == LevelManager.GameState.Win) {
                return;
            }
            if (characterScript.Health <= 0) { // If the minion's health is equal to or below 0
                _minionState = MinionState.Die; // Die
            } else { // Otherwise...
                if (actionDelay > 0) { // If the action delay has not yet counted down
                    actionDelay--; // Count it down by 1
                    return; // And wait a frame
                } else { // If the action delay is equal to or lower than 0
                    actionDelay = ActionDelay; // Reset the action delay and execute the rest of the code
                }

                if (Physics.CheckSphere(transform.position, attackRange, PlayerLayer)) {
                    //Debug.Log("Player is in range");
                    _minionState = MinionState.AttackPlayer;
                } else if (bossCharacter.Health <= 50) { // If the boss' health is 50 or lower
                    _minionState = MinionState.FollowPlayer; // Protect the boss
                } else {
                    _minionState = MinionState.ProtectBoss;
                }
            }

            switch (_minionState) {
                case MinionState.ProtectBoss: { // follow the boss to make sure the player can't reach him, and attack the player if they're close
                        target = bossCharacter.transform; // Set target to boss
                        if (Vector3.Distance(transform.position, target.position) < attackRange) { // If the boss is within range
                            bossCharacter.Health++; // Heal the boss
                        } else if (Vector3.Distance(transform.position, target.position) > 5f) { // Else,
                            GetNewPathWithRandomOffset(); // Pathfind to the boss
                        }
                        break;
                    }
                case MinionState.FollowPlayer: { // follow the player, go on the offensive
                        target = player; // Set the target to player
                        if (Physics.CheckSphere(transform.position, attackRange, PlayerLayer)) { // If the player is in range to attack
                            //Debug.Log("Player is in range");
                            _minionState = MinionState.AttackPlayer; // Attack the player
                        } else if (!isFollowingPath) { // If the player is not in range, and the minion isn't already following a path
                            GetNewPathWithRandomOffset(); // Get a new path towards the player
                        }
                        break;
                    }
                case MinionState.AttackPlayer: { // Attack the player, once the player is in range
                        //Debug.Log("Attack player");
                        target = player; // Set the target to the player
                        Attack(target); // Attack the player
                        
                        //if (!Physics.CheckSphere(transform.position, attackRange, PlayerLayer)) { // If the player is no longer close by
                        //    _minionState = MinionState.FollowPlayer; // Go back to following the player
                        //}
                        break;
                    }
                case MinionState.Die: { // The minion dies
                        StopAllCoroutines(); // All coroutines are stopped
                        gameObject.SetActive(false); // Game object is deactivated 
                        break;
                    }
            }
        }

        private void Attack(Transform _target) {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            shooting.FireBullet();
        }

        private void GetNewPathWithRandomOffset() {
            int pathEndX = Random.Range(-4, 4);
            int pathEndY = Random.Range(-4, 4);
            Vector3 pathEnd = target.position + new Vector3(pathEndX, 0, pathEndY);
            PathRequestManager.RequestPath(transform.position, pathEnd, OnPathFound);
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            if (pathSuccessful) {
                path = newPath;
                StopCoroutine("FollowPath");
                isFollowingPath = false;
                StartCoroutine("FollowPath");
                isFollowingPath = true;
            }
        }

        IEnumerator FollowPath() {
            Vector3 currentWaypoint;
            if (path == null || path.Length == 0 || path[0] == null)
            {
                yield break;
            }
            currentWaypoint = path[0];
            while (true) {
                if (transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex >= path.Length) {
                        targetIndex = 0;
                        path = new Vector3[0];
                        isFollowingPath = false;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }

        public void OnDrawGizmos() {
            if (path != null) {
                for (int i = targetIndex; i < path.Length; i++) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);
                    if (i == targetIndex) {
                        Gizmos.DrawLine(transform.position, path[i]);
                    } else {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }

        public enum MinionState {
            FollowPlayer,
            AttackPlayer,
            ProtectBoss,
            Die
        }

    }
}
