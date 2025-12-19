using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int _score = 0;
    public int score {  get { return _score; } private set { _score = value; } }
    [SerializeField] int monsterScore = 15;
    [SerializeField] int receiptScore = 3;
    private void Awake()
    {
        instance = this;
    }

    public void MonsterScore(bool isSkill)
    {

        score += (isSkill ? monsterScore*5 : monsterScore) + Random.Range(0, monsterScore + 1);
        UIManager.Instance.UpdateScore(score);
    }

    public void ReceiptScore()
    {
        score += receiptScore + Random.Range(0, receiptScore + 1);
        UIManager.Instance.UpdateScore(score);
    }
    //잡템스코어
}
