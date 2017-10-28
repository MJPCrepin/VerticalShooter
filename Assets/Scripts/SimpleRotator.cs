using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour {

    [Header("Parameters")]
    public float x;
    public float y;
    public float z;
    public float speed;

	void Update ()
    {
        gameObject.transform.Rotate(new Vector3(x, y, z) * speed * Time.deltaTime);
    }
}
