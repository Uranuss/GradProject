using UnityEngine;
using System.Collections;

public class HPBar : MonoBehaviour {
	[SerializeField]
	SpriteRenderer YellowBar = null;
	[SerializeField]
	SpriteRenderer RedBar = null;
	[SerializeField]
	SpriteRenderer BGBar = null;
	[SerializeField]
	bool Hide = false;
	[SerializeField]
	float HIDETIME = 1.0f;

	[SerializeField]
	float RedTime = 1.0f;

	float rate = 1.0f;
	float redRate = 1.0f;

	// Use this for initialization
	void Start () {
	}

	float hideTimer = 0;
	float moveRate = 0f;
	// Update is called once per frame
	void Update () {
		if(moveRate != 0)
		{
			if(rate < redRate)
			{
				redRate += (moveRate * Time.deltaTime * GameController.Instance.AniScale) / RedTime;
				ChangeVisible(RedBar, redRate);
			}
			if(rate >= redRate)
			{
				redRate = rate;
				ChangeVisible(RedBar, redRate);
				moveRate = 0;
			}
		} else if(Hide) {
			if(hideTimer >= HIDETIME) {
				//hide
				BGBar.gameObject.SetActive(false);
			}
			hideTimer += Time.deltaTime * GameController.Instance.AniScale;
		}
	}

	void UpdateGauge()
	{
		ChangeVisible(YellowBar, rate);
		ChangeVisible(RedBar, redRate);
	}

	void ChangeVisible(SpriteRenderer s, float r)
	{
		s.material.SetFloat("_Cutoff", 1.0f-r);
	}

	public void setRate(float nrate)
	{
		if(nrate > rate)
		{
			rate = nrate;
			ChangeVisible(YellowBar, rate);
			redRate = rate;
			ChangeVisible(RedBar, redRate);
		} else {
			rate = nrate;
			ChangeVisible(YellowBar, rate);
			moveRate = rate - redRate;
		}
		if(Hide)
		{
			//show
			BGBar.gameObject.SetActive(true);
		}
	}
}
