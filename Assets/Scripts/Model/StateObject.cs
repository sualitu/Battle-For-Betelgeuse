using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateObject
{
	public List<Unit> Units { get; set; }
	public Hex TargetHex { get; set; }
	public Player Caster { get; set; }
	public Player Opponent { get; set; }
	
	public StateObject(List<Unit> units, Hex targetHex, Player caster, Player opponent) {
		Units = units;
		TargetHex = targetHex;
		Caster = caster;
		Opponent = opponent;
	}
}
