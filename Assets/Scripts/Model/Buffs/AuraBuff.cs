using UnityEngine;
using System.Collections.Generic;

public class AuraBuff : UnitBuff
{
	UnitBuffAction onAdd;

	public void NotifyOnMovement(Unit unit, Hex target) {
		if(owner == unit) {
			List<Unit> oldAffUnits = AffectedTiles().FindAll(h => UnitIsAffected(h.Unit)).ConvertAll<Unit>(h => h.Unit);
			List<Unit> newAffUnits = AffectedTiles(target).FindAll(h => UnitIsAffected(h.Unit)).ConvertAll<Unit>(h => h.Unit);
			oldAffUnits.FindAll(u => !newAffUnits.Contains(u)).ForEach(u => onRemove(u));
			newAffUnits.FindAll(u => !oldAffUnits.Contains(u)).ForEach(u => onAdd(u));
			return;
        }
		if((IsInBuff(unit.Hex) ^ IsInBuff(target)) && UnitIsAffected(unit)) {
			if(IsInBuff(target)) {
				onAdd(unit);
			} else {
				onRemove(unit);
			}
		} 
	}

	public void NotifyOnDeath(Unit unit) {
		if(unit == owner) {
			List<Unit> affUnits = AffectedTiles().FindAll(h => UnitIsAffected(h.Unit)).ConvertAll<Unit>(h => h.Unit);
			affUnits.ForEach(u => onRemove(u));
			GameControl.gameControl.AuraBuffs.Remove(this);
		}
	}

	public void CheckBuffOn(Unit unit) {
		if(IsInBuff(unit.Hex) && UnitIsAffected(unit)) {
			onAdd(unit);
		}
	}

	bool UnitIsAffected(Unit unit) {
		return unit != null && unit != owner && (debuff ^ owner.Team == unit.Team);
	}

	Unit owner = null;
	int cardinality;
	bool debuff = false;

	bool IsInBuff(Hex hex, Hex source) {
		return PathFinder.BreadthFirstSearch(source, GameControl.gameControl.GridControl.Map, cardinality, 0).Contains(hex);
	}

	bool IsInBuff(Hex hex) {
		return IsInBuff(hex, owner.Hex);
	}

	List<Hex> AffectedTiles(Hex source) {
		return PathFinder.BreadthFirstSearch(source, GameControl.gameControl.GridControl.Map, cardinality, 0);
	}

	List<Hex> AffectedTiles() {
		return AffectedTiles(owner.Hex);
	}

	public AuraBuff(string name, Unit owner, int cardinality,
	                UnitBuffAction onRemove = null,
	                UnitBuffAction onAdd = null,
	                bool debuff = false) {
		this.name = name;
		this.debuff = debuff;
		this.owner = owner;
		this.cardinality = cardinality;
		GameControl.gameControl.AuraBuffs.Add(this);
		this.onRemove = onRemove ?? DoNothing;
		this.onAdd = onAdd ?? DoNothing;
		AffectedTiles().FindAll(h => h.Unit != null).ForEach(h => onAdd(h.Unit));
	}


}

