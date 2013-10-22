using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	public GameObject Explosion;
	
	public void Hit() {
		Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
