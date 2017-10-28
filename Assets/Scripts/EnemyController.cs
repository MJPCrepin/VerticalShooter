using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [Header("Properties")]

    private string playerTag = "Player";
    public float firingRange, firingRate;
    private float fireCooldown;
    private Vector3 verticalOffset;
    private int hitpoints;

    [Header("Object References")]

    public GameObject camera;
    private GameObject player;
    private Transform enemy;
    public GameObject bulletPrefab;

    void Start()
    {
        hitpoints = 5;
        firingRange = 12f;
        fireCooldown = 0;
        firingRate = 0.5f; // bullets per second

        // Aim somewhere above player's actual position
        verticalOffset = new Vector3(0, Random.Range(0, 7f), 0);

        enemy = gameObject.GetComponent<Transform>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Update()
    {
        if (hitpoints <= 0) BlowUp();

        // Enemies always face the player
        var playerDirection = (player.transform.position + verticalOffset) - transform.position;
        var lookRotation = Quaternion.LookRotation(playerDirection);
        enemy.transform.rotation = lookRotation; // look at player

        var playerIsWithinFiringRange = playerDirection.sqrMagnitude < firingRange * firingRange;
        var readyToShoot = fireCooldown <= 0;

        if (playerIsWithinFiringRange && readyToShoot)
        {
            Shoot();
            fireCooldown = 1f / firingRate;
        }
        fireCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletObject.AddComponent(typeof(EnemyBulletBehaviour)); // separate behaviour for damage tracking
    }

    public void DamageDealt()
    {
        hitpoints--;
    }

    private void BlowUp()
    {
        Destroy(gameObject);
    }
}
