using UnityEngine;
using System.Collections;

public class Stage01BossSkinnedObject : BossSkinnedObject {

	[SerializeField]
	float BaseX = 13f;
	float AttackX = 2f;
	float StartX = 36f;
	bool attack = false;

	[SerializeField]
	Stage01BossAssistor Assistor = null;

	readonly float SPEED = 10f;

	new protected void Awake()
	{
		Vector3 pos = transform.position;
		pos.x = StartX;
		transform.position = pos;
		base.Awake();
	}

	// Use this for initialization
	new protected void Start () {
        BottleHide();
		clubTrail.enabled = false;
		shieldAnim.speed = GameController.Instance.AniScale;
		base.Start ();
	}

	float waitTimer = 0f;
	// Update is called once per frame
	new protected void Update () {
		base.Update ();
		if(!updatable) return;
		switch(state)
		{
			case BOSS1STATE.INTRO: UpdateIntro(); break;
			case BOSS1STATE.BASE: UpdateBaseState(); break;
			case BOSS1STATE.BOTTLE: UpdateBottle(); break;
		case BOSS1STATE.CALLSUBGUN: state=BOSS1STATE.BASE; break;
		case BOSS1STATE.CALLSUBBOMB: state=BOSS1STATE.BASE; break;
		case BOSS1STATE.CLUB: break;
		case BOSS1STATE.CLUBH: UpdateAttack(); break;
		case BOSS1STATE.CLUBM: UpdateAttack(); break;
		case BOSS1STATE.CLUBL: UpdateAttack(); break;
		case BOSS1STATE.GUARD: state=BOSS1STATE.BASE; break;
		case BOSS1STATE.GUARDBOTTLE: state=BOSS1STATE.BASE; break;
		}
		UpdateShock();
	}

	void UpdateIntro()
	{
		float moveX = BaseX-transform.position.x;
		float dir = 0;
		if(moveX > SPEED*Time.deltaTime) dir = 1;
		else if(moveX < -SPEED*Time.deltaTime) dir = -1;
		else state = BOSS1STATE.BASE;
		transform.Translate(GameController.Instance.AniScale * Time.deltaTime * dir * SPEED, 0 , 0, transform.parent);
	}

	BOSS1STATE _state = BOSS1STATE.INTRO;
	BOSS1STATE state {
		get { return _state; }
		set {
			if(value != _state)
			{
				_exitState();
				_state = value;
				_enterState();
			}
		}
	}
	void _exitState()
	{
		switch(_state)
		{
			case BOSS1STATE.BASE: break;
			case BOSS1STATE.BOTTLE: animator.SetBool("bottle",false); break;
			case BOSS1STATE.CALLSUBGUN: if(!Assistor.isMouseActive) { Assistor.CallMouse(0); coolTimer = 3f; } break;
			case BOSS1STATE.CALLSUBBOMB: if(!Assistor.isMouseActive) { Assistor.CallMouse(1); coolTimer = 3f; } break;
			case BOSS1STATE.CLUB: break;
			case BOSS1STATE.CLUBH: animator.SetBool("high",false); clubTrail.enabled = true; break;
			case BOSS1STATE.CLUBM: animator.SetBool("mid",false); clubTrail.enabled = true; break;
			case BOSS1STATE.CLUBL: animator.SetBool("low",false); clubTrail.enabled = true; break;
			case BOSS1STATE.GUARD: break;
			case BOSS1STATE.GUARDBOTTLE: break;
		}
	}
	void _enterState()
	{
		switch(_state)
		{
			case BOSS1STATE.BASE: break;
			case BOSS1STATE.BOTTLE: animator.SetBool("bottle",true); break;
			case BOSS1STATE.CALLSUBGUN: break;
			case BOSS1STATE.CALLSUBBOMB: break;
			case BOSS1STATE.CLUB: 
				switch(Random.Range(0,3))
				{
					case 0: state = BOSS1STATE.CLUBH; break;
					case 1: state = BOSS1STATE.CLUBM; break;
					case 2: state = BOSS1STATE.CLUBL; break;
				}
				break;
			case BOSS1STATE.CLUBH: animator.SetBool("high",true); break;
			case BOSS1STATE.CLUBM: animator.SetBool("mid",true); break;
			case BOSS1STATE.CLUBL: animator.SetBool("low",true); break;
			case BOSS1STATE.GUARD: break;
			case BOSS1STATE.GUARDBOTTLE: break;
		}
	}

