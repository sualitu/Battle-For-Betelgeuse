using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class CombatControl : MonoBehaviour
{
	public void Combat(Unit attacker, Unit defender) {
		defender.Damage(attacker.Buffs.Exists(b => b is DeathTouchBuff) ? int.MaxValue :  attacker.Attack);
		if(!attacker.IsRanged()) {
			attacker.Damage(defender.Buffs.Exists(b => b is DeathTouchBuff) ? int.MaxValue :  defender.Attack);
		}
		attacker.Move(attacker.MovementLeft());
	}
}

