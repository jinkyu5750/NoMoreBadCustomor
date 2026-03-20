using UnityEngine;
using UnityEngine.EventSystems;

public class PlayGameButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (TutorialManager.instance.finish_Lobby)
        {
            TutorialManager.instance.OffPanel_Lobby();
            GameManager.Instance.StartGame();
        }
    }
}
