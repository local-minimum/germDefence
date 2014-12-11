using UnityEngine;
using System.Collections;

public class WavyLineRenderer : MonoBehaviour {

	LineRenderer myRenderer;

	public float waveTravellingSpeed = 1f;

	public Vector3 waveAxis = Vector3.up;

	public float amplitude = 1f;
	public bool scalingAmplitude = false;

	public float frequency = 1f;
	public bool scalingFrequency = false;

	[SerializeField]
	private int _length = 30;

	public Vector3 startWave = Vector3.zero;

	[HideInInspector]
	public Vector3 endWave = Vector3.zero;

	private float lastIndexAsFloat;

	public int length {
		get {
			return _length;
		}

		set {
			myRenderer.SetVertexCount(value);
			_length = value;
			lastIndexAsFloat = _length - 1f;
		}
	}

	// Use this for initialization
	void Start () {
		myRenderer = gameObject.GetComponent<LineRenderer>();
		length = _length;
	}
	
	// Update is called once per frame
	void Update () {
		if (!myRenderer.enabled)
			return;

		int i = 0;
		float d = Vector3.Distance(startWave, endWave);
		float a = scalingAmplitude ? amplitude / d : amplitude;
		while (i < _length) {
			Vector3 pos = Vector3.Lerp(startWave, endWave, i / (lastIndexAsFloat));
			if (scalingFrequency)
				pos += waveAxis * Mathf.Sin(frequency * i / (_length * d) + Time.time * waveTravellingSpeed) * a; 
			else
				pos += waveAxis * Mathf.Sin(frequency * i / _length + Time.time * waveTravellingSpeed) * a; 		 
			myRenderer.SetPosition(i, pos);
			i++;
		}
	}
}