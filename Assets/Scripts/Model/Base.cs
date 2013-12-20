using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : Unit
{
	public string prefabString = "Buildings/spacestation";
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
		Hex.Unit = this;
		Hex.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(h => h.renderer.material.color = Color.clear);
		Hex.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(h => h.Unit = this);
	}
	
	public override void FromCard(EntityCard card) {
		model = (GameObject) Instantiate((GameObject) Resources.Load(prefabString));
		model.transform.parent = transform;
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

