using UnityEngine;
using System.Collections;

public class PlayerProperty : MonoBehaviour {

	[SerializeField]
	private float _hp = 100f;
	[SerializeField]
	private float _power = 5f;
	[SerializeField]
	private int _level = 0;
	[SerializeField]
	private int _exp = 0;

	[SerializeField]
	private HPBarSetter hpbar = null;
	[SerializeField]
	private HPBarSetter expbar = null;

	private readonly float [] _maxHP = { 100f, 110f, 120f, 130f, 140f, 150f, 160f, 170f, 180f, 190f };
	private readonly int [] _expCap = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 0 };
	private readonly float [] _powerBase = { 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f };
	private const int MAXLEVEL = 10;

	static readonly float[] JUMPHEIGHT = { 4f, 3f };

	public float hp
	{
		get {
			return _hp;
		}
	}

	public float power
	{
		get {
			return _power;
		}
	}

	public float jumpHeight(int jumpCount)
	{
		return JUMPHEIGHT[jumpCount-1];
	}

	public int level
	{
		get {
			return _level+1;
		}
	}
	
	public int exp
	{
		get {
			return _exp;
		}
		set {
			_exp += value;
			if(_exp >= _expCap[_level] && _level < MAXLEVEL)
			{
				_exp -= _expCap[_level];
				levelup();
			}
			updateUI();
		}
	}

	private void levelup()
	{
		_level++;
		_hp = _maxHP[_level];
		_power = _powerBase[_level];
	}

	public void damage(float dam)
	{
		_hp -= dam;
		gameObject.GetComponent<JumpObject>().damaged();
		if(_hp < 0) _hp = 0;
		updateUI ();
	}

	// Use this for initialization
	void Start () {
		if(expbar) expbar.setHPRate(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void updateUI()
	{
		if(hpbar) hpbar.setHPRate(_hp/_maxHP[_level]);
		if(expbar) expbar.setHPRate((float)_exp/(float)_expCap[_level]);
	}
}
