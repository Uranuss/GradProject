using UnityEngine;
using System.Collections;

public class Stage01BossAssistor : BossSkinnedObject {
	[SerializeField]
	Transform RootNode = null;

	// Use this for initialization
	void Start () {
		hideBomb();
		hideGun();
		transform.localPosition = Vector3.zero;
	}
	float angle = 0f;
	float tangle = 0f;
	readonly float ANGLESPEED = 10.0f;
	float angleTimer = 0;
	new protected void Update()
	{
		base.Update();
	}
	// Update is called once per frame
	protected void FixedUpdate () {
		if(updatable)
		{
			switch(state)
			{
				case MOUSESTATE.WAIT: break;
				case MOUSESTATE.UPMOVE: UpdateMoveUpDown(); break;
				case MOUSESTATE.DOWNMOVE: UpdateMoveUpDown(); break;
				case MOUSESTATE.OUT: UpdateOut(); break;
				case MOUSESTATE.GUNREADY: UpdateGun(); break;
				case MOUSESTATE.BOMBREADY: UpdateBomb(); break;
			}

			UpdateRopeRot();
		}
	}

	float timer = 0;
	void UpdateGun()
	{
		if(timer <= 0)
		{
			state = MOUSESTATE.OUT;
		}
		timer -= Time.deltaTime * GameController.Instance.AniScale;
	}

	void UpdateBomb()
	{
		if(timer <= 0)
		{
			state = MOUSESTATE.OUT;
		}
		timer -= Time.deltaTime * GameController.Instance.AniScale;
	}

	void UpdateOut()
	{
		if(timer <= 0)
		{
			state = MOUSESTATE.UPMOVE;
		}
		timer -= Time.deltaTime * GameController.Instance.AniScale;
	}

	public enum MOUSESTATE 
	{
		WAIT,
		DOWNMOVE,
		UPMOVE,
		GUNREADY,
		BOMBREADY,
		OUT,
	};

	MOUSESTATE _state = MOUSESTATE.WAIT;
	MOUSESTATE state 
	{
		get { return _state; }
		set {
			if(_state != value)
			{
				ExitState();
				_state = value;
				EnterState();
			}
		}
	}

	int actionType = 0;
	public void CallMouse(int type)
	{
		// gun or bomb
		if(MOUSESTATE.WAIT == state || MOUSESTATE.UPMOVE == state)
		{
			state = MOUSESTATE.DOWNMOVE;
			actionType = type;
		}
	}
	public bool isMouseActive {
		get { return (MOUSESTATE.WAIT != state && MOUSESTATE.UPMOVE != state); }
	}

	void EnterState()
	{
		switch(state)
		{
			case MOUSESTATE.OUT: timer = 1.5f; break;
			case MOUSESTATE.UPMOVE: animator.SetFloat("move",10f); break;
			case MOUSESTATE.DOWNMOVE: animator.SetFloat("move",-10f); break;
			case MOUSESTATE.GUNREADY: animator.SetBool("gun", true); timer = 5f; break;
			case MOUSESTATE.BOMBREADY: animator.SetTrigger("bomb"); timer = 5f; break;
		}
	}

	void ExitState()
	{
		switch(state)
		{
			case MOUSESTATE.UPMOVE: animator.SetFloat("move",0f); break;
			case MOUSESTATE.DOWNMOVE: animator.SetFloat("move",0f); break;
			case MOUSESTATE.GUNREADY: animator.SetBool("gun", false); break;
			case MOUSESTATE.BOMBREADY: animator.SetTrigger("fire"); break;
			case MOUSESTATE.OUT: animator.ResetTrigger("fire"); break;
		}
	}

	void UpdateMoveUpDown()
	{
		float y = animator.GetFloat("move");
		float t = Time.deltaTime * GameController.Instance.AniScale;
		if(Mathf.Abs(y) > 0.1)
		{
			transform.Translate(0,y*t,0,RootNode);
		}
		if(transform.localPosition.y < -10)
		{
			transform.localPosition = new Vector3(0,-10,0);
			switch(actionType)
			{
				case 0:
					state = MOUSESTATE.GUNREADY;
					break;
				case 1:
					state = MOUSESTATE.BOMBREADY;
					break;
			}
		}
		if(transform.localPosition.y > 0)
		{
			transform.localPosition = new Vector3(0,0,0);
			state = MOUSESTATE.WAIT;
		}
	}

	void UpdateRopeRot()
	{
		if(angle > 0)
		{
			float time = Time.deltaTime*GameController.Instance.AniScale;
			angle -= (angle * 0.5f) * time;
			angleTimer += time;
			if(angleTimer > 2)
			{
				angleTimer -= 2;
				if(angle < 1) angle = 0;
			}
			tangle = Mathf.Sin (angleTimer*Mathf.PI)*angle;
			RootNode.transform.rotation = Quaternion.Euler(0,0,tangle);
		}
	}

	public override void hitted(Vector3 attackDir, string hitName)
	{
		angle += 15f;
		if(attackDir.x > 0 && angleTimer > 0.5f && angleTimer < 1.5f)
			angleTimer = 0;
		if(attackDir.x < 0 && angleTimer < 0.5f && angleTimer > 1.5f)
			angleTimer = 1;
		animator.SetTrigger("hitted");
		state = MOUSESTATE.OUT;
	}

	void hittedEnd()
	{
		switch(state) {
			case MOUSESTATE.BOMBREADY:
				state = MOUSESTATE.UPMOVE;
				break;
			case MOUSESTATE.GUNREADY:
				state = MOUSESTATE.UPMOVE;
				break;
		}
	}

	[SerializeField]
	GameObject bombObject = null;
	[SerializeField]
	GameObject gunObject = null;

	void showBomb()
	{
		bombObject.SetActive(true);
	}
	void hideBomb()
	{
		bombObject.SetActive(false);
	}

	void showGun()
	{
		gunObject.SetActive(true);
	}
	void hideGun()
	{
		gunObject.SetActive(false);
	}
}
