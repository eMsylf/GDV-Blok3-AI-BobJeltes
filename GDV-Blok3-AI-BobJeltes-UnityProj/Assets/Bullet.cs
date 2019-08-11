using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int Damage; // This is inherited from the player or enemy
    public float BulletLifetime;
    public LayerMask EnemyLayer;

    void Start() {
        StartCoroutine(DespawnAfter(BulletLifetime));
    }

    private void OnCollisionEnter(Collision collision) {
        StopCoroutine(DespawnAfter(BulletLifetime));
        if (collision.gameObject.GetComponent<Character>() != null) {
            Character _character = collision.gameObject.GetComponent<Character>();
            if (!_character.IsPlayer) {
                Debug.Log("Hit a character that is not the player");
            }
            _character.TakeDamage(Damage);
        }
        gameObject.SetActive(false);
    }

    private IEnumerator DespawnAfter(float _seconds) {
        yield return new WaitForSeconds(_seconds);
        gameObject.SetActive(false);
    }

}
