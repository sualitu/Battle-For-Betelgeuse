using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	
	// Properties
	public int Attack { get; set; }
	public string UnitName { get; set; }
	public string Id { get; set; }
	public int Team { get; set; }
	public Hex Hex { get; set; }
	public int MaxHealth { get; set; }
	public int MaxMovement { get; set; }
		
	GameObject model;
	int i = 0;
		
	// Fields
	private int damageTaken;
	private int moved;
	
	// Unity 
	public Transform explosion;
	public GameObject MissilePrefab;
	
	
	public void Damage(int i) {
		damageTaken += i;
	}
	
	public int CurrentHealth() {
		return MaxHealth - damageTaken;
	}
	
	public void Move(int i) {
		moved += i;
	}
	
	public int MovementLeft() {
		return MaxMovement - moved;
	}
	
	public void ResetStats() {
		if(damageTaken > 0) {
			damageTaken -= 1;
		}
		moved = 0;
	}
	
	public void AttackTarget(Hex hex) {
		if(hex.Unit == null) return;
		System.Object[] args = new System.Object[2];
		args[0] = 5;
		args[1] = hex;
		FireMissiles(args);
		//iTween.RotateTo(gameObject, iTween.Hash ("rotation", hex.renderer.bounds.center,
		//									     "oncomplete", "FireMissiles",
		//										 "oncompleteparams", args));
	}
	
	public void FireMissiles(System.Object[] args) {
		int i = (int) args[0];
		for(int j = 0; j < i; j++) {
		Hex hex = (Hex) args[1];
			GameObject missile = (GameObject) Instantiate(MissilePrefab, new Vector3(transform.position.x, transform.position.y+1f,transform.position.z), transform.localRotation);
			Vector3 targetPos = new Vector3(hex.transform.position.x+Random.Range (-0.5f, 0.5f), hex.transform.position.y+1f, hex.transform.position.z+Random.Range (-0.5f, 0.5f));
			float deltaX = Mathf.Abs(missile.transform.position.x - targetPos.x)/3;
			float deltaZ = Mathf.Abs(missile.transform.position.z - targetPos.z)/3;
			Vector3 v1 = new Vector3(missile.transform.position.x + deltaX + Random.Range (-2f, 2f), Random.Range (-0.5f, 0.5f), missile.transform.position.z + deltaZ + Random.Range (-2f, 2f));
			Vector3 v2 = new Vector3(missile.transform.position.x + deltaX*2 + Random.Range (-2f, 2f), Random.Range (-0.5f, 0.5f), missile.transform.position.z + deltaZ*2 + Random.Range (-2f, 2f));
			Vector3[] t = new Vector3[3];
			t[0] = v1;
			t[1] = v2;
			t[2] = targetPos;
			iTween.MoveTo(missile.gameObject, iTween.Hash ("path", t,
														   "islocal", true,
														   "orienttopath", true,
														   "easetype", "easeInQuad",
														   "time", 2,
														   "delay", 0.5f*j,
														   "oncomplete", "Hit"));
		}
	}
	
	public void MovementDone(bool combatFollows) {
	}
	
	public void FromCard (Card card) {
		model = (GameObject) Instantiate(card.Prefab, new Vector3(transform.position.x, transform.position.y+1f,transform.position.z), card.Prefab.transform.localRotation);
		model.transform.parent = transform;
		Attack = card.Attack;
		MaxHealth = card.Health;
		MaxMovement = card.Movement;
		UnitName = card.Name;
	}
		
	void Update () {
		
		if(CurrentHealth() < 1) {
			Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		if(i <= 7500) {		
			transform.position = new Vector3(transform.position.x, transform.position.y + (i < 3750 ? 0.000001f*(1+i) : 0.000001f*(7501-i)) , transform.position.z);
			i += 100;
		} else if (i <= 15000) {
			transform.position = new Vector3(transform.position.x, transform.position.y - (i < 11250 ? 0.000001f*(1+i-7500) : 0.000001f*(15001-i)), transform.position.z);
			i += 100;
		} else {
			i = 0;
		}
	}
}
