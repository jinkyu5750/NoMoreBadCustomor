using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score=0;
    [SerializeField] int monsterScore = 15;
    [SerializeField] int receiptScore = 3;
    private void Awake()
    {
        instance = this;
    }

    public void MonsterScore()
    {
        score += monsterScore + Random.Range(0, monsterScore+1);
        UIManager.Instance.UpdateScore(score);
    }

    public void ReceiptScore()
    {
        score += receiptScore + Random.Range(0, receiptScore + 1);
        UIManager.Instance.UpdateScore(score);
    }
    //잡템스코어
}
