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
    private float containedPower;

    [Header("Object References")]

    public GameObject camera;
    private GameObject player, explosion;
    private Transform enemy;
    public GameObject bulletPrefab;

    void Start()
    {
        hitpoints = 5;
        firingRange = 12f;
        fireCooldown = 0;
        firingRate = 0.5f; // bullets per second
        containedPower = 500f; // player saps energy when destroyed

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
        player.GetComponent<PlayerController>().GainEnergy(containedPower);

        // Explosion prefab
        GameObject impact = Instantiate((GameObject)Resources.Load("EnemyExplosion"), transform.position, transform.rotation);
        Destroy(impact, 3f); // cleanup after 3s

        Destroy(gameObject);
    }
}
