using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour {

    public int id;
    public CameraDivisionEffect cde;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Transform cam = cde.GetCamera(id);
        //transform.LookAt(cam, cam.up);
        //transform.Rotate(Vector3.up * 180);
	}
}
