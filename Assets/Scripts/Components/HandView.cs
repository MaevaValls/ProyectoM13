using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Atonement.Animation;
using Atonement.Pooling;
using Atonement.Notifications;
using Atonement.AspectContainer;

public class HandView : MonoBehaviour {
	public List<Transform> cards = new List<Transform>();
	public Transform activeHandle;
	public Transform inactiveHandle;

	void OnEnable () {
		this.AddObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	//Añade una carta a la mano del jugador
	public IEnumerator AddCard (Transform card, bool showPreview, bool overDraw) {
		//Muestra la animación de preview
		if (showPreview) {
			var preview = ShowPreview (card);
			while (preview.MoveNext ())
				yield return null;
			
			var tweener = card.Wait (1);
			while (tweener != null)
				yield return null;
		}

		//Muestra la animación de descarte o añade la carta a la lista de cartas
		if (overDraw) {
			var discard = OverdrawCard (card);
			while (discard.MoveNext ())
				yield return null;
		} else {
			cards.Add (card);
			var layout = LayoutCards ();
			while (layout.MoveNext ())
				yield return null;
		}
	}

	//Animación para que el jugador vea la carta antes de añadirse a la mano
	public IEnumerator ShowPreview (Transform card) {
		Tweener tweener = null;
		card.RotateTo (activeHandle.rotation);
		tweener = card.MoveTo (activeHandle.position, Tweener.DefaultDuration, EasingEquations.EaseOutBack);
		var cardView = card.GetComponent<CardView> ();
		while (tweener != null) {
			if (!cardView.isFaceUp) {
				var toCard = (Camera.main.transform.position - card.position).normalized;
				if (Vector3.Dot (card.up, toCard) > 0)
					cardView.Flip (true);
			}
			yield return null;
		}
	}

	//Ajusta la posición de las cartas cuando se añaden a la mano
	public IEnumerator LayoutCards (bool animated = true) {
		var overlap = 0.4f;
		var width = cards.Count * overlap;
		var xPos = -(width / 2f);
		var duration = animated ? 0.25f : 0;

		Tweener tweener = null;
		for (int i = 0; i < cards.Count; ++i) {
			var canvas = cards [i].GetComponentInChildren<Canvas> ();
			canvas.sortingOrder = i;

			var position = inactiveHandle.position + new Vector3 (xPos, 0, 0);
			cards [i].RotateTo (inactiveHandle.rotation, duration);
			tweener = cards [i].MoveTo (position, duration);
			xPos += overlap;
		}

		while (tweener != null)
			yield return null;
	}

	public CardView GetView (Card card) {
		foreach (Transform t in cards) {
			var cardView = t.GetComponent<CardView> ();
			if (cardView.card == card) {
				return cardView;
			}
		}
		return null;
	}

	public void Dismiss (CardView card) {
		cards.Remove (card.transform);

		card.gameObject.SetActive (false);
		card.transform.localScale = Vector3.one;

		var poolable = card.GetComponent<Poolable> ();
		var pooler = GetComponentInParent<BoardView> ().cardPooler;
		pooler.Enqueue (poolable);
	}

	IEnumerator OverdrawCard (Transform card) {
		Tweener tweener = card.ScaleTo (Vector3.zero, 0.5f, EasingEquations.EaseInBack);
		while (tweener != null)
			yield return null;
		Dismiss (card.GetComponent<CardView> ());
	}

	void OnValidatePlayCard (object sender, object args) {
		var action = sender as PlayCardAction;
		if (GetComponentInParent<PlayerView> ().player.index == action.card.ownerIndex) {
			action.perform.viewer = PlayCardViewer;
			action.cancel.viewer = CancelPlayCardViewer;
		}
	}

	//Juega una carta y la elimina de la mano del jugador
	IEnumerator PlayCardViewer (IContainer game, GameAction action) {
		var playAction = action as PlayCardAction;
		CardView cardView = GetView (playAction.card);
		if (cardView == null)
			yield break;
		
		cards.Remove (cardView.transform);
		StartCoroutine (LayoutCards (true));
		var discard = OverdrawCard (cardView.transform);
		while (discard.MoveNext ())
			yield return null;
	}

	IEnumerator CancelPlayCardViewer (IContainer game, GameAction action) {
		var layout = LayoutCards (true);
		while (layout.MoveNext ())
			yield return null;
	}
}