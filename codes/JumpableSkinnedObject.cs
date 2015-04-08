using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 스킨 애니메이션 되는 오브젝트중 점프 행동이 들어가는 오브젝트.

public class JumpableSkinnedObject : SkinnedObject {
	//Settable Variable
	[SerializeField]
	private int jumpCount = 2;
	[SerializeField]
	private float jumpTime = 0.66f;
	[SerializeField]
	private float [] jumpHeight = new float[2] { 4.0f, 3.0f };

	public float baseX = 0;
	public float baseY = 0;

	const float SLIDEINPUTTIME = 0.1f;

	private float targetX = 0;
	private float moveTime = 0.3f;
	public void MoveTo(float x)
	{
		targetX = x;
	}

	// Use this for initialization
	new protected void Start () {
		base.Start ();
	}

	// Update is called once per frame
	new protected void Update () {
		//print ("normal : " + Time.deltaTime.ToString());
		base.Update();
	}

	protected void FixedUpdate()
	{
		if(updatable)
		{
			delayProcess();
			UpdateJump();
			UpdateMove();
		}
	}

	void UpdateMove()
	{
		if(baseX != targetX)
		{
			float t = (Time.deltaTime*GameController.Instance.AniScale) / moveTime;
			if( targetX - baseX > t) baseX += t;
			else if( targetX - baseX < -t) baseX -= t;
			else baseX = targetX;
		}
	}

	protected void UpdateJump()
	{
		if(_jumpCount > 0)
		{
			if(_jumpTimer > 0) {
				float height = jumpHeight[_jumpCount-1];
				float y = (1.0f - Mathf.Pow(_jumpTimer*(2f/jumpTime)-1.0f, 2f)) * height;
				transform.position = new Vector3(baseX, baseY + y, 0);

				if(baseY + y < -4f) {
					dropEvent();
				}
			}
			_jumpTimer+=Time.deltaTime*GameController.Instance.AniScale;
		} else {
			transform.position = new Vector3(baseX, baseY, 0);
		}
	}

	public void Jump(float delay)
	{
		if( delay > 0 )
		{
			delayList.Add(delay);
		}
		else{
			realJump(0);
		}
	}

	protected void jumpCancel()
	{
		delayList.Clear();
		_jumpCount = 0;
		_jumpTimer = 0;
	}

	void floor()
	{
		_jumpCount = 0;
		_jumpTimer = 0;
		floorTrigger();
	}

	protected virtual void jumpTrigger()
	{
		animator.SetTrigger("jump");
		animator.ResetTrigger("floor");
	}
	protected virtual void floorTrigger()
	{	
		animator.ResetTrigger("jump");
		animator.SetTrigger("floor");
	}
	protected virtual void fallTrigger()
	{	
		animator.ResetTrigger("floor");
		animator.SetTrigger("fall");
	}
	protected virtual void slideValue(bool slide)
	{
		animator.SetBool("slide", slide);
	}
	protected virtual void collisionTrigger()
	{
		animator.SetTrigger("collision");
	}
	public void ResetCollision()
	{
		animator.ResetTrigger("collision");
	}
	protected virtual void dropEvent()
	{
		print ("dropped");
		_jumpCount = 2;
		_jumpTimer = jumpTime/2;
		baseY = 4f-jumpHeight[1];
	}

	private bool slide = false;
	public void Slide()
	{
		if(!slide && _jumpTimer < SLIDEINPUTTIME && _jumpCount < 2)
		{
			if(_jumpCount > 0)
			{
				floor ();
			}
			slideValue(true);
			slide = true;
			BoxCollider box = GetComponent<BoxCollider>();
			Vector3 center = box.center;
			Vector3 size = box.size;
			center.y/=2;
			size.y/=2;
			box.center = center;
			box.size = size;
		}
	}
	public void StandFromSlide()
	{
		if(slide)
		{
			slideValue(false);
			slide = false;
			BoxCollider box = GetComponent<BoxCollider>();
			Vector3 center = box.center;
			Vector3 size = box.size;
			center.y*=2;
			size.y*=2;
			box.center = center;
			box.size = size;
		}
	}

	private void realJump(float delayOffset)
	{
		if(_jumpCount < jumpCount)
		{
			baseY = transform.position.y;
			jumpTrigger();
			_jumpCount++;
			_jumpTimer = -delayOffset;
		}
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
	
	void OnCollisionEnter(Collision collision) {
		int layer = collision.gameObject.layer;
		print ("enter " + collision.gameObject.name);
		switch(layer)
		{
			case 9:
			case 10:
				{
					if(_jumpTimer > jumpTime/2f)
					{
						baseY = collider.transform.position.y;
						floor ();
					}
				}
				break;
			case 13:
				{
					collisionTrigger();
				}
				break;
		}
	}

	void OnCollisionExit(Collision collision) {
	}

	void OnTriggerEnter(Collider tCollider)
	{
		int layer = tCollider.gameObject.layer;
		switch(layer)
		{
		case 9:
		case 10:
		{
			if(transform.position.y + 0.25f >= tCollider.transform.position.y)
			{
				if(_jumpTimer > jumpTime/2f)
				{
					baseY = tCollider.transform.position.y;
					floor ();
				}
			}
			_collisionCount++;
		}
			break;
		case 13:
		{
			collisionTrigger();
		}
			break;
		}

		/*
		if(collider.GetType () == typeof(BoxCollider))
		{
			BoxCollider box = (BoxCollider)collider;
			print(box.center.ToString());
			print( ((BoxCollider)this.collider).center.ToString() ); 
		}
		if(_jumpTimer > jumpTime/2f)
		{
			baseY = collider.transform.position.y;
			floor ();
		}
		*/
	}
	
	void OnTriggerExit(Collider collider)
	{
		int layer = collider.gameObject.layer;
		switch(layer)
		{
		case 9:
		case 10:
		{
			_collisionCount--;
			if(_jumpCount == 0 && _collisionCount == 0)
			{
				_jumpCount = 1;
				_jumpTimer = jumpTime/2;
				baseY-=jumpHeight[0];
				fallTrigger();
			}
		}
			break;
		case 13:
		{
			collisionTrigger();
		}
			break;
		}
	}

	private int _collisionCount = 0;

	// for only calculation
	private int _jumpCount = 0;
	private float _jumpTimer = 0;
	private List<float> delayList = new List<float>();
}
