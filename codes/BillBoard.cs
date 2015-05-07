using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

	public Camera cam;
	// Use this for initialization
	void Start () {
        if(cam == null) cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.LookAt(gameObject.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
	}
}
