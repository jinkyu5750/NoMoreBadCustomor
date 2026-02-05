using System.Collections.Generic;
using Unity.Collections;

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
    public int receiptPoint;
}
[System.Serializable]
public class ShopData
{
    public List<ownedItem> purchasedItem = new(); // 리스트를 쓰는 이유?


    public struct ownedItem
    {
        public int itemID;
        public int count;
    }


    public void AddItem(int itemID) // 초과구매는 어떡하지? 여기서 처리하려면 itemID가아니라 info를받아야됨
    {
        int idx = purchasedItem.FindIndex(x => x.itemID == itemID);
        
        if(idx<0)
        {
            purchasedItem.Add(new ownedItem { itemID = itemID ,count =1});
        }
        else
        {
            ownedItem i = purchasedItem[idx];
            i.count++;
            purchasedItem[idx] = i;
        }
  
    }
}
[System.Serializable]
public class AchivementData
{
    public List<string> achivement = new();
}

