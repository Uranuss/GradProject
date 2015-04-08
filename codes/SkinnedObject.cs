using UnityEngine;
using System.Collections;

// 모든 애닝메이션을 갖는 오브젝트의 기본형.
// 시스템 메뉴에 의한 중지. 애니메이션 스피드 조절.
// 공통적인 애니메이션 관리를 담당.


public class SkinnedObject : MonoBehaviour {

	[SerializeField]
	protected Animator animator = null;

	private bool _updatable = true;
	protected bool updatable {
		get { return _updatable; }
	}

	protected void Awake()
	{
		if(animator == null)
		{
			animator = GetComponent<Animator>();
			if(animator == null)
				animator = GetComponentInChildren<Animator>();
			if(animator == null)
				Debug.Break();
		}
	}

	// Use this for initialization
	protected void Start () {
	}
	
	// Update is called once per frame
	protected void Update () {
		float scale = GameController.Instance.AniScale;
		if(scale > 0)
			_updatable = true;
		else
			_updatable = false;
		animator.speed = scale;
	}
}
