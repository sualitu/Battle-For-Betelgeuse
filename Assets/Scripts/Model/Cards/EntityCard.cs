using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	public virtual bool HasSpecialAbility {
		get {
			return false;
		}
	}

	public virtual void SpecialAbility(StateObject s) {
		if(!HasSpecialAbility) {
			Debug.LogWarning("Special ability attempted on card without special ability");
		}
	}

	public virtual int Value() {
		return Attack + Health + Movement + StandardSpecials.Sum (ss => ss.Value());
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

	public virtual int OnDamaged(StateObject s, int d) {
		return d;
	}
	
	public EntityCard() : base() {
		Prefab = (GameObject) Resources.Load(PrefabPath);
		ProjectilePrefab = (GameObject) Resources.Load("Projectiles/"+Projectile);
	}
}

