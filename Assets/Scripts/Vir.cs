﻿using UnityEngine;
using System.Collections;

public class Vir : Enemy {

	public Vector3 dropOffset = Vector3.down;
	private bool warping = false;
	private float warpTime = 0f;

	public uiMeter immunity;
	public float damage = 2f;
	private float isBeingHurtTime;
	public float isBeingHurtDuration = 0.4f;

	new void Start () {
		lives = 1;
		base.Start(); 
		immunity = levelCoordinator.immunity;
		isBeingHurtTime = -2 * isBeingHurtDuration;
	}

	// Update is called once per frame
	void Update () {
		bool isBeingHurt = levelCoordinator.playTime - isBeingHurtTime < isBeingHurtDuration;
		if (!particleSystem.isPlaying && isBeingHurt)
			particleSystem.Play();
		else if (particleSystem.isPlaying && !isBeingHurt)
			particleSystem.Stop();

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
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Shot") {
			Dna dna = other.gameObject.GetComponent<Dna>();
			lives -= dna.HitSmall();
		} else if (other.tag == "SurfaceDefence")
			isBeingHurtTime = levelCoordinator.playTime;
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
