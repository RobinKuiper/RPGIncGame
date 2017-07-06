using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.FantasyHeroes.Scripts;

public class Journey : MonoBehaviour {
	public Character Character;
	public Player Player;
	public PlayerInventory PI;
	public FreeParallax parallax;
	public FreeParallax cparallax;

	public Text journeyTimerText;
	public Text activityTimerText;
	public Text activityText;

	public int journeyTime = 120;
	private int journeyTimeLeft;

	public int activityTime = 6;
	private int activityTimeLeft;

	private bool active = false;
	private bool inActivity = false;

	private Activity activity;
	private List<Activity> activitiesList;

	private System.Random sR = new System.Random();

	[SerializeField]
	private ItemDataBaseList itemDatabase;

	void Start(){
		parallax.Speed = 0.0f;

		if (itemDatabase == null)
			itemDatabase = (ItemDataBaseList)Resources.Load ("ItemDatabase");

		journeyTimerText.enabled = false;
		activityTimerText.enabled = false;
		activityText.enabled = false;

		activitiesList = new List<Activity> ();
		Activity act1 = new Activity ();
		act1.activityName = "SLAY";
		act1.activityText = "Slaying some monsters";
		act1.activityTime = 6;
		act1.activityAnimation = "Attack1H";

		activitiesList.Add (act1);

		Activity act2 = new Activity ();
		act2.activityName = "WOOD";
		act2.activityText = "Cutting some wood";
		act2.activityTime = 6;
		act2.activityAnimation = "Stand";

		activitiesList.Add (act2);

		Activity act3 = new Activity ();
		act3.activityName = "MINE";
		act3.activityText = "Mining some ores";
		act3.activityTime = 6;
		act3.activityAnimation = "Stand";

		activitiesList.Add (act3);
	}

	void Update(){
		journeyTimerText.text = journeyTimeLeft.ToString ();
		activityTimerText.text = activityTimeLeft.ToString ();

		if (active) {
			journeyTimerText.enabled = true;
			parallax.Speed = -10.0f;
			cparallax.Speed = -4.0f;
			if (inActivity) {
				parallax.Speed = 0.0f;
				cparallax.Speed = -2.0f;
				activityTimerText.enabled = true;
				activityText.enabled = true;
				activityText.text = activity.activityText;
				Character.Animator.Play (activity.activityAnimation);
			} else {
				activityTimerText.enabled = false;
				activityText.enabled = false;
				Character.Animator.Play ("Walk");
			}
		} else if (!active) {
			parallax.Speed = 0.0f;
			cparallax.Speed = -2.0f;
			journeyTimerText.enabled = false;
			Character.Animator.Play ("Stand");
		}
	}		

	public void click_startJourney(){
		if (Player.Health > 0 && !active) {
			journeyTimeLeft = journeyTime;
			active = true;
			StartCoroutine(journeyTick());

			startActivity ();
		}
	}

	private void stopJourney(){
		active = false;
		StopCoroutine (journeyTick ());
	}

	private void startActivity(int modifier = 0){
		activity = activitiesList [Random.Range (0, activitiesList.Count)];
		int activityStart = UnityEngine.Random.Range (modifier + 2, modifier + 16);
		if (journeyTimeLeft >= activityStart) {
			Invoke ("doActivity", activityStart);
		}
			
		/*Rigidbody2D tree = (Rigidbody2D)Instantiate (Resources.Load ("treePrefab"));
		tree.transform.position = new Vector2 (9.0f, -4.1f);
		tree.velocity = transform.forward * -2;*/
	}

	private void doActivity(){
		activityTimeLeft = activity.activityTime;
		inActivity = true;
		StartCoroutine(activityTick());
		startActivity (8);
	}

	IEnumerator journeyTick(){
		while (active) {
			if (!inActivity)
			if (journeyTimeLeft > 0)
				journeyTimeLeft -= 1;
			else
				stopJourney ();
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator activityTick(){
		while (inActivity) {
			switch (activity.activityName) {
			case "SLAY":
				int modifier = System.Convert.ToInt32 (Mathf.Pow (1.35f, Player.Level));
				Player.Health -= sR.Next (2 * modifier, 15 * modifier) - Player.Armor;
				Player.Experience += sR.Next (2 * modifier, 11 * modifier) * Player.Damage / 10;
				break;

			case "WOOD":
				PI.addItemToInventory(33, sR.Next (0, 4));
				break;

			case "MINE":
				Item item = itemDatabase.getWeightedItemByType (ItemType.Ores);
				int value = 0;
				if (item.rarity <= 1000 && item.rarity >= 800)
					value = sR.Next (5, 10);
				else if (item.rarity < 800 && item.rarity >= 600)
					value = sR.Next (4, 8);
				else if (item.rarity < 600 && item.rarity >= 400)
					value = sR.Next (3, 6);
				else if (item.rarity < 400 && item.rarity >= 200)
					value = sR.Next (2, 4);
				else if (item.rarity < 200 && item.rarity >= 10)
					value = sR.Next (1, 2);
				else if (item.rarity < 10 && item.rarity >= 6)
					value = 1;
				else if (item.rarity < 6)
					value = sR.Next (0, 1);
				PI.addItemToInventory (item.itemID, value);
				break;
			}
				
			if (activityTimeLeft > 0)
				activityTimeLeft -= 1;
			else {
				inActivity = false;
				StopCoroutine (activityTick ());
			}

			yield return new WaitForSeconds (1);
		}
	}
}

class Activity{
	public string activityName;
	public string activityText;
	public int activityTime;
	public string activityAnimation;
}