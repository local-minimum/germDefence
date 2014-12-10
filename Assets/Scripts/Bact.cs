﻿using UnityEngine;
using System.Collections;

public class Bact : MonoBehaviour {
	
	public int lives = 3;

	public float baseSpeed = 1f;

	[Range(0, 10)]
	public float speedIrregularity = 0.1f;

	[Range(0, 10)]
	public float speedIrregularityFreq = 1f;

	[Range(0, 1)]
	public float altitudeIrregularity = 0.01f;

	[Range(0, 10)]
	public float altitudeIrregularityFreq = 1f;

	[Range(0, 10)]
	public float warpAltitudeLoss = 0.1f;

	private float speedPerlinX = 0f;
	private float speedPerlinY = 0f;

	private float altitudePerlinX = 0f;
	private float altitudePerlinY = 0f;

	private Vector3 spawnPos;
	private float warpIteration = 0f;

	[Range(0, 1)]
	public float bombP = 0.1f;

	public Vir bomb;

	private bool kamikazeMode = false;
	private bool warping = false;
	private float warpTime = 0f;

	// Use this for initialization
	public void Prep () {

		speedPerlinX = Random.value * 100;
		altitudePerlinX = Random.value * 100;
		spawnPos = transform.localPosition;
		if (Random.value < 0.5f)
			FlyLeft();
	}

	public void FlyLeft() {
		Vector3 scale = transform.localScale;
		scale.x *= -1f;
		transform.localScale = scale;
		spawnPos.x *= -1f;
		baseSpeed *= -1f;
	}

	public void StartAtBase() {
		transform.localPosition = spawnPos;
	}

	// Update is called once per frame
	void Update () {
		if (warping && Time.timeSinceLevelLoad - warpTime > 0.5f)
			warping = false;

		rigidbody2D.velocity = baseSpeed * (1 + Mathf.PerlinNoise(speedPerlinX, speedPerlinY) * speedIrregularity) * Vector2.right;
		transform.localPosition = new Vector3(transform.localPosition.x, 
		                                      spawnPos.y - (warpIteration * warpAltitudeLoss) + altitudeIrregularity * Mathf.PerlinNoise(altitudePerlinX, altitudePerlinY), 
		                                      transform.localPosition.z);
		if (Random.value < bombP * Time.deltaTime)
			DropBomb();
	}

	void FixedUpdate() {
		speedPerlinY += speedIrregularityFreq;
		altitudePerlinY += altitudeIrregularityFreq;
	}

	void OnBecameInvisible() {
		if (warping)
			return;

		if (kamikazeMode) {
			warping = true;
			warpTime = Time.timeSinceLevelLoad;
			Vector3 screenPos = transform.position;
			screenPos.x *= -0.96f;
			transform.position = screenPos;
		} else {
			warpIteration++;
			transform.localPosition = spawnPos;
		}
	}

	void DropBomb() {
		Vir b = (Vir) Instantiate(bomb);
		b.Bomb(gameObject);
	}

	void SetBombTarget(Transform ground) {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Ground") {
			SetBombTarget(other.transform);
			kamikazeMode = true;
		} else if (other.tag == "Shot") {
			Dna dna = other.gameObject.GetComponent<Dna>();
			lives -= dna.Hit();
			if (lives < 0) {
				Destroy(gameObject);
				return;
			}

		}
	}
}