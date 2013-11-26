using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
	public Unit Base { get; set; }
	public GameControl gameControl;
	public int Team;
	public List<Card> Deck { get; set; }
	public List<Card> Hand { get; set; }
	public List<GUICard> GuiHand { get; set; }
	public bool Ai { get; set; }
	public Card selectedCard;
	public GUICard selectedGUICard;
	public List<Hex> targets = new List<Hex>();
	
	public int Points { get; set; }
	
	public int MaxMana = 0;
	public int ManaSpend = 0;
	
	public Player(List<Card> deck) {
		Deck = deck;
		Hand = new List<Card>();
		GuiHand = new List<GUICard>();
	}
		
	public int ManaLeft() {
		return MaxMana - ManaSpend;
	}
	
	public void SpendMana(int i) {
		ManaSpend += i;
	}
	
	public void DrawHand() {
		for(int i = 0; i < Settings.StartingHandCount; i++) { DrawCard(); }
	}
	
	public void DrawCard() {
		if(Hand.Count >= Settings.MaxHandCount) {
			return ;
		}
		Card card = null;
		int cardsLeft = Deck.Count;
		if(cardsLeft > 1) {
			card = Deck[Random.Range(0, Deck.Count)];
			
		} else if(cardsLeft == 1) {
			card = Deck[0];
		}
		Deck.Remove(card);
		Hand.Add(card);
		if(Team == 1) {
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
		gameControl.mouseControl.PlayModeOn = true;
		targets.ForEach(h => h.renderer.material.color = Settings.StandardTileColour);
		targets = new List<Hex>();
		SortCards ();
	}
	
	public void DeselectCard() {
		targets.ForEach(h => h.renderer.material.color = Settings.StandardTileColour);
		targets = new List<Hex>();
		selectedGUICard = null;
		selectedCard = null;
		gameControl.mouseControl.PlayModeOn = true;
	}
	
	public void SetTargetsForCard(Card card) {
		targets = card.Targets(new StateObject(gameControl.units, null, this, (gameControl.thisPlayer == this) ? gameControl.enemyPlayer : gameControl.thisPlayer));
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
					gameControl.networkControl.PlayNetworkCardOn(selectedCard, Base.Hex);
				} else {
					gameControl.PlayCardOnHex(selectedCard, Base.Hex, System.Guid.NewGuid().ToString());
				}
			}
			gameControl.mouseControl.PlayModeOn = false;
			targets.ForEach(h => h.renderer.material.color = Settings.MovableTileColour);
		} else {
			gameControl.audioControl.PlayErrorSound();
			gameControl.guiControl.ShowSmallSplashText(Dictionary.NotEnoughMana);
		}
	}
	
	public void SortCards() {
		int c = Hand.Count;
		if(c > 0) {
			List<List<int>> positions = gameControl.handControl.handPositions[c-1];
			for(int i = 0; i < c; i++) {
				GuiHand[i].SetPosition(positions[i][0], positions[i][1]);
				GuiHand[i].Rotation = positions[i][2];
			}
		}
	}
}

