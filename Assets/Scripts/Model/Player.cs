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
	public List<Hex> targets;
	
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
		Hand.Add(card);
		if(!Ai) {
			GUICard guiCard = ((GameObject) Object.Instantiate(gameControl.CardPrefab)).GetComponent<GUICard>();
			guiCard.SetCard(card);
			guiCard.Owner = this;
			GuiHand.Add(guiCard);
			SortCards();
		}
	}
	
	public void PlayCard() {
		GUICard card = selectedGUICard;
		SpendMana(card.card.Cost);
		Hand.Remove(card.card);
		GuiHand.Remove(card);
		card.Played();
		gameControl.mouseControl.PlayModeOn = true;
		targets.ForEach(h => h.renderer.material.color = Color.white);
		targets = null;
		SortCards ();
	}
	
	public void SelectCard(GUICard card) {
		if(card.card.Cost <= ManaLeft()) {
			selectedGUICard = card;
			selectedCard = card.card;
			targets = new List<Hex>();
			Base.Hex.Adjacent(gameControl.gridControl.Map).ForEach(h => h.Adjacent(gameControl.gridControl.Map).ForEach(he => targets.Add(he)));
			targets.RemoveAll(h => h.Unit != null);
			targets.ForEach(h => h.renderer.material.color = Color.green);
			gameControl.mouseControl.PlayModeOn = false;
		}
	}
	
	public void SortCards() {
		int c = Hand.Count;
		List<List<int>> positions = gameControl.handControl.handPositions[c-1];
		for(int i = 0; i < c; i++) {
			GuiHand[i].x = positions[i][0];
			GuiHand[i].y = positions[i][1];
			GuiHand[i].Rotation = positions[i][2];
		}
	}
}

