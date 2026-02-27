using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    RectTransform touchArea;
    private Image joystick_Parent;
    private Image joystick_Child;

    Vector2 startPos;
    private void Start()
    {
        touchArea = transform.parent.GetComponent<RectTransform>();
        joystick_Parent = GetComponent<Image>();
        joystick_Child = GetComponentInChildren<Image>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(touchArea,
            eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {

            float range = joystick_Parent.GetComponent<RectTransform>().sizeDelta.x;
            Vector2 pos = (eventData.position - startPos).magnitude > range ?
                (eventData.position - startPos).normalized * range : eventData.position;

            joystick_Child.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick_Parent.enabled = true;
        startPos = eventData.position;
        OnDrag(eventData);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        joystick_Parent.enabled = false;
    }
}
