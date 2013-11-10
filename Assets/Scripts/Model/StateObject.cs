using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateObject
{
	public List<Unit> Units { get; set; }
	public Unit TargetUnit { get; set; }
	public Player Caster { get; set; }
	public Player Opponent { get; set; }
	
	public StateObject(List<Unit> units, Unit targetUnit, Player caster, Player opponent) {
		Units = units;
		TargetUnit = targetUnit;
		Caster = caster;
		Opponent = opponent;
	}
}
