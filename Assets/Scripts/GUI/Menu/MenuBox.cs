using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuBox : MonoBehaviour {
	
	public AudioClip HoverSound;
	Menu parent;
	public int index = -1;
	List<Vector3> positions = new List<Vector3>();
	public bool clicked = false;
	bool moving = false;
	public MenuOption menuOption;
	
	
	// Use this for initialization
	void Start () {
		positions.Add(new Vector3(0, 4, 0));
		positions.Add(new Vector3(0, 0, 0));
		positions.Add(new Vector3(0, -4, 0));
		positions.Add(new Vector3(0, -8, 0));
		parent = GameObject.Find("Menu").GetComponent<Menu>();			
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseEnter() {
		
		if(clicked || index == 0 || moving) {
		} else {
			Camera.main.audio.PlayOneShot(HoverSound);
		}	
	}
	
	void OnMouseOver() {
		if(clicked || index == 0 || moving) {
		} else {
			iTween.MoveTo (gameObject, iTween.Hash ("x", positions[index].x+3, "islocal", true));
		}		
	}
	
	void OnMouseExit() {
		if(clicked || index == 0|| moving) {
		} else {
			Camera.main.audio.PlayOneShot(HoverSound);	
			iTween.MoveTo (gameObject, iTween.Hash ("x", positions[index].x,"islocal", true));
		}
	}
	
	public void SetMenuOption(MenuOption mo) {
		menuOption = mo;
		
	}
	
	void OnMouseDown() {
		if(index != 0) {
			int i = index;
			clicked = true;
			index = 0;
			
			iTween.MoveTo (gameObject, iTween.Hash ("x", positions[index].x,"islocal", true, "oncomplete", "MoveTop"));
			parent.Clicked(i);
		}
	}
	
	void MoveTop() {
		iTween.MoveTo(gameObject, iTween.Hash("y", positions[index].y,
											  "x", positions[index].x,
											  "time", 1,
											  "delay", 0, "islocal", true, "oncomplete", "MoveBack", "oncompleteparams", 0));
	}
	
	void MoveBack(int delay) {
		if(!clicked) {
			foreach(Transform child in transform.transform) {
				child.gameObject.GetComponent<TextMesh>().text = menuOption.Title;
			}
			iTween.MoveTo(gameObject, iTween.Hash ("y", positions[index].y, "delay", delay, "islocal", true, "oncomplete", "ToggleMoving"));
		}
		clicked = false;
	}
	
	void ToggleMoving() {
		moving = moving ? false : true;
	}
}
