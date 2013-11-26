using UnityEngine;
using System.Collections;

public class Nuke : MonoBehaviour
{
	GameObject explosion;
	Vector3 target;
	// Use this for initialization
	void Start ()
	{
		explosion = (GameObject) Resources.Load("Effects/Nuke");
	}
	
	public void Launch(Hex target) {
		this.target = target.collider.bounds.center;
		iTween.MoveTo(gameObject, iTween.Hash ("y", 100,
			"islocal", true,
			"orienttopath", true,
			"easetype", "easeInQuad",
			"time", 5,
			"oncomplete", "Land"));
	}
	
	void Land() {
		transform.localPosition = new Vector3(target.x, 100, target.z);
		iTween.MoveTo(gameObject, iTween.Hash ("y", 0,
			"islocal", true,
			"orienttopath", true,
			"easetype", "easeInQuad",
			"time", 5,
			"oncomplete", "Hit"));
	}
	
	void Hit() {
		Instantiate(explosion, target, Quaternion.identity);
		//GetComponent<Missile>().Hit();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

