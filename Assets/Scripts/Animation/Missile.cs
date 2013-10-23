using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	public GameObject Explosion;
	
	public void Hit() {
		Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
