using UnityEngine;
using System.Collections;

public class ScrollBoard : MonoBehaviour {

	[SerializeField]
	private MeshRenderer meshRenderer = null;
	[SerializeField]
	private float speedScale = 0.1f;
	[SerializeField]
	private int axis;
	[SerializeField]
	private bool normal = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameParameters.isPause)
			return;
		TexMove("_MainTex");
		if(normal) TexMove("_BumpMap");
	}
	private void TexMove(string textureName)
	{
		Vector2 offset = meshRenderer.material.GetTextureOffset(textureName);
		switch(axis)
		{
		case 0:
			offset.x += GameParameters.MoveSpeed*speedScale * Time.deltaTime;
			break;
		case 1:
			offset.x -= GameParameters.MoveSpeed*speedScale * Time.deltaTime;
			break;
		case 2:
			offset.y += GameParameters.MoveSpeed*speedScale * Time.deltaTime;
			break;
		case 3:
			offset.y -= GameParameters.MoveSpeed*speedScale * Time.deltaTime;
			break;
		default:
			break;
		}
		meshRenderer.material.SetTextureOffset(textureName, offset);
	}
}
