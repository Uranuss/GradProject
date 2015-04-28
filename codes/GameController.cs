using UnityEngine;
using System.Collections;

// 유일한 게임 상태머신 객체 게임 상태를 관장.
public class GameController : MonoBehaviour {
	private static GameController _instance = null;
	public static GameController Instance
	{
		get{
			if(_instance == null)
			{
				_instance = (GameController)FindObjectOfType(typeof(GameController));
			}
			if(_instance == null)
			{
				GameObject obj = new GameObject("GameController");
				_instance = obj.AddComponent<GameController>();
			}
			return _instance;
		}
	}
	[SerializeField]
	PlayerSkinnedObject player = null;

	private bool _boss = false;
	private Transform _bossPosition = null;

	public bool bossState {
		get {
			return _boss;
		}
	}
	public Vector3 bossPosition {
		get {
			if(_boss) return _bossPosition.position;
			else return Vector3.zero;
		}
	}
	public void BossStart(GameObject boss)
	{
		if(boss) {
			_boss = true;
			_bossPosition = boss.transform;
		}
	}
	public void BossEnded()
	{
		if(_boss)
		{
			_boss = false;
			_bossPosition = null;
		}
	}

	public void Awake()
	{
		if(player == null)
		{
			player = FindObjectOfType<PlayerSkinnedObject>();
		}
	}
	private bool _freezed = false;
	public void freeze(bool f)
	{
		_freezed = f;
	}

	public void SpeedControll(float fv)
	{
		this._animScale = fv;
		this._speedScale = fv;
	}

	public void pause()
	{
		if(_state != GAMESTATE.PAUSE) state = GAMESTATE.PAUSE;
		else stateReverse();
	}
	public bool onRun = false;
	// Use this for initialization
	void Start () {
		state = GAMESTATE.INTRO;
		if(onRun) {
			state = GAMESTATE.RUN;
		}
	}

	float INTROTIME = 3.0f;
	float IntroTimer = 3.0f;
	// Update is called once per frame
	void Update () {
		switch(_state)
		{
			case GAMESTATE.INTRO:
				IntroTimer -= Time.deltaTime;
				if(IntroTimer <= 0)
				{
					state = GAMESTATE.RUN;
				} else IntroTimer -= Time.deltaTime;
				break;
			default:
				break;
		}
	}

	void exitState(GAMESTATE state)
	{
		switch(state)
		{
			case GAMESTATE.INTRO:
				if(player) player.startRun();
				break;
			default:
				break;
		}
	}
	
	void enterState(GAMESTATE state)
	{
		switch(state)
		{
			case GAMESTATE.PAUSE:
				_speedScale = 0;
				_animScale = 0;
				_controllable = false;
				break;
			case GAMESTATE.INTRO:
				_speedScale = 0;
				_animScale = 1;
				IntroTimer = INTROTIME;
				_controllable = false;
				break;
			case GAMESTATE.GAMEOVER:
				_speedScale = 0;
				_animScale = 1;
				_controllable = false;
				break;
			case GAMESTATE.BOSS:
				_speedScale = 1;
				_animScale = 1;
				_controllable = true;
				break;
			case GAMESTATE.RUN:
				_speedScale = 1;
				_animScale = 1;
				_controllable = true;
				break;
			case GAMESTATE.STORE:
				_speedScale = 0.5f;
				_animScale = 0.5f;
				_controllable = false;
				break;
			default:
				_speedScale = 1;
				_animScale = 1;
				_controllable = true;
				break;
		}
	}

	const string SOUNDKEY = "SoundMuted";
	const string MUSICKEY = "MusicMuted";
	public bool EffectMute(bool mute) 
	{
		bool muted = (PlayerPrefs.GetInt(SOUNDKEY, 0) == 1);
		if(mute)
		{
			AudioListener.volume = 0;
			PlayerPrefs.SetInt(SOUNDKEY, 1);
		}
		else
		{
			AudioListener.volume = 1;
			PlayerPrefs.SetInt(SOUNDKEY, 0);
		}
		PlayerPrefs.Save ();
		return muted;
	}
	public bool MusicMute(bool mute) 
	{
		bool muted = (PlayerPrefs.GetInt(MUSICKEY, 0) == 1);
		if(mute)
		{
			PlayerPrefs.SetInt(MUSICKEY, 1);
		}
		else
		{
			PlayerPrefs.SetInt(MUSICKEY, 0);
		}
		PlayerPrefs.Save ();
		return muted;		
	}

	private bool _controllable = false;
	public bool Controllable { get { return _controllable; } }

	[SerializeField]
	private float _MoveSpeed = 7f;
	public float MoveSpeed { get { return _MoveSpeed; } }

	private float _speedScale = 1.0f;
	public float SpeedScale{ get { if(_freezed) return 0; else return _speedScale; } }
	private float _animScale = 1.0f;
	public float AniScale{ get { if(_freezed) return 0; else return _animScale; } }

	private GAMESTATE _state = GAMESTATE.INIT;
	private GAMESTATE _prvstate = GAMESTATE.INIT;
	public void stateReverse()
	{
		state = _prvstate;
	}
	public GAMESTATE state {
		set {
			if(value != _state)
			{
				exitState(_state);
				enterState(value);
				_prvstate = _state;
				_state = value;
			}
		}
	}

	public enum GAMESTATE
	{
		INIT,
		INTRO,
		RUN,
		STORE,
		BOSS,
		PAUSE,
		GAMEOVER,
	};
}
