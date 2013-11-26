using UnityEngine;
using System.Collections;

public class SpaceStation : MonoBehaviour {

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
		trans.RotateAround(center, new Vector3(0.2f,1f,0.2f), rot*0.01f);
	}
}
