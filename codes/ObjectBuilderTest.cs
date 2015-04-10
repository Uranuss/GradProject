#if UNITY_EDITOR_WIN

using UnityEditor;
using UnityEngine;
using System.Collections;

public class ObjectBuilderTest : MonoBehaviour
{
	public GameObject obj;
	public Vector3 spawnPoint;

	void OnGUI()
	{
	}

	public void BuildObject(bool [] floor)
	{
		GameObject go = new GameObject("floorBlock");
		go.tag = "Floor";
		go.layer = LayerMask.NameToLayer("Floor");
		/*
		int len = 0;
		int start = -1;
		for(int i = 0 ; i < 36; i++)
		{
			if(floor[i]) {
				if(start == -1) start = i;
				GameObject sub = (GameObject)Instantiate(obj, new Vector3(i, 0, 0), Quaternion.identity);
				sub.tag = "Floor";
				sub.layer = LayerMask.NameToLayer("Floor");
				sub.transform.SetParent(go.transform);
				len++;
			} else if(len > 0) {
				BoxCollider col = go.AddComponent<BoxCollider>();
				col.center = new Vector3((float)start + (float)len / 2f, -1f, 0);
				col.size = new Vector3(len, 3, 5);
				len = 0;
				start = -1;
			}
		}
		if(len > 0) {
			BoxCollider col = go.AddComponent<BoxCollider>();
			col.center = new Vector3((float)start + (float)len / 2f, -1, 0);
			col.size = new Vector3(len, 3, 5);
			len = 0;
		}
		/*/
		for(int i = 0 ; i < 36; i++)
		{
			if(floor[i]) {
				GameObject sub = (GameObject)Instantiate(obj, new Vector3(i, 0, 0), Quaternion.identity);
				sub.tag = "Floor";
				sub.layer = LayerMask.NameToLayer("Floor");
				sub.transform.SetParent(go.transform);
				BoxCollider col = go.AddComponent<BoxCollider>();
				col.center = new Vector3((float)i + 0.5f, -1f, 0);
				col.size = new Vector3(1, 3, 5);
			}
		}
		//*/
		go.transform.position = spawnPoint;
	}
}


[CustomEditor(typeof(ObjectBuilderTest))]
public class EditorTest : Editor
{
	private bool [] floor = new bool[36] { true,true,true,true,true,true,true,true,true,true, true,true,true,true,true,true,true,true,true,true, true,true,true,true,true,true,true,true,true,true, true,true,true,true,true,true };
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		ObjectBuilderTest test = (ObjectBuilderTest)target;
		for(int i = 0; i < 36; ++i)
		{
			floor[i] = GUILayout.Toggle(floor[i], (1+i).ToString());
		}
		if(GUILayout.Button("build"))
		{
			test.BuildObject(floor);
		}
	}
}

#endif