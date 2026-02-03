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
            Debug.Log(item.name);
            GameObject itemList = Instantiate(itemListPrefab);
            itemList.transform.SetParent(itemList_Parent, false);//UI는 false를 해줘야함

            itemList.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
            itemList.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.itemName;
            itemList.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.discription;
            itemList.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = item.price[0].ToString();


        }

    }
}
