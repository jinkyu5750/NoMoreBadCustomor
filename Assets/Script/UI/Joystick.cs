using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public enum attackDir { Dash=1,Upper,Lower}
    public attackDir dir { get; private set; }

    [SerializeField] float minDragDistance = 25f;

    RectTransform touchArea;
    private Image joystick_Parent;
    private Image joystick_Child;

    Vector2 startPos;

  
    private void Start()
    {
        touchArea = GetComponent<RectTransform>();
        joystick_Parent = transform.Find("Joystick").GetComponent<Image>();
        joystick_Child = transform.Find("Joystick/Joystick_Child").GetComponent<Image>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(touchArea,
            eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {

            float range = joystick_Parent.rectTransform.sizeDelta.x / 2;
            Vector2 dragDir = localPoint - startPos;
            Vector2 pos = dragDir.magnitude > range ? dragDir.normalized * range : dragDir; //clampmagnitude도 있음

            joystick_Child.rectTransform.anchoredPosition = pos;


            if (dragDir.magnitude > minDragDistance)
            {
                if (Mathf.Abs(dragDir.x) > Mathf.Abs(dragDir.y)) // (4,3) -> Dash공격 
                {
                    if (dragDir.x > 0)
                        dir = attackDir.Dash;
                }
                else
                {
                    if (dragDir.y > 0)
                        dir = attackDir.Upper;
                    else
                        dir = attackDir.Lower;

                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick_Parent.enabled = true;
        joystick_Child.enabled = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(touchArea, eventData.position, eventData.pressEventCamera, out startPos);
        joystick_Parent.rectTransform.anchoredPosition = startPos;

        OnDrag(eventData);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        joystick_Parent.enabled = false;
        joystick_Child.enabled = false;
        dir = 0;

    }

    
}
