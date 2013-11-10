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
		targets.ForEach(h => h.renderer.material.color = Color.white);
		targets = new List<Hex>();
		SortCards ();
	}
	
	public void DeselectCard() {
		targets.ForEach(h => h.renderer.material.color = Color.white);
		targets = new List<Hex>();
		selectedGUICard = null;
		selectedCard = null;
		gameControl.mouseControl.PlayModeOn = true;
	}
	
	public void SetTargetsForCard(Card card) {
		targets = new List<Hex>();
		if(!typeof(SpellCard).IsAssignableFrom(card.GetType())) {
			Base.Hex.Adjacent(gameControl.gridControl.Map).ForEach(h => h.Adjacent(gameControl.gridControl.Map).ForEach(he => targets.Add(he)));
			if(!typeof(UnitCard).IsAssignableFrom(card.GetType())) {
				HashSet<Hex> hsTargets = new HashSet<Hex>(targets);
				foreach(Hex hex in hsTargets) {
					targets.AddRange(PathFinder.BreadthFirstSearch(hex, gameControl.gridControl.Map, 4, Team));
				}	
			}
			targets.RemoveAll(h => h.Unit != null);
		} else {
			targets = ((SpellCard) card).Targets(new StateObject(gameControl.units, null, this, (gameControl.thisPlayer == this) ? gameControl.enemyPlayer : gameControl.thisPlayer));
		}
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
			gameControl.mouseControl.PlayModeOn = false;
			targets.ForEach(h => h.renderer.material.color = Color.green);
		} else {
			gameControl.audioControl.PlayErrorSound();
			gameControl.guiControl.ShowSmallSplashText(Dictionary.NotEnoughMana);
		}
	}
	
	public void SortCards() {
		int c = Hand.Count;
		List<List<int>> positions = gameControl.handControl.handPositions[c-1];
		for(int i = 0; i < c; i++) {
			GuiHand[i].SetPosition(positions[i][0], positions[i][1]);
			GuiHand[i].Rotation = positions[i][2];
		}
	}
}

