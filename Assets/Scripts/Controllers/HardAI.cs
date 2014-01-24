using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

public class HardAI : AIControl
{	
	bool isAggressive = true;

	void DeterminePlayStyle() {
		Say ("Determining play style");
		int myFlags = 0;
		int humanFlags = 0;
		foreach(Flag flag in gameControl.Flags) {
			if(flag.OwnerTeam == player.Team) myFlags++;
			if(flag.OwnerTeam == gameControl.ThisPlayer.Team) humanFlags++;
		}
		try {
			int turnsTillWin = (Settings.VictoryRequirement - player.Points) / gameControl.FlagCountValue(myFlags);
			int turnsTillLoss = (Settings.VictoryRequirement - gameControl.ThisPlayer.Points) /  gameControl.FlagCountValue(humanFlags);
			
			isAggressive = turnsTillWin > turnsTillLoss;
		} catch {
			isAggressive = humanFlags >= myFlags;
		}
		Say (isAggressive ?  "Aggressive playstyle" : "Defensive playstyle");
	}

	int CalculateHexValue(Hex hex) {
		return CalculateHexValue(hex, gameControl.Units.ConvertAll<MockUnit>(u => new MockUnit(u)));
	}

	bool IsDefended(Flag flag) {
		return flag.Hexs.FindAll(h => h.Unit != null && h.Unit.Team == flag.OwnerTeam).Count > 0;
	}

	int MyPresence(Flag flag, List<MockUnit> state) {
		List<MockUnit> flagMocks = state.FindAll(mu => mu.Team == player.Team && flag.Hexs.Contains(mu.Hex));
		return flagMocks.Sum (mu => mu.Value(DoNothing));
	}

	int DoNothing(Hex hex) { return 0; }

	int CalculateHexValue(Hex hex, List<MockUnit> state) {
		// Calculate flag values
		int max = int.MinValue;
		foreach(Flag f in gameControl.Flags) {
			int thisValue = 0;
			if(f.OwnerTeam != player.Team && (!IsDefended(f) || isAggressive)) {
				thisValue = 15 - MyPresence(f, state) - Mathf.FloorToInt (hex.Distance(f.Hex));
			} else {
				thisValue = 10 - MyPresence(f, state) - Mathf.FloorToInt (hex.Distance(f.Hex));
			}
			max = Mathf.Max (max, thisValue);
		}
		return max;
	}

	
	#region Play Cards
	List<List<Card>> cardOptions = new List<List<Card>>();
	List<Card> chosenHand = new List<Card>();

	/// <summary>
	/// Finds all possible combinations of cards that can be played.
	/// </summary>
	/// <returns>The card combinations.</returns>
	List<List<Card>> CardPlayCombinations() {		
		List<List<Card>> result = new List<List<Card>>();
		foreach(List<Card> l in player.Hand.PowerSet<Card>().ToList().ConvertAll<List<Card>>(ie => ie.ToList())) {
			int i = 0;
			l.ForEach(c => i += c.Cost);
			if(i <= player.ManaLeft()) {
				result.Add(l);
			}
		}
		return result;		
	}

	int EntityCardValue(EntityCard card) {
		return card.Value();
	}
	
	int SpellOnHexValue(SpellCard sCard, Hex hex) {
		if(hex.Unit != null) {
			MockUnit mo = new MockUnit(hex.Unit);
			int originValue = mo.Value(CalculateHexValue);
			mo = sCard.MockOnPlay(mo);
			int newValue = mo.Value(CalculateHexValue);
			if(mo.Team == player.Team) {
				return ((originValue - newValue) * -1);
			} else {
				return ((originValue - newValue));
			}
		}
		return -1;
	}
	
	int ValuateCard(Card card) {
		int v = int.MinValue;
		if(typeof(EntityCard).IsAssignableFrom(card.GetType())) {
			v = EntityCardValue((EntityCard) card);
		} else if(typeof(SpellCard).IsAssignableFrom(card.GetType())) {
			SpellCard sCard = (SpellCard) card;
			player.SetTargetsForCard(card);
			foreach(Hex hex in player.targets) {
				int k = SpellOnHexValue(sCard, hex);
				v = Mathf.Max (k, v);
			}
			player.targets = new List<Hex>();
		} 
		return v;
	}
	
	int ValulateCardCombOption(List<Card> cards) {
		int v = 0;
		cards.ForEach(c => v += ValuateCard(c));
		return v;
	}

