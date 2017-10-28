using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour {

    public float bulletSpeed;
    private GameObject bulletImpact;

    private void Start()
    {
        bulletImpact = (GameObject)Resources.Load("BulletImpact");
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
            player.DamageDealt(10f);

            // Generate bullet impact effect when hit
            GameObject impact = Instantiate(bulletImpact, transform.position, transform.rotation);
            Destroy(impact, 3f); // cleanup after 3s

            Destroy(gameObject); // destroy bullet
        }
    }
}
