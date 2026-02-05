using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            itemList.GetComponent<ShopItemPanel>().InitItemList(item);
        }

    }
    
    public void PurchaseItem()
    {
        Debug.Log(transform.name);
        int itemID = transform.parent.parent.GetComponent<ShopItemPanel>().item.itemID; // 해당 패널의 아이템ID찾기
   
        foreach(var item in items)
        {
            if(item.itemID == itemID)
            {
                GameManager.Instance.dataManager.playerData.shopData.AddItem(itemID);
                break;
            }
        }

        Debug.Log("아이템구매실패");
    }
}
