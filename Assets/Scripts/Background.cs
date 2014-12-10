using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	MovieTexture tex = null;
	// Use this for initialization
	void Start () {
		tex = ((MovieTexture) renderer.material.mainTexture);
		tex.loop = true;
		tex.Play();
	}
}
