using UnityEngine;
using System.Collections.Generic;


public delegate int HexEvaluator(Hex hex);

public class MockUnit 
{
	public int Attack;
	public int CurrentHealth;
	public int MaxHealth;
	public Team Team;
	public Hex Hex;
	public List<UnitBuff> Buffs;
	public int Movement;
	public string Id;
	
	public MockUnit(Unit unit) {
		Attack = unit.Attack;
		CurrentHealth = unit.CurrentHealth();
		MaxHealth = unit.MaxHealth;
		Team = unit.Team;
		Hex = unit.Hex;
		Buffs = unit.Buffs;
		Id = unit.Id;
		Movement = unit.MaxMovement;
	}
	
	public int Value(HexEvaluator he) {
		if(CurrentHealth < 1) return 0;
		int result = 0;
		result += Attack;
		result += CurrentHealth;
		result += MaxHealth;
		result += (Movement) / 3;
		result += Buffs.Count*10;

		result += he(Hex);

		return result;
	}


	public void ApplyMockAttack(MockUnit target) {
		int dmgTaken = target.Buffs.Exists (b => b is DeathTouchBuff) ? int.MaxValue : target.Attack;
		if(Buffs.Exists(b => b is RangedBuff)) { Buffs.RemoveAll(b => b is ForceFieldBuff); dmgTaken = 0; }
		int dmgInflicted = Buffs.Exists(b => b is ForceFieldBuff) ? 0 : Buffs.Exists(b => b is DeathTouchBuff) ? int.MaxValue : Attack;
		if(target.Buffs.Exists(b => b is RangedBuff)) { target.Buffs.RemoveAll(b => b is ForceFieldBuff); dmgInflicted = 0; }
		CurrentHealth -= dmgTaken;
		target.CurrentHealth -= dmgInflicted;
	}

	public int MockAttack(MockUnit target, HexEvaluator he) {
		int oldValue = Value (he);
		int oldTargetValue = target.Value (he);
		int dmgTaken = target.Buffs.Exists (b => b is DeathTouchBuff) ? int.MaxValue : target.Attack;
		if(Buffs.Exists(b => b is RangedBuff)) { Buffs.RemoveAll(b => b is ForceFieldBuff); dmgTaken = 0; }
		int dmgInflicted = Buffs.Exists(b => b is ForceFieldBuff) ? 0 : Buffs.Exists(b => b is DeathTouchBuff) ? int.MaxValue : Attack;
		if(target.Buffs.Exists(b => b is RangedBuff)) { target.Buffs.RemoveAll(b => b is ForceFieldBuff); dmgInflicted = 0; }
		if(dmgTaken > CurrentHealth) {
			// I die
			if(dmgInflicted > target.CurrentHealth) {
				// Target dies
				return 10 * (oldValue - oldTargetValue);
			} else {
				// Target survives
				return 10 * (dmgInflicted - oldValue);
			}
		} else {
			// I survive
			if(dmgInflicted > target.CurrentHealth) {
				// Target dies
				return 10 * (oldTargetValue - 
						(oldValue - Value(he)) - // Change in my value
						(he(target.Hex) - he(Hex)) - // Change in value of hex
						dmgTaken); // Damage taken;
			} else {
				// Target survives
				return 10 * ((oldTargetValue - target.Value (he)) + // Change in target value (from buffs etc)
						dmgInflicted - // Damage taken by target
						(oldValue - Value(he)) - // Change in my value
						(he(Hex) - he(target.Hex)) - // Change in value of hex
						dmgTaken); // Damage taken
			}
			
		}
	}


}

