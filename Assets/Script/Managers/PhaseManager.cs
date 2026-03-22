using UnityEngine;

[System.SerializableAttribute]
public class Phase
{
    public float runSpeed;
    public float soundPitch; // speed
}
public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    public int curPhase { get; private set;}
    [SerializeField] public Phase[] phase;

    [SerializeField] Player player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        curPhase = 0;
    }
    private void Update()
    {

        if (ScoreManager.instance.score > 500f && ScoreManager.instance.score <= 1500f && curPhase == 0)
            SetPhase(1);
        else if (ScoreManager.instance.score > 1500f &&ScoreManager.instance.score < 2000f && curPhase == 1)
            SetPhase(2);
        else if (ScoreManager.instance.score >= 2000f && ScoreManager.instance.score < 3000 && curPhase == 2)
            SetPhase(3);
        else if(ScoreManager.instance.score >=3000)
        {
            int step = Mathf.FloorToInt(ScoreManager.instance.score / 1000f);

            if (step % 2 == 0)
                SetPhase(2);
            else
                SetPhase(3);

        }
    }

    public void SetPhase(int level)
    {
        curPhase = level;
        player.SetRunSpeed(phase[level].runSpeed);
        SoundManager.instance.SetBGMPitch(phase[level].soundPitch);

    }
}

