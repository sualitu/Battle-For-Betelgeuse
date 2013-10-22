using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class HandControl : MonoBehaviour
{
	public List<List<List<int>>> handPositions = new List<List<List<int>>>();
	
	void Start() {
		MakeHandPositions();
	}	
	
	void MakeHandPositions() {
		int x = Screen.width/2-100;
		int y = Screen.height-200;
		int cardWidth = 150;
		// One card
		List<List<int>> cards = new List<List<int>>();
		List<int> cardsOne = new List<int>();
		cardsOne.Add(x);
		cardsOne.Add(y);
		cardsOne.Add(0);
		cards.Add(cardsOne);
		handPositions.Add(cards);
		// Two cards
		cards = new List<List<int>>();
		cardsOne = new List<int>();
		cardsOne.Add(Mathf.FloorToInt(x-cardWidth*0.5f));
		cardsOne.Add(y);
		cardsOne.Add(-10);
		cards.Add(cardsOne);
		List<int> cardsTwo = new List<int>();
		cardsTwo.Add(Mathf.FloorToInt(x+cardWidth*0.5f));
		cardsTwo.Add(y);
		cardsTwo.Add(10);
		cards.Add(cardsTwo);
		handPositions.Add(cards);
		// Three cards
		cards = new List<List<int>>();
		cardsOne = new List<int>();
		cardsOne.Add(Mathf.FloorToInt(x-cardWidth*1f));
		cardsOne.Add(y);
		cardsOne.Add(-10);
		cards.Add(cardsOne);
		cardsTwo = new List<int>();
		cardsTwo.Add(Mathf.FloorToInt(x));
		cardsTwo.Add(y-20);
		cardsTwo.Add(0);
		cards.Add(cardsTwo);
		List<int> cardsThree = new List<int>();
		cardsThree.Add(Mathf.FloorToInt(x+cardWidth*1f));
		cardsThree.Add(y);
		cardsThree.Add(10);
		cards.Add(cardsThree);
		handPositions.Add(cards);
		// Four cards
		cards = new List<List<int>>();
		cardsOne = new List<int>();
		cardsOne.Add(Mathf.FloorToInt(x-cardWidth*1.5f));
		cardsOne.Add(y);
		cardsOne.Add(-20);
		cards.Add(cardsOne);
		cardsTwo = new List<int>();
		cardsTwo.Add(Mathf.FloorToInt(x-cardWidth*0.5f));
		cardsTwo.Add(y-20);
		cardsTwo.Add(-10);
		cards.Add(cardsTwo);
		cardsThree = new List<int>();
		cardsThree.Add(Mathf.FloorToInt(x+cardWidth*0.5f));
		cardsThree.Add(y-20);
		cardsThree.Add(10);
		cards.Add(cardsThree);
		List<int> cardsFour = new List<int>();
		cardsFour.Add(Mathf.FloorToInt(x+cardWidth*1.5f));
		cardsFour.Add(y);
		cardsFour.Add(20);
		cards.Add(cardsFour);
		handPositions.Add(cards);
		// Five cards
		cards = new List<List<int>>();
		cardsOne = new List<int>();
		cardsOne.Add(Mathf.FloorToInt(x-cardWidth*2f));
		cardsOne.Add(y);
		cardsOne.Add(-20);
		cards.Add(cardsOne);
		cardsTwo = new List<int>();
		cardsTwo.Add(Mathf.FloorToInt(x-cardWidth));
		cardsTwo.Add(y-10);
		cardsTwo.Add(-10);
		cards.Add(cardsTwo);
		cardsThree = new List<int>();
		cardsThree.Add(Mathf.FloorToInt(x));
		cardsThree.Add(y-20);
		cardsThree.Add(0);
		cards.Add(cardsThree);
		cardsFour = new List<int>();
		cardsFour.Add(Mathf.FloorToInt(x+cardWidth));
		cardsFour.Add(y-10);
		cardsFour.Add(10);
		cards.Add(cardsFour);
		List<int> cardsFive = new List<int>();
		cardsFive.Add(Mathf.FloorToInt(x+cardWidth*2f));
		cardsFive.Add(y);
		cardsFive.Add(20);
		cards.Add(cardsFive);
		handPositions.Add(cards);
	}
}

