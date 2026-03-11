using UnityEngine;

[System.SerializableAttribute]
public class Phase
{
    public float runSpeed;
    public float soundPitch; // speed
}
public class PhaseManager : MonoBehaviour
{
    [SerializeField] Phase curPhase;
    [SerializeField] Phase[] phase;

    [SerializeField] Player player;
    private void Start()
    {
        curPhase = phase[0];
    }
    private void Update()
    {

        if (ScoreManager.instance.score > 1000f && ScoreManager.instance.score <= 2000f && curPhase == phase[0])
            SetPhase(1);
        else if (ScoreManager.instance.score > 2000f && curPhase == phase[1])
            SetPhase(2);

    }

    public void SetPhase(int level)
    {
        curPhase = phase[level];
        player.SetRunSpeed(phase[level].runSpeed);
        SoundManager.instance.SetBGMPitch(phase[level].soundPitch);

    }
}