	private enum BOSS1STATE
	{
		INTRO,
		BASE,
		BOTTLE,
		CALLSUBGUN,
		CALLSUBBOMB,
		CLUB,
		CLUBH,
		CLUBM,
		CLUBL,
		GUARD,
		GUARDBOTTLE
	};
	float coolTimer = 5f;
	void UpdateBaseState()
	{
		if(coolTimer <= 0)
		{
			ChangeState();
			coolTimer = 0;
		}
		else coolTimer -= Time.deltaTime * GameController.Instance.AniScale;
	}
	void ChangeState()
	{
		switch(state)
		{
			case BOSS1STATE.BASE:
				switch(Random.Range(0,10))
				{
					case 0: state = BOSS1STATE.BOTTLE; break;
					case 1: state = BOSS1STATE.CALLSUBGUN; break;
					case 2: state = BOSS1STATE.BOTTLE; break;
					case 3: state = BOSS1STATE.CALLSUBGUN; break;
					case 4: state = BOSS1STATE.BOTTLE; break;
					case 5: state = BOSS1STATE.CALLSUBBOMB; break;
					case 6: state = BOSS1STATE.CALLSUBGUN; break;
					case 7: state = BOSS1STATE.CALLSUBBOMB; break;
					case 8: state = BOSS1STATE.BOTTLE; break;
					case 9: state = BOSS1STATE.CLUB; break;
				}
				break;
			case BOSS1STATE.GUARD:
				switch(Random.Range(0,9))
				{
					case 0: state = BOSS1STATE.BOTTLE; break;
					case 1: state = BOSS1STATE.CALLSUBGUN; break;
					case 2: state = BOSS1STATE.BOTTLE; break;
					case 3: state = BOSS1STATE.CALLSUBGUN; break;
					case 4: state = BOSS1STATE.BOTTLE; break;
					case 5: state = BOSS1STATE.CALLSUBBOMB; break;
					case 6: state = BOSS1STATE.CALLSUBGUN; break;
					case 7: state = BOSS1STATE.CALLSUBBOMB; break;
					case 8: state = BOSS1STATE.BOTTLE; break;
				}
				break;
			case BOSS1STATE.BOTTLE:
				state = BOSS1STATE.BASE;
				break;
		}
		actionTimer = 0;
	}

	float actionTimer = 0;
	void UpdateBottle()
	{
		if(actionTimer >= 1.0f) { 
			state = BOSS1STATE.BASE;
			coolTimer = 5.0f;
		}
		actionTimer += Time.deltaTime * GameController.Instance.AniScale;
	}
	void UpdateAttack()
	{
		if(actionTimer <= 1.0f) { 

		} else if(actionTimer <= 2.0f) {
			transform.Translate(Time.deltaTime * GameController.Instance.AniScale * -20f, 0, 0, transform.parent);
		} else if(actionTimer <= 3.0f) {
			state = BOSS1STATE.BASE;
			coolTimer = 5.0f;
		}
		actionTimer += Time.deltaTime * GameController.Instance.AniScale;
	}

	float attackShock = 0f;
	float SHOCKSPEED = 5f;
	void UpdateShock()
	{
		float t = Time.deltaTime * GameController.Instance.AniScale;
		if(Mathf.Abs(attackShock) > 0.1f)
		{
			transform.Translate(t*attackShock, 0, 0, transform.parent);
			attackShock*=0.9f;
		}
		else 
		{
			attackShock = 0f;
		}
		float moveX = BaseX-transform.position.x;
		float dir = 0;
		if(moveX > SHOCKSPEED*Time.deltaTime) dir = 1;
		else if(moveX < -SHOCKSPEED*Time.deltaTime) dir = -1;
		transform.Translate(GameController.Instance.AniScale * Time.deltaTime * dir * SHOCKSPEED, 0 , 0, transform.parent);
	}

	public override void hitted(Vector3 attackDir, string hitName)
	{
		if(hitName == "body")
		{
			animator.SetTrigger("hitted");
			attackShock += attackDir.x * 15f;
		} else {
			if(hitName == "shield")
			{
				shieldAnim.SetTrigger("hitted");
			} else if(hitName == "boat")
			{
				if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
					animator.SetTrigger("boatHitted");
			}
			attackShock += attackDir.x * 20f;
		}
	}

    // Animation Supoort Function
	void ChangeBoat()
	{
		boat.SetActive(false);
		foreach(GameObject obj in crackedBoat)
		{
			obj.SetActive(true);
		}
	}
	void DeadBoss()
	{
		dead();
	}

	[SerializeField]
	Animator shieldAnim = null;

	void guard()
	{
		shieldAnim.SetBool("guard", true);
	}
	void unguard()
	{
		shieldAnim.SetBool("guard", false);
	}

	[SerializeField]
	GameObject boat = null;
	[SerializeField]
	GameObject [] crackedBoat = null;
    [SerializeField]
    GameObject attackBottle = null;
	[SerializeField]
	TrailRenderer clubTrail = null;

    void BottleShow()
    {
        if(attackBottle) attackBottle.SetActive(true);
    }

	void TrailHide()
	{
		clubTrail.enabled = false;
	}

	[SerializeField]
	GameObject BottlePrefab = null;
	[SerializeField]
	Transform HandTransform = null;
	void BottleThrow()
	{
		//Attack Bottle Throw
		GameObject bottle = (GameObject)Instantiate(BottlePrefab);
		bottle.transform.position = HandTransform.position;
	}

    void BottleHide()
    {
        if(attackBottle) attackBottle.SetActive(false);
    }
}
