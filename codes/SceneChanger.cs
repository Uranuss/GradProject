using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {
	[SerializeField]
	private UnityEngine.UI.Button button = null;
	[SerializeField]
	private string sceneName = null;

	// Use this for initialization
	void Start () {
		button.onClick.AddListener(() => { sceneChange(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void sceneChange()
	{
		Application.LoadLevel(sceneName);
	}
}