	bool ChooseCardOption() {
		if(cardOptions.Count < 1) {
			return false;
		}
		if(ValulateCardCombOption(cardOptions[0]) > ValulateCardCombOption(chosenHand)) {
			chosenHand = cardOptions[0];
		}
		cardOptions.RemoveAt(0);
		return true;
	}

	bool PlayCard() {
		if(chosenHand.Count < 1) {
			return false;
		}
		PlayCard (chosenHand[0]);	
		chosenHand.Remove(chosenHand[0]);
		return true;
	}

	bool PlayCard(Card card) {
		if(card == null) { return false; }
		if(typeof(SpellCard).IsAssignableFrom(card.GetType())) { return PlaySpellCard((SpellCard) card); }
		if(typeof(EntityCard).IsAssignableFrom(card.GetType())) { return PlayEntityCard((EntityCard) card); }
		return false;
	}

	bool PlayEntityCard(EntityCard card) {
		Hex targetHex = null;
		
		player.SetTargetsForCard(card);
		if(player.targets.Count < 1) {
			return false;
		}
		
		foreach(Hex hex in player.targets) {
			if(targetHex == null) { 
				targetHex = hex; 
			} else {
				if(CalculateHexValue(hex) > CalculateHexValue(targetHex)) {
					targetHex = hex;
				}
			}
		}
		
		// Play card
		gameControl.EnemyCardPlayed(card);
		gameControl.PlayCardOnHex(card, targetHex, System.Guid.NewGuid().ToString());
		player.SpendMana(card.Cost);
		player.Hand.Remove(card);
		return true;
	}

	bool PlaySpellCard(SpellCard card) {
		player.SetTargetsForCard(card);
		Hex targetHex = null;
		foreach(Hex hex in player.targets) {
			if(targetHex == null || SpellOnHexValue(card, hex) > SpellOnHexValue(card, targetHex)) {
				targetHex = hex;
			}
		}
		if(targetHex == null) { return false; }
		gameControl.EnemyCardPlayed(card);
		gameControl.PlayCardOnHex(card, targetHex, System.Guid.NewGuid().ToString());
		player.SpendMana(card.Cost);
		player.Hand.Remove(card);
		return true;
	}

	#endregion Play Cards

	#region Unit Movement
	Dictionary<Unit, Hex> plannedMoves;
	int plannedMovesValue = 0;
	List<List<Unit>> unitOrderings;
	List<List<Unit>> unitPowerSet;
	List<Unit> untouchedUnits;
	
	bool MoveUnits() {
		if(plannedMoves.Count < 1) {
			return false;
		}
		Unit unit = null;
		foreach(Unit u in plannedMoves.Keys) {
			unit = u;
			break;
		}
		Say ("Moving " + unit.UnitName + " from " + unit.Hex.GridPosition + " to " + plannedMoves[unit].GridPosition);
		unit.PrepareMove(plannedMoves[unit]);
		plannedMoves.Remove(unit);
		untouchedUnits.Remove(unit);
		return true;
	}

	/// <summary>
	/// Finds the unit orderings. Essentially the powerset of all units owned by the AI.
	/// </summary>
	/// <returns>The unit orderings.</returns>
	List<List<Unit>> FindUnitOrderings() {
		List<List<Unit>> result = new List<List<Unit>>();
		List<Unit> s = unitPowerSet.First();
		unitPowerSet.Remove(s);
		result.AddRange(s.Permute().ToList().ConvertAll<List<Unit>>(e => e.ToList ()));
		return result;
	}

	void EvaluateUnitOrdering(List<Unit> ordering) {
		List<MockUnit> mus = new List<MockUnit>();
		Dictionary<Unit, Hex> thisPlan = new Dictionary<Unit, Hex>();
		gameControl.Units.ForEach(u => mus.Add(new MockUnit(u)));

		int orderingValue = 0;
		foreach(Unit u in ordering) {
			MockUnit thisMock = mus.Find(mu => mu.Id == u.Id);
			List<Hex> targets = PathFinder.BreadthFirstSearch(thisMock.Hex, GameControl.gameControl.GridControl.Map, u.MovementLeft(), player.Team);
			Hex target = thisMock.Hex;
			int oldValue = CalculateHexValue(target, mus);
			foreach(Hex h in targets) {
				if(target == null || h.Unit == null && CalculateHexValue(h, mus) > oldValue) {
					target = h;
					oldValue = CalculateHexValue(h, mus);
				} else if(h.Unit != null && h.Unit.Team != player.Team) {
					MockUnit targetMock = mus.Find (mu => mu.Id == h.Unit.Id);
					if(targetMock.CurrentHealth > 0) {
						int value = thisMock.MockAttack(targetMock, (hex => CalculateHexValue(hex, mus)));
						if(value > oldValue) {
							target = h;
							oldValue = value;
						}
					}
				}
			}
			orderingValue += oldValue;
			if(target.Unit != null) {
				thisMock.ApplyMockAttack(mus.Find (mu => mu.Id == target.Unit.Id));
			}
			thisMock.Hex = target;
			thisPlan.Add(u, target);
		}

		if(orderingValue > plannedMovesValue) {
			plannedMovesValue = orderingValue;
			plannedMoves = thisPlan;
		} 
	}

