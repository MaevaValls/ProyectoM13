using System.Collections;
using UnityEngine;
using Atonement.Notifications;
using Atonement.AspectContainer;
using Atonement.Extensions;

public class DrawCardsView : MonoBehaviour {
	void OnEnable () {
		this.AddObserver (OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction> ());
		this.AddObserver (OnPrepareDrawCards, Global.PrepareNotification<OverdrawAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction> ());
		this.RemoveObserver (OnPrepareDrawCards, Global.PrepareNotification<OverdrawAction> ());
	}

	void OnPrepareDrawCards (object sender, object args) {
		var action = args as DrawCardsAction;
		action.perform.viewer = DrawCardsViewer;
	}

	IEnumerator DrawCardsViewer (IContainer game, GameAction action) {
		yield return true;
		var drawAction = action as DrawCardsAction;
		var boardView = GetComponent<BoardView> ();
		var playerView = boardView.playerViews [drawAction.player.index];

		//Por cada carta robada
		for (int i = 0; i < drawAction.cards.Count; ++i) {
			playerView.deck.PlayDrawCard();
			//Cambia el tamaño del modelo 3D del mazo
			int deckSize = action.player[Zones.Deck].Count + drawAction.cards.Count - (i + 1);
			playerView.deck.ShowDeckSize ((float)deckSize / (float)Player.maxDeck);

			//Activa el modelo de la carta que estaba arriba del todo en el mazo
			var cardView = boardView.cardPooler.Dequeue ().GetComponent<CardView> ();
			cardView.Flip (false);
			cardView.card = drawAction.cards [i];
			cardView.transform.ResetParent (playerView.hand.transform);
			cardView.transform.position = playerView.deck.topCard.position;
			cardView.transform.rotation = playerView.deck.topCard.rotation;
			cardView.gameObject.SetActive (true);

			//Muestra la preview y añade la carta a la mano
			var showPreview = action.player.mode == ControlModes.Local;
			var overDraw = action is OverdrawAction;
			var addCard = playerView.hand.AddCard (cardView.transform, showPreview, overDraw);
			while (addCard.MoveNext ())
				yield return null;
			
		}
	}
}