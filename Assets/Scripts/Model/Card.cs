using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Card
{
	public CardType cardType;
	public GameObject Prefab { get; set; }
	public GameObject ProjectilePrefab { get; set; }
	public abstract string Name { get; }
	public abstract string Projectile { get; }
	public abstract int Attack { get; }
	public abstract int Health { get; }
	public abstract int Movement { get; }
	public abstract int Cost { get; }
	public abstract int id { get; }
	public delegate void SpecialAbility(StateObject s);
	public List<StandardSpecial> StandardSpecials { get; set; }
	public abstract string PrefabPath { get; }
	public string CardText = "";
	
	public virtual void OnNewTurn(StateObject s) {
		
	}
	
	public virtual void OnPlay(StateObject s) {
		StandardOnPlay(s);
	}
	
	public virtual void OnAttack() {
	}
	
	public virtual bool OnAttacked() {
		return StandardOnAttacked();
	}
	
	public static Hashtable cardTable = new Hashtable();
	
	public Card() {
		Prefab = (GameObject) Resources.Load(PrefabPath);
		ProjectilePrefab = (GameObject) Resources.Load("Projectiles/"+Projectile);
		if(!cardTable.Contains(id)) {
			Card.cardTable.Add(id, this);
		}
		StandardSpecials = new List<StandardSpecial>();
		setCardText();
	}
	
	public void setCardText() {
		foreach(StandardSpecial sp in StandardSpecials) {
			CardText += sp.ToString() + "\n";
		}
	}
	
	
	public static List<Card> RandomDeck() {
		List<Card> result = new List<Card>();
		
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new CarrierCard());
		result.Add(new BattleCruiserCard());
		result.Add(new BattleCruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new TurretCard());
		result.Add(new TurretCard());
		result.Add(new TurretCard());
		result.Add(new TurretCard());
		
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());	
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());	
		result.Add(new ReinforceCard());
		result.Add(new ReinforceCard());
		result.Add(new ReinforceCard());
		result.Add(new ReinforceCard());
		return result;
	}
	
	protected bool StandardOnAttacked() {
		foreach(StandardSpecial ss in StandardSpecials) {
			if(ss.GetType() == typeof(StandardSpecial.Defenseless)) {
				return false;
			}
		}
		return true;
	}
	
	protected void StandardOnPlay(StateObject s) {
		foreach(StandardSpecial ss in StandardSpecials) {
			if(ss.GetType() == typeof(StandardSpecial.Boost)) {
				s.TargetUnit.Move(-((StandardSpecial.Boost) ss).Amount);
			}
		}
	}
}

public enum CardType { UNIT, BUILDING, SPELL }
