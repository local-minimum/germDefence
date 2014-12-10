using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public Camera mainCam = null;

	[Range(0f,100f)]
	public float shakeFactor = 1f;

	[Range(0f, 10f)]
	public float shakeFreq = 0.01f;

	private float shakeX = 0f;
	private float shakeY = 0f;

	private float perlinMean = 0.4652489f;

	public GameObject shot;

	private bool _activated = true;

	public uiMeter energyMeter;
	private Level lvl ;

	// Use this for initialization
	void Start () {
		shakeX = Random.value * 100f;
		shakeY = Random.value * 100f;
		lvl = GameObject.FindObjectOfType<Level>();

		if (!mainCam)
			mainCam = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!_activated && lvl.paused)
			return;

		Vector3 aim = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		aim.z = 0f;
		shakeY += Time.deltaTime * Mathf.Clamp(shakeFreq * aim.magnitude, 0f, 10f);
//		Debug.Log(aim.magnitude);
		transform.LookAt(Vector3.forward, Vector3.Cross(Vector3.forward, aim));
		transform.Rotate(Vector3.forward,
		                 90f + Mathf.Clamp((Mathf.PerlinNoise(shakeX, shakeY) - perlinMean) * shakeFactor * aim.sqrMagnitude, -30f, 30f), 
		                 Space.World);

		aim = transform.localEulerAngles;
		aim.z = Mathf.Clamp(aim.z, 120f, 240f);
		transform.localEulerAngles = aim;

		if (Input.GetButtonDown("Fire1") && energyMeter.Drain(5f)) {
			GameObject s = (GameObject) Instantiate(shot);
			s.transform.position = transform.position;
			s.transform.rotation = transform.rotation;
			s.transform.Rotate(Vector3.forward, 180f, Space.World);
		}
	}

	public bool activated {
		get {
			return _activated;
		}

		set {
			_activated = value;
		}
	}

}
