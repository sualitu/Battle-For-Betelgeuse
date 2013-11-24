using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flag : Unit
{
	public string prefabString = "";
	List<Hex> hexs;
	public override int Team {
		get {
			return 0;
		}
	}
	
	public int OwnerTeam { get; set; }
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
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
				switch(OwnerTeam) {
				case 1:
					c = Settings.OwnedFlagTileColour;
					break;
				case 2:
					c = Settings.EnemyFlagTileColour;
					break;
				default:
					c = Settings.NeutralFlagTileColour;
					break;
				}
				h.renderer.material.color = c;
			}
		}
	}
	
	public List<Hex> Hexs {
		get {
			if(hexs == null) {
				hexs = PathFinder.BreadthFirstSearch(Hex, GameControl.gameControl.gridControl.Map, 2, Team);
			}
			return hexs;
		}
	}
	
	public override void FromCard(EntityCard card) {
		model = (GameObject) Instantiate((GameObject) Resources.Load(prefabString));
		model.transform.parent = transform;
	}

	public override void OnNewTurn (StateObject s)
	{
		// Check owner status
		switch(OwnerTeam) {
		case 0: // Flag is neutral.
			if(Hexs.Exists(h => h.Unit != null && h.Unit != this)) {
				bool p1 = Hexs.Exists(h => h.Unit != null && h.Unit.Team == 1);
				bool p2 = Hexs.Exists(h => h.Unit != null && h.Unit.Team == 2);
				if(p1 && !p2) {
					OwnerTeam = 1;
				} else if(!p1 && p2) {
					OwnerTeam = 2;
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
		case 1: return "Your";
		case 2: return "Enemy";
		default: return "Neutral";
		}
	}

	public override string ConstructTooltip ()
	{
		return OwnerString() + " Flag";
	}
}
