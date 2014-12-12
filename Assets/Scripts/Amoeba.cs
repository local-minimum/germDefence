using UnityEngine;
using System.Collections;

public class Amoeba : Enemy {

	public Transform[] launchPoints;

	public float launchVelocityMin;
	public float launchVelocityMax;

	new void Start() {
		base.Start();
		Launch();
	}

	void Launch() {
		Transform t = launchPoints[Random.Range(0, launchPoints.Length)];
		myRB.transform.position = t.position;
		myRB.velocity = t.up * Random.Range(launchVelocityMin, launchVelocityMax);
	}
}
