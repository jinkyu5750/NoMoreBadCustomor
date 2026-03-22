using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int _score = 0;
    public int maxCombo { get; private set; }
    public int score { get { return _score; } private set { _score = value; } }
    public float playTime { get; private set; } = 0f; // score랑 표현이 같은건가..?

    [SerializeField] int monsterScore = 15;
    [SerializeField] int receiptScore = 3;
    public bool warningStart = false;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        playTime += Time.deltaTime;

        if (PhaseManager.instance.curPhase == 2 && !warningStart)
        {
            warningStart = true;
            UIManager.Instance.MoveWarningPanel();
            SoundManager.instance.PlaySFX("MovePanel");
        }
    }
    public void MonsterScore(bool isSkill)
    {

        score += (isSkill ? monsterScore * 5 : monsterScore) + Random.Range(0, monsterScore + 1);
        UIManager.Instance.UpdateScore(score);
    }

    public void ReceiptScore()
    {
        score += receiptScore + Random.Range(0, receiptScore + 1);
        UIManager.Instance.UpdateScore(score);
    }
    //잡템스코어

    public void ResultScore()
    {
        GameManager.Instance.dataManager.IncreaseReceiptPoint(score/3);
    }

    public void SetMaxCombo(int maxCombo)
    {
        this.maxCombo = maxCombo;
    }
}
