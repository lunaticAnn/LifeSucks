// make sure our projects syncs with each other
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.RotateAroundLocal(Vector3.up, 0.1f);
	}
}
