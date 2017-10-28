using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("Properties")]

    private float firingRange;
    private float thrustForce, idleThrust;
    private Rigidbody playerRb;
    public int hitpoints;

    [Header("Object References")]

    public GameObject bulletPrefab;
    public GameObject camera; // camera y used for score
    public Text score;
    public int highScore;
    public GameObject floor; // To destroy when far enough

    // Enemy target settings
    private Transform target;
    private string targetTag = "Enemy";
    private float targetingRange;
    private float targetUpdateInterval = 0.333f; // in seconds
    private Quaternion lookingAtEnemy; // used for directing bullets

    void Start () // objects instantiated here for children safety
    {
        firingRange = 13f;
        playerRb = gameObject.GetComponentInChildren<Rigidbody>();
        thrustForce = 15f;
        idleThrust = 0.95f; // idle velocity multiplier (smoother descent)
        hitpoints = 10;
        InvokeRepeating("UpdateTarget", 0, targetUpdateInterval);
    }
	
	void FixedUpdate () // Physics events
    {
        var anyButtonIsPressed = Input.anyKey;

        if (anyButtonIsPressed) // Apply force to player
        {
            var jumpVector = new Vector3(0,thrustForce,0);
            playerRb.AddRelativeForce(jumpVector,ForceMode.Force);
        }
        else // Lose force (minus idle force)
        {
            playerRb.velocity *= idleThrust;
        }
	}

    private void Update()
    {
        // Slow rotation, purely aesthetic
        if (transform.position.y > 1)
        { 
            transform.Rotate(new Vector3(0, 90, 0) * 0.1f * Time.deltaTime);
        }

        // Destroy floor (cleanup)
        if (transform.position.y > 10)
        {
            Destroy(floor);
        }

        // Game over scenarios
        if (transform.position.y < highScore - 7) GameOver(); // if player falls too far back
        if (hitpoints <= 0) GameOver(); // out of hp

        // Calculate highscore (based on cam height) and display
        highScore = Mathf.CeilToInt(camera.transform.position.y);
        score.text = highScore.ToString();

        if (target == null) return;
    }

    public void DamageDealt() // called by bullet if payer hit
    {
        hitpoints--;
    }

    public void GameOver()
    {
        print("Game over *explosions*");
        Destroy(gameObject);
    }

    private void UpdateTarget() // detect closest enemy and shoot at it
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag); // all enemies

        float shortestDistance = Mathf.Infinity;
        Vector3 directionToClosestEnemy;

        foreach (GameObject enemy in enemies) // find closest of all enemies
        {
            var enemyDirection = transform.position - enemy.transform.position;
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance) // declare closest and aim at it
            {
                shortestDistance = distanceToEnemy;
                directionToClosestEnemy = enemyDirection;
                lookingAtEnemy = Quaternion.LookRotation(enemyDirection);
            }
        }

        if (shortestDistance <= firingRange)
        {
            Shoot(lookingAtEnemy);
        }
    }

    private void Shoot(Quaternion direction)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, direction);
        bulletObject.AddComponent(typeof(PlayerBulletBehaviour)); // separate behaviour for damage tracking
    }

}
