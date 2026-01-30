using TMPro;
using UnityEngine;

public class LobbyReceiptPointUI : MonoBehaviour
{
     private TextMeshProUGUI receiptPointText;

    private void Start()
    {
        receiptPointText = GetComponent<TextMeshProUGUI>();
        receiptPointText.text = GameManager.Instance.dataManager.playerData.currencyData.receiptPoint.ToString();

    }
    private void OnEnable()
    {

    }
}
