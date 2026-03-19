using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Script
{
    public string script;
    public Vector2 scriptPos;

    public Script(string script, Vector2 scriptPos)
    {
        this.script = script;
        this.scriptPos = scriptPos;
    }
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    Script[] lobbyScripts;
    int idx = 0;
    Script[] playScripts;

    [SerializeField] Image blackBackground;
    [SerializeField] RectTransform scriptPanel;
    TextMeshProUGUI script;

    [SerializeField] private GameObject playButton;
    private void Awake()
    {

    }
    void Start()
    {
        script = scriptPanel.GetComponentInChildren<TextMeshProUGUI>();
        lobbyScripts[0] = new Script("시작", new Vector2(0f, 0f));

        lobbyScripts[1] = new Script("asd", new Vector2(340f, -250f));
        lobbyScripts[2] = new Script("asd", new Vector2(-250f, 200f));
        lobbyScripts[3] = new Script("zxc", new Vector2(700f, 200f));

    }


    // 버튼(배경) 클릭시 다음 단계로
    public void nextTutorial()
    {
        if (idx > lobbyScripts.Length) return;

        idx++;

        script.text = lobbyScripts[idx].script;
        scriptPanel.anchoredPosition = lobbyScripts[idx].scriptPos;


        if (idx == 1)
        {
            playButton.layer = 14;
        }
        else if (idx == 2)
        {
            playButton.layer = 0;

        }
    }

}
