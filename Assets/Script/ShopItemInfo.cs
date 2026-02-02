using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemInfo",fileName ="ShopItemInfo")]
public class ShopItemInfo : ScriptableObject
{

    public Sprite image;
    public string itemName;
    public string discription;
    public int maxLv;
    public int[] price;
}
