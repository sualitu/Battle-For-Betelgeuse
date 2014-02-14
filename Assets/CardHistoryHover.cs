using UnityEngine;
using System.Collections;

public class CardHistoryHover : MonoBehaviour {

	public Card Card;
	GUICard guiCard;

	void Start() {
	}

	void OnHover(bool isOver) {
		if(isOver && guiCard == null) {
			guiCard = ((GameObject) Object.Instantiate(Assets.Instance.CardPrefab)).GetComponent<GUICard>();
			guiCard.SetInfo(Card, GameControl.gameControl.ThisPlayer);
			guiCard.ForcePlaceCard(GameControl.gameControl.GuiControl.MousePosition.x, GameControl.gameControl.GuiControl.MousePosition.y); 
			guiCard.HandCard = false;
		} else if(guiCard != null) {
			Destroy(guiCard);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(guiCard != null) {
			guiCard.SetPosition(GameControl.gameControl.GuiControl.MousePosition.x, GameControl.gameControl.GuiControl.MousePosition.y);
		}
	}
}
