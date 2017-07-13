using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tree : MonoBehaviour {

	private float level = 1;
	private float maxhealth;
	private float health;

	private Player Player;
	private Vars Config;

	private bool move = true;

	Rigidbody2D body;

	private System.Random sR = new System.Random();
	public PlayerInventory PI;

	// Use this for initialization
	void Start () {
		Config = GameObject.Find ("Engine").GetComponent<Vars> ();
		PI = GameObject.Find ("Player").GetComponent<PlayerInventory> ();

		body = GetComponent<Rigidbody2D> ();

		// Set enemy stats accordingly
		level = (level > 0) ? level : 1;
		maxhealth = Mathf.Pow(10 * level, 1.35f);
		health = maxhealth;

		// Set start position outside of screen.
		Vector2 v = Camera.main.ViewportToWorldPoint (new Vector2 (1.2f, 0.12f));
		transform.position = v;
	}

	// Update is called once per frame
	void Update () {
		if (move) {
			body.velocity = new Vector2 (-5, 0);
		} else {
			body.velocity = new Vector2 (0, 0);
		}
	}

	/*void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.name == "Player") {
			Config.cuttingWood = true;
			move = false;
		}
	}*/

	#region Setters/Getters
	public float Level{
		get{ return this.level; }
		set{ this.level = value; }
	}

	public float maxHealth{
		get{ return this.maxhealth; }
		set{ this.maxhealth = value; }
	}
	public float Health{
		get{ return this.health; }
		set{ 
			this.health = (value > this.maxhealth) ? this.maxhealth : value; 
			if (this.health <= 0) {
				Destroy (gameObject);
				PI.addItemToInventory (33, System.Convert.ToInt32 (Mathf.Round (sR.Next (System.Convert.ToInt32 (Mathf.Pow (10 * level, 1.35f)), System.Convert.ToInt32 (Mathf.Pow (20 * level, 1.35f))))));
			}
		}
	}
	#endregion
}
