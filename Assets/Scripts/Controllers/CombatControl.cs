using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class CombatControl : MonoBehaviour
{
	public void Combat(Unit attacker, Unit defender) {
		if(attacker.Buffs.Exists(b => b is DeathTouchBuff))
			defender.Kill();
		else
			defender.Damage(attacker.Attack);
		if(!attacker.IsRanged()) {
			if(defender.Buffs.Exists(b => b is DeathTouchBuff))
			   attacker.Kill();
			else
			   attacker.Damage(defender.Attack);
		}
		attacker.Move(attacker.MovementLeft());
	}
}

