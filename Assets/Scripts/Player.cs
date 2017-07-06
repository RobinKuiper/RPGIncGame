using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.FantasyHeroes.Scripts;

public class Player : MonoBehaviour {

	private float level = 1;
	private float statpoints = 0;
	private float experience = 0;
	private float maxhealth = 100;
	private float health = 60;
	private float damage = 5;
	private float armor = 5;
//	private float hpr = 1;

	public GameObject HPMANACanvas;
	public Character Character;

	private float levelupexp;

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

	void Update(){
		
	}

	private void levelUp() {
		this.Level += 1;
		this.statpoints += 1;
		levelupexp = this.level * (120 * 1.65f);
		this.Experience = 0;
	}

	private void UpdateHPBar()
	{
		hpText.text = (this.health + "/" + this.maxhealth);
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
			this.health = (value > this.maxhealth) ? this.maxhealth : value; 
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