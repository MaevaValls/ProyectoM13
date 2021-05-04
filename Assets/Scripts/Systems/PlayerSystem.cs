using UnityEngine;
using Atonement.AspectContainer;
using Atonement.Notifications;
using Atonement.Extensions;

public class PlayerSystem : Aspect, IObserve {

	public void Awake () {
		this.AddObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
		this.AddObserver (OnPerformDrawCards, Global.PerformNotification<DrawCardsAction> (), container);
		this.AddObserver (OnPerformFatigue, Global.PerformNotification<FatigueAction> (), container);
		this.AddObserver (OnPerformOverDraw, Global.PerformNotification<OverdrawAction> (), container);
		this.AddObserver (OnPerformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
		this.RemoveObserver (OnPerformDrawCards, Global.PerformNotification<DrawCardsAction> (), container);
		this.RemoveObserver (OnPerformFatigue, Global.PerformNotification<FatigueAction> (), container);
		this.RemoveObserver (OnPerformOverDraw, Global.PerformNotification<OverdrawAction> (), container);
		this.RemoveObserver (OnPerformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
	}

	//En cambiar de turno el jugador roba una carta
	void OnPerformChangeTurn (object sender, object args) {
		var action = args as ChangeTurnAction;
		var match = container.GetAspect<DataSystem> ().match;
		var player = match.players [action.targetPlayerIndex];
		DrawCards (player, 1);
	}

	//En robar cartas
	void OnPerformDrawCards (object sender, object args) {
		var action = args as DrawCardsAction;

		//Si el mazo tiene 0 cartas se aplica fatiga
		int deckCount = action.player [Zones.Deck].Count;
		int fatigueCount = Mathf.Max(action.amount - deckCount, 0);
		for (int i = 0; i < fatigueCount; ++i) {
			var fatigueAction = new FatigueAction (action.player);
			container.AddReaction (fatigueAction);
		}

		//Si la mano no tiene espacio para mas cartas se queman las extra
		int roomInHand = Player.maxHand - action.player [Zones.Hand].Count;
		int overDraw = Mathf.Max ((action.amount - fatigueCount) - roomInHand, 0);
		if (overDraw > 0) {
			var overDrawAction = new OverdrawAction (action.player, overDraw);
			container.AddReaction (overDrawAction);
		}

		//Se cambia la zona de las cartas del mazo a la mano
		int drawCount = action.amount - fatigueCount - overDraw;
		action.cards = action.player [Zones.Deck].Draw (drawCount);
		foreach (Card card in action.cards)
			ChangeZone (card, Zones.Hand);
	}

	void OnPerformFatigue (object sender, object args) {
		var action = args as FatigueAction;
		action.player.fatigue++;

		var damageTarget = action.player.hero [0] as IDestructable;
		var damageAction = new DamageAction (damageTarget, action.player.fatigue);
		container.AddReaction (damageAction);
	}

	void OnPerformOverDraw (object sender, object args) {
		var action = args as OverdrawAction;
		action.cards = action.player [Zones.Deck].Draw (action.amount);
		foreach (Card card in action.cards)
			ChangeZone (card, Zones.Graveyard);
	}

	void OnPerformPlayCard (object sender, object args) {
		var action = args as PlayCardAction;
		if (action.card.zone == Zones.Hand)
			ChangeZone (action.card, Zones.Graveyard);
	}

	void DrawCards (Player player, int amount) {
		var action = new DrawCardsAction (player, amount);
		container.AddReaction (action);
	}

	void ChangeZone (Card card, Zones zone, Player toPlayer = null) {
		var cardSystem = container.GetAspect<CardSystem> ();
		cardSystem.ChangeZone (card, zone, toPlayer);
	}
}