using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	// Properties
	public int Attack { get; set; }
	public string UnitName { get; set; }
	public string Id { get; set; }
	public virtual int Team { get; set; }
	public Hex Hex { get; set; }
	public int MaxHealth { get; set; }
	public int MaxMovement { get; set; }
	public EntityCard Card { get; set; }
	public List<Hex> movable = new List<Hex>();
	List<UnitBuff> buffs = new List<UnitBuff>();

	protected GameObject model;
	protected int i = 0;

	// Fields
	private int damageTaken;
	private int moved;
	public Unit attacking = null;
	private bool activelyAttacking = false;
	List<GameObject> projectiles = new List<GameObject>();
	Dictionary<UnitBuff, GameObject> effects = new Dictionary<UnitBuff, GameObject>();
	
	// Unity 
	public Transform explosion;
	public GameObject ProjectilePrefab;
	
	public virtual void OnNewTurn(StateObject s) {
		buffs.FindAll(b => b.Duration == 0).ForEach(b => buffs.Remove (b));
		buffs.ForEach(b => b.OnNewTurn(this));
		Card.OnNewTurn(s);
	}

	public List<UnitBuff> Buffs {
		get {
			return buffs;
		}
	}
	
	public void AddBuff(UnitBuff buff) {
		buff.OnApplication(this);
		buffs.Add(buff);
	}

	public void RemoveBuff(UnitBuff buff) {
		if(buff.HasEffect) {
			Destroy(effects[buff]);
			effects.Remove(buff);
		}
		buff.OnRemoved(this);
		buffs.Remove(buff);
	}

	void Start() {
		GameControl.gameControl.auraBuffs.ForEach(ab => ab.CheckBuffOn(this));
	}
	
	public void Damage(int i) {
		if(buffs.Exists(ub => typeof(ForceFieldBuff).IsAssignableFrom(ub.GetType()))) {
			List<UnitBuff> buffsToRemove = new List<UnitBuff>();
			foreach(UnitBuff buff in buffs) {
				if(typeof(ForceFieldBuff).IsAssignableFrom(buff.GetType())) {
					buffsToRemove.Add(buff);
				}
			}
			buffsToRemove.ForEach(buff => RemoveBuff(buff));
		} else {
			damageTaken += i;
		}
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
		moved = 0;
	}
	
	public void AttackTarget(Unit unit, int delay) {
		Card.OnAttack(new StateObject(GameControl.gameControl.units, Hex, null, null));
		if(unit == null) return;
		System.Object[] args = new System.Object[2];
		args[0] = 5;
		args[1] = unit.Hex;
		Move (int.MinValue);
		iTween.LookTo(gameObject, iTween.Hash ("lookTarget", new Vector3(unit.Hex.renderer.bounds.center.x, 0f, unit.Hex.renderer.bounds.center.z),
			"time", 1,
			"delay", delay,
			"onstart", "Attacked",
			"onstarttarget", unit.gameObject,
			"onstartparams", this,
			"oncomplete", "FireMissiles",
			"oncompleteparams", args));
		
	}
	
	public void Attacked(Unit attacker) {
		bool defend = Card.OnAttacked(new StateObject(GameControl.gameControl.units, Hex, null, null));
		if(defend && !attacker.IsRanged()) {
			Hex hex = attacker.Hex;
			System.Object[] args = new System.Object[2];
			args[0] = 5;
			args[1] = hex;
			Move (int.MinValue);
			iTween.LookTo(gameObject, iTween.Hash ("lookTarget", new Vector3(hex.renderer.bounds.center.x, 0f, hex.renderer.bounds.center.z),
			"time", 1,
			"oncomplete", "FireMissiles",
			"oncompleteparams", args));
		}
	}
	
	
	public void FireMissiles(System.Object[] args) {
		activelyAttacking = true;
		int i = (int) args[0];
		for(int j = 0; j < i; j++) {
			AudioControl.PlayAudioFileAt("Missiles/ManyMissiles", new Vector3(transform.position.x, transform.position.y+1f,transform.position.z));		
			Hex hex = (Hex) args[1];
			GameObject missile = (GameObject) Instantiate(ProjectilePrefab, new Vector3(transform.position.x, transform.position.y+1f,transform.position.z), transform.localRotation);
			projectiles.Add(missile);
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
				"delay", (Random.Range (0.3f, 0.7f))*j,
				"oncomplete", "Hit"));
		}
	}

	public void AddEffect(UnitBuff buff, GameObject prefab) {
		GameObject go = (GameObject) Instantiate(prefab, new Vector3(transform.position.x+prefab.transform.position.x, transform.position.y+prefab.transform.position.y,transform.position.z+prefab.transform.position.z), prefab.transform.localRotation);
		effects.Add(buff, go);
		go.transform.parent = transform;
	}
	
	public void PrepareMove(Hex hex) {
		List<Hex> path = PathFinder.DepthFirstSearch(Hex, hex, GameControl.gameControl.gridControl.Map, MovementLeft());
		path.ForEach(h => h.renderer.material.color = Settings.StandardTileColour);
		if(path.Count > 0 && (hex.Unit == null || (Team != hex.Unit.Team && hex.Unit.Team != 0))) {
			GameControl.gameControl.auraBuffs.ForEach(ab => ab.NotifyOnMovement(this, hex));
			movable.ForEach(h => h.renderer.material.color = Settings.StandardTileColour);
			movable = new List<Hex>();
			GameControl.gameControl.mouseControl.DeselectHex();
			if(hex.Unit == null) {				
				MoveBy(path);
				if(Team == GameControl.gameControl.thisPlayer.Team) {
					GameControl.gameControl.mouseControl.SelectHex(hex);
				}
			} else {
				int delay = 0;
				if(path.Count > 1) {
					path.RemoveAt(path.Count-1);
					delay = path.Count;
					if(Team == GameControl.gameControl.thisPlayer.Team) {
						GameControl.gameControl.mouseControl.SelectHex(path[path.Count-1]);
					}
					MoveBy(path);
				} 
				attacking = hex.Unit;
				AttackTarget(hex.Unit, delay);
			}
		} else {
			if(GameControl.gameControl.state == State.MYTURN) {
				GameControl.gameControl.audioControl.PlayErrorSound();
				GameControl.gameControl.guiControl.ShowSmallSplashText(Dictionary.cannotMoveThereError);		
			}
		}
		
	}
	
	public bool IsRanged() {
		return buffs.Exists(b => b is RangedBuff);
	}
	
	public virtual string ConstructTooltip() {
		string s = ((Team == GameControl.gameControl.thisPlayer.Team) ? "Your " : "Enemy ") + UnitName + "\nAttack: " + Attack + 
			"\nHealth: " + (CurrentHealth() < 1 ? "0" : (CurrentHealth()).ToString()) + " / " + MaxHealth.ToString() + 
				"\nMovement: " + (MovementLeft() < 1 ? "0" : (MovementLeft()).ToString()) + " / " + MaxMovement.ToString();
		return s;
	}
	
	void MoveBy(List<Hex> path) {
		Move (path.Count);
		Hex prevHex = Hex;
		Hex newHex = path.FindLast(h => true);
		List<Transform> p = path.ConvertAll<Transform>(h => h.transform);
		gameObject.GetComponent<AudioSource>().Play();
		if(p.Count > 1) {
			iTween.MoveTo(gameObject, iTween.Hash (
				"path", p.ToArray(),
				"time", p.Count,
				"orienttopath", true,
				"easetype", "easeInOutQuad",
				"oncomplete", "MovementDone"));
		} else {
			iTween.MoveTo(gameObject, iTween.Hash (
				"position", p[0],
				"time", p.Count,
				"orienttopath", true,
				"easetype", "easeInOutQuad",
				"oncomplete", "MovementDone"));
		}
		prevHex.Unit = null;
		Hex = newHex;
		newHex.Unit = this;
	}
	
	public void MovementDone() {
		gameObject.GetComponent<AudioSource>().Stop();
	}
	
	public virtual void FromCard (EntityCard card) {
		Card = card;
		model = (GameObject) Instantiate(card.Prefab, new Vector3(transform.position.x+card.Prefab.transform.position.x, transform.position.y+card.Prefab.transform.position.y,transform.position.z+card.Prefab.transform.position.z), card.Prefab.transform.localRotation);
		model.transform.parent = transform;
		Attack = card.Attack;
		MaxHealth = card.Health;
		MaxMovement = card.Movement;
		UnitName = card.Name;
		ProjectilePrefab = card.ProjectilePrefab;
	}
		
	void Update () {
		if(GameControl.gameControl.mouseControl.selectedUnit == this) {
			movable.ForEach(h => h.renderer.material.color = GameControl.gameControl.mouseControl.mouseOverHex == h ? Settings.MouseOverTileColour : Settings.MovableTileColour);	
		}
		if(CurrentHealth() < 1) {
			Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
			GameControl.gameControl.units.Remove(this);
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
		// TODO Do this properly
		if(attacking != null && activelyAttacking) {
			projectiles.RemoveAll(p => p == null);
			if(projectiles.Count < 1) {
				GameControl.gameControl.combatControl.Combat(this, attacking);
				attacking.activelyAttacking = false;
				attacking.attacking = null;
				activelyAttacking = false;
				attacking = null;
			}
		}
	}
}
