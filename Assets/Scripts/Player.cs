using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.FantasyHeroes.Scripts;

public class Player : MonoBehaviour {

	private float level = 1;
	private float statpoints = 0;
	private float experience = 0;
	private float maxhealth = 100;
	private float health = 100;
	private float damage = 5;
	private float armor = 5;
//	private float hpr = 1;

	public GameObject HPMANACanvas;
	public Character Character;
	public Texture2D Texture2H;
	public Texture2D Texture1H;

	private float levelupexp;
	private GameObject target;

	public Vars Config;

	#region GUI Vars
	Text hpText;
	Image hpImage;

	Text levelText;
	Text damageText;
	Text armorText;

	Image expImage;
	#endregion

	void Start () {
		levelupexp = this.Level * (120 * 1.65f);

		Texture1H = Character.Weapon;

		if (HPMANACanvas != null)
		{
			hpText = HPMANACanvas.transform.GetChild (1).GetChild (0).GetChild (0).GetComponent<Text> ();
			hpImage = HPMANACanvas.transform.GetChild(1).GetChild (0).GetComponent<Image>();
			levelText = HPMANACanvas.transform.GetChild (4).GetComponent<Text> ();
			damageText = HPMANACanvas.transform.GetChild (2).GetComponent<Text> ();
			armorText = HPMANACanvas.transform.GetChild (3).GetComponent<Text> ();
			expImage = HPMANACanvas.transform.GetChild(5).GetChild (0).GetComponent<Image>();

		    UpdateHPBar();
			UpdateExpBar();
			updateStatTexts ();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Enemy") {
			Config.inFight = true;
			target = other.gameObject;
			Character.WeaponType = WeaponType.Melee1H;
			Character.ReplaceSprite(Character.WeaponRenderer, Texture1H);
			StartCoroutine (attack());
		}

		if (other.transform.tag == "Tree") {
			Config.cuttingWood = true;
			target = other.gameObject;
			Character.WeaponType = WeaponType.Melee2H;
			Character.ReplaceSprite(Character.WeaponRenderer, Texture2H);
			StartCoroutine (cut());
		}

		if (other.transform.tag == "Vein") {
			Config.isMining = true;
			target = other.gameObject;
			Character.WeaponType = WeaponType.Melee2H;
			Character.ReplaceSprite(Character.WeaponRenderer, Texture2H);
			StartCoroutine (mine());
		}
	}

	IEnumerator attack(){
		float time = Character.Animator.runtimeAnimatorController.animationClips.First(x => x.name == "Attack1H").length;
		while (true) {
			if (GameObject.FindGameObjectsWithTag ("Enemy").Length == 0 || Config.Dead) {
				Config.inFight = false;
				yield break;
			}
			Character.Animator.Play ("Attack1H");
			Invoke ("doDamage", time / 2);
			yield return new WaitForSeconds (time + 1);
		}
	}

	IEnumerator cut(){
		float time = Character.Animator.runtimeAnimatorController.animationClips.First(x => x.name == "Attack2H").length;
		while (true) {
			if (GameObject.FindGameObjectsWithTag ("Tree").Length == 0 || Config.Dead) {
				Config.cuttingWood = false;
				yield break;
			}
			Character.Animator.Play ("Attack2H");
			Invoke ("doDamage", time / 2);
			yield return new WaitForSeconds (time + 1);
		}
	}

	IEnumerator mine(){
		float time = Character.Animator.runtimeAnimatorController.animationClips.First(x => x.name == "Attack2H").length;
		while (true) {
			if (GameObject.FindGameObjectsWithTag ("Vein").Length == 0 || Config.Dead) {
				Config.isMining = false;
				yield break;
			}
			Character.Animator.Play ("Attack2H");
			Invoke ("doDamage", time / 2);
			yield return new WaitForSeconds (time + 1);
		}
	}

	private void doDamage(){
		if(GameObject.FindGameObjectsWithTag ("Enemy").Length > 0 && !Config.Dead)
			target.GetComponent<Orc>().Health -= damage;

		if(GameObject.FindGameObjectsWithTag ("Tree").Length > 0 && !Config.Dead)
			target.GetComponent<Tree>().Health -= damage;

		if(GameObject.FindGameObjectsWithTag ("Vein").Length > 0 && !Config.Dead)
			target.GetComponent<Ore>().Health -= damage;
	}

	private void levelUp() {
		this.Level += 1;
		this.statpoints += 1;
		levelupexp = this.level * (120 * 1.65f);
		this.Experience = 0;
	}

	private void UpdateHPBar()
	{
		hpText.text = (Mathf.Round(this.health) + "/" + Mathf.Round(this.maxhealth));
		hpImage.fillAmount = this.health / this.maxhealth;
	}

	private void UpdateExpBar()
	{
		expImage.fillAmount = this.experience / this.levelupexp;
	}

	private void updateStatTexts()
	{
		damageText.text = "Damage: " + this.damage;
		armorText.text = "Armor: " + this.armor;
		levelText.text = this.level.ToString();
	}

	#region Setters/Getters
	public float Level{
		get{ return this.level; }
		set{ 
			this.level = value; 
			updateStatTexts ();
		}
	}
	public float Experience{
		get{ return this.experience; }
		set{ 
			if (value >= levelupexp)
				levelUp ();
			else
				this.experience = value;

			UpdateExpBar();
		}
	}
	public float maxHealth{
		get{ return this.maxhealth; }
		set{ 
			this.maxhealth = value; 
			UpdateHPBar();
		}
	}
	public float Health{
		get{ return this.health; }
		set{ 
			value = (value < this.health) ? (value + this.armor < this.health) ? value + this.armor : this.health : value;
			this.health = (value > this.maxhealth) ? this.maxhealth : value; 
			if (value <= 0) {
				Config.Dead = true;
				Character.Animator.Play ("Die");
			}
			UpdateHPBar();
		}
	}
	public float Armor{
		get{ return this.armor; }
		set{ 
			this.armor = value; 
			updateStatTexts ();
		}
	}
	public float Damage{
		get{ return this.damage; }
		set{ 
			this.damage = value;
			updateStatTexts ();
		}
	}
	#endregion
}