// This code belongs to Unity3D College, and was modified by Bob Jeltes https://www.youtube.com/watch?v=YdERlPfwUb0&t=9s&ab_channel=Unity3dCollege

using System.Linq;
using UnityEngine;

public class Drone : MonoBehaviour {
    public Team Team => _team;
    [SerializeField] private Team _team;
    [SerializeField] private LayerMask _layerMask;
    [Range(.1f, 10f)]
    [SerializeField] private float speed = 5f;
    private float speedMultiplier = 1f;
    [SerializeField] private Shooting shooting;
    [SerializeField] private Shooting shooting2;
    public GameObject minion;

    private float _attackRange = 3f;
    private float _rayDistance = 5.0f;
    private float _stoppingDistance = 1.5f;

    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private Character _target;
    private Character character;
    private DroneState _currentState;

    private Boss boss;
    public int ShotDelay = 10;
    private int shotDelay;
    public int PanicTimer = 100;
    private int panicTimer;

    private void Start() {
        shotDelay = ShotDelay;
        panicTimer = PanicTimer;

        boss = GetComponent<Boss>();
        character = GetComponent<Character>();
    }

    private void Update() {
        if (character.Health <= 50) {
            _currentState = DroneState.Panic;
        }

        switch (_currentState) {
            case DroneState.Wander: {
                    if (NeedsDestination()) {
                        GetDestination();
                    }

                    transform.rotation = _desiredRotation;

                    transform.Translate(Vector3.forward * Time.deltaTime * speed);

                    var rayColor = IsPathBlocked() ? Color.red : Color.green;
                    Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                    while (IsPathBlocked()) {
                        //Debug.Log("Path Blocked");
                        GetDestination();
                    }

                    var targetToAggro = CheckForAggro();
                    if (targetToAggro != null) {
                        _target = targetToAggro.GetComponent<Character>();
                        //_target = targetToAggro.GetComponent<Drone>();
                        _currentState = DroneState.Chase;
                    }

                    break;
                }
            case DroneState.Chase: {
                    if (_target == null) {
                        _currentState = DroneState.Wander;
                        return;
                    }

                    transform.LookAt(_target.transform);
                    transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                    if (Vector3.Distance(transform.position, _target.transform.position) < _attackRange) {
                        _currentState = DroneState.Attack;
                    }
                    break;
                }
            case DroneState.Attack: {
                    if (_target != null) {
                        if (shotDelay <= 0) {
                            if (shooting != null) {
                                shooting.FireBullet();
                            }
                            if (shooting2 != null) {
                                shooting2.FireBullet();
                            }
                            shotDelay = ShotDelay;
                        } else {
                            shotDelay -= 1;
                        }
                    }
                    if (Vector3.Distance(transform.position, _target.transform.position) > _attackRange) {
                        _currentState = DroneState.Wander;
                    }

                    break;
                }
            case DroneState.Panic: {
                    speedMultiplier = 2f;
                    if (panicTimer <= 0) {
                        GetDestination();
                        shooting.FireBullet();
                        panicTimer = PanicTimer;
                    } else {
                        panicTimer -= 1;
                    }

                    transform.rotation = _desiredRotation;

                    transform.Translate(Vector3.forward * Time.deltaTime * speed * speedMultiplier);
                    var rayColor = IsPathBlocked() ? Color.red : Color.green;
                    Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                    while (IsPathBlocked()) {
                        //Debug.Log("Path Blocked");
                        GetDestination();
                    }

                    break;
                }
            case DroneState.SpawnMinion: {
                    // Spawn a minion behind the boss

                    break;
                }
        }
    }

    private bool IsPathBlocked() {
        Ray ray = new Ray(transform.position, _direction);
        RaycastHit[] hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        return hitSomething.Any();
    }

    private void GetDestination() {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                               new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f,
                                   UnityEngine.Random.Range(-4.5f, 4.5f));

        _destination = new Vector3(testPosition.x, 1f, testPosition.z);

        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }

    private bool NeedsDestination() {
        if (_destination == Vector3.zero)
            return true;

        var distance = Vector3.Distance(transform.position, _destination);
        if (distance <= _stoppingDistance) {
            return true;
        }
        return false;
    }



    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private Transform CheckForAggro() {
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for (var i = 0; i < 24; i++) {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius)) {
                var drone = hit.collider.GetComponent<Drone>();
                var character = hit.collider.GetComponent<Character>();
                if (drone != null && drone.Team != gameObject.GetComponent<Drone>().Team) {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return drone.transform;
                } else if (character != null) {
                    if (character.IsPlayer) {
                        Debug.DrawRay(pos, direction * hit.distance, Color.red);
                        return character.transform;
                    }
                } else {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            } else {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
            }
            direction = stepAngle * direction;
        }

        return null;
    }
}

public enum Team {
    Red,
    Blue
}

public enum DroneState {
    Wander,
    Chase,
    Attack,
    Panic,
    SpawnMinion
}