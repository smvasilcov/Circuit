using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    Transform mainCamera;
	// Use this for initialization
	void Start () {
        mainCamera = FindObjectOfType<Camera>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(mainCamera);
	}
}
