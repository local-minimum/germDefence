using UnityEngine;
using System.Collections;

public class Amoeba : Enemy {

	public Transform[] launchPoints;

	public float launchVelocityMin;
	public float launchVelocityMax;

	[Range(0, 1000)]
	public float force = 200f;

	public float forceLateralBounds = 0.8f;
	public float forceTorque = 45f;

	new void Start() {
		base.Start();
		Launch();
	}

	void Launch() {
		Transform t = launchPoints[Random.Range(0, launchPoints.Length)];
		myRB.transform.position = t.position;
		myRB.velocity = t.up * Random.Range(launchVelocityMin, launchVelocityMax);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Shot") {
			Dna dna = other.gameObject.GetComponent<Dna>();
			int dmg = dna.Hit();
			if (dmg == 0)
				return;
			lives -= dmg;
		
			myRB.AddForce(
				(Vector2.up + 
			 	Vector2.right * Random.Range(-forceLateralBounds, forceLateralBounds)).normalized * force, 
				ForceMode2D.Impulse);

			myRB.AddTorque(Random.Range(-forceTorque, forceTorque), ForceMode2D.Impulse);
			/*More reasonable but not working physics
			RaycastHit2D hit = Physics2D.Raycast(
				other.transform.position, 
			    transform.position - other.transform.position, 10f,
			    LayerMask.NameToLayer("Enemies"));

			if (hit) {
				myRB.AddForceAtPosition(hit.normal * force, hit.point, ForceMode2D.Impulse);
			} else {
				Debug.Log("No hit");
			}*/
		}
	}
}
