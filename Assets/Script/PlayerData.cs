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
    public int receiptPoint;
}
[System.Serializable]
public class ShopData
{
    public List<string> purchasedItem = new(); // 리스트를 쓰는 이유?
}
[System.Serializable]
public class AchivementData
{
    public List<string> achivement = new();
}

