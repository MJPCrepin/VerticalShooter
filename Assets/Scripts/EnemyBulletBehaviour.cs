using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour {

    public float bulletSpeed;

    private void Start()
    {
        bulletSpeed = 40f;
        this.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
    }

	void Update ()
    {
        // If bullet misses target and is out of bounds, destroy
        if (transform.position.x > 20 || transform.position.x < -20)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player.DamageDealt();

            Destroy(gameObject); // destroy bullet
        }
    }
}
