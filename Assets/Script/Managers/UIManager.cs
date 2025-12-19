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

        stopButton.onClick.AddListener(ShowMenuPanel);
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


    public void ShowMenuPanel()
    {
        menuPanel.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(menuPanel.transform.DOScale(new Vector2(1.1f, 1.1f), 0.05f).SetEase(ease));
        seq.Append(menuPanel.transform.DOScale(new Vector2(0.95f, 0.95f), 0.05f).SetEase(ease));
        seq.SetUpdate(true); // timeScale에 영향받지 않음
        seq.Play().OnComplete(() =>
        {
            Time.timeScale = 0f; // 애니메이션 완료 후 시간 정지
        });
    }

    public void HideMenuPanel()
    {
        menuPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
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

    public void ResultPanel(bool isActive) // 패널관련은 bool로 합쳐도될듯
    {
        resultPanel.gameObject.SetActive(isActive);
        //여기서 스코어얻은걸로 게임매니저에도 업데이트..?
        if (isActive)
        {
            transform.GetChild(2222).GetComponent<TextMeshProUGUI>().text = ScoreManager.instance.score.ToString();// 점수부분만 따로 뗄까 편하게
            // transform.GetChild(2222).GetComponent<TextMeshProUGUI>().text = GameMa //게임매니저에서 플탐재야됨
            
            Sequence seq = DOTween.Sequence();
            seq.Append(resultPanel.transform.DOScale(new Vector2(1.1f, 1.1f), 0.05f).SetEase(ease));
            seq.Append(resultPanel.transform.DOScale(new Vector2(0.95f, 0.95f), 0.05f).SetEase(ease));
            seq.Play();

        }
        else
        {
            LoadingManager.instance.LoadScene("LobbyScene");

            // 킨다음 점수,시간 업뎃
        }


    }
}

