using UnityEngine;
using System.Collections;

public class WoodMover : MonoBehaviour {
	private float speed = 7f;
	void Start() {
	}

	void Update() {
		if(GameParameters.isPause)
			return;
		transform.Translate(speed * Time.deltaTime, 0, 0);
		if(transform.localPosition.x <= -20.0)
		{
			Destroy (gameObject);
		}
	}
}

public class WoodGenerator : MonoBehaviour {
	[SerializeField]
	private GameObject wood01 = null;
	[SerializeField]
	private GameObject wood02 = null;

	private float time = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(GameParameters.isPause)
			return;
		if(time >= 4f)
		{
			copyGen ();
			time -= 4f;
		}
		time += Time.deltaTime;
	}

	private void copyGen() {
		GameObject wood = null;
		switch(Random.Range(0, 2))
		{
			case 0:
				wood = (GameObject)Instantiate(wood01);
				break;
			case 1:
				wood = (GameObject)Instantiate(wood02);
				break;
		}
		wood.transform.SetParent(transform);
		wood.transform.localPosition = new Vector3(20, 0, 0);
		wood.transform.localScale = new Vector3(1,1,2);
		wood.AddComponent<WoodMover>();
		wood.SetActive(true);
	}
}
