using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    //패널에 붙을 스크립트

    public ShopItemInfo item { get; private set; }


    public void InitItemList(ShopItemInfo item)
    {
        this.item = item;
        InitShopPanelUI();

    }
    public void InitShopPanelUI()
    {
        transform.GetChild(1).GetComponent<Image>().sprite = item.image;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.itemName;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.discription;
        transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = item.price[0].ToString();

    }

}
