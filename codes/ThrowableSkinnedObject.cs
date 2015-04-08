using UnityEngine;
using System.Collections;

public class ThrowableSkinnedObject : JumpableSkinnedObject {

	private PlayerSkinnedObject player = null;
	public void registPlayer(PlayerSkinnedObject pl) { if(player == null) player = pl; }

	private TrailRenderer trail = null;

	bool grabbed = false;
	bool throwed = false;
	bool endAction = false;
	float throwTimer = 0;

	const float THROWTIME = 1.0f;
	const float SPEED = 10f;
	const float MOVERATE = 1.5f;

	new protected void Awake()
	{
		base.Awake();
		if(trail == null)
		{
			trail = GetComponent<TrailRenderer>();
		}
	}

	// Use this for initialization
	new protected void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	new protected void Update () {
		base.Update();
		if(grabbed || throwed)
		{
			if(throwed) UpdateThrow();
		}
		else if (endAction)
		{
			UpdateEndAction();
		}
	}

	void UpdateThrow()
	{
		if( throwTimer > 0 )
		{
			float t = Time.deltaTime * GameController.Instance.AniScale;
			transform.Translate( new Vector3( t * SPEED, 0 , 0 ));
			throwTimer -= t;
		} else {
			throwTimer = 0;
			EndThrow();
		}
	}

	float endActionTimer = 0;
	void UpdateEndAction()
	{
		Vector3 pos = transform.position;
		float t = Time.deltaTime * GameController.Instance.AniScale;
		pos.x += -t * GameController.Instance.MoveSpeed * MOVERATE;
		if(pos.y > 0) pos.y -= t*(endActionTimer+1.0f)*9.8f; else pos.y = 0;
		transform.position = pos;
		endActionTimer+=t;
		if(pos.x < -7)
		{
			ExitEndAction();
		}
	}

	void ExitEndAction()
	{
		if(trail) trail.enabled = false;
		endAction = false;
		baseY = 0f;
		animator.ResetTrigger("fall");
		animator.ResetTrigger("jump");
		animator.ResetTrigger("floor");
		animator.SetBool("slide",false);
		animator.SetTrigger("throwend");
		jumpCancel();
		if(player) player.addSub(this);
	}

	void EndThrow()
	{
		animator.ResetTrigger("fall");
		animator.ResetTrigger("jump");
		animator.ResetTrigger("floor");
		animator.SetBool("slide",false);
		animator.SetTrigger("throwfail");
		transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
		throwed = false;
		endActionTimer = 0;
		endAction = true;
	}

	// Update is called once per frame
	new protected void FixedUpdate () {
		if(grabbed || throwed || endAction)
		{
			
		}
		else {
			base.FixedUpdate();
		}
	}

	public void Grabbed()
	{
		animator.SetTrigger("throw");
		jumpCancel();
		grabbed = true;
	}

	public void ThrowFailed()
	{
		grabbed = false;
		endAction = true;
		endActionTimer = 0;
		transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
        animator.SetTrigger("throwfail");
	}

	public void Throw()
	{
		grabbed = false;
		throwed = true;
		throwTimer = THROWTIME;
		if(trail) trail.enabled = true;
	}
}
