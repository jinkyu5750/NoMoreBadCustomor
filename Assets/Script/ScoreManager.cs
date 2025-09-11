using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score=0;
    private void Awake()
    {
        instance = this;
    }

    public void MonsterScore()
    {
        score += 15 + Random.Range(0, 10);
        UIManager.Instance.UpdateScore(score);
    }

    //잡템스코어
}
