using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpObject : MonoBehaviour {

	private int m_NowJumpCount = 0;
	[SerializeField]
	private int m_MaxJumpCount = 2;

	private float m_BaseX = 0;

	public void setX(float x)
	{
		m_BaseX = x;
	}

	[SerializeField]
	private Animator m_Animator = null;

	[SerializeField]
	private bool m_isPlayer = false;

	[SerializeField]
	private bool m_Slidable = false;
	[SerializeField]
	private bool m_DamageAnimation = false;

	private readonly float [] m_JUMPHEIGHT = { 4f, 3f };
	private readonly float m_JUMPTIME = 1f / 1.5f;

	private List<float> delayList = new List<float>();

	private ANIMATION_STATE m_State = ANIMATION_STATE.DEFAULT;
	private int m_StateCount = 0;

	public bool jumpAction = true;

	public ANIMATION_STATE state
	{
		get { return m_State; }
		set {
			if(m_Animator != null)
			{
				if(m_State != value)
				{
					switch(value)
					{
						case ANIMATION_STATE.DEFAULT:
							m_Animator.SetInteger("state", 0);
							break;
						case ANIMATION_STATE.JUMP:
							m_Animator.SetInteger("state", 1);
							break;
						case ANIMATION_STATE.DOUBLE_JUMP:
							m_Animator.SetInteger("state", 6);
							break;
						case ANIMATION_STATE.DROP:
							m_Animator.SetInteger("state", 3);
							break;
						case ANIMATION_STATE.SLIDE:
							if(m_Slidable) {
								gameObject.GetComponent<BoxCollider>().size = new Vector3(1,0.85f,1);
								gameObject.GetComponent<BoxCollider>().center = new Vector3(0.25f,0.425f,0);
							}
							m_Animator.SetInteger("state", 2);
							break;
						case ANIMATION_STATE.DAMAGED:
							m_Animator.SetInteger("state", 4);
							break;
					}
					if(m_State == ANIMATION_STATE.SLIDE && m_Slidable)
					{
						gameObject.GetComponent<BoxCollider>().size = new Vector3(1,1.7f,1);
						gameObject.GetComponent<BoxCollider>().center = new Vector3(0.25f,0.85f,0);
					}
					m_State = value;
					m_StateCount = 0;
				}
			}
		}
	}

	private bool isFloor
	{
		get { return m_isFloor; }
		set { m_isFloor = value; }
	}

	private float m_JumpTimer = 0f;
	private bool m_isFloor = true;

	private float m_BaseY = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(slideForce)
		{
			if(slide()) slideForce = false;
		}
		if(GameParameters.isPause) {
			if(m_Animator) m_Animator.speed = 0;
			return;
		} else {
			if(m_Animator) m_Animator.speed = 1;
		}
		if(!jumpAction) return;
		delayProcess();
		if(m_NowJumpCount > 0)
		{
			jumpPosition();
			timerUpdate();
		}
		flooring();
		stateManage();
	}

	private void stateManage()
	{
		if(m_StateCount > 0)
		{
			if(m_Animator != null) {
				if((ANIMATION_STATE.DAMAGED == state) && (m_Animator.GetCurrentAnimatorStateInfo(1).IsName("default")))
					state = ANIMATION_STATE.DEFAULT;
			}
			else if(state == ANIMATION_STATE.DAMAGED) state = ANIMATION_STATE.DEFAULT;
			if(state == ANIMATION_STATE.DOUBLE_JUMP) state = ANIMATION_STATE.JUMP;
		}
		m_StateCount++;
	}

	private void delayProcess()
	{
		if(delayList.Count > 0)
		{
			for(int i = 0; i < delayList.Count; ++i)
			{
				delayList[i] -= Time.deltaTime;
			}
			if(delayList[0] <= 0) 
			{
				realJump(delayList[0]);
				delayList.RemoveAt(0);
			}
		}
	}

	private bool realJump(float overDelay)
	{
		if(m_NowJumpCount < m_MaxJumpCount && state != ANIMATION_STATE.DAMAGED)
		{
			m_NowJumpCount++;
			m_JumpTimer = -overDelay;
			isFloor = false;
			m_BaseY = transform.position.y;
			switch(m_NowJumpCount)
			{
				case 1:
					state = ANIMATION_STATE.JUMP;
					break;
				case 2:
					state = ANIMATION_STATE.DOUBLE_JUMP;
					break;
			}

			return true;
		}
		return false;
	}

	private void flooring()
	{
		if(transform.position.y < 0)
		{
			m_BaseY = 0;
			isFloor = true;
			m_NowJumpCount = 0;
			m_JumpTimer = 0;
		}
		if(isFloor && m_NowJumpCount == 0)
		{
			m_NowJumpCount = 0;
			transform.position = new Vector3(m_BaseX, m_BaseY, 0);
			//anim.SetInteger("state",0);
			if(state == ANIMATION_STATE.JUMP || state == ANIMATION_STATE.DROP)
			{
				state = ANIMATION_STATE.DEFAULT;
			}
			m_JumpTimer = 0;
		}
	}

	private void jumpPosition()
	{
		if(m_JumpTimer < 0) return;
		float jumpHeight = m_JUMPHEIGHT[m_NowJumpCount-1];
		float y = (1.0f - Mathf.Pow(m_JumpTimer*(2f/m_JUMPTIME)-1.0f, 2f)) * jumpHeight;
		transform.position = new Vector3(m_BaseX, m_BaseY + y, 0);
	}

	private void timerUpdate()
	{
		m_JumpTimer+=Time.deltaTime;
	}

	public void damaged()
	{
		state = ANIMATION_STATE.DAMAGED;
	}

	void OnCollisionEnter(Collision collision) {
		if(GameParameters.isPause)
			return;
		if(!jumpAction) return;
		//print (gameObject.name + " and "  + collision.gameObject.name + "is Collision");
		if((collision.gameObject.layer == 9 || collision.gameObject.layer == 10) && m_NowJumpCount > 0 && !isFloor) {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.red);
				if(contact.normal.y > 0 && m_JumpTimer > m_JUMPTIME/2f) {
					m_BaseY = collision.gameObject.transform.position.y;
					isFloor = true;
					m_NowJumpCount = 0;
					m_JumpTimer = 0;
					//onFloor
				}
			}
		} else if(collision.gameObject.layer == 13) {
			if(m_DamageAnimation) {
				if(m_isPlayer) GetComponent<PlayerProperty>().damage(1);
			}
			damaged();
			//anim.SetInteger("state", 4);
			//anim.SetInteger("state", 0);
		} else {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.blue);
			}
		}
	}
	
	void OnCollisionExit(Collision collision) {
		if(GameParameters.isPause)
			return;
		if(!jumpAction) return;
		if(collision.gameObject.layer == 10 && m_NowJumpCount == 0 && isFloor) {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.white);
			}
			//outFloor
			m_BaseY = collision.gameObject.transform.position.y - m_JUMPHEIGHT[0];
			m_JumpTimer = m_JUMPTIME/2;
			m_NowJumpCount=1;
			isFloor = false;
			//anim.SetInteger("state",3);
			state = ANIMATION_STATE.DROP;
		} else {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.blue);
			}
		}
	}

	public bool jump(float delay) {
		if( delay > 0 )
		{
			delayList.Add(delay);
		}
		else{
			return realJump(0);
		}
		return false;
	}

	private bool slideForce = false;

	public bool slide(bool force = false)
	{
		if((m_JumpTimer < GameParameters.ControlFlipTimer && m_NowJumpCount < 2 && (state == ANIMATION_STATE.JUMP || state == ANIMATION_STATE.DEFAULT)))
		{
			if(state == ANIMATION_STATE.JUMP)
			{
				m_JumpTimer = 0;
				m_NowJumpCount = 0;
				m_isFloor = true;
				transform.position = new Vector3(m_BaseX, m_BaseY, 0);
			}
			if(delayList.Count > 0)
			{
				delayList.Clear();
			}
			state = ANIMATION_STATE.SLIDE;
			return true;
		}
		if(force) slideForce = true;
		return false;
	}

	public void stopSlide()
	{
		slideForce = false;
		if(state == ANIMATION_STATE.SLIDE)
		{
			state = ANIMATION_STATE.DEFAULT;
		}
	}

	public enum ANIMATION_STATE
	{
		DEFAULT,
		JUMP,
		DOUBLE_JUMP,
		DROP,
		SLIDE,
		HITTED,
		DAMAGED
	}
}
