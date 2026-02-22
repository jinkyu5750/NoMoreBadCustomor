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
            itemList.GetComponent<ShopItemPanel>().InitItemList(this, item);
        }

    }

    public bool TryPurchase(int itemIdx, int itemLv, int ownedReceiptPoint)
    {

        /*     if (itemLv >= items[itemIdx].maxLv || items[itemIdx].price[itemLv] > ownedReceiptPoint) //이거.. 맥스일떄 두번째 조건은 OutOfIndex이지만 걍 일케 쓰자 깔끔함
             {
                 Debug.Log("맥스레벨이거나 포인트가 부족함");
                 return false;
             }*/

        if (itemLv >= items[itemIdx].maxLv)
        {
            Debug.Log("맥스레벨임");
            return false;
        }

        if (items[itemIdx].price[itemLv] > ownedReceiptPoint) //그냥 분리함 ㅎㅎ;
        {
            Debug.Log("포인트가 부족함");
            return false;
        }

        return true;

    }

    public bool PurchaseItem(int itemID)
    {
        int itemIdx = items.FindIndex(x => x.itemID == itemID);
        int itemLv = GameManager.Instance.dataManager.playerData.shopData.GetItemLevel(itemID);
        int ownedReceiptPoint = GameManager.Instance.dataManager.playerData.currencyData.receiptPoint;

        if (!TryPurchase(itemIdx, itemLv, ownedReceiptPoint))
            return false;


        GameManager.Instance.dataManager.DecreaseReceiptPoint(items[itemIdx].price[itemLv]);
        GameManager.Instance.dataManager.playerData.shopData.AddItem(itemID);
        return true;
    }
}
