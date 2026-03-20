using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public bool finish_Lobby { get; private set; } = false;
    public bool finish_Play { get; private set; } = false;

    Script[] scripts_Lobby = new Script[5];
    Script[] scripts_Play = new Script[5];

    int idx_Lobby = 0;
    int idx_Play = 0;


    [Header("Lobby")]
    [SerializeField] Image blackBackground_Lobby;
    [SerializeField] RectTransform scriptPanel_Lobby;
    TextMeshProUGUI text_Lobby;

    [Header("Play")]
    [SerializeField] Image blackBackground_Play;
    [SerializeField] RectTransform scriptPanel_Play;
    TextMeshProUGUI text_Play;


    [SerializeField] private GameObject playButton;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals("LobbyScene"))
        {
            if (finish_Lobby)
                OffPanel_Lobby();

            text_Lobby = scriptPanel_Lobby.GetComponentInChildren<TextMeshProUGUI>();
            scripts_Lobby[0] = new Script("시작", new Vector2(0f, 0f));
            scripts_Lobby[1] = new Script("asd", new Vector2(340f, -250f));
            scripts_Lobby[2] = new Script("asd", new Vector2(-300f, -250f));
            scripts_Lobby[3] = new Script("asd", new Vector2(-300f, 250f));
            scripts_Lobby[4] = new Script("zxc", new Vector2(340f, -250f));
        }
        else if (scene.name.Equals("PlayScene"))
        {
            if (finish_Play)
                OffPanel_Play();

            text_Play = scriptPanel_Play.GetComponentInChildren<TextMeshProUGUI>();
        }


  

    }


    // 버튼(배경) 클릭시 다음 단계로
    public void nextTutorial_Lobby()
    {
        if (idx_Lobby > scripts_Lobby.Length) return;

        idx_Lobby++;

        text_Lobby.text = scripts_Lobby[idx_Lobby].script;
        scriptPanel_Lobby.anchoredPosition = scripts_Lobby[idx_Lobby].scriptPos;


        if (idx_Lobby == 1)
        {
            playButton.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            playButton.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        }
        else if (idx_Lobby == 2)
        {
            blackBackground_Lobby.rectTransform.SetSiblingIndex(1);

        }
        else if (idx_Lobby == 3)
        {
            blackBackground_Lobby.rectTransform.SetSiblingIndex(0);
        }
        else if (idx_Lobby == 4)
        {
            finish_Lobby = true;
            blackBackground_Lobby.rectTransform.SetSiblingIndex(2);

        }
    }

    public void OffPanel_Lobby()
    {
        if (blackBackground_Lobby.gameObject.activeSelf)
        {
            blackBackground_Lobby.gameObject.SetActive(false);
            scriptPanel_Lobby.gameObject.SetActive(false);
        }
    }
    public void OffPanel_Play()
    {
        if (blackBackground_Play.gameObject.activeSelf)
        {
            blackBackground_Play.gameObject.SetActive(false);
            scriptPanel_Play.gameObject.SetActive(false);
        }
    }
}
