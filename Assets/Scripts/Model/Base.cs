using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : Unit
{
	public string prefabString = "Buildings/spacestation";
	List<Hex> hexs;
	public override Team Team {
		get {
			return Team.NEUTRAL;
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
		Hex.Unit = this;
		Hex.Adjacent(GameControl.gameControl.GridControl.Map).ForEach(h => h.renderer.material.color = Color.clear);
		Hex.Adjacent(GameControl.gameControl.GridControl.Map).ForEach(h => h.Unit = this);
	}
	
	public override void FromCard(EntityCard card) {
		Model = (GameObject) Instantiate((GameObject) Resources.Load(prefabString));
		Model.transform.parent = transform;
	}
	
	public override void OnNewTurn (StateObject s)
	{

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
		return OwnerString() + " Base";
	}
}

