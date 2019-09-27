using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class BehaviorCollection : MonoBehaviour
{
    public Transform PlayerArt;
    public Transform Gun;
    public Bullet Bullet;
    public LayerMask EnemyLayer;
    public int Damage = 1;
    public float ShotSpeed;
    private int beamDuration;
    MeshRenderer HitBoxVisual;
    //public BoxCollider HitBox;
    //public int BeamDuration = 3;
    private Character character;
    bool isPlayer;

    private void Start() {
        if (GetComponent<Character>() == null) {
            isPlayer = false;
        } else {
            character = GetComponent<Character>();
            isPlayer = character.IsPlayer;
        }
    }


    // Ik kan gewoon functionaliteit die ik al gemaakt heb hierin plempen
    [Task]
    private void Attack(Transform _target) {
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
    }
}
