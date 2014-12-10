using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SurfaceDefence : MonoBehaviour {

	public float speed = 1f;
	public float eatingOneLifeTime = 1f;
	private Rigidbody2D myRB;

	private HashSet<Enemy> fighting = new HashSet<Enemy>();
	/*
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
	*/

	void Start() {
		myRB = gameObject.GetComponentInParent<Rigidbody2D>();
	}

	void Update() {
		float acceleration = (float) 2 * (Input.mousePosition.x - Screen.width / 2) / Screen.width;
		myRB.AddForce(Vector2.right * speed * acceleration * Time.deltaTime + Vector2.up * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Enemy e = other.GetComponent<Enemy>();
//		Debug.Log(other.tag);
		if (e) {
			fighting.Add(e);
			StartCoroutine(eatingEnemies(e));
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		Enemy e = other.GetComponent<Enemy>();
		if (e)
			fighting.Remove(e);
	}

	IEnumerator<WaitForSeconds> eatingEnemies(Enemy e) {
		while (e.lives > 0) {
			yield return new WaitForSeconds(eatingOneLifeTime);
			if (!fighting.Contains(e))
				break;
			e.lives--;
		}
	}

}
