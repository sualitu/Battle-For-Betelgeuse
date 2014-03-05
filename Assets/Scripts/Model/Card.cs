using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Card
{
	public CardType cardType;
	public string CardText = "";
	public abstract string Name { get; }
	public abstract int Cost { get; }

	public virtual bool IsTargetless {
		get {
			return false;
		}
	}
		
	protected List<StandardSpecial> StandardSpecials { get; set; }
	
	public abstract List<Hex> Targets(StateObject s);
	
	public virtual void OnPlay(StateObject s) {
		StandardOnPlay(s);
	}	

	public static void InitiateCards() {
		ProtectionDeck();
		DestructionDeck();
		ControlDeck();
	}

	public virtual Faction Faction {
		get {
			return Faction.NEUTRAL;
		}
	}

	public virtual string Image {
		get {
			return "unknown";
		}
	}
	
	protected void StandardOnPlay(StateObject s) {
		foreach(StandardSpecial ss in StandardSpecials) {
			if(ss.GetType() == typeof(StandardSpecial.Boost)) {
				s.MainHex.Unit.Move(-((StandardSpecial.Boost) ss).Amount);
			}
			if(ss.GetType() == typeof(StandardSpecial.ForceField)) {
				s.MainHex.Unit.AddBuff(new ForceFieldBuff());
			}
			if(ss.GetType() == typeof(StandardSpecial.Ranged)) {
				s.MainHex.Unit.AddBuff(new RangedBuff());
			}
			if(ss.GetType() == typeof(StandardSpecial.DeathTouch)) {
				s.MainHex.Unit.AddBuff(new DeathTouchBuff());
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
	
	public static List<Card> ProtectionDeck() {
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
		result.Add(new ShieldedCollierCard());
		result.Add(new ShieldedCollierCard());
		result.Add(new ForceFieldCard());
		result.Add(new ForceFieldCard());
		result.Add (new ImperialFighterCard());
		result.Add (new ImperialFighterCard());
		result.Add (new BarrierShipCard());
		result.Add (new BarrierShipCard());
		result.Add (new PrimeCommanderCard());
		result.Add (new EmptySpaceCard());
		result.Add (new EmptySpaceCard());
		result.Add (new InstantServiceCard());
		result.Add (new InstantServiceCard());
		return result;
	}
	
	public static List<Card> ControlDeck() {
		List<Card> result = new List<Card>();
		result.Add(new ExplorerCard());
		result.Add(new ExplorerCard());
		result.Add(new DematerializeCard());
		result.Add(new DematerializeCard());
		result.Add(new GeneratorsCard());
		result.Add(new GeneratorsCard());
		result.Add(new PurgeCard());
		result.Add(new PurgeCard());
		result.Add(new CongreveShipCard());
		result.Add(new CongreveShipCard());
		result.Add(new FaultyLaunchersCard());
		result.Add(new FaultyLaunchersCard());
		result.Add(new FaultyThrustersCard());
		result.Add(new FaultyThrustersCard());
		result.Add(new MindControlCard());
		result.Add(new MindControlCard());
		result.Add(new CruiserCard());
		result.Add(new CruiserCard());
		result.Add(new DestroyerCard());
		result.Add(new DestroyerCard());
		result.Add(new FighterSquadCard());
		result.Add(new FighterSquadCard());
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add (new DisturbtronCard());
		result.Add (new DisturbtronCard());
		result.Add(new FrigateCard());
		result.Add(new FrigateCard());
		result.Add(new TurretCard());
		result.Add(new TurretCard());
		result.Add (new FluytCard());
		result.Add (new FluytCard());
		result.Add(new DromonCard());
		result.Add(new DromonCard());
		return result;
	}
	
	public static List<Card> DestructionDeck() {
		List<Card> result = new List<Card>();
		result.Add(new MajorReconstructionCard());
		result.Add(new MajorReconstructionCard());	
		result.Add(new MinorReconstructionCard());
		result.Add(new MinorReconstructionCard());
		result.Add(new PreciseMissileCard());
		result.Add(new PreciseMissileCard());
		result.Add(new SmallNukeCard());
		result.Add(new SmallNukeCard());
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
		result.Add(new MadScientistCard());
		result.Add(new SaboteurCard());
		result.Add(new SaboteurCard());
		result.Add (new ExplorerCard());
		result.Add (new ExplorerCard());
		result.Add(new SpecializedAttacksCard());
		result.Add(new SpecializedAttacksCard());
		result.Add(new ExplorationRocketCard());
		result.Add(new ExplorationRocketCard());
		result.Add(new SalvageCard());
		result.Add(new SalvageCard());
		result.Add(new DestructiveLoadCard());
		result.Add(new DestructiveLoadCard());
		result.Add (new NuclearWeaponsCard());
		result.Add (new NuclearWeaponsCard());
		result.Add(new TurretCard());
		result.Add(new TurretCard());
		return result;
	}
}

public enum CardType { UNIT, BUILDING, SPELL }

public enum Faction { GOOD, NEUTRAL, EVIL, CONTROL }
