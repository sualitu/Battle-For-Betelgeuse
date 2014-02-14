using UnityEngine;
using System.Collections;

public class SplashImage : MonoBehaviour
{
	float duration = 2f;
	float alpha;
	Transform t;
	GUITexture tm;
	bool fadingIn = true;
	int i = 100;
	// Use this for initialization
	void Start () {
		tm = GetComponent<GUITexture>();
		alpha = 0;
		tm.color = new Color(tm.color.r,tm.color.g,tm.color.b,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(fadingIn) {
			if(alpha > 0.6f) {
				fadingIn = false;
			} else {
				alpha += Time.deltaTime/duration;
				tm.color = new Color(tm.color.r,tm.color.g,tm.color.b,alpha);
			}
		} else {
			if(i < 0) {
				if( alpha > 0) {
					alpha -= Time.deltaTime/duration;
					tm.color = new Color(tm.color.r,tm.color.g,tm.color.b,alpha);
					
				} else {
					Destroy(gameObject);
				}
			} else {
				i--;
			}
		}
	}
}

