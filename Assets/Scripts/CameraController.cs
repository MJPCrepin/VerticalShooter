using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    void Update () {
        if (player.transform.position.y > this.transform.position.y)
        {
            this.transform.position = new Vector3(0, player.transform.position.y, -10);
        }
        
	}
}
