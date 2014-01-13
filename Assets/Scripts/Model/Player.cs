using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
	public Unit Base { get; set; }
	GameControl gameControl;
	public Team Team { get; set; }
	public List<Card> Deck { get; set; }
	public List<Card> Hand { get; set; }
	public List<GUICard> GuiHand { get; set; }
	public bool Ai { get; set; }
	public Card selectedCard;
	public GUICard selectedGUICard;
	public List<Hex> targets = new List<Hex>();
	
	public int Points { get; set; }
	
	int maxMana = 0;

	public int MaxMana {
		get {
			return maxMana > Settings.MaxMana ? Settings.MaxMana : maxMana;
		}
		set {
			maxMana = value;
		}
	}
	public int ManaSpend = 0;
	
	public Player(List<Card> deck, GameControl gameControl) {
		Deck = deck;
		Hand = new List<Card>();
		GuiHand = new List<GUICard>();
		this.gameControl = gameControl;
	}
		
	public int ManaLeft() {
		return MaxMana - ManaSpend;
	}
	
	public void SpendMana(int i) {
		ManaSpend += i;
	}
	
	public void DrawHand() {
		DrawCards (Settings.StartingHandCount);
	}

	public void DrawCards(int count) {
		for(int i = 0; i < count; i++) { DrawCard(); }
	}
	
	public void DrawCard() {
		if(Hand.Count >= Settings.MaxHandCount || Deck.Count < 1) {
			return;
		}
		Card card = null;
		card = Deck.RandomElement();

		Deck.Remove(card);
		Hand.Add(card);
		if(Team == Team.ME) {
			GUICard guiCard = ((GameObject) Object.Instantiate(gameControl.CardPrefab)).GetComponent<GUICard>();
			guiCard.SetInfo(card, this);
			GuiHand.Add(guiCard);
			SortCards();
		}
	}
	
	public void PlayCard() {
		GUICard guiCard = selectedGUICard;
		SpendMana(guiCard.Card.Cost);
		Hand.Remove(guiCard.Card);
		GuiHand.Remove(guiCard);
		guiCard.Played();
		gameControl.MouseControl.PlayModeOn = true;
		targets.ForEach(h => h.renderer.material.color = Settings.StandardTileColour);
		targets = new List<Hex>();
		SortCards ();
	}
	
	public void DeselectCard() {
		targets.ForEach(h => h.renderer.material.color = Settings.StandardTileColour);
		targets = new List<Hex>();
		selectedGUICard = null;
		selectedCard = null;
		gameControl.MouseControl.PlayModeOn = true;
	}
	
	public void SetTargetsForCard(Card card) {
		targets = card.Targets(new StateObject(gameControl.Units, null, this, (gameControl.ThisPlayer == this) ? gameControl.EnemyPlayer : gameControl.ThisPlayer));
	}
	
	public void SelectCard(GUICard guiCard) {
		if(selectedGUICard != null) {
			selectedGUICard.Deselect();
			DeselectCard();
		}
		if(guiCard.Card.Cost <= ManaLeft()) {
			guiCard.Select();
			selectedGUICard = guiCard;
			selectedCard = guiCard.Card;
			SetTargetsForCard(guiCard.Card);
			if(targets.Count < 1) {
				// TODO Do this properly. This should be centralized.
				if(GameControl.IsMulti) {
					gameControl.NetworkControl.PlayNetworkCardOn(selectedCard, Base.Hex);

				} else {
					gameControl.PlayCardOnHex(selectedCard, Base.Hex, System.Guid.NewGuid().ToString());
				}
				DeselectCard();
			}
			gameControl.MouseControl.PlayModeOn = false;
			targets.ForEach(h => h.renderer.material.color = Settings.MovableTileColour);
		} else {
			gameControl.AudioControl.PlayErrorSound();
			gameControl.GuiControl.ShowSmallSplashText(Dictionary.NotEnoughMana);
		}
	}
	
	public void SortCards() {
		int c = Hand.Count;
		int x = Mathf.FloorToInt(Screen.width/2-100);
		int y = Mathf.FloorToInt(Screen.height-200);
		bool oddCount = c % 2 == 0;
		for(int i = 0; i < c; i++) {
			float relativePosition = (i - (c/2) + (oddCount ? 0.5f : 0f));
			GuiHand[i].SetPosition(x+Mathf.FloorToInt(relativePosition*(150-(c*7))), y);
			GuiHand[i].Rotation = Mathf.FloorToInt(relativePosition*(12-c));
		}
	}
}

public enum Team { NEUTRAL, ME, ENEMY }
