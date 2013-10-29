using UnityEngine;
using System.Collections;

public class SplashText : MonoBehaviour {
	
	float duration = 1.5f;
	float alpha;
	Transform t;
	GUIText tm;
	// Use this for initialization
	void Start () {
		tm = GetComponent<GUIText>();
		alpha = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if( alpha > 0) {
			alpha -= Time.deltaTime/duration;
			tm.color = new Color(tm.color.r,tm.color.g,tm.color.b,alpha);
			
		} else {
			Destroy(gameObject);
		}
	}
}
