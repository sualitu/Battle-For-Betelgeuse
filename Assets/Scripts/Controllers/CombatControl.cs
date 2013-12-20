using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class CombatControl : MonoBehaviour
{	
	/// <summary>
	/// Combat the specified attacker and defender.
	/// </summary>
	/// <param name='attacker'>
	/// Attacker.
	/// </param>
	/// <param name='defender'>
	/// Defender.
	/// </param>
	public void Combat(Unit attacker, Unit defender) {
		defender.Damage(attacker.Buffs.Exists(b => b is DeathTouchBuff) ? int.MaxValue :  attacker.Attack);
		if(!attacker.IsRanged()) {
			attacker.Damage(defender.Buffs.Exists(b => b is DeathTouchBuff) ? int.MaxValue :  defender.Attack);
		}
		attacker.Move(attacker.MovementLeft());
	}
}

