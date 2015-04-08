using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageAssembler : MonoBehaviour {
	[SerializeField]
	private GameObject StartBlock = null;
	[SerializeField]
	private GameObject [] NormalFloorBlock = null;
	[SerializeField]
	private GameObject StoreFloorBlock = null;
	[SerializeField]
	private CanvasGroup StoreUIGroup = null;

	[SerializeField]
	private float regenFloorPoint = 26f;

	List<GameObject> list = new List<GameObject>();

	const float SIZEOFBLOCK = 36f;

	void Awake()
	{
		if(StartBlock != null)
		{
			AddBlock (StartBlock, -SIZEOFBLOCK);
			AddBlock (StartBlock, 0);
		}
	}

	private void AddBlock(GameObject origin, float xPos)
	{
		GameObject go = (GameObject)Instantiate(origin);
		go.SetActive(true);
		go.transform.SetParent(transform);
		go.transform.position = new Vector3(xPos,0,0);
		list.Add(go);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	private bool makeStore = false;
	// Update is called once per frame
	void FixedUpdate () {
		float move = Time.deltaTime * GameController.Instance.MoveSpeed * GameController.Instance.SpeedScale;
		for(int i = 0; i < list.Count; i++)
		{
			list[i].transform.Translate(-move, 0, 0);
			if(list[i].transform.position.x < -52) 
			{
				GameObject go = list[i];
				list.RemoveAt(i);
				Destroy(go);
				i--;
			}
		}
		if(makeStore)
		{
			GameObject go = (GameObject)Instantiate(StoreFloorBlock);
			go.SetActive(true);
			go.transform.SetParent(transform);
			go.transform.position = new Vector3(list[list.Count-1].transform.position.x+36.0f, 0, 0);
			list.Add(go);
			makeStore = false;
		}
		if(list[list.Count-1].transform.position.x+36.0f <= regenFloorPoint) {
			int index = Random.Range(0, NormalFloorBlock.Length);
			GameObject go = (GameObject)Instantiate(NormalFloorBlock[index]);
			go.SetActive(true);
			go.transform.SetParent(transform);
			go.transform.position = new Vector3(list[list.Count-1].transform.position.x+36.0f, 0, 0);
			list.Add(go);
		}
	}

	public void MakeStoreBlock()
	{
		makeStore = true;
	}
}
