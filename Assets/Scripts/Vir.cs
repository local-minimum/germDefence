﻿using UnityEngine;
using System.Collections;

public class Vir : Enemy {

	public Vector3 dropOffset = Vector3.down;

	private float isBeingHurtTime;
	public float isBeingHurtDuration = 0.4f;

	new void Start () {
		base.Start(); 
		lives = 1;
		isBeingHurtTime = -2 * isBeingHurtDuration;
	}

	// Update is called once per frame
	new void Update () {
		base.Update();
		bool isBeingHurt = levelCoordinator.playTime - isBeingHurtTime < isBeingHurtDuration;
		if (!particleSystem.isPlaying && isBeingHurt)
			particleSystem.Play();
		else if (particleSystem.isPlaying && !isBeingHurt)
			particleSystem.Stop();
	}

	public void ParentFlightLeft() {
		dropOffset.x *= -1f;
	}

	public void Bomb(Enemy parent) {
		transform.parent = parent.transform.parent;
		transform.localPosition = parent.transform.localPosition + dropOffset;
		Vector3 v = parent.myRB.velocity;
		v.y = 0f;
		myRB.velocity = v;
	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Shot") {
			Dna dna = other.gameObject.GetComponent<Dna>();
			lives -= dna.HitSmall();
		} else if (other.tag == "SurfaceDefence")
			isBeingHurtTime = levelCoordinator.playTime;
	}

}
