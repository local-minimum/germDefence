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

	private int value {
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
		if (gameObject) {
			Destroy(gameObject);
		}
	}

}
