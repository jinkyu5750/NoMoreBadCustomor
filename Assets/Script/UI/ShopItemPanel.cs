using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    //패널에 붙을 스크립트

    [SerializeField]
    private Shop shop;


    public ShopItemInfo item { get; private set; }

    public void InitItemList(Shop shop,ShopItemInfo item)
    {
        this.item = item;
        this.shop = shop;
        InitShopPanelUI();

    }
    public void InitShopPanelUI()
    {
        transform.GetChild(1).GetComponent<Image>().sprite = item.image;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.itemName;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.discription;
        transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = item.price[0].ToString();

    }

    public void ClickPurchaseButton()
    {
        shop.PurchaseItem(item.itemID);
    }

}
