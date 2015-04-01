using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	[SerializeField]
	private int BGMIndex = 0;

	void Awake()
	{
	}

	void Start()
	{
		BGMManager.Instance.playBGM(BGMIndex);
	}

	void Update()
	{
	}
}
