using UnityEngine;
using System.Collections;

public class MindControlCard : SpellCard
{

	public override int Cost {
		get {
			return 15;
		}
	}

	public override System.Collections.Generic.List<Hex> Targets (StateObject s)
	{
		return s.Units.FindAll(u => u.Team != s.Caster.Team && u != s.Opponent.Base).ConvertAll<Hex>(u => u.Hex);
	}
	
	
	public MindControlCard() {
		CardText += "Take control over an enemy unit or building.";
	}

	public override string Name {
		get {
			return "Mind Control";
		}
	}

	public override int MockOnPlay (MockUnit mo, HexEvaluator he)
	{
		return mo.Value(he);
	}

	public override void SpellEffect (StateObject s)
	{
		s.MainHex.Unit.Team = s.Caster.Team;
		s.MainHex.Unit.Move(int.MaxValue);
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}
}

