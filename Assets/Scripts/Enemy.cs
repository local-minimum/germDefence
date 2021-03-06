﻿using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

	private bool killing = false;

	[SerializeField]
	public int _lives = 3;

	[Range(0, 1000)]
	public int startValue = 1000;

	[Range(0, 100)]
	public int valueDecay = 100;

	[Range(0, 10)]
	public float damage = 2f;

	private float awakeTime;

	protected Level levelCoordinator;

	public float landingForce = 0.4f;
	protected bool hurting = false;
	public ParticleSystem injectionEffect;
	public GameObject[] hurtSprites;

	protected bool warping = false;
	protected float warpTime = 0f;

	[HideInInspector]
	public Rigidbody2D myRB;

	public int lives {
		get {
			return _lives;
		}

		set {
			if (value > 0) {
				_lives = value;
				for (int i=0; i<hurtSprites.Length; i++)
					hurtSprites[i].SetActive(i >= value - 1);
			} else {
				_lives = 0;
				Death();
			}
		}
	}

	public int value {
		get {
			int val = startValue - valueDecay * Mathf.RoundToInt(levelCoordinator.playTime - awakeTime);
			return val >= 0 ? val : 0;
		}
	}

	protected void Awake() {
		levelCoordinator = GameObject.FindObjectOfType<Level>();
		myRB = gameObject.GetComponentInParent<Rigidbody2D>();
	}

	// Use this for initialization
	protected void Start () {

		awakeTime = levelCoordinator.playTime;
		foreach (GameObject go in hurtSprites)
			go.SetActive(false);
	}

	protected void Death() {
		if (killing)
			return;
		killing = true;
		levelCoordinator.ReportDeadEnemy(this);
		if (myRB && myRB.gameObject) {
			Destroy(myRB.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			gameObject.layer = LayerMask.NameToLayer("GroundEnemies");
			myRB.velocity = Vector2.zero;
			if (!hurting) {
				hurting = true;
				Animator a = gameObject.GetComponent<Animator>();
				a.SetBool("landed", true);
				iTween.PunchPosition(
					levelCoordinator.mainCam.gameObject,
					iTween.Hash(
					"amount", Vector3.down,
					"time", landingForce));
				injectionEffect.Play();
			}
		}
	}

	protected void OnBecameInvisible() {
		if (warping)
			return;
		
		warping = true;
		warpTime = Time.timeSinceLevelLoad;
		
		Vector3 screenPos = myRB.transform.position;
		screenPos.x *= -0.96f;
		myRB.transform.position = screenPos;
		
	}

	protected void Update() {
		if (hurting) 
			levelCoordinator.immunity.Drain(damage * Time.deltaTime);

		if (warping && Time.timeSinceLevelLoad - warpTime > 0.5f)
			warping = false;
	}
}
