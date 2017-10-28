using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour {

    public float bulletSpeed;
    private GameObject bulletImpact;

    private void Start()
    {
        bulletImpact = (GameObject)Resources.Load("BulletImpact");
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

            // Generate bullet impact effect when hit
            GameObject impact = Instantiate(bulletImpact, transform.position, transform.rotation);
            Destroy(impact, 3f); // cleanup after 3s

            Destroy(gameObject); // destroy bullet
        }
    }
}
