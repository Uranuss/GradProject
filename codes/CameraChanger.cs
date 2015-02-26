using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour {

	[SerializeField]
	private Camera mainCam = null;
	[SerializeField]
	private UnityEngine.UI.Button button = null;

	// Use this for initialization
	void Start () {
		button.onClick.AddListener(() => { cameraViewChange(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void cameraViewChange()
	{
		if(!mainCam.isOrthoGraphic)
		{
			mainCam.orthographic = true;
		}
		else
		{
			mainCam.orthographic = false;
		}
	}
}
