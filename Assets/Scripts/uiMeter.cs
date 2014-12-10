using UnityEngine;
using System.Collections;

public class uiMeter : MonoBehaviour {

	bool _elevatedRegen = false;

	public float baseRegen = 2f;
	public float elevatedRegenFactor = 2f;

	public float maxValue = 100f;


	private float imgCurWidth = 0f;
	private bool _hasHitZero = false;

	private UnityEngine.UI.Image im = null;

	public float curValue {
		get {
			return maxValue * imgCurWidth;
		}

		set {
			imgCurWidth = Mathf.Clamp(value, 0f, maxValue) / maxValue;
			if (imgCurWidth == 0f)
				_hasHitZero = true;
		}
	}

	public bool elevatedRegen {
		set {
			_elevatedRegen = value;
		}
	}

	public bool hasHitZero {
		get {
			return _hasHitZero;
		}
	}

	// Use this for initialization
	void Start () {
		im = gameObject.GetComponent<UnityEngine.UI.Image>();
		imgCurWidth = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		Regen();
	}

	void Regen() {
		if (_elevatedRegen) {
			imgCurWidth = Mathf.Clamp01(imgCurWidth + baseRegen * elevatedRegenFactor / maxValue * Time.deltaTime);
		} else {
			imgCurWidth = Mathf.Clamp01(imgCurWidth + baseRegen / maxValue * Time.deltaTime);
		}
	}

	public void Reset() {
		imgCurWidth = 1f;
		_hasHitZero = false;
	}

	public bool Drain(float amount) {
		float v = curValue;
		bool ret = v >= amount;
		curValue = v - amount;
		return ret;
	}

	[ExecuteInEditMode]
	void OnGUI() {
		im.fillAmount = Mathf.Clamp(imgCurWidth, 0.001f, 1f);
	}
}
