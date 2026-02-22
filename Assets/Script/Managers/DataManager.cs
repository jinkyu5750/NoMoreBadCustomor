using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager 
{
    public PlayerData playerData { get; private set; }

    public void Init()
    {
        playerData = new PlayerData();
    }
    public void IncreaseReceiptPoint(int point) => playerData.currencyData.AddReceiptPoint(point);
   
    public void DecreaseReceiptPoint(int point) => playerData.currencyData.AddReceiptPoint(-point); 

}
