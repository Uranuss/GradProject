using UnityEngine;
using System.Collections;

public class FloorGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject baseGround = null;
	// Use this for initialization
	private GameObject [] floorInstance = new GameObject[10];
	void Start () {
		for(int i = 0; i < 10; ++i)
		{
			floorInstance[i] = (GameObject)Instantiate(baseGround);
			floorInstance[i].SetActive(true);
			floorInstance[i].transform.SetParent(transform);
			floorInstance[i].transform.position = new Vector3(i*5 - 20, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(GameParameters.isPause)
			return;

		for(int i = 0; i < 10; ++i)
		{
			floorInstance[i].transform.Translate(GameParameters.MoveSpeed*Time.deltaTime, 0, 0);
			if(floorInstance[i].transform.position.x <= -25.0f)
			{
				floorInstance[i].transform.Translate(-50.0f, 0, 0);
			}
		}
	}
}
