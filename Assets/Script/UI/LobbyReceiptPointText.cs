using TMPro;
using UnityEngine;

public class LobbyReceiptPointUI : MonoBehaviour
{

    private void Start()
    {
    
      UIManager.Instance.SetReceiptPointText(GameManager.Instance.dataManager.playerData.currencyData.receiptPoint.ToString());

    }
 
}
