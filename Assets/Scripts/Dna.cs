using UnityEngine;
using System.Collections;

public class Dna : MonoBehaviour {

	[Range(0, 10)]
	public float shotSpeed = 1f;

	bool notShot = true;
	private bool loaded = true;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 4f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!loaded)
			return;

		if (notShot) {
			rigidbody2D.velocity = transform.up * shotSpeed;
			notShot = false;
		}
	}

	public int HitSmall() {
		particleSystem.startLifetime = 0.25f;
		particleSystem.startSpeed = 7.5f;
		return Hit();
	}

	public int Hit() {
		rigidbody2D.velocity = Vector3.zero;
		loaded = false;
		renderer.enabled = false;
		particleSystem.Play();

		return 1;
	}
}
