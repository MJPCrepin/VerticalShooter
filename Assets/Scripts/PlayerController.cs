using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("Properties")]

    private float thrustForce, idleThrust;
    private Rigidbody playerRb;
    public int hitpoints;

    [Header("Object References")]

    public GameObject camera; // camera y used for score
    public Text score;
    public int highScore;
    public GameObject floor; // To destroy when far enough

    void Start ()
    {
        playerRb = gameObject.GetComponentInChildren<Rigidbody>();
        thrustForce = 15f;
        idleThrust = 0.95f; // idle velocity multiplier (smoother descent)
        hitpoints = 10;
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
    }

    public void DamageDealt()
    {
        hitpoints--;
    }

    public void GameOver()
    {
        print("Game over *explosions*");
        Destroy(gameObject);
    }

}
