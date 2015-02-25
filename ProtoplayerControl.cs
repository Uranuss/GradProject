using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Type_2_PlayerControl : MonoBehaviour {
	/*
	public LayerMask m_CollisionMask;

	public EventSystem m_EventSystem;

	private Transform m_Transform = null;
	private Transform m_HandTransform = null;

	public GameObject m_Chakram = null;

	public Material m_PlayerMaterial = null;

	private float m_InvicibleTime = 1f;
	private float m_PlayerSpeed = 10f;
	private float m_JumpSpeed = 1f;

	private bool isMouseClicking = false;
	private bool isPlayerHitFromEnemy = false;
	private bool isJumpButtonDown = false;
	private bool isJumping = false;
	private bool isJumpDown = false;
	private bool isButtonClick = false;
	private bool isScreenClick = false;

	private Vector3 m_PlayerFirstPosition = Vector3.zero;
	private Vector3 fireVector = Vector3.zero;
	private Vector3 mSpeed = Vector3.zero;

	void Start () {
		m_Transform = this.transform;
		m_HandTransform = GameObject.Find("Hand").transform;

		m_PlayerMaterial.color = new Color(1f, 1f, 1f);

		m_PlayerFirstPosition = m_Transform.position;
	}

	void Update () {

		//▼캐릭터의 이동.
		m_Transform.Translate(new Vector3(Time.deltaTime * m_PlayerSpeed,
		                                  0f,
		                                  0f));
		//▲캐릭터의 이동.

		//▼캐릭터의 점프.
		if(isJumpButtonDown && !isJumping)
		{
			isJumpButtonDown = false;
			isJumping = true;
			m_JumpSpeed = 3f;
			mSpeed.y = 5.0f;
		}

//		if(isJumping)
//		{
//			if(isJumpDown && m_Transform.position.y >= m_PlayerFirstPosition.y)
//			{
//				m_JumpSpeed += Time.deltaTime * 9.8f;
//				m_Transform.Translate(Vector3.down * m_JumpSpeed * Time.deltaTime * 10f);
//				if(m_Transform.position.y <= m_PlayerFirstPosition.y)
//				{
//					m_JumpSpeed = 3f;
//					m_Transform.position = new Vector3(m_Transform.position.x,
//					                                   m_PlayerFirstPosition.y,
//					                                   m_Transform.position.z);
//					isJumpDown = false;
//					isJumping = false;
//				}
//			}
//			else if(m_Transform.position.y >= 0.93f && !isJumpDown)
//			{
//				isJumpDown = true;
//				m_JumpSpeed = 0f;
//			}
//			else
//			{
//				m_JumpSpeed -= Time.deltaTime * 9.8f;
//				if(m_JumpSpeed <= 1f)
//				{
//					m_JumpSpeed = 1f;
//				}
//				print(m_JumpSpeed);
//				m_Transform.Translate(Vector3.up * m_JumpSpeed * Time.deltaTime * 10f);
//			}
//		}
		//▲캐릭터의 점프.

		//▼캐릭터의 점프2.
		mSpeed.y -= 9.8f * Time.deltaTime;;
		Vector3 pos = m_Transform.position;
		m_Transform.Translate(0, mSpeed.y * Time.deltaTime, 0);
		if (m_Transform.position.y <= m_PlayerFirstPosition.y) {
			pos = m_Transform.position;
			m_Transform.position = new Vector3(pos.x ,m_PlayerFirstPosition.y, pos.z);
			mSpeed = Vector3.zero;
			if(isJumping) isJumping = false;
		}
		//▲캐릭터의 점프2.

		//▼체크럼 발사.
		if(m_EventSystem.IsPointerOverGameObject(0))
		{
			isButtonClick = true;
		}
		else 
		{
			isButtonClick = false;
		}

		for(int i=0; i < Input.touchCount; i++)
		{
			int index = 0;

			if(isButtonClick)
			{
				index = 1;
			}

			fireVector = new Vector3(Camera.main.ScreenToWorldPoint(Input.GetTouch(index).position).x,
			                         Camera.main.ScreenToWorldPoint(Input.GetTouch(index).position).y,
			                         0f) - m_HandTransform.position;
			
			if(Input.GetTouch(index).phase == TouchPhase.Began)
			{
				isMouseClicking = true;
				isScreenClick = true;
				
			}else if(Input.GetTouch(index).phase == TouchPhase.Ended && isScreenClick)
			{
				isMouseClicking = false;
				isScreenClick = false;
				StartCoroutine(ThrowChakram(fireVector));
			}
			
			if(isMouseClicking)
			{
				m_HandTransform.rotation = Quaternion.FromToRotation(Vector2.right, fireVector.normalized);
			}
		}
		//▲체크럼 발사.
	}

	private IEnumerator ThrowChakram(Vector3 fireVector)
	{
		GameObject chakram = Instantiate(m_Chakram, m_HandTransform.position, m_HandTransform.rotation) as GameObject;
		yield return null;
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Enemy")
		{
			if(!isPlayerHitFromEnemy)
			{
				isPlayerHitFromEnemy = true;
				m_PlayerMaterial.color = new Color(1f, 0f, 0f);
				StartCoroutine(Invincible());
			}
		}
	}

	private IEnumerator Invincible()
	{
		yield return new WaitForSeconds(m_InvicibleTime);
		m_PlayerMaterial.color = new Color(1f, 1f, 1f);
		isPlayerHitFromEnemy = false;
	}

	public float GetPlayerSpeed()
	{
		return m_PlayerSpeed;
	}

	public void UpSpeed()
	{
		m_PlayerSpeed += 1f;
		if(m_PlayerSpeed >= 13f)
		{
			m_PlayerSpeed = 13f;
		}
	}

	public void DownSpeed()
	{
		m_PlayerSpeed -= 1f;
		if(m_PlayerSpeed <= 3f)
		{
			m_PlayerSpeed = 3f;
		}
	}

	public void ClickJumpButtonDown()
	{
		isJumpButtonDown = true;
	}

	public void ClickJumpButtonUp()
	{
		isJumpButtonDown = false;
	}
	*/
}
