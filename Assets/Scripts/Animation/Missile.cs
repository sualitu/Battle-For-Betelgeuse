using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	public GameObject Explosion;
	
	public void Hit() {
		AudioControl.PlayAudioFileAt("Explosions/SmallExplosion", transform.position);
		Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
