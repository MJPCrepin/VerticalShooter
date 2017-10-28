using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour {

    public float bulletSpeed;

    private void Start()
    {
        bulletSpeed = -50f; // negative = silly hack to get the bullets to fire correct way
        this.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
    }

	void Update ()
    {
        // If bullet misses target and is out of bounds, destroy
        if (transform.position.x > 40 || transform.position.x < -40)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyController>();
            enemy.DamageDealt();

            Destroy(gameObject); // destroy bullet
        }
    }
}
