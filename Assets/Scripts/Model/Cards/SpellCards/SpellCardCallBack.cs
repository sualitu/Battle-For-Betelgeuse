using UnityEngine;
using System.Collections;

public class SpellCardCallBack : MonoBehaviour {
	StateObject s;
	SpellCard target;
	float delay;
	bool invoked = false;
	
	void Update() {
		if(invoked) {
			StartCoroutine(Delay ());
		}
	}
	
	public void Setup(StateObject s, SpellCard target, float delay) {
		this.s = s;
		this.target = target;
		this.delay = delay;
		invoked = true;
	}
	
	IEnumerator Delay() {
		yield return new WaitForSeconds (delay);
		target.SpellEffect(s);
		Destroy(this);
	}	
}
