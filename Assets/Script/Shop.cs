using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<ShopItemInfo> items; // GPT씨는 ShopItemInfo를 한 단계 더 SO로 감쌀것을 추천해줬지만.. 지금은 필요없어보임.
    [SerializeField] private GameObject itemListPrefab;

    [SerializeField] Transform itemList_Parent;

    private void Start()
    {
        itemList_Parent = transform.Find("Scroll View/Viewport/Content");

        if (itemList_Parent == null)
            Debug.Log(" itemList_Parent is NULL ");
        else
            InitShop();
    }

    public void InitShop()
    {
        foreach (var item in items)
        {
            GameObject itemList = Instantiate(itemListPrefab);
            itemList.transform.SetParent(itemList_Parent, false);//UI는 false를 해줘야함
            itemList.GetComponent<ShopItemPanel>().InitItemList(this,item);
        }

    }

    public bool TryPurchase(int itemID)
    {
        int itemIdx = items.FindIndex(x => x.itemID == itemID);

        int itemLv = GameManager.Instance.dataManager.playerData.shopData.purchasedItem[itemID].count;
        int ownedReceiptPoint = GameManager.Instance.dataManager.playerData.currencyData.receiptPoint;

        if (items[itemIdx].price[itemLv] >ownedReceiptPoint || items[itemIdx].maxLv <= itemLv )
        {
            Debug.Log("돈이모자르거나 맥스레벨임");
            return false;

        }

        return true;

    }

    public void PurchaseItem(int itemID)
    {
        TryPurchase(itemID)
      /*  }

        GameManager.Instance.dataManager.playerData.currencyData.receiptPoint -= items[itemIdx].price[itemLv];
        GameManager.Instance.dataManager.playerData.shopData.AddItem(itemID);*/
    }
}
