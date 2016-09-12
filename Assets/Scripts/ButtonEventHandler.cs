using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public EventTrigger.TriggerEvent onPointerDown;
	public EventTrigger.TriggerEvent onPointerUp;

	public void OnPointerDown(PointerEventData eventData) {
		onPointerDown.Invoke(eventData);
	}

	public void OnPointerUp(PointerEventData eventData) {
		onPointerUp.Invoke(eventData);
	}
}