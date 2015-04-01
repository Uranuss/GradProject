using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour {

	[SerializeField]
	private TestItemButton storeCall = null;
	// Use this for initialization
	void Start () {
	
	}

	float angle = 180;
	// Update is called once per frame
	void Update () {
		transform.localRotation = transform.localRotation * Quaternion.AngleAxis(angle * Time.deltaTime, Vector3.up);
	}


	void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == "Player")
		{
			storeCall.MakeStoreBlock();
			BGMManager.Instance.playEffect(20);
			Destroy(gameObject);
		}
	}
	void OnTriggerExit(Collider collider)
	{
	}

}
