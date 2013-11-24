using UnityEngine;
using System.Collections;

public class CommHub: MonoBehaviour {

	Transform trans;
	float rot;
	Vector3 center;
	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform>();
		rot = 7.5f;
		center = collider.bounds.center;
	}
	
	// Update is called once per frame
	void Update () {
		
		trans.RotateAround(center, new Vector3(0f,1f,0f), rot*0.01f);
	}
}
