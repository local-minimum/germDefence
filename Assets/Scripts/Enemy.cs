using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private bool killing = false;

	[SerializeField]
	public int _lives = 3;

	[Range(0, 1000)]
	public int startValue = 1000;

	[Range(0, 100)]
	public int valueDecay = 100;

	private float awakeTime;

	protected Level levelCoordinator;

	public float landingForce = 0.4f;
	protected bool hurting = false;
	public ParticleSystem injectionEffect;

	public int lives {
		get {
			return _lives;
		}

		set {
			if (value > 0)
				_lives = value;
			else {
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

	// Use this for initialization
	protected void Start () {
		levelCoordinator = GameObject.FindObjectOfType<Level>();
		awakeTime = levelCoordinator.playTime;
	}

	protected void Death() {
		if (killing)
			return;
		killing = true;
		levelCoordinator.ReportDeadEnemy(this);
		if (gameObject) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			gameObject.layer = LayerMask.NameToLayer("GroundEnemies");
			rigidbody2D.velocity = Vector2.zero;
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
}
