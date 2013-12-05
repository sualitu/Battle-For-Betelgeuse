using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Card
{
	public CardType cardType;
	public string CardText = "";
	public abstract string Name { get; }
	public abstract int Cost { get; }

		
	public delegate void SpecialAbility(StateObject s);
	public List<StandardSpecial> StandardSpecials { get; set; }
	
	public abstract List<Hex> Targets(StateObject s);
	
	public virtual void OnPlay(StateObject s) {
		StandardOnPlay(s);
	}	

	public virtual Faction Faction {
		get {
			return Faction.NEUTRAL;
		}
	}

	public virtual string Image {
		get {
			return "Unknown";
		}
	}
	
	protected void StandardOnPlay(StateObject s) {
		foreach(StandardSpecial ss in StandardSpecials) {
			if(ss.GetType() == typeof(StandardSpecial.Boost)) {
				s.TargetHex.Unit.Move(-((StandardSpecial.Boost) ss).Amount);
			}
			if(ss.GetType() == typeof(StandardSpecial.ForceField)) {
				s.TargetHex.Unit.AddBuff(new ForceFieldBuff());
			}
		}
	}
	
	public static Hashtable cardTable = new Hashtable();
	
	public Card() {
		if(!cardTable.Contains(Name)) {
			Card.cardTable.Add(Name, this);
		} 
		StandardSpecials = new List<StandardSpecial>();
		setStandardCardText();
	}
	
	public void setStandardCardText() {
		foreach(StandardSpecial sp in StandardSpecials) {
			CardText += sp.ToString() + "\n";
		}
	}
	
	public static List<Card> GoodDeck() {
		List<Card> result = new List<Card>();
		result.Add (new IntergalacticPoliticsCard());
		result.Add (new DeepSpaceExplorationCard());
		result.Add (new DeepSpaceExplorationCard());
		result.Add (new TimeDistortionCard());
		result.Add (new TimeDistortionCard());
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());	
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new BattleCruiserCard());
		result.Add(new BattleCruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new ReinforceCard());
		result.Add(new ReinforceCard());
		result.Add(new MiningVesselCard());
		result.Add(new MiningVesselCard());
		result.Add(new ForceFieldCard());
		result.Add(new ForceFieldCard());
		result.Add (new ImperialFighterCard());
		result.Add (new ImperialFighterCard());
		result.Add(new ShieldedCollierCard());
		result.Add(new ShieldedCollierCard());
		result.Add (new BarrierShipCard());
		result.Add (new BarrierShipCard());
		result.Add (new PrimeCommanderCard());
		return result;
	}
	
	public static List<Card> NeutralDeck() {
		List<Card> result = new List<Card>();
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());	
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new ReinforceCard());
		result.Add(new ReinforceCard());
		result.Add(new BattleCruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new CarrierCard());
		result.Add(new MindControlCard());
		result.Add(new MindControlCard());
		result.Add(new FaultyThrustersCard());
		result.Add(new FaultyThrustersCard());
		result.Add(new FaultyLaunchersCard());
		result.Add(new FaultyLaunchersCard());
		result.Add(new ImproveThrustersCard());
		result.Add(new ImproveThrustersCard());
		result.Add(new ImproveThrustersCard());
		result.Add(new ImproveThrustersCard());
		return result;
	}
	
	public static List<Card> EvilDeck() {
		List<Card> result = new List<Card>();
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());	
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new SmallNukeCard());
		result.Add(new SmallNukeCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new CarrierCard());
		result.Add(new CarrierCard());
		result.Add(new SelfDestructCard());
		result.Add(new SelfDestructCard());
		result.Add(new FinalSacrificeCard());
		result.Add(new FinalSacrificeCard());
		result.Add(new GreatNukeCard());
		result.Add(new GreatNukeCard());
		result.Add(new MadScientistCard());
		result.Add(new SaboteurCard());
		result.Add(new SaboteurCard());
		result.Add(new SaboteurCard());
		result.Add(new SaboteurCard());
		return result;
	}
	
	public static List<Card> AIDeck() {
		List<Card> result = new List<Card>();
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new CarrierCard());
		result.Add(new CarrierCard());
		result.Add(new CarrierCard());
		result.Add(new BattleCruiserCard());
		result.Add(new BattleCruiserCard());
		result.Add(new BattleCruiserCard());
		result.Add(new BattleCruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
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
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		return result;
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
		result.Add(new SmallNukeCard());
		result.Add(new SmallNukeCard());
		result.Add(new SmallNukeCard());
		result.Add(new SmallNukeCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		
		return result;
	}
	
	public static void InitCards() {
		new TurretCard();
		new FaultyLaunchersCard();
		new FaultyThrustersCard();
		new FinalSacrificeCard();
		new GreatNukeCard();
		new ImproveThrustersCard();
		new MadScientistCard();
		new MajorReconstructionCard();
		new MindControlCard();
		new MinorReconstructionCard();
		new PreciseMissileCard();
		new ReinforceCard();
		new SelfDestructCard();
		new SmallNukeCard();
		new BattleCruiserCard();
		new CarrierCard();
		new CruiserCard();
		new DestroyerCard();
		new ExplorerCard();
		new FighterSquadCard();
		new MiningVesselCard();
		new SaboteurCard();
	}	
}

public enum CardType { UNIT, BUILDING, SPELL }

public enum Faction { GOOD, NEUTRAL, EVIL }
