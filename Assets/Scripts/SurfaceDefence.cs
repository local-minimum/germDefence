﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SurfaceDefence : MonoBehaviour {

	static HashSet<SurfaceDefence> surfaceDefenders = new HashSet<SurfaceDefence>();

	public LayerMask indicatorRayLayerHits;
	public float speed = 1f;
	public float eatingOneLifeTime = 1f;
	private Rigidbody2D myRB;
	public float rayVerticalOffset = 0.2f;
	public float rayDuration = 2f;

	private float acceleration = 0f;
	private HashSet<Enemy> fighting = new HashSet<Enemy>();

	static SurfaceDefence leftMost {
		get {
			return surfaceDefenders.OrderBy(sd => sd.transform.position.x).First();
		}
	}

	static SurfaceDefence rightMost {
		get {
			return surfaceDefenders.OrderByDescending(sd => sd.transform.position.x).First();
		}
	}

	void Start() {
		surfaceDefenders.Add(this);
		myRB = gameObject.GetComponentInParent<Rigidbody2D>();
	}

	void Update() {
		acceleration = (float) 2 * (Input.mousePosition.x - Screen.width / 2) / Screen.width;

		if (rightMost == this)
			DrawEnemyDetectionFeedback(
				Physics2D.Raycast(
					transform.position + Vector3.up * rayVerticalOffset, 
					Vector2.right, rayDuration, indicatorRayLayerHits));

		if (leftMost == this)
			DrawEnemyDetectionFeedback(
				Physics2D.Raycast(
				transform.position + Vector3.up * rayVerticalOffset, 
				Vector2.right * -1, rayDuration, indicatorRayLayerHits));

//		Debug.DrawRay(transform.position + Vector3.up * rayVerticalOffset, Vector3.right, Color.red, 10f);
	}

	void FixedUpdate() {
		myRB.AddForce(Vector2.right * speed * acceleration  + Vector2.up);

	}

	void DrawEnemyDetectionFeedback(RaycastHit2D hit) {
		if (!hit)
			return;

		Debug.DrawLine(transform.position, hit.point + Vector2.up * -rayVerticalOffset);
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
