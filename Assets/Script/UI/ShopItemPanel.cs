using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    //패널에 붙을 스크립트

    [SerializeField]
    private Shop shop;

    int itemLevel = 0;
    int itemValue = 0;
    public ShopItemInfo item { get; private set; }

    private AnimatorOverrideController controller;
    private Animator ani;
    private void Awake()
    {

        ani = transform.GetChild(1).GetComponent<Animator>();
        controller = new AnimatorOverrideController(ani.runtimeAnimatorController);
        ani.runtimeAnimatorController = controller;
    }
    public void InitItemList(Shop shop, ShopItemInfo item)
    {
        this.item = item;
        this.shop = shop;
        InitShopPanelUI();

    }
    public void InitShopPanelUI()
    {


        itemLevel = GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(item.itemID);
        if (itemLevel < item.maxLv)
            itemValue = GameManager.Instance.dataManager.playerData.shopData.GetValueByLevel(item.itemID);


        controller["ShopPanelAnimationClip"] = item.clip;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.itemName;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format(item.discription, itemValue);
        transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = itemLevel == item.maxLv ? "-" : item.price[itemLevel].ToString();
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = itemLevel == item.maxLv ? "Lv.MAX" : $"Lv.{itemLevel} > Lv.{itemLevel + 1}";


    }

    public void ClickPurchaseButton()
    {
        if (shop.PurchaseItem(item.itemID))
            InitShopPanelUI();
    }

}
