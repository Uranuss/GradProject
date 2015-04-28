using UnityEngine;
using System.Collections;

public class ExBomb : MonoBehaviour {
	[SerializeField]
	Transform BombObject = null;
	[SerializeField]
	ParticleSystem particle = null;
	public void setPosition(Vector3 pos)
	{
		BombObject.position = pos;
		target = BombObject.localPosition;
	}

	// Use this for initialization
	void Start () {
		if(BombObject.localPosition == Vector3.zero)
		{
			BombObject.position = new Vector3(18, 5, 3);
			target = BombObject.localPosition;;
		}
	}

	float timer = 0;
	float TIME = 3f;
	int step = 0;
	Vector3 target = Vector3.zero;
	// Update is called once per frame
	void Update () {
		if(timer > TIME)
		{
			switch(step)
			{
				//bomb!
				case 0:
					particle.Play();
					break;
				case 1:
					break;
				case 2:
					Destroy (gameObject);
					return;
				default: break;
			}
			step++;
			timer = 0;
		}
		float t = Time.deltaTime * GameController.Instance.AniScale;
		timer += t;
		if(step == 0)
		{
			BombObject.renderer.material.color = Mathf.Sin(timer*2f*Mathf.PI)*Color.cyan + Color.red;
			Vector3 pos = target * (1f - timer / TIME);
			pos.y = BombObject.localPosition.y;
			BombObject.localPosition = pos;
			BombObject.Translate(0, acc*t, 0, transform);
			if(BombObject.localPosition.y < 0) BombObject.localPosition += new Vector3(0,-BombObject.localPosition.y, 0);
			acc -= 0.7f * t;
			if(timer > 2)
			{
				float s = Mathf.Pow(timer-2f,2);
				BombObject.localScale = Vector3.one * (1+s);
			}
		}
	}
	float acc = 1f;
}
