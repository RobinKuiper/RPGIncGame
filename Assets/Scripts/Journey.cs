using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.FantasyHeroes.Scripts;

public class Journey : MonoBehaviour {
	public Player Player;
	public Vars Config;

	public FreeParallax parallax;
	public FreeParallax cparallax;

	public Text journeyTimerText;

	public int journeyTime = 120;
	private int journeyTimeLeft;

	private int enemies;

	void Start(){
		journeyTimerText.enabled = false;
	}

	void Update(){
		enemies = GameObject.FindGameObjectsWithTag ("Enemy").Length;

		if (Config.onJourney) {
			journeyTimerText.enabled = true;
			journeyTimerText.text = journeyTimeLeft.ToString ();
		} else {
			journeyTimerText.enabled = false;
		}

		if (Config.onJourney && enemies == 0 && !Config.cuttingWood && !Config.isMining) {
			parallax.Speed = -10.0f;
			cparallax.Speed = -4.0f;
			Player.Character.Animator.Play ("Walk");
		} else if (Config.onJourney && (enemies >= 1 || Config.cuttingWood || Config.isMining)) {
			parallax.Speed = 0.0f;
			cparallax.Speed = -2.0f;
			if(!Config.inFight && !Config.cuttingWood && !Config.isMining)
				Player.Character.Animator.Play ("Alert1H");
		}
	}

	public void click_startJourney(){
		if (Player.Health > 0 && Config.Idle) {
			journeyTimeLeft = journeyTime;
			Config.onJourney = true;
			StartCoroutine(journeyTick());
			startActivity ();
		}
	}

	IEnumerator journeyTick(){
		while (Config.onJourney) {
			if (journeyTimeLeft > 0)
				journeyTimeLeft -= 1;
			else {
				Config.onJourney = false;
				yield break;
			}
			yield return new WaitForSeconds (1);
		}
	}

	private void startActivity(int modifier = 0){
		int activityStart = UnityEngine.Random.Range (modifier + 2, modifier + 16);
		if (journeyTimeLeft >= activityStart) {
			Invoke ("doActivity", activityStart);
		}
	}

	private void doActivity(){
		int activity = UnityEngine.Random.Range (0, 3);
		if (activity == 0) {
			GameObject Orc = (GameObject)Instantiate (Resources.Load ("Prefabs/orc"));
		} else if (activity == 1) {
			GameObject Tree = (GameObject)Instantiate (Resources.Load ("Prefabs/treePrefab"));
		} else if (activity == 2) {
			GameObject Ore = (GameObject)Instantiate (Resources.Load ("Prefabs/orePrefab"));
		}
		startActivity (15);
	}
}
