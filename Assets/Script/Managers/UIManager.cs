using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{


    public static UIManager Instance;

    //플레이 씬 내 UI
    [SerializeField] private Image hpBar;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button resultExitButton;
    [SerializeField] private Button resultRetryButton;
    [SerializeField] private Image menuPanel;
    [SerializeField] private Image resultPanel;
    [SerializeField] private Image skillGage;
    private Image skillGageBar;
    [SerializeField] private Text skillGageText;
    [SerializeField] private Ease ease;
    [SerializeField] private Text scoreText;


    //로비 씬 내 UI
    [SerializeField] private Button playButton;

    [SerializeField] private Image skillPanel_Top;
    [SerializeField] private Image skillPanel_Down;


    Tweener shakeSkillGage;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;


    }
    void Start()
    {
        skillGageBar = skillGage.transform.Find("Gage/GageBar").GetComponent<Image>();

        stopButton.onClick.AddListener(() => MenuPanel(true));
        resultExitButton.onClick.AddListener(() => ResultPanel(false, false));
        resultRetryButton.onClick.AddListener(() => ResultPanel(false, true));

        menuPanel.gameObject.SetActive(false);
    }



    public void UpdateHPBar(float hp)
    {
        hpBar.fillAmount = hp;
    }
    public void UpdateSkillGage(float gage)
    {

        skillGageBar.fillAmount = (float)gage / 100;
        skillGageText.text = (gage <= 100 ? gage : 100).ToString() + "%";

        if (gage > 100)
            FullSkillGageBar();

    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void FullSkillGageBar()
    {
        skillGage.GetComponentInChildren<Animator>().enabled = true;
        skillGage.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 5), 20, 10).SetLoops(-1);
    }

    public void ResetSkillGageBar()
    {
        skillGage.GetComponentInChildren<Animator>().enabled = false;

        skillGage.transform.DOKill();
        skillGage.transform.rotation = Quaternion.identity;
    }


    public void MenuPanel(bool isActive)
    {
        menuPanel.gameObject.SetActive(isActive);

        if (isActive)
        {

            Sequence seq = DOTween.Sequence();
            seq.Append(menuPanel.transform.DOScale(new Vector2(1.1f, 1.1f), 0.05f).SetEase(ease));
            seq.Append(menuPanel.transform.DOScale(new Vector2(0.95f, 0.95f), 0.05f).SetEase(ease));
            seq.SetUpdate(true); // timeScale에 영향받지 않음
            seq.Play().OnComplete(() =>
            {
                Time.timeScale = 0f; // 애니메이션 완료 후 시간 정지
            });
        }
        else
            Time.timeScale = 1f;

    }

    public void ResultPanel(bool isActive, bool isRetry = false) // 채찍피티가혼낸점//  아래 중 하나라도 생기면 리팩토링 타이밍이야:
    {                                                                              //  버튼마다 ResultPanel(false, true/false)가 난무한다
        resultPanel.gameObject.SetActive(isActive);                               //  결과창에서 선택지가 3개 이상 된다
                                                                                  //  네가 if (isActive) 안에 또 if를 넣기 시작한다
        if (isActive)                                                           //  버튼마다 ResultPanel(false, true/false)가 난무한다
        {
            resultPanel.transform.Find("ScoreText").GetChild(0).GetComponent<TextMeshProUGUI>().text = ScoreManager.instance.score.ToString();
            resultPanel.transform.Find("TimeText").GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)(ScoreManager.instance.playTime - 1f)).ToString() + " Sec";

            resultPanel.rectTransform.DOAnchorPosY(0f, 1f).SetEase(Ease.OutBounce);
        }
        else
        {
            if (isRetry)
                LoadingManager.instance.LoadScene("PlayScene", true); // 다시하기 // 페이드아웃필요할듯
            else
                LoadingManager.instance.LoadScene("LobbyScene", true); // 나가기


        }


    }

    public void MoveSkillPanel(bool start)
    {
        if (start)
        {
            skillPanel_Top.rectTransform.DOAnchorPosY(0f, 0.5f).SetEase(ease);
            skillPanel_Down.rectTransform.DOAnchorPosY(0f, 0.5f).SetEase(ease);
        }
        else
        {
            skillPanel_Top.rectTransform.DOAnchorPosY(300f, 0.5f).SetEase(ease);
            skillPanel_Down.rectTransform.DOAnchorPosY(-300f, 0.5f).SetEase(ease);
        }

    }

}

