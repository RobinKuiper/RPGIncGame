using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Orc : MonoBehaviour {

	private float level = 1;
	private float maxhealth;
	private float health;
	private float damage;
	private float armor;
	private float experience;

	private Player Player;
	public Animator animator;
	public EnemyHPBar HPBar;
	private Vars Config;

	public float speed = 1;

	private bool move = true;

	Rigidbody2D body;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("Player").GetComponent<Player> ();
		HPBar = GameObject.Find ("Engine").GetComponent<EnemyHPBar> ();
		Config = GameObject.Find ("Engine").GetComponent<Vars> ();

		body = GetComponent<Rigidbody2D> ();
		Flip ();

		// Set enemy stats accordingly
		level = (level > 0) ? level : 1;
		maxhealth = Mathf.Pow(8 * level, 1.35f);
		health = maxhealth;
		damage = Mathf.Pow(4 * level, 1.35f);
		armor = Mathf.Pow(1 * level, 1.35f);
		experience = Mathf.Pow (10 * level, 1.35f);

		// Set start position outside of screen.
		Vector2 v = Camera.main.ViewportToWorldPoint (new Vector2 (1.2f, 0.1f));
		transform.position = v;

		UpdateHPBar();
		updateStatTexts ();
	}

	// Update is called once per frame
	void Update () {
		if (Config.Dead)
			Destroy (gameObject);

		if (move) {
			body.velocity = new Vector2 (-3, 0);
			animator.Play ("walk");
		} else {
			body.velocity = new Vector2 (0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.name == "Player") {
			move = false;
			StartCoroutine ("attack");
		}
	}

	IEnumerator attack(){
		float time = animator.runtimeAnimatorController.animationClips.First(x => x.name == "attack").length;
		while (true) {
			animator.Play ("attack");
			Invoke ("doDamage", time / 2);
			yield return new WaitForSeconds (time);
		}
	}

	private void doDamage(){
		Player.Health -= damage;
	}

	public void Flip()
	{
		var s = transform.localScale;
		s.x *= -1;
		transform.localScale = s;
	}

	private void UpdateHPBar()
	{
		HPBar.hpText.text = (Mathf.Round(this.health) + "/" + Mathf.Round(this.maxhealth));
		HPBar.hpImage.fillAmount = this.health / this.maxhealth;
	}

	private void updateStatTexts()
	{
		HPBar.damageText.text = "Damage: " + Mathf.Round(this.damage);
		HPBar.armorText.text = "Armor: " + Mathf.Round(this.armor);
		HPBar.levelText.text = this.level.ToString();
	}

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
			value = (value < this.health) ? (value + this.armor < this.health) ? value + this.armor : this.health : value;
			this.health = (value > this.maxhealth) ? this.maxhealth : value; 
			UpdateHPBar ();

			if (this.health <= 0) {
				Destroy (gameObject);
				Player.Experience += experience;
			}
		}
	}
	public float Armor{
		get{ return this.armor; }
		set{ 
			this.armor = value; 
		}
	}
	public float Damage{
		get{ return this.damage; }
		set{ 
			this.damage = value;
		}
	}
	#endregion
}
