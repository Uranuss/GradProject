using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {
	[SerializeField]
	private AudioClip BGM00 = null;
	[SerializeField]
	private AudioClip BGM01 = null;
	[SerializeField]
	private AudioClip BGM02 = null;
	[SerializeField]
	private AudioClip BGM03 = null;
	[SerializeField]
	private AudioClip BGM04 = null;
	[SerializeField]
	private AudioClip BGM05 = null;

	private const float volume = 0.2f;

	private AudioSource [] audioSources = new AudioSource[6];

    [SerializeField]
    private AudioClip Effect00 = null;
    [SerializeField]
    private AudioClip Effect01 = null;
    [SerializeField]
    private AudioClip Effect02 = null;
    [SerializeField]
    private AudioClip Effect03 = null;
    [SerializeField]
    private AudioClip Effect04 = null;
    [SerializeField]
    private AudioClip Effect05 = null;
    [SerializeField]
    private AudioClip Effect06 = null;
    [SerializeField]
    private AudioClip Effect07 = null;
    [SerializeField]
    private AudioClip Effect08 = null;
    [SerializeField]
    private AudioClip Effect09 = null;
    [SerializeField]
    private AudioClip Effect10 = null;
    [SerializeField]
    private AudioClip Effect11 = null;
    [SerializeField]
    private AudioClip Effect12 = null;
    [SerializeField]
    private AudioClip Effect13 = null;
    [SerializeField]
    private AudioClip Effect14 = null;
    [SerializeField]
    private AudioClip Effect15 = null;
    [SerializeField]
    private AudioClip Effect16 = null;
    [SerializeField]
    private AudioClip Effect17 = null;
    [SerializeField]
    private AudioClip Effect18 = null;
    [SerializeField]
    private AudioClip Effect19 = null;
    [SerializeField]
    private AudioClip Effect20 = null;
    [SerializeField]
    private AudioClip Effect21 = null;
    [SerializeField]
    private AudioClip Effect22 = null;
    [SerializeField]
    private AudioClip Effect23 = null;
    [SerializeField]
    private AudioClip Effect24 = null;
    [SerializeField]
    private AudioClip Effect25 = null;
    [SerializeField]
    private AudioClip Effect26 = null;
    [SerializeField]
    private AudioClip Effect27 = null;
    [SerializeField]
    private AudioClip Effect28 = null;
    [SerializeField]
    private AudioClip Effect29 = null;
    [SerializeField]
    private AudioClip Effect30 = null;


	private static BGMManager instance = null;
	public static BGMManager Instance {
		get { return instance; }
	}

	private AudioClip clip(int index) 
	{
		switch(index)
		{
			case 0: return BGM00;
			case 1: return BGM01;
			case 2: return BGM02;
			case 3: return BGM03;
			case 4: return BGM04;
			case 5: return BGM05;
			default: return null;
		}
	}

	private void initialize()
	{
		for(int i = 0; i < 6; ++i)
		{
			if(clip (i))
			{
				AudioSource audio = gameObject.AddComponent<AudioSource>();
				audio.clip = clip (i);
				audio.ignoreListenerVolume = true;
				audio.loop = true;
				audio.volume = volume;
				audioSources[i] = audio;
			} else {
				audioSources[i] = null;
			}
		}
	}

	void Awake() {
		if(instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		initialize();
		playBGM (0);
		DontDestroyOnLoad(this.gameObject);
	}

	int prvBGM = -1;
	int nowBGM = -1;

	public void playBGM(int index)
	{
		if(audioSources[index] && nowBGM != index)
		{
			prvBGM = nowBGM;
			audioSources[index].volume = 0;
			audioSources[index].Play();
			nowBGM = index;
		}
	}
	public void returnBGM()
	{
		playBGM(prvBGM);
	}

    AudioClip getClip(int index)
    {
        switch(index)
        {
            case 0: return Effect00;
            case 1: return Effect01;
            case 2: return Effect02;
            case 3: return Effect03;
            case 4: return Effect04;
            case 5: return Effect05;
            case 6: return Effect06;
            case 7: return Effect07;
            case 8: return Effect08;
            case 9: return Effect09;
            case 10: return Effect10;
            case 11: return Effect11;
            case 12: return Effect12;
            case 13: return Effect13;
            case 14: return Effect14;
            case 15: return Effect15;
            case 16: return Effect16;
            case 17: return Effect17;
            case 18: return Effect18;
            case 19: return Effect19;
            case 20: return Effect20;
            case 21: return Effect21;
            case 22: return Effect22;
            case 23: return Effect23;
            case 24: return Effect24;
            case 25: return Effect25;
            case 26: return Effect26;
            case 27: return Effect27;
            case 28: return Effect28;
            case 29: return Effect29;
            case 30: return Effect30;
            default: return null;
        }
    }

    public void playEffect(int index)
    {
        AudioClip clip = getClip(index);
        if(clip) AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

	// Use this for initialization
	void Start () {
	}

	float fadeTime = 0.5f;
	
	// Update is called once per frame
	void Update () {
		if(prvBGM != -1)
		{
			audioSources[prvBGM].volume = Mathf.Max(0f, audioSources[prvBGM].volume - Time.deltaTime / fadeTime);
			if(audioSources[prvBGM].volume == 0) audioSources[prvBGM].Stop();
		}
		if(nowBGM != -1)
		{
			audioSources[nowBGM].volume = Mathf.Min(1.0f, audioSources[nowBGM].volume + Time.deltaTime / fadeTime);
		}
	}
}
