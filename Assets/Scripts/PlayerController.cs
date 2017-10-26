using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // Player behaviour variables
    private float thrustForce = 15f;
    private float idleThrust = 0.95f; // idle velocity multiplier (smoother descent)
    private Rigidbody playerRb;

    // Score variables
    public GameObject camera; // camera y used for score
    public Text score;
    public int highScore;

    // To destroy when far enough
    public GameObject floor;

    void Start ()
    {
        playerRb = gameObject.GetComponentInChildren<Rigidbody>();
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

        // Game over if player falls too far back
        if (transform.position.y < highScore - 7)
        {
            print("GameOver");
        }

        // Calculate highscore (based on cam height) and display
        highScore = Mathf.CeilToInt(camera.transform.position.y);
        score.text = highScore.ToString();
    }


}
