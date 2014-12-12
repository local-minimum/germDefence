using UnityEngine;
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

	new public void Start() {
		base.Start();
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
	new void Update () {
		base.Update();
		if (warping && Time.timeSinceLevelLoad - warpTime > 0.5f)
			warping = false;

		myRB.velocity = baseSpeed * (1 + Mathf.PerlinNoise(speedPerlinX, speedPerlinY) * speedIrregularity) * Vector2.right;
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

	new void OnBecameInvisible() {

		if (!warping && !kamikazeMode) 
			warpIteration++;

		base.OnBecameInvisible();

	}

	void DropBomb() {
		Vir b = (Vir) Instantiate(bomb);
		b.Bomb((Enemy) this);
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
			int hit = dna.Hit();
			if (hit == 0)
				return;
			lives -= hit;

			bombF *= leakFactor;
			bombFVar *= leakFactor;

		}
	}
}
