using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [Header("Object References")]

    public Transform camera;
    private float cameraDistance;

    // Arrays containing platforms with enemies on them
    public GameObject[] enemiesLeft = new GameObject[3];
    public GameObject[] enemiesRight = new GameObject[3];
    private int smallestArraySize;

    // Keep track of generated platforms
    private Queue<GameObject> platforms = new Queue<GameObject>();
    private float highestPlatLeft, highestPlatRight;
    private float nominalPlatformInterval = 10f;

    void Start ()
    {
        // Avoids indexOutOfBounds exception if enemies arrays of different length
        smallestArraySize = Mathf.Min(enemiesLeft.Length, enemiesRight.Length);

        // Used to spawn enemies close to player (away from camera)
        cameraDistance = 0 - camera.position.z;

        // Initial enemy platform spawns
        GenerateRandomPlatform(true, 15f); //left
        GenerateRandomPlatform(false, 15f); //right
    }
	
	void Update () {

        // Generate new left/right platform if player is reaching the top one
        if (camera.position.y > highestPlatLeft - nominalPlatformInterval)
        {
            GenerateRandomPlatform(true, highestPlatLeft+nominalPlatformInterval);
        }
        if (camera.position.y > highestPlatRight - nominalPlatformInterval)
        {
            GenerateRandomPlatform(false, highestPlatRight+nominalPlatformInterval);
        }

        // Delete overtaken platforms
        if (camera.position.y > (platforms.Peek().transform.position.y + 10))
        {
            Destroy(platforms.Dequeue());
        }
	}

    private void GenerateRandomPlatform(bool leftSide, float nominalVerticalDistance)
    {
        // Randomly select a platform from array
        int randPlatNum = Mathf.FloorToInt(Random.Range(0, smallestArraySize)); 

        // Noise to avoid platforms always lining up (boring)
        int randVertNoise = Mathf.FloorToInt(Random.Range(-3f, 3f)); 
        float actualVertDist = nominalVerticalDistance + randVertNoise;

        // Place enemy left or right according to bool
        float xPosition = -5; // default initialised left
        switch (leftSide) 
        {
            case true: xPosition = -5f; break;
            case false: xPosition = 5f; break;
        }

        // New platform position
        Vector3 newPosition = new Vector3(xPosition, actualVertDist, cameraDistance);

        // Spawn left or right asset based on position
        switch ((int)xPosition)
        {
            case -5: highestPlatLeft = actualVertDist; // Save y pos as ref for next left platform
                InstantiatePlatform(enemiesLeft[randPlatNum], newPosition, enemiesLeft[randPlatNum].transform.localRotation); break;
            case 5: highestPlatRight = actualVertDist; // Save y pos as ref for next right platform
                InstantiatePlatform(enemiesRight[randPlatNum], newPosition, enemiesRight[randPlatNum].transform.localRotation); break;
        }
    }

    // Separated to easily generate initial/special platforms
    private void InstantiatePlatform(GameObject platform, Vector3 newPosition, Quaternion identity)
    {
        GameObject newPlat = Instantiate(platform, newPosition, identity);
        platforms.Enqueue(newPlat);
    }

}
