using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Engine : MonoBehaviour {
	public PlayerInventory PI;
	public Player Player;

	private System.Random r = new System.Random();

	[SerializeField]
	private ItemDataBaseList itemDatabase;
	List<Item> oreList;

	void Start(){
		if (itemDatabase == null) {
			itemDatabase = (ItemDataBaseList)Resources.Load ("ItemDatabase");
			oreList = itemDatabase.getItemsByType (ItemType.Ores);
		}

		StartCoroutine (AutoTick ());
	}

	IEnumerator AutoTick(){
		float mine_i=0; float wood_i = 0;
		while (true) {
			if (PI.Adventurers > 0)
				Player.Experience += (0.1f * Mathf.Pow (1.30f, PI.Adventurers)) / 10;

			mine_i++;
			if(PI.Miners > 0){
				if(mine_i >= (500/Mathf.Pow (1.25f, PI.Miners))){
					PI.addItemToInventory (get_weighted_item (oreList).itemID);
					mine_i=0;
				}
			}

			wood_i++;
			if(PI.Woodcutters > 0){
				if(wood_i >= (250/Mathf.Pow (1.25f, PI.Woodcutters))){
					PI.addItemToInventory (33);
					wood_i=0;
				}
			}
			yield return new WaitForSeconds(0.10f);
		}
	}

	public void AutoExpPerSec(){
		//Hero.stats["changeStat ("gold", gps / 10, true);
	//	Player.Experience += xps / 10;
	}


	public Item get_weighted_item(List<Item> list){
		list = oreList.OrderBy (o => o.rarity).ToList ();
		int totalWeight = oreList.Sum (i => i.rarity);

		int randomNumber = r.Next (0, totalWeight);

		foreach (Item item in list) {
			if (randomNumber < item.rarity) {
				return item;
			}
			randomNumber -= item.rarity;
		}
		return null;
	}

	/*public Item get_weighted_item(List<Item> list){
		//list = oreList.OrderByDescending (o => o.rarity).ToList ();
		int totalWeight = oreList.Sum (i => i.rarity);

		//int randomNumber = r.Next (0, totalWeight);
		float itemWeightIndex = r.Next (0, totalWeight);//(float)r.NextDouble() * totalWeight;
		float currentWeightIndex = 0;

		foreach (Item item in list) {
			currentWeightIndex -= item.rarity;
			if (currentWeightIndex >= itemWeightIndex) {
				return item;
			}
		}
		return null;
	}*/
}