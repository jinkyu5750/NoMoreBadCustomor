using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopItemInfo", fileName ="ShopItemInfo")]
public class ShopItemInfo : ScriptableObject
{
    public int itemID;

    public AnimationClip clip;
    public string itemName;
    public string discription;
    public int maxLv;
    public int[] price;
 
}
