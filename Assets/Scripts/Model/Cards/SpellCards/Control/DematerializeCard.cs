using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DematerializeCard : SpellCard {
	
	public override int Cost {
		get {
			return 0;
		}
	}
	
	public override string Name {
		get {
			return "Dematerialize";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Opponent.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}

	public override string Image {
		get {
			return "dematerialise";
		}
	}

	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}

	public override void SpellAnimation (StateObject s)
	{
		Object.Instantiate((GameObject) Resources.Load ("Effects/Debuff"), s.MainHex.collider.bounds.center , Quaternion.identity);
		SpellEffect(s);
	}
	
	public DematerializeCard() {
		CardText += "Destroy an enemey unit. Owner draws 3 cards.";
	}

	public override int MockOnPlay (MockUnit mo, HexEvaluator he)
	{
		return 20;
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.MainHex.Unit.Kill();
		s.Opponent.DrawCards(3);
	}
}
