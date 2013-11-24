using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EntityCard : Card
{
	public GameObject Prefab { get; set; }
	public GameObject ProjectilePrefab { get; set; }
	public abstract string Projectile { get; }
	public abstract int Attack { get; }
	public abstract int Health { get; }
	public abstract int Movement { get; }
	public abstract string PrefabPath { get; }
	
	
	public virtual void OnNewTurn(StateObject s) {
		
	}
	
	protected bool StandardOnAttacked() {
		foreach(StandardSpecial ss in StandardSpecials) {
			if(ss.GetType() == typeof(StandardSpecial.Defenseless)) {
				return false;
			}
		}
		return true;
	}
	
	public virtual void OnAttack(StateObject s) {
	}
	
	public virtual bool OnAttacked(StateObject s) {
		return StandardOnAttacked();
	}
	
	public EntityCard() : base() {
		Prefab = (GameObject) Resources.Load(PrefabPath);
		ProjectilePrefab = (GameObject) Resources.Load("Projectiles/"+Projectile);
	}
}

