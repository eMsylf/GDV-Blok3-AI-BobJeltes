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
        private MinionState _minionState;

        public int TimeBeforeNewAction = 120;
        int timeBeforeNewAction;

        public Boss boss;
        Character bossCharacter;
        Shooting shooting;
        Transform player;

        void Start() {
            GetNewPathWithRandomOffset();
            timeBeforeNewAction = TimeBeforeNewAction;

            player = target;
            bossCharacter = boss.GetComponent<Character>();
            shooting = GetComponent<Shooting>();
        }

        private void Update() {
            timeBeforeNewAction -= 1;

            if (bossCharacter.Health <= 50) {
                _minionState = MinionState.ProtectBoss;
                target = boss.transform;
                GetNewPathWithRandomOffset();
            } else if (timeBeforeNewAction <= 0) {
                if (Physics.CheckSphere(transform.position, 2f, PlayerLayer)) {
                    Debug.Log("Player is in range");
                    _minionState = MinionState.AttackPlayer;
                } else {
                    GetNewPathWithRandomOffset();
                }
                timeBeforeNewAction = TimeBeforeNewAction;
            }

            switch (_minionState) {
                case MinionState.ProtectBoss: {
                        // follow the boss to make sure the player can't reach him
                        break;
                    }
                case MinionState.FollowPlayer: {
                        // follow the player, go on the offensive
                        
                        break;
                    }
                case MinionState.AttackPlayer: {
                        // attack the player, once the player is in range
                        target = player;
                        transform.LookAt(target);
                        shooting.FireBullet();

                        if (!Physics.CheckSphere(transform.position, 2f, PlayerLayer)) {
                            _minionState = MinionState.FollowPlayer;
                        }
                        break;
                    }
            }
            
        }

        private void Attack(Transform target) {
            //target.GetComponent<Player>;
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
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath() {
            Vector3 currentWaypoint = path[0];
            while (true) {
                if (transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex >= path.Length) {
                        targetIndex = 0;
                        path = new Vector3[0];
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
            ProtectBoss
        }

    }
}
