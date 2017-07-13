using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour {

	public GameObject HPCanvas;

	#region GUI Vars
	public Text hpText;
	public Image hpImage;

	public Text levelText;
	public Text damageText;
	public Text armorText;
	#endregion

	// Use this for initialization
	void Start () {
		hpText = HPCanvas.transform.GetChild (1).GetChild (0).GetChild (0).GetComponent<Text> ();
		hpImage = HPCanvas.transform.GetChild(1).GetChild (0).GetComponent<Image>();
		levelText = HPCanvas.transform.GetChild (4).GetComponent<Text> ();
		damageText = HPCanvas.transform.GetChild (2).GetComponent<Text> ();
		armorText = HPCanvas.transform.GetChild (3).GetComponent<Text> ();

		HPCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length > 0)
			HPCanvas.SetActive (true);
		else
			HPCanvas.SetActive (false);
	}
}
