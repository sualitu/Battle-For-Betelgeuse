using UnityEngine;
using System.Collections;

public class MindControlCard : SpellCard
{

	public override int Cost {
		get {
			return 10;
		}
	}


	public override int id {
		get {
			return 12;
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

	public override void SpellEffect (StateObject s)
	{
		s.TargetUnit.Team = s.Caster.Team;
	}
}

