using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {
	
	Transform trans;
	float rot;
	Vector3 center;
	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform>();
		rot = Random.Range(-50f,50f);
		center = collider.bounds.center;
	}
	
	// Update is called once per frame
	void Update () {
		trans.RotateAround(center, Vector3.forward, rot*0.01f);
	}
}
