using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	private GameObject panel;
	public Button AdventureButton;
	public Button WoodcutterButton;
	public Button MinerButton;

	public float adventureBasePrice = 10;
	public float woodcutterBasePrice = 50;
	public float minerBasePrice = 45;

	private float adventurePrice;
	private float woodcutterPrice;
	private float minerPrice;

	public PlayerInventory PI;

	void Start() {
		this.panel = this.transform.GetChild (0).gameObject;
		this.panel.SetActive (false);
	}

	void Update() {
		if (Input.GetKeyDown("s"))
		{
			if (!this.panel.activeSelf)
				this.panel.SetActive(true);
			else
				this.panel.SetActive(false);
		}

		this.adventurePrice = Mathf.Round (this.adventureBasePrice * Mathf.Pow (1.65f, PI.Adventurers));
		this.woodcutterPrice = Mathf.Round (this.woodcutterBasePrice * Mathf.Pow (1.65f, PI.Woodcutters));
		this.minerPrice = Mathf.Round (this.minerBasePrice * Mathf.Pow (1.65f, PI.Miners));

		this.AdventureButton.transform.Find ("Price").GetComponent<Text> ().text = this.adventurePrice.ToString();
		this.WoodcutterButton.transform.Find ("Price").GetComponent<Text> ().text = this.woodcutterPrice.ToString();
		this.MinerButton.transform.Find ("Price").GetComponent<Text> ().text = this.minerPrice.ToString();
	}

	public void buyAdventure(){
		if (PI.canBuy (this.adventurePrice)) {
			PI.Gold -= this.adventurePrice;
			PI.Adventurers += 1;
		}
	}

	public void buyWoodcutter(){
		if (PI.canBuy (this.woodcutterPrice)) {
			PI.Gold -= this.woodcutterPrice;
			PI.Woodcutters += 1;
		}
	}

	public void buyMiner(){
		if (PI.canBuy (this.minerPrice)) {
			PI.Gold -= this.minerPrice;
			PI.Miners += 1;
		}
	}
	/*
		float xps = 0.1f * Mathf.Pow (1.30f, Engine.control.adventures);
		cost = Mathf.Round (10 * Mathf.Pow (1.65f, Engine.control.adventures));
		cost = Mathf.Round (50 * Mathf.Pow (1.65f, Engine.control.woodcutters));
		cost = Mathf.Round (45 * Mathf.Pow (1.65f, Engine.control.miners));
	*/
}
