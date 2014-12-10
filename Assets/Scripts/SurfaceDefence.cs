using UnityEngine;
using System.Collections;

public class SurfaceDefence : MonoBehaviour {

	public float speed = 1f;

	private Transform _target;


	public float targetDistance {
		get {
			return (_target.position - transform.position).x;
		}
	}
	                         
	// Use this for initialization
	void Start () {
	
	}
	

	// Update is called once per frame
	void Update () {
		if (_target) {
			if (targetDistance > 0.01f)
				rigidbody2D.velocity = new Vector2(Mathf.Clamp(targetDistance, -1f, 1f), 1);

		}
	}

	public void Attack(Transform other) {
		_target = other;
	}
}
