using UnityEngine;
using System.Collections;

public class BossSkinnedObject : SkinnedObject {

	// Use this for initialization
	new protected void Start () {
		GameController.Instance.BossStart(gameObject);
		base.Start();
	}

	protected void dead()
	{
		GameController.Instance.BossEnded();
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	new protected void Update () {
		base.Update();
	}

	virtual public void hitted(Vector3 attackDir, string hitName) {}
}
