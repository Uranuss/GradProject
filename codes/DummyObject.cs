using UnityEngine;
using System.Collections;

public class DummyObject : MonoBehaviour {
	void Awake() 
	{
	}

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
        Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
