using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Card
{
	public GameObject Prefab { get; set; }
	public abstract string Name { get; }
	public abstract int Attack { get; }
	public abstract int Health { get; }
	public abstract int Movement { get; }
	public abstract int Cost { get; }
	public abstract int id { get; }
	public delegate void SpecialAbility(StateObject s);
	public List<StandardSpecial> StandardSpecials { get; set; }
	public abstract string PrefabPath { get; }
	
	public abstract void OnPlay(StateObject s);
	public static Hashtable cardTable = new Hashtable();
	
	public Card() {
		Prefab = (GameObject) Resources.Load(PrefabPath);
		if(!cardTable.Contains(id)) {
			Card.cardTable.Add(id, this);
		}
		StandardSpecials = new List<StandardSpecial>();
	}
	
	public static List<Card> RandomDeck() {
		List<Card> result = new List<Card>();
		result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());result.Add(new CowCard());
		result.Add(new CowCard());
		
		return result;
	}
	
	protected void StandardOnPlay(StateObject s) {
		foreach(StandardSpecial ss in StandardSpecials) {
			if(ss.GetType() == typeof(StandardSpecial.Boost)) {
				s.TargetUnit.Move(-((StandardSpecial.Boost) ss).Amount);
			}
		}
	}
}
