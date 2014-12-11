﻿using UnityEngine;
using System.Collections;

public class Bact : Enemy {

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

	[Range(0, 2)]
	public float leakFactor = 0.85f;

	public GameObject hurt1;
	public GameObject hurt2;

	private float speedPerlinX = 0f;
	private float speedPerlinY = 0f;

	private float altitudePerlinX = 0f;
	private float altitudePerlinY = 0f;

	private Vector3 spawnPos;
	private float warpIteration = 0f;

	[Range(0, 10)]
	public float bombF = 1f;

	[Range(0, 10)]
	public float bombFVar = 0.1f;

	private float nextBomb = 0f;

	public Vir bomb;

	private bool kamikazeMode = false;
	private bool warping = false;
	private float warpTime = 0f;

	new public void Start() {
		base.Start();
		hurt1.SetActive(false);
		hurt2.SetActive(false);
		nextBomb = levelCoordinator.playTime + bombF + Random.Range(-bombFVar, bombFVar);
	}

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

		if (nextBomb < levelCoordinator.playTime)
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
		nextBomb = levelCoordinator.playTime + bombF + Random.Range(-bombFVar, bombFVar);
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
			if (lives == 2)
				hurt1.SetActive(true);
			else if (lives == 1)
				hurt2.SetActive(true);
			else
				return;

			bombF *= leakFactor;
			bombFVar *= leakFactor;

		}
	}
}
