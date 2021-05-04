using UnityEngine;
using UnityEngine.EventSystems;
using Atonement.Notifications;

public class Clickable : MonoBehaviour, IPointerClickHandler {
	public const string ClickedNotification = "Clickable.ClickedNotification";

	//Permite crear eventos en hacer click
	public void OnPointerClick(PointerEventData eventData) {
		this.PostNotification (ClickedNotification, eventData);
	}
}