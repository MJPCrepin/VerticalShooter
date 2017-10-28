using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public int score;

    [Header("Object References")]

    private Rigidbody playerRb;
    public Text scoreText;
    public Image healthBar, powerBar;
    public GameObject bulletPrefab, camera, floor, playerExplosion, bars, gameOver;

    [HideInInspector]

    private float firingRange, thrustForce, idleThrust, velocity;
    private float currentHitpoints, initialHitpoints, currentPower, initialPower;
    

    // Enemy target settings
    private Transform target;
    private string targetTag = "Enemy";
    private float targetingRange, targetUpdateInterval;
    private Quaternion lookingAtEnemy; // used for directing bullets

    void Start () // objects instantiated here for children safety
    {
        firingRange = 13f;
        targetUpdateInterval = 0.333f; // in seconds
        playerRb = gameObject.GetComponentInChildren<Rigidbody>();
        thrustForce = 15f;
        idleThrust = 0.95f; // idle velocity multiplier (smoother descent)
        currentHitpoints = initialHitpoints = 100;
        currentPower = initialPower = 10000;
        InvokeRepeating("UpdateTarget", 0, targetUpdateInterval);
    }
	
	void FixedUpdate () // Physics events
    {
        // Get player input
        var anyButtonIsPressed = Input.anyKey;

        if (anyButtonIsPressed) // Apply force to player
        {
            var jumpVector = new Vector3(0,thrustForce,0);
            playerRb.AddRelativeForce(jumpVector,ForceMode.Force);
            currentPower -= (playerRb.velocity.magnitude * 2); // lose power with thrust
        }
        else // Lose force (minus idle force)
        {
            playerRb.velocity *= idleThrust;
            currentPower -= (playerRb.velocity.magnitude); // lose half power when idle
        }
    }

    private void Update()
    {

        // Slow rotation, purely aesthetic
        if (transform.position.y > 1)
        { 
            transform.Rotate(new Vector3(0, 90, 0) * 0.1f * Time.deltaTime);
        }

        // Update health and power bars
        healthBar.fillAmount = currentHitpoints / initialHitpoints;
        powerBar.fillAmount = currentPower / initialPower;

        // Destroy floor (cleanup)
        if (transform.position.y > 10)
        {
            Destroy(floor);
        }

        // Game over scenarios
        if (transform.position.y < score - 7) GameOver(); // if player falls too far back
        if (currentHitpoints <= 0) GameOver(); // out of hp
        if (currentPower <= 0) GameOver(); // out of power

        // Calculate highscore (based on cam height) and display
        score = Mathf.CeilToInt(camera.transform.position.y);
        scoreText.text = score.ToString();

        if (target == null) return; // Don't shoot if there's no target
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

    public void DamageDealt(float damage) // called by bullet if payer hit
    {
        currentHitpoints -= damage;
    }

    public void GainEnergy(float power)
    {
        currentPower += power;
    }

    public void GameOver()
    {
        // Player explosion
        GameObject impact = Instantiate(playerExplosion, transform.position, transform.rotation);
        Destroy(impact, 3f); // cleanup after 3s

        // Deactivate player and stop shooting by removing all platforms
        gameObject.SetActive(false);
        gameObject.GetComponent<EnemySpawner>().DeleteAllPlatforms();

        // UI elements
        bars.SetActive(false);
        gameOver.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