	bool PlanMoves() {
		// Find all unit orders
		if(unitPowerSet.Count > 0) {
			unitOrderings.AddRange (FindUnitOrderings());
			return true;
		}
		//
		if(unitOrderings.Count < 1) {
			Say ("Done valuating boards");
			return false;
		}
		for(int i = 0; i < 1 && unitOrderings.Count > 0; i++) {
			List<Unit> ord = unitOrderings.First ();
			EvaluateUnitOrdering(ord);
			unitOrderings.Remove (ord);
		}
		return true;
	}
	#endregion Unit Movement

	void resetDataStructures() {
		Say ("Resetting Data Structures");
		unitOrderings = new List<List<Unit>>();
		plannedMoves = new Dictionary<Unit, Hex>();
		plannedMovesValue = 0;
		untouchedUnits = new List<Unit>();
	}

	bool MoveLeftOvers() {
		if(untouchedUnits.Count < 1) {
			return false;
		}
		Say(untouchedUnits.Count + " untouched units left");
		Unit unit = untouchedUnits.First();
		MockUnit thisMock = new MockUnit(unit);
		List<Hex> targets = PathFinder.BreadthFirstSearch(thisMock.Hex, GameControl.gameControl.GridControl.Map, unit.MovementLeft(), player.Team);
		Hex target = thisMock.Hex;
		int oldValue = CalculateHexValue(target);
		foreach(Hex h in targets) {
			if(target == null || h.Unit == null && CalculateHexValue(h) > oldValue) {
				target = h;
				oldValue = CalculateHexValue(h);
			} else if(h.Unit != null && h.Unit.Team != player.Team) {
				MockUnit targetMock = new MockUnit(h.Unit);
				if(targetMock.CurrentHealth > 0) {
					int value = thisMock.MockAttack(targetMock, CalculateHexValue);
					if(value > oldValue) {
						target = h;
						oldValue = value;
					}
				}
			}
		}
		if(target != thisMock.Hex) {
			Say ("Moving " + unit.UnitName + " from " + unit.Hex.GridPosition + " to " + target.GridPosition);
			unit.PrepareMove(target);
		} else {
			Say ("Not moving " + unit.UnitName);
		}
		untouchedUnits.Remove(unit);

		return true;
	}

	int i = 0;
	void DoMove() {
		switch(aistate) {
		case AIState.ENEMYTURN: 
			Say ("It is my turn!");
			aistate++;
			resetDataStructures();
			DeterminePlayStyle(); 
			break;
		case AIState.BUILDINGHAND: 
			cardOptions = CardPlayCombinations(); 
			aistate++; 
			break;
		case AIState.ANALYZINGCARDOPTIONS: 
			if(!ChooseCardOption()) { 
				aistate++; 
			} 
			break;
		case AIState.PLAYINGCARDS: 
			chosenHand.RemoveAll(c => ValuateCard(c) <= 0);
			if(i == 0) {
				if(!PlayCard()) {
					unitPowerSet = gameControl.Units.FindAll(u => u.Team == player.Team && u.MovementLeft() > 0).PowerSet().ToList ().ConvertAll<List<Unit>>(e => e.ToList());
					untouchedUnits = gameControl.Units.FindAll(u => u.Team == player.Team && u.MovementLeft() > 0);
					// unitPowerSet.RemoveAll(l => l.Count > 3)
					Say("Removed " + unitPowerSet.RemoveAll(l => l.Count > 3) + " items from the power set");
					aistate++; 
				}
				i = 100;
			} else {
				i--;
			}

			break;
		case AIState.ANALYZINGBOARD: 
			if(!PlanMoves()) {
				aistate++; 
			}
			break;
		case AIState.MOVINGUNITS: 
			if(!MoveUnits() && !MoveLeftOvers()) { 
				aistate++; 
			} 
			break;
		case AIState.DONE:
			Say ("Ending turn");
			EndTurn(); 
			break;
		default: break;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(MyTurn() && gameControl.NoMovesInProgress()) {
			DoMove();
		}
	}
}
