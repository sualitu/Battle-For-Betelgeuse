using UnityEngine;
using System.Collections;

public class FactionHovor : MonoBehaviour {

	public GameObject label;

	void OnHover(bool isOver) {
		float startAlpha = isOver ? 0f : 1f;
		TweenAlpha currTween = label.GetComponent<TweenAlpha>();
		if(currTween != null) {
			startAlpha = currTween.alpha;
			Destroy(currTween);
		}
		TweenAlpha tween = label.AddComponent<TweenAlpha>();
		if(isOver) {
			label.SetActive(true);
		} else {
			tween.eventReceiver = this.gameObject;
			tween.callWhenFinished = "Disable";
		}
		tween.from = startAlpha;
		tween.to = isOver ? 1f : 0f;
		tween.Reset();
		tween.Play(true);
	}

	void Disable() {
		label.SetActive(false);
	}
}
