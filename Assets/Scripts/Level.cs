using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	public Bact bacteria;
	public Camera mainCam;

	public float betweenBactTime = 2f;
	public float betweenBactTimeIrregularity = 0.3f;
	public float nextBact = -1f;

	public uiMeter immunity;
	bool _gameOver = false;
	bool _paused = false;
	private float pausedTime = 0f;
	private float lastPauseTime = 0f;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		if (!mainCam)
			mainCam = Camera.main;
	}

	public bool paused {

		get {
			return _paused || _gameOver;
		}

		set {

			if (value)
				lastPauseTime = Time.time;
			else
				pausedTime += Time.time - lastPauseTime;

			_paused = value;
		}
	}

	public float playTime {

		get {
			return Time.timeSinceLevelLoad;
		}
	}

	// Update is called once per frame
	void Update () {
		if (paused)
			return;

		if (immunity.hasHitZero)
			GameOver();

		if (nextBact < Time.timeSinceLevelLoad)
			SpawnBact();
	}

	void SpawnBact() {
		Bact b = (Bact) Instantiate(bacteria);
		b.Prep();
		b.StartAtBase();
		nextBact = Time.timeSinceLevelLoad + betweenBactTime * (1f + betweenBactTimeIrregularity * Random.value);
	}

	void GameOver() {
		Debug.Log("Game Over");
		_gameOver = true;
		StartCoroutine(Outro());
	}

	IEnumerator<WaitForSeconds> Outro() {
		while (Time.timeScale > 0.1f) {
			Time.timeScale *= 0.95f;
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds(0.3f);
		Application.LoadLevel(Application.loadedLevel);
	}
}
