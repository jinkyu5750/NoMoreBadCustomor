using TMPro;
using UnityEngine;

public class TimelineScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI badCustomerText;

    string[] player = new string[8];
    string[] badCustomer = new string[3];

    int playerIdx=0;
    int badCustomerIdx=0;
    private void Awake()
    {
        player[0] = "안녕히 가세요 ~";
        player[1] = "뭐야.. 반말에 돈까지 던지네";
        player[2] = "진상 제대로다 . . . .";
        player[3] = ". . .";
        player[4] = "뭐지, 어제부터 갑자기 진상이 왜이렇게 많아졌지 ? ? . . .";
        player[5] = " ! ! ! 저게 뭐야 ! ! !";
        player[6] = "과자가 . . 걸어다녀 . . ?";
        player[7] = "저걸 따라가면 진상을 알 수 있을거야";

        badCustomer[0] = "안녕하세요, 담배 하나 주세요 ! !";
        badCustomer[1] = "감사합니다 안녕히계세요~";

        badCustomer[2] = "야 알바야 담배 하나 .";


    }

    public void Script(bool isPlayer, int i)
    {
        if (isPlayer)
            playerText.text = player[i];
        else
            badCustomerText.text = badCustomer[i];
    }

    public void PlayerSignal()
    {
        Script(true, playerIdx++);
    }
    public void BadCustomerSignal()
    {
        Script(false, badCustomerIdx++);
    }



}
