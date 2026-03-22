using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    //최고스코어,시간 ??를 기록해야할까?
    public CurrencyData currencyData = new CurrencyData();
    public ShopData shopData = new ShopData();
    public AchivementData achivementData = new AchivementData();
}
[System.Serializable]
public class CurrencyData
{
    public int receiptPoint { get; private set; } = 100;

    public void AddReceiptPoint(int point)
    {
        receiptPoint += point;
     //   UIManager.Instance.SetReceiptPointText(receiptPoint.ToString());
    }
}
public class ShopData
{
    public List<OwnedItem> purchasedItem = new(); // 리스트를 쓰는 이유?


    public struct OwnedItem
    {
        public int itemID;
        public int level;
    }
    private int FindItemIndex(int itemID) => purchasedItem.FindIndex(x => x.itemID == itemID);
    public int GetItemLevel(int itemID)
    {
        int idx = FindItemIndex(itemID);

        if (idx < 0)
            return 0;
        else
            return purchasedItem[idx].level;
    }
    public int GetValueByLevel(int itemID)
    {
        int idx = FindItemIndex(itemID);

        return itemID == 3 || itemID == 1 ? ((GetItemLevel(itemID) + 1) * 10) / 2 : GetItemLevel(itemID) + 1;
    }
    public void AddItem(int itemID)
    {
        int idx = FindItemIndex(itemID);


        if (idx < 0)
        {
            purchasedItem.Add(new OwnedItem { itemID = itemID, level = 1 });
        }
        else
        {
            OwnedItem i = purchasedItem[idx];
            i.level++;
            purchasedItem[idx] = i;
        }

    }

    public void AddItem_TestMode()
    {

        purchasedItem.Add(new OwnedItem { itemID = 0, level = 3 });
        purchasedItem.Add(new OwnedItem { itemID = 1, level = 3 });
        purchasedItem.Add(new OwnedItem { itemID = 2, level = 3 });
        purchasedItem.Add(new OwnedItem { itemID = 3, level = 5 });
        purchasedItem.Add(new OwnedItem { itemID = 4, level = 1 });



    }
}

public class AchivementData
{
    public List<string> achivement = new();
}

