using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateObject
{
	public List<Unit> Units { get; set; }
	public Unit TargetUnit { get; set; }
	
	public StateObject(List<Unit> units, Unit targetUnit) {
		Units = units;
		TargetUnit = targetUnit;
	}
}
