using UnityEngine;
using System.Collections;

public class RumbleHalo : MonoBehaviour {
	[SerializeField]
	private float TIMERATE = 1.0f;


	// Use this for initialization
	void Start () {
	}

	private float time = 0;
	// Update is called once per frame
	void Update () {
		float rate = (time/TIMERATE);
		float v = Mathf.Cos(rate*Mathf.PI);


		time += Time.deltaTime;
		if(time >= TIMERATE)
		{
			time-=TIMERATE;
		}
	}
}
