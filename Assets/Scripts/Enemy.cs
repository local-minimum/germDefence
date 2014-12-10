using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[Range(0, 1000)]
	public int startValue = 1000;

	[Range(0, 100)]
	public int valueDecay = 100;

	private float awakeTime;

	protected Level levelCoordinator;

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


}
