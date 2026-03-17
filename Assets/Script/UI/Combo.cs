using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Combo : MonoBehaviour
{
    [SerializeField] private Image[] numberUI;
    [SerializeField] private Sprite[] numberImage;


    public void SetComboUI(int combo)
    {
        if (combo == 0)
            return;

        string str = combo.ToString();
        int idx = str.Length - 1;
        for (int i = numberUI.Length - 1; i >= 0; i--)
        {
            if (idx < 0)
            {
                numberUI[i].gameObject.SetActive(false);
                continue;
            }

            int number = str[idx--] - '0'; // string to int

            numberUI[i].sprite = numberImage[number];
            numberUI[i].gameObject.SetActive(true);
            numberUI[i].transform.DOKill();
            numberUI[i].transform.localScale = Vector3.one;
            numberUI[i].transform.DOPunchScale(Vector3.one * 0.35f, 0.25f, 6, 0.5f);
        }
    }
}
