using System.Collections;
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


    Script[] scripts_Lobby = new Script[5];
    Script[] scripts_Play = new Script[8];

    public  int idx_Lobby = 0;
    public int idx_Play = 0;


    [Header("Lobby")]
    [SerializeField] Image blackBackground_Lobby;
    [SerializeField] RectTransform scriptPanel_Lobby;
    TextMeshProUGUI text_Lobby;

    [Header("Play")]
    [SerializeField] Image blackBackground_Play;
    [SerializeField] RectTransform scriptPanel_Play;
    TextMeshProUGUI text_Play;


    [SerializeField] private GameObject playButton;

    public bool playerStop = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
     
    }
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals("LobbyScene"))
        {

            text_Lobby = scriptPanel_Lobby.GetComponentInChildren<TextMeshProUGUI>();
            scripts_Lobby[0] = new Script("시작", new Vector2(0f, 0f));
            scripts_Lobby[1] = new Script("asd", new Vector2(340f, -250f));
            scripts_Lobby[2] = new Script("asd", new Vector2(-300f, -250f));
            scripts_Lobby[3] = new Script("asd", new Vector2(-300f, 250f));
            scripts_Lobby[4] = new Script("zxc", new Vector2(340f, -250f));

            nextTutorial_Lobby();
        }
        else if (scene.name.Equals("PlayScene"))
        {

            text_Play = scriptPanel_Play.GetComponentInChildren<TextMeshProUGUI>();
            scripts_Play[0] = new Script("안녕 내가 알려줄게", new Vector2(0f, -400f));
            scripts_Play[1] = new Script("대쉬를해", new Vector2(0f, -400f));
            scripts_Play[2] = new Script("어퍼를해", new Vector2(0f, -400f));
            scripts_Play[3] = new Script("아래를해", new Vector2(0f, -400f));
            scripts_Play[4] = new Script("3번까지", new Vector2(0f, -400f));
            scripts_Play[5] = new Script("스킬", new Vector2(0f, -400f));
            scripts_Play[6] = new Script("파이팅", new Vector2(0f, -400f));
            scripts_Play[7] = new Script("", new Vector2(0f, -400f));






            nextTutorial_Play();
        }




    }


    // 버튼(배경) 클릭시 다음 단계로
    public void nextTutorial_Lobby()
    {
            
        if (idx_Lobby >= scripts_Lobby.Length || GameManager.Instance.finishTutorial_Lobby) return;

        text_Lobby.text = scripts_Lobby[idx_Lobby].script;
        scriptPanel_Lobby.anchoredPosition = scripts_Lobby[idx_Lobby].scriptPos;
        if (idx_Lobby == 0)
        {
            blackBackground_Lobby.gameObject.SetActive(true);
            scriptPanel_Lobby.gameObject.SetActive(true);

        }
        else if (idx_Lobby == 1)
        {
            playButton.GetComponent<SpriteRenderer>().sortingOrder = 1;
            playButton.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;

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
            blackBackground_Lobby.rectTransform.SetSiblingIndex(2);
            GameManager.Instance.FinishTutorial_Lobby();
        }

        idx_Lobby++;


    }
    // 조작시 다음 단계로
    public void nextTutorial_Play()
    {
        if (idx_Play >= scripts_Play.Length|| GameManager.Instance.finishTutorial_Play) return;

        text_Play.text = scripts_Play[idx_Play].script;

        if (idx_Play == 0)// 시작 
        {
            StartCoroutine(SetPlayerStop(true));
        }
        else if (idx_Play == 1)  // 대쉬 설명
        {
            return;
        }
        else if (idx_Play == 2) // 점프 설명
        {
            return;
        }
        else if (idx_Play == 3) // 하단 설명
        {
            return;
        }
        else if (idx_Play == 4) // 3회 설명
        {
            
        }
        else if (idx_Play == 5)// 스킬 설명
        {
        }
        else if (idx_Play == 6) // 파이팅
        {
        }
        else if (idx_Play == 7)
        {
            Finish_Play();
            GameManager.Instance.FinishTutorial_Play();
        }
        idx_Play++;

    }

    public void Finish_Lobby()
    {
        if (blackBackground_Lobby.gameObject.activeSelf)
        {
            blackBackground_Lobby.gameObject.SetActive(false);
            scriptPanel_Lobby.gameObject.SetActive(false);
           
        }
    }
    public void Finish_Play()
    {
        if (blackBackground_Play.gameObject.activeSelf)
        {
            blackBackground_Play.gameObject.SetActive(false);
            scriptPanel_Play.gameObject.SetActive(false);
            playerStop = false;
        }
    }

    public IEnumerator SetPlayerStop(bool stop)
    {
        if (GameManager.Instance.finishTutorial_Play) yield break;
        if (stop)
        {
            yield return new WaitForSeconds(0.3f);
            playerStop = true;

            blackBackground_Play.gameObject.SetActive(true);
            scriptPanel_Play.gameObject.SetActive(true);
        }
        else
        {
            playerStop = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
