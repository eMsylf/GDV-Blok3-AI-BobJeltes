using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    public Transform RotationBody;
    public Transform ShotPoint1;
    public Transform ShotPoint2;
    public Bullet Bullet;
    public LayerMask EnemyLayer;
    public int Damage = 1;
    public float ShotSpeed = 1f;
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
        if (Input.GetButtonDown("Fire2") && isPlayer) {
            FireBullet();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == EnemyLayer) {
            Debug.Log("Hit enemy");
        }
    }

    public void FireBullet() {
        if (ShotPoint1 != null) {
            Bullet bullet = Instantiate(Bullet, ShotPoint1.position + ShotPoint1.forward, RotationBody.rotation);
            bullet.Damage = Damage;
            bullet.EnemyLayer = EnemyLayer;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * ShotSpeed, ForceMode.Impulse);
        }
        if (ShotPoint2 != null) {
            Bullet bullet = Instantiate(Bullet, ShotPoint2.position + ShotPoint2.forward, RotationBody.rotation);
            bullet.Damage = Damage;
            bullet.EnemyLayer = EnemyLayer;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * ShotSpeed, ForceMode.Impulse);
        }
    }

}
