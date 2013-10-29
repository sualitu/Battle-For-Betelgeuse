using UnityEngine;
using System.Collections;

public class TextScrolling : MonoBehaviour {
	
	float scroll = 0.5f;
	float duration = 1.5f;
	float alpha;
	Transform t;
	TextMesh tm;
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
		tm = GetComponent<TextMesh>();
		alpha = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if( alpha > 0) {
			t.position = new Vector3(t.position.x, 2f, t.position.z + scroll*Time.deltaTime);
			alpha -= Time.deltaTime/duration;
			tm.color = new Color(tm.color.r,tm.color.g,tm.color.b,alpha);
			
		} else {
			Destroy(gameObject);
		}
	}
}
