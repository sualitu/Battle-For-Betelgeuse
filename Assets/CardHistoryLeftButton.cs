using UnityEngine;
using System.Collections;

public class CardHistoryLeftButton : MonoBehaviour {

	
	bool isOver = false;
	
	void OnPress(bool isOver) {
		this.isOver = isOver;
	}
	
	void Update() {
		Vector2 old = Assets.Instance.CardHistory.GetComponent<UIDraggablePanel>().relativePositionOnReset;
		if(old.x > 0 && GameControl.gameControl.CardHistory.Count > 5 && isOver) {
			Assets.Instance.CardHistory.GetComponent<UIDraggablePanel>().relativePositionOnReset = new Vector2(old.x - (0.1f/GameControl.gameControl.CardHistory.Count), old.y);
			Assets.Instance.CardHistory.GetComponent<UIDraggablePanel>().ResetPosition();
		}
	}
}
