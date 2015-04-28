using UnityEngine;
using System.Collections;

public class BossCollisionChecker : MonoBehaviour {
	[SerializeField]
	BossSkinnedObject boss = null;
	[SerializeField]
	Renderer bossRenderer = null;
	[SerializeField]
	string collisionName = "";

	bool _invincible = false;
	public bool isInvincible {
		get { return _invincible; }
		set { _invincible = value; }
	}

	float timer = 0f;
	Color baseColor = Color.white;
	void Awake()
	{
		if(bossRenderer == null && renderer != null)
		{
			bossRenderer = renderer;
		}
		baseColor = bossRenderer.material.color;
	}
	void FixedUpdate()
	{
		if(timer > 0)
		{
			bossRenderer.material.color = Color.red;
			GameController.Instance.freeze(true);
		}
		else
		{
			GameController.Instance.freeze(false);
			bossRenderer.material.color = baseColor;
		}
		timer -= Time.deltaTime;
		if(timer < 0) timer = 0;
	}

	void OnTriggerEnter(Collider tCollider)
	{
		if(_invincible) return;
		if(tCollider.tag == "Player" && tCollider.gameObject.layer == 11)
		{
			ThrowableSkinnedObject tObj = tCollider.gameObject.GetComponent<ThrowableSkinnedObject>();
			if(tObj && tObj.isThrowed)
			{
				boss.hitted(tObj.dir, collisionName);
				timer = 0.05f;
			}
		}
	}

	void OnTriggerExit(Collider tCollider)
	{
	}
}
