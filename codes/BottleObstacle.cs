using UnityEngine;
using System.Collections;

public class BottleObstacle : MonoBehaviour {
	float SPEED = 0f;
	float ROTATE = 0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateThrow();
		UpdateRoll();
		UpdateZ();
	}

	float accel = 5f;
	void UpdateThrow()
	{
		float t = Time.deltaTime * GameController.Instance.AniScale;
		accel += t*9.8f;
		transform.Translate (0, -t * accel, 0, transform.parent);
		if(transform.position.y < 0.3f)
		{
			Vector3 pos = transform.position;
			pos.y = 0.3f;
			transform.position = pos;
		}

	}

	void UpdateZ()
	{
		if(transform.position.z != 0.8f)
		{
			Vector3 pos = transform.position;
			float ss = 50f;
			pos.z -= ss*Time.deltaTime;
			if(pos.z < 0.8f) pos.z = 0.8f;
			transform.position = pos;
		}
	}

	void UpdateRoll()
	{
		float t = Time.deltaTime * GameController.Instance.SpeedScale;
		transform.Translate(-t * (GameController.Instance.MoveSpeed + SPEED), 0, 0, transform.parent);
		transform.Rotate(0,0,t*ROTATE);
		
		if(transform.position.x < -10)
		{
			Destroy (gameObject);
		}
	}
}
