using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateObject
{
	public List<Unit> Units { get; set; }
	public Hex MainHex { get; set; }
	public Hex SecondaryHex { get; set; }
	public Player Caster { get; set; }
	public Player Opponent { get; set; }
	
	public StateObject(List<Unit> units, Hex mainhex, Hex secondaryHex, Player caster, Player opponent) {
		Units = units;
		MainHex = mainhex;
		SecondaryHex = secondaryHex;
		Caster = caster;
		Opponent = opponent;
	}
}
