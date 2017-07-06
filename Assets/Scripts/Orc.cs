using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : MonoBehaviour {

	public Animator animator;

	// Use this for initialization
	void Start () {
		Flip ();
	}
	
	// Update is called once per frame
	void Update () {
		animator.Play ("walk");
	}

	public void Flip()
	{
		var s = transform.localScale;
		s.x *= -1;
		transform.localScale = s;
		//lookRight = !lookRight;
	}
}
