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
	int killScore = 0;
	public float timeScoreBonus = 1.5f;
	public UnityEngine.UI.Text scoreUI;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		if (!mainCam)
			mainCam = Camera.main;

	}

	private bool gameOver {
		get {
			return _gameOver;
		}

		set {
			if (!value) {
				Debug.LogError("Can't unset game over!");
			} else {
				lastPauseTime = Time.time;
				_gameOver = value;
			}
		}
	}

	public bool paused {

		get {
			return _paused || _gameOver;
		}

		set {
			if (gameOver) {
				Debug.LogWarning("Pausing when game is over is not pausing");
				return;
			} else if (value)
				lastPauseTime = Time.time;
			else
				pausedTime += Time.time - lastPauseTime;

			_paused = value;
		}
	}

	public float playTime {

		get {
			if (paused)
				return lastPauseTime - pausedTime;
			else 
				return Time.timeSinceLevelLoad - pausedTime;
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

	void OnGUI() {
		scoreUI.text = (killScore + Mathf.RoundToInt(playTime * timeScoreBonus)).ToString("D8");
	}

	void SpawnBact() {
		Bact b = (Bact) Instantiate(bacteria);
		b.Prep();
		b.StartAtBase();
		nextBact = Time.timeSinceLevelLoad + betweenBactTime * (1f + betweenBactTimeIrregularity * Random.value);
	}

	public void ReportDeadEnemy(Enemy e) {
//		Debug.Log(string.Format("{0} worth {1}", e.name, e.value));
		killScore += e.value;
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
