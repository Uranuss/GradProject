using UnityEngine;
using System.Collections;

public class PlayMenuEvent : MonoBehaviour {
	[SerializeField]
	private Animator SettingAnimator = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	// click on setting
	public void setting()
	{
		if(!SettingAnimator.GetBool("onSetting"))
		{
			SettingAnimator.SetBool("onSetting", true);
			//GameParameters.isPause = true;
			GameParameters.State = GameParameters.GameState.GS_PAUSE;
		}
	}

	public void resume()
	{
		if(SettingAnimator.GetBool("onSetting"))
		{
			SettingAnimator.SetBool("onSetting", false);
			GameParameters.stateReverse();
		}
	}

	public void retry()
	{
		Application.LoadLevel("GamePlay");
		//GameParameters.isPause = false;
		GameParameters.State = GameParameters.GameState.GS_RUN;
	}

	public void exit()
	{
		Application.LoadLevel("MainMenu");
		GameParameters.State = GameParameters.GameState.GS_RUN;
		//GameParameters.isPause = false;
	}

}
