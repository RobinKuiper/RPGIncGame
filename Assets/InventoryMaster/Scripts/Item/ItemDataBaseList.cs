using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDataBaseList : ScriptableObject
{             //The scriptableObject where the Item getting stored which you create(ItemDatabase)

    [SerializeField]
    public List<Item> itemList = new List<Item>();              //List of it

	private System.Random r = new System.Random();

    public Item getItemByID(int id)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemID == id)
                return itemList[i].getCopy();
        }
        return null;
    }

    public Item getItemByName(string name)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemName.ToLower().Equals(name.ToLower()))
                return itemList[i].getCopy();
        }
        return null;
    }

	public List<Item> getItemsByType(ItemType type)
	{
		List<Item> items = new List<Item> ();
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList [i].itemType == type)
				items.Add (itemList [i].getCopy ());
		}
		return items;
	}

	public Item getWeightedItemByType(ItemType type)
	{
		List<Item> itemList = getItemsByType (type).OrderBy (o => o.rarity).ToList ();
		int totalWeight = itemList.Sum (i => i.rarity);

		int randomNumber = r.Next (0, totalWeight);

		foreach (Item item in itemList) {
			if (randomNumber < item.rarity) {
				return item;
			}
			randomNumber -= item.rarity;
		}
		return null;
	}
}
