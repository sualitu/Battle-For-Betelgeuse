using UnityEngine;
using System.Collections.Generic;

public class AuraBuff : UnitBuff
{
	UnitBuffAction onAdd;

	public void NotifyOnMovement(Unit unit, Hex target) {
		if(owner == unit) {
			List<Unit> oldAffUnits = AffectedTiles().FindAll(h => h.Unit != null).ConvertAll<Unit>(h => h.Unit);
			List<Unit> newAffUnits= AffectedTiles(target).FindAll(h => h.Unit != null).ConvertAll<Unit>(h => h.Unit);
			oldAffUnits.FindAll(u => !newAffUnits.Contains(u)).ForEach(u => onRemove(u));
			newAffUnits.FindAll(u => !oldAffUnits.Contains(u)).ForEach(u => onAdd(u));
			return;
        }
		if((IsInBuff(unit.Hex) ^ IsInBuff(target))) {
			if(IsInBuff(target)) {
				onAdd(unit);
			} else {
				onRemove(unit);
			}
		} 
	}

	public void CheckBuffOn(Unit unit) {
		if(IsInBuff(unit.Hex)) {
			onAdd(unit);
		}
	}

	Unit owner = null;
	int cardinality;

	bool IsInBuff(Hex hex, Hex source) {
		return PathFinder.BreadthFirstSearch(source, GameControl.gameControl.gridControl.Map, cardinality, 0).Contains(hex);
	}

	bool IsInBuff(Hex hex) {
		return IsInBuff(hex, owner.Hex);
	}

	List<Hex> AffectedTiles(Hex source) {
		return PathFinder.BreadthFirstSearch(source, GameControl.gameControl.gridControl.Map, cardinality, 0);
	}

	List<Hex> AffectedTiles() {
		return AffectedTiles(owner.Hex);
	}

	public AuraBuff(string name, Unit owner, int cardinality,
	                UnitBuffAction onRemove = null,
	                UnitBuffAction onAdd = null) {
		Name = name;
		this.owner = owner;
		this.cardinality = cardinality;
		GameControl.gameControl.auraBuffs.Add(this);
		this.onRemove = onRemove ?? DoNothing;
		this.onAdd = onAdd ?? DoNothing;
		AffectedTiles().FindAll(h => h.Unit != null).ForEach(h => onAdd(h.Unit));
	}


}
