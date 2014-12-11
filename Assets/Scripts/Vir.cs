﻿using UnityEngine;
using System.Collections;

public class Vir : Enemy {

	public Vector3 dropOffset = Vector3.down;

	private bool warping = false;
	private float warpTime = 0f;

	public uiMeter immunity;
	public float damage = 2f;
	private bool hurting = false;


	new void Start () {
		lives = 1;
		base.Start(); 
		immunity = levelCoordinator.immunity;
	}

	// Update is called once per frame
	void Update () {

		if (hurting) {
			immunity.Drain(damage * Time.deltaTime);
			return;
		}

		if (warping && Time.timeSinceLevelLoad - warpTime > 0.5f)
			warping = false;
	}

	public void ParentFlightLeft() {
		dropOffset.x *= -1f;
	}

	public void Bomb(GameObject parent) {
		transform.parent = parent.transform.parent;
		transform.localPosition = parent.transform.localPosition + dropOffset;
		Vector3 v = parent.rigidbody2D.velocity;
		v.y = 0f;
		rigidbody2D.velocity = v;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			rigidbody2D.velocity = Vector2.zero;
			hurting = true;
			Animator a = gameObject.GetComponent<Animator>();
			a.SetBool("landed", true);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Shot") {
			Dna dna = other.gameObject.GetComponent<Dna>();
			dna.HitSmall();
			Death();
		}
	}

	void OnBecameInvisible() {
		if (warping)
			return;

		warping = true;
		warpTime = Time.timeSinceLevelLoad;

		Vector3 screenPos = transform.position;
		screenPos.x *= -0.96f;
		transform.position = screenPos;

	}

}
