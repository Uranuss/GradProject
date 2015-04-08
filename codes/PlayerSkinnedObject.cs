using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkinnedObject : JumpableSkinnedObject {
	[SerializeField]
	private float INVINCIBLE_TIME = 1.0f;
	[SerializeField]
	private Transform handTransform = null;

	const float CHAR_DISTANCE = 1.5f;
	const float JUMP_DELAY = 0.2f;
	const float FLIPTIMER = 0.1f;
    const int COLLISION_DAMAGE = 20;

	int PlayerHP = 100;
    
	[SerializeField]
	ThrowableSkinnedObject [] subChars = new ThrowableSkinnedObject [3];

	void SetDamage(int damage)
	{
		PlayerHP -= damage;
		if(PlayerHP <= 0)
		{
			//gameOver;
			GameController.Instance.state = GameController.GAMESTATE.GAMEOVER;
		}
	}

	private ThrowableSkinnedObject grabbedObject = null;

	// Use this for initialization
	new void Start () {
		base.Start();

		for(int i = 0; i < subChars.Length; ++i)
		{
			if(subChars[i]) {
				subChars[i].registPlayer(this);
				subChars[i].baseX = -((1+i) * CHAR_DISTANCE);
				subChars[i].MoveTo( -((1+i) * CHAR_DISTANCE));
			}
		}
	}
	
	// Update is called once per frame
	new void Update () {
		if(GameController.Instance.Controllable)
		{
			if(Input.touchSupported) {
				touchInputSystem();
			} else {
				mouseInputSystem();
			}
		}
		base.Update();
		if(updatable) UpdateInvincibleTime();
	}

	new void FixedUpdate()
	{
		base.FixedUpdate();
		if(updatable)
		{
			if(jumpActionFlag)
			{
				controlTimer += Time.deltaTime;
				if(controlTimer >= FLIPTIMER)
				{
					//jump
					Jump (0);
					for(int i = 0; i < subChars.Length; ++i)
					{
						if(subChars[i]) subChars[i].Jump((1+i) * JUMP_DELAY);
					}
					jumpActionFlag = false;
				} else if(moveYValue < -10)
				{
					moveYValue = 0;
					//slide
					ThrowFailed();
					Slide();
					for(int i = 0; i < subChars.Length; ++i)
					{
						if(subChars[i]) subChars[i].Slide ();
					}
					jumpActionFlag = false;
				}
			}
		}
	}
	
	void UpdateInvincibleTime()
	{
		float time = animator.GetFloat("invincible");
		if(time > 0)
		{
			time-=Time.deltaTime*GameController.Instance.AniScale;
			animator.SetFloat ("invincible",time);
		}
	}

	void grab()
	{
		if(subChars[0] != null && grabbedObject == null) 
		{
			grabTrigger();
		}
	}

	public void RealGrab()
	{
		grabbedObject = subChars[0];
		for(int i = 0; i < subChars.Length-1; ++i)
		{
			subChars[i] = subChars[i+1];
			if(subChars[i] != null)
				subChars[i].MoveTo( -((1+i) * CHAR_DISTANCE));
		}
		subChars[subChars.Length-1] = null;
		grabbedObject.transform.SetParent(handTransform);
		grabbedObject.transform.localRotation = Quaternion.identity;
		grabbedObject.transform.localPosition = new Vector3(0,-0.5f,0);
		grabbedObject.Grabbed();
	}

	public void RealThrow()
	{
		if(grabbedObject != null)
		{
			grabbedObject.transform.SetParent(transform.parent);
			grabbedObject.transform.localRotation = Quaternion.identity;
			grabbedObject.Throw();
			grabbedObject = null;
		}
	}

	void throwGrabbed()
	{
		throwTrigger();
	}

	public void addSub(ThrowableSkinnedObject sub)
	{
		for(int i = 0; i < subChars.Length; ++i)
		{
			if(subChars[i] == null)
			{
				subChars[i] = sub;
				subChars[i].transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
				subChars[i].baseX = -7f;
				subChars[i].MoveTo( -((1+i) * CHAR_DISTANCE));
				subChars[i].transform.localPosition = new Vector3(-7f,0,0);
				break;
			}
		}
	}

	protected virtual bool grabTrigger()
	{
		if(!animator.GetBool ("grab"))
		{
			animator.SetBool("grab", true);
			return true;
		}
		return false;
	}
	
	protected virtual void throwTrigger()
	{
		if(animator.GetBool ("grab"))
		{
			animator.SetBool("grab", false);
			animator.SetTrigger("throw");
			animator.ResetTrigger("throwfail");
		}
	}

	void ThrowFailed()
	{
		if(grabbedObject != null)
		{
			grabbedObject.transform.SetParent(transform.parent);
			if(grabbedObject.transform.position.y < 0)
			{
				Vector3 pos = grabbedObject.transform.position;
				pos.y = 0;
				grabbedObject.transform.position = pos;
			}
			grabbedObject.ThrowFailed();
			grabbedObject = null;
		}
		if(animator.GetBool ("grab")) animator.SetBool("grab", false);
	}

	protected override void collisionTrigger ()
	{
		if(animator.GetFloat("invincible") > 0) return;
		ThrowFailed();
		animator.SetFloat("invincible", INVINCIBLE_TIME);
		SetDamage(COLLISION_DAMAGE);
		base.collisionTrigger ();
	}

	void began(float x, float y, int id)
	{
		float width = UnityEngine.Screen.width;
		float height = UnityEngine.Screen.height;
		if(!isLeftTouched && x <= width/2f)
		{
			isLeftTouched = true;
			leftFingerID = id;
			controlTimer = 0;
			moveYValue = 0;
			jumpActionFlag = true;
			controlStart = new Vector2(x,y);
		}
		if(!isRightTouched && x > width/2f && x < width-100f)
		{
			if(!animator.GetBool("slide"))
			{
				isRightTouched = true;
				rightFingerID = id;
				grab();
			}
		}
	}

	void moved(float x, float y, int id)
	{
		if(id == leftFingerID)
		{
			float my = y - controlStart.y;
			if(moveYValue > my) moveYValue = my;
		}
		/*
		if(id == rightFingerID)
		{
		}
		*/
	}

	void ended(float x, float y, int id)
	{
		if(id == leftFingerID)
		{
			StandFromSlide();
			for(int i = 0; i < subChars.Length; ++i)
			{
				if(subChars[i]) subChars[i].StandFromSlide();
			}
			leftFingerID = -1;
			isLeftTouched = false;
		}
		if(id == rightFingerID)
		{
			// shot
			throwGrabbed();
			rightFingerID = -1;
			isRightTouched = false;
		}
	}

	void touchInputSystem()
	{
		// it use touch controll
		foreach (Touch touch in Input.touches) {
			switch (touch.phase) {
			case TouchPhase.Began:
				began (touch.position.x, touch.position.y, touch.fingerId);
				break;
			case TouchPhase.Moved:
				moved (touch.position.x, touch.position.y, touch.fingerId);
				break;
			case TouchPhase.Stationary:
				break;
			case TouchPhase.Ended:
				ended (touch.position.x, touch.position.y, touch.fingerId);
				break;
			case TouchPhase.Canceled:
				break;
			}
		}
	}

	void mouseInputSystem()
	{
		// control emulation on mouse
		if (Input.GetMouseButtonDown (0)) {
			began (Input.mousePosition.x, Input.mousePosition.y, 255);
		} else if (Input.GetMouseButtonUp (0)) {
			ended (Input.mousePosition.x, Input.mousePosition.y, 255);
		} else {
			moved (Input.mousePosition.x, Input.mousePosition.y, 255);
		}
	}

	private bool jumpActionFlag = false;
	private float controlTimer = 0;
	private float moveYValue = 0;
	private Vector2 controlStart = Vector2.zero;
	private int leftFingerID = -1;
	private int rightFingerID = -1;
	private bool isLeftTouched = false;
	private bool isRightTouched = false;
}
