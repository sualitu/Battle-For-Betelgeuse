using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flag : Unit
{
	public string prefabString = "";
	List<Hex> hexs;
	public override Team Team {
		get {
			return Team.NEUTRAL;
		}
	}

	GameObject ring;

	Team ownerTeam;
	
	public Team OwnerTeam { 
		get {
			return ownerTeam;
		}
		set {
			ownerTeam = value;
			Destroy (ring);
			if(ownerTeam == Team.ME) {
				Model.renderer.material.mainTexture = Assets.Instance.OwnedFlag;
				ring = (GameObject) Instantiate(Assets.Instance.GreenFlagRing, transform.position, Quaternion.identity);
			}
			else if(ownerTeam == Team.ENEMY) {
				Model.renderer.material.mainTexture = Assets.Instance.EnemyFlag;
				ring = (GameObject) Instantiate(Assets.Instance.RedFlagRing, transform.position, Quaternion.identity);
			}
			else {
				Model.renderer.material.mainTexture = Assets.Instance.NeutralFlag;
				ring = (GameObject) Instantiate(Assets.Instance.GreyFlagRing, transform.position, Quaternion.identity);
			}
		}
	}
	// Use this for initialization
	void Start ()
	{
		OwnerTeam = Team.NEUTRAL;
	}
	
	// Update is called once per frame
	void Update ()
	{
		ColourizeHexs();
		Hex.renderer.material.color = Color.clear;
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
	
	public void ColourizeHexs() {
		if(hexs != null) {
			foreach(Hex h in hexs) {
				Color c;
				if(GameControl.gameControl.ThisPlayer.targets.Contains(h)) {
					c = Settings.MovableTileColour;
				} else {
					switch(OwnerTeam) {
					case Team.ME:
						c = Settings.OwnedFlagTileColour;
						break;
					case Team.ENEMY:
						c = Settings.EnemyFlagTileColour;
						break;
					default:
						c = Settings.NeutralFlagTileColour;
						break;
					}
				}
				h.renderer.material.color = c;
			}
		}
	}
	
	public List<Hex> Hexs {
		get {
			if(hexs == null) {
				hexs = PathFinder.BreadthFirstSearch(Hex, GameControl.gameControl.GridControl.Map, 2, Team);
			}
			return hexs;
		}
	}

	public void SwapOwner() {
		if(OwnerTeam != Team.NEUTRAL) OwnerTeam = OwnerTeam == Team.ME ? Team.ENEMY : Team.ME;
	}
	
	public override void FromCard(EntityCard card) {
		Model = (GameObject) Instantiate(Assets.Instance.Flag);
		Model.transform.parent = transform;
	}

	public override void OnNewTurn (StateObject s)
	{
		// Check owner status
		switch(OwnerTeam) {
		case 0: // Flag is neutral.
			if(Hexs.Exists(h => h.Unit != null && h.Unit != this)) {
				bool p1 = Hexs.Exists(h => h.Unit != null && h.Unit.Team == Team.ME);
				bool p2 = Hexs.Exists(h => h.Unit != null && h.Unit.Team == Team.ENEMY);
				if(p1 && !p2) {
					OwnerTeam = Team.ME;
				} else if(!p1 && p2) {
					OwnerTeam = Team.ENEMY;
				}
			}
			break;
		default: // Flag is owned.
			if(Hexs.Exists(h => h.Unit != null && h.Unit != this)) {
				if(!Hexs.Exists(h => h.Unit != null && h.Unit.Team == OwnerTeam)) {
					OwnerTeam = 0;
				}
			}
			break;
		}
	}
	
	public string OwnerString() {
		switch(OwnerTeam) {
		case Team.ME: return "Your";
		case Team.ENEMY: return "Enemy";
		default: return "Neutral";
		}
	}

	public override string ConstructTooltip ()
	{
		return OwnerString() + " Flag";
	}
}

