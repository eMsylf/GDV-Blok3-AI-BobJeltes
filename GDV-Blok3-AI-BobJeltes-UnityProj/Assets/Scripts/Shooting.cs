using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
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

    void Start() {
        //HitBoxVisual = HitBox.GetComponent<MeshRenderer>();
        if (GetComponent<Character>() == null) {
            isPlayer = false;
        } else {
            character = GetComponent<Character>();
            isPlayer = character.IsPlayer;
        }
    }

    void Update() {
        beamDuration -= 1;
        if (Input.GetButtonDown("Fire2") && beamDuration <= 0 && isPlayer) {
            FireBullet();
            //beamDuration = BeamDuration;
            //Debug.Log("Shoot");
            //HitBox.enabled = true;
            //HitBoxVisual.enabled = true;
        } else if (beamDuration <= 0) {
            //HitBox.enabled = false;
            //HitBoxVisual.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == EnemyLayer) {
            Debug.Log("Hit enemy");
        }
    }

    public void FireBullet() {
        Bullet bullet = Instantiate(Bullet, Gun.position + Gun.forward, PlayerArt.rotation);
        bullet.Damage = Damage;
        bullet.EnemyLayer = EnemyLayer;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * ShotSpeed, ForceMode.Impulse);
    }

}
