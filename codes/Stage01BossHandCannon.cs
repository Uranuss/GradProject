using UnityEngine;
using System.Collections;

public class Stage01BossHandCannon : MonoBehaviour {
	[SerializeField]
	Animator animator = null;
	[SerializeField]
	float COOLTIME = 2.0f;
	[SerializeField]
	Transform boneTransform = null;
	[SerializeField]
	LineRenderer lineRenderer = null;
	// Use this for initialization
	void Start () {
	}
	float coolTime = 0.1f;
	// Update is called once per frame
	void Update () {
		if(coolTime == 0)
		{
			Vector3 dir = -boneTransform.position;
			dir.y = 0;
			RaycastHit hit;
			if(Physics.Raycast(boneTransform.position,dir, out hit, 40, LayerMask.GetMask("Player")))
			{
				PlayerSkinnedObject player =  hit.collider.GetComponent<PlayerSkinnedObject>();
				if(player)
				{
					player.Attack(10);
					animator.SetTrigger("fire");
					SoundManager.Instance.playEffect(14);
					coolTime = COOLTIME;
					lineRenderer.enabled = false;
				}
			}
		} else {
			coolTime -= Time.deltaTime;
			if(coolTime < 0) {
				coolTime = 0;
				SoundManager.Instance.playEffect(13);
				lineRenderer.enabled = true;
			}
		}
	}
}
