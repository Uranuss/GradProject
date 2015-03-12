using UnityEngine;
using System.Collections;

public class HPBarSetter : MonoBehaviour {

	[SerializeField]
	private GameObject life_bar = null;

	// Use this for initialization
	void Start () {
	
	}

	//private float timer = 0;
	// Update is called once per frame
	void Update () {
		/*
		if(timer >= 1.0f)
		{
			float rate = Random.Range(0f,1f);
			setHPRate(rate);
			timer -= 1.0f;
		}
		timer += Time.deltaTime;
		*/
	}

	public void setHPRate(float rate)
	{
		life_bar.transform.localScale = new Vector3(rate,1f,1f);
		life_bar.transform.localPosition = new Vector3(-(1.0f-rate)*4.9f/2f, 0, 0);
	}
}
