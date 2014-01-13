using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.localRotation = Quaternion.identity;
		StartAnimation();
	}

	Hex hex; 

	public Hex Hex { 
		get {
			return hex;
		}
		set {
			hex = value;
			renderer.material.mainTexture = hex.Unit != null ? Assets.Instance.AttackHex : Assets.Instance.MoveHex;
		}
	}

	public void MoveUp() {
		iTween.MoveTo(gameObject, iTween.Hash("name", "HoverMovement",
		                                      "y", 0.60f,
		                                      "time", 1f,
		                                      "oncomplete", "MoveDown",
		                                      "easetype", "easeInOutQuad"));
	}

	public void MoveDown() {
		iTween.MoveTo(gameObject, iTween.Hash("name", "HoverMovement",
		                                      "y", 0.25f,
		                                      "time", 1f,
		                                      "oncomplete", "MoveUp",
		                                      "easetype", "easeInOutQuad"));
	}

	public void MoveTo(Vector3 spot) {
		iTween.Stop (gameObject);
		transform.localPosition = new Vector3(spot.x, 0.25f, spot.z);
		StartAnimation();
	}

	void StartAnimation() {
		MoveUp ();
	}

	int i = 0;

	// Update is called once per frame
	void Update () {
		if(iTween.Count (gameObject) < 1) {
			i++;
			if(i > 5) {
				MoveUp ();
				i = 0;
			}
		}
	}
}
