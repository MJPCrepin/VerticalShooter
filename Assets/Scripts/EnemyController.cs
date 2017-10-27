using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject camera;
    private GameObject player;
    private Transform enemy;

    private string playerTag = "Player";
    private float firingRange = 5f;

    void Start()
    {
        enemy = gameObject.GetComponent<Transform>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Update()
    {
        // Enemies face the player
        Vector3 playerDirection = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
        enemy.transform.rotation = lookRotation;


    }
}
