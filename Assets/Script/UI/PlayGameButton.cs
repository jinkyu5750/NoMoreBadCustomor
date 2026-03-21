using UnityEngine;
using UnityEngine.EventSystems;

public class PlayGameButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.finishTutorial_Lobby)
        {
            TutorialManager.instance.Finish_Lobby();
            GameManager.Instance.StartGame();
        }
    }
}
