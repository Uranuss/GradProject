using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	private int leftFingerID = -1;
	private int rightFingerID = -1;
	private bool isLeftTouched = false;
	private bool isRightTouched = false;
	[SerializeField]
	private Animator anim = null;
	[SerializeField]
	private GameObject subChar1 = null;
	[SerializeField]
	private GameObject subChar2 = null;
	[SerializeField]
	private GameObject subChar3 = null;

	const float JUMPTIME = 1.0f/1.5f;

	float jumpTimer = 0;
	int jumpCount = 0;
	const int maxJumpCount = 2;
	bool isSlide = false;
	private float [] subJumpTimer = new float[3];
	private bool [] subJumpFlag = new bool[3];

	private GameObject [] subChar = { null, null, null };


	private float baseY = 0f;
	private bool isFloor = true;

	// Use this for initialization
	void Start () {
		subChar[0] = subChar1;
		subChar[1] = subChar2;
		subChar[2] = subChar3;
		for(int i = 0; i < 3; i++) {
			subChar[i].transform.position = new Vector3((float)((i+1)*-1.5), 0, 0);
			subChar[i].SetActive(true);
			subJumpTimer[i] = 0;
			subJumpFlag[i] = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(GameParameters.isPause)
			return;

		if(Input.touchSupported) {
			touchInputSystem();
		} else {
			mouseInputSystem();
		}
		if(isLeftTouched || jumpControlFlag) {
			controlTimer += Time.deltaTime;
			if(controlTimer >= GameParameters.JumpPreInputTimer) jumpControlFlag = false;
			if(!isSlide && jumpControlFlag)
			{
				jumpAction();
			}
		}
		if(jumpCount > 0)
		{
			float jumpheight = gameObject.GetComponent<PlayerProperty>().jumpHeight(jumpCount);
			float y = (1.0f - Mathf.Pow(jumpTimer*(2f/JUMPTIME)-1.0f, 2f)) * jumpheight;
			transform.position = new Vector3(0, baseY + y, 0);
			//transform.rigidbody.velocity = Vector3.zero;

			if(!subJumpFlag[0] && jumpTimer >= 0.3f)
			{
				subJumpFlag[0] = true;
			}
			if(!subJumpFlag[1] && jumpTimer >= 0.6f)
			{
				subJumpFlag[1] = true;
			}
			if(!subJumpFlag[2] && jumpTimer >= 0.9f)
			{
				subJumpFlag[2] = true;
			}

			if(isFloor /* jumpTimer >= JUMPTIME */ ) {
				jumpCount = 0;
				transform.position = new Vector3(0, baseY, 0);
				//rigidbody.position = new Vector3(0, baseY, 0);
				anim.SetInteger("state",0);
				jumpTimer = 0;
			}
			else
			{
				jumpTimer+=Time.deltaTime;
			}
		}
		if(transform.position.x != 0)
		{
			transform.Translate(-transform.position.x,0,0);
		}
		if(transform.position.y != baseY && jumpCount == 0)
		{
			transform.position = new Vector3(0,baseY,0);
		}
		for(int i = 0; i < 3; i++)
		{
			float y = 0;
			if(subJumpFlag[i])
			{
				float jumpheight = gameObject.GetComponent<PlayerProperty>().jumpHeight(1);
				y = (1.0f - Mathf.Pow(subJumpTimer[i]*(2f/JUMPTIME)-1.0f, 2f)) * jumpheight;
				if(subJumpTimer[i] >= JUMPTIME) 
				{
					subJumpFlag[i] = false;
					y = 0;
					subJumpTimer[i] = 0;
				}
				else
				{
					subJumpTimer[i] += Time.deltaTime;
				}
			}
			if(subChar[i] != null)
			{
				subChar[i].transform.position = new Vector3((float)((i+1)*-1.5), y, 0);
			}
		}
		if(transform.position.y < 0)
		{
			baseY = 0;
			isFloor = true;
			jumpTimer = 0;
		}
		if(anim.GetCurrentAnimatorStateInfo(1).IsName("_down_by_obstacle"))
		{
			anim.SetInteger("state", 0);
		}
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("_jump_start") && anim.GetInteger("state") == 6)
		{
			anim.SetInteger("state", 1);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if(GameParameters.isPause) return;
		if((collision.gameObject.layer == 9 || collision.gameObject.layer == 10) && jumpCount > 0 && !isFloor) {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.red);
				if(contact.normal.y > 0) {
					baseY = collision.gameObject.transform.position.y;
					isFloor = true;
					jumpTimer = 0;
				}
			}
		} else if(collision.gameObject.layer == 13) {
			anim.SetInteger("state", 4);
			//anim.SetInteger("state", 0);
		} else {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.blue);
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		if(GameParameters.isPause) return;
		if(collision.gameObject.layer == 10 && jumpCount == 0 && isFloor) {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.white);
			}
			baseY = collision.gameObject.transform.position.y;
			jumpCount++;
			jumpTimer = JUMPTIME;
			isFloor = false;
			anim.SetInteger("state",3);
		} else {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay(contact.point, contact.normal, Color.blue);
			}
		}
	}
	
	private void touchInputSystem()
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

	private void mouseInputSystem()
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

	private float controlTimer = 0;
	private Vector2 controlStart = Vector2.zero;
	private Vector2 controlResult = Vector2.zero;
	private GameObject AttackSubChar = null;

	void began(float x, float y, int id)
	{
		if(isLeftTouched && isRightTouched) return;
		if(x < Screen.width/2f && !isLeftTouched)
		{
			isLeftTouched = true;
			leftFingerID = id;
			controlTimer = 0;
			controlStart = new Vector2(x,y);
			//jump
			jumpControlFlag = true;
			/*
			if(!isJump && anim.GetCurrentAnimatorStateInfo(0).IsName("_run") && anim.GetCurrentAnimatorStateInfo(1).IsName("default")) {
				isJump = true;
				isFloor = false;
				jumpTimer = 0;
				anim.SetInteger("state", 1);
			}
			*/
		} else if(!isSlide && !isRightTouched) {
			isRightTouched = true;
			rightFingerID = id;
			GameObject temp = subChar[0];
			if(temp != null)
			{
				subChar[0] = subChar[1];
				subChar[1] = subChar[2];
				subChar[2] = null;
				temp.transform.SetParent(gameObject.transform);
				AttackSubChar = temp;
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
				Vector3 pos = ray.GetPoint(9.5f);
				AttackSubChar.transform.LookAt( new Vector3(pos.x, pos.y, -0.5f) );
				//AttackSubChar.rigidbody.rotation.SetLookRotation( new Vector3(pos.x, pos.y, -0.5f) );
				//AttackSubChar.rigidbody.position = new Vector3(0, 0.5f, -0.5f);
				AttackSubChar.transform.localPosition = new Vector3(0, 0.5f, -0.5f);
				AttackSubChar.rigidbody.velocity = Vector3.zero;
			}
			anim.SetInteger("throw",1);
		}
	}

	private bool jumpControlFlag = false;
	void jumpAction()
	{
		if(jumpCount < maxJumpCount && anim.GetCurrentAnimatorStateInfo(1).IsName("default")) {
			jumpCount++;
			baseY = transform.position.y;
			isFloor = false;
			jumpTimer = 0;
			switch(jumpCount)
			{
				case 1:
					anim.SetInteger("state", 1);
					break;
				case 2:
					anim.SetInteger("state", 6);
					break;
			}
			jumpControlFlag = false;
		}
	}

	void moved(float x, float y, int id)
	{
		if(!isLeftTouched && !isRightTouched) return;
		if(leftFingerID != id && rightFingerID != id) return;
		if(leftFingerID == id)
		{
			controlResult = new Vector2(x,y) - controlStart;
			if( controlResult.y < -10f && !isSlide && controlTimer <= GameParameters.ControlFlipTimer && jumpTimer <= GameParameters.ControlFlipTimer && jumpCount < 2 ) {
				if(jumpCount == 1) { 
					anim.SetInteger("state", 0);
					jumpCount = 0;
					isFloor = true;
					transform.position = new Vector3(0, baseY, 0);
				}
				isSlide = true;
				anim.SetInteger("state", 2);
				//transform.Rotate(0,0,90);
				BoxCollider bc = gameObject.GetComponent<BoxCollider>();
				bc.size = new Vector3(1f, 0.85f, 1f);
				bc.center = new Vector3(0.25f, 0.425f, 0);
			}
		}
		if(rightFingerID == id)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
			Vector3 pos = ray.GetPoint(9.5f);
			if(AttackSubChar != null)
			{
				//AttackSubChar.rigidbody.rotation.SetLookRotation( new Vector3(pos.x, pos.y, -0.5f) );
				AttackSubChar.transform.LookAt( new Vector3(pos.x, pos.y, -0.5f) );
				//AttackSubChar.rigidbody.position = new Vector3(0, 0.5f, -0.5f);
				AttackSubChar.transform.localPosition = new Vector3(0, 0.5f, -0.5f);
				AttackSubChar.rigidbody.velocity = Vector3.zero;
			}
		}
	}

	public bool shiftSubChar(GameObject obj)
	{
		for(int i = 0; i < 3; i++)
		{
			if(subChar[i] == null)
			{
				subChar[i] = obj;
				obj.transform.SetParent(gameObject.transform.parent);
				obj.transform.rotation = Quaternion.identity;
				return true;
			}
		}
		return false;
	}

	void ended(float x, float y, int id)
	{
		if(!isLeftTouched && !isRightTouched) return;
		if(leftFingerID != id && rightFingerID != id) return;
		if(leftFingerID == id)
		{
			//jumpControlFlag = false;
			if(isSlide) {
				isSlide = false;
				//transform.rotation = Quaternion.identity;
				BoxCollider bc = gameObject.GetComponent<BoxCollider>();
				bc.size = new Vector3(1f, 1.7f, 1f);
				bc.center = new Vector3(0.25f, 0.85f, 0);
				anim.SetInteger("state", 0);
			} /* else if(controlTimer <= GameParameters.ControlFlipTimer) {
				//jump
				if(!isJump && anim.GetCurrentAnimatorStateInfo(0).IsName("_run") && anim.GetCurrentAnimatorStateInfo(1).IsName("default")) {
					isJump = true;
					isFloor = false;
					jumpTimer = 0;
					anim.SetInteger("state", 1);
				}
			}
			*/
			controlTimer = 0;
			isLeftTouched = false;
			leftFingerID = -1;
		}
		if(rightFingerID == id)
		{
			// shot
			if(AttackSubChar != null)
			{
				AttackSubChar.transform.localPosition = new Vector3(0, 0, 0);
				AttackSubChar.transform.SetParent(transform.parent);
				//AttackSubChar.transform.Translate(0,0, 0.5f);
				//AttackSubChar.rigidbody.position = new Vector3(0,0,0);
				AttackSubChar.AddComponent<WeaponMove>();
				AttackSubChar = null;
			}
			anim.SetInteger("throw",0);
			isRightTouched = false;
			rightFingerID = -1;
		}
	}
};

