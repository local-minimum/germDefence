using UnityEngine;
using System.Collections;

public class AntibodyFlow : MonoBehaviour {

	public float noHealthRate = 0f;
	public float fullHealthRate = 3f;
	public float noHealthSpeed = 0.2f;
	public float fullHealthSpeed = 1.2f;

	Level levelCoordinator;

	// Use this for initialization
	void Start () {
		levelCoordinator = GameObject.FindObjectOfType<Level>();
	}
	
	// Update is called once per frame
	void Update () {
		particleSystem.emissionRate = Mathf.Lerp(noHealthRate, fullHealthRate,
		                                         levelCoordinator.immunity.curPercent);

		particleSystem.startSpeed = Mathf.Lerp(noHealthSpeed, fullHealthSpeed,
		                                       levelCoordinator.immunity.curPercent);
	}
}
