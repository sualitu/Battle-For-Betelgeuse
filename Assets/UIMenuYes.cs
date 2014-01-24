using UnityEngine;
using System.Collections;

public class UIMenuYes : MonoBehaviour {

	void OnClick() {
		Fade.FadeOut();
		GameControl.gameControl.LeaveGame();
	}
}
