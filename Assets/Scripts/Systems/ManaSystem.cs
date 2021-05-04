﻿using Atonement.AspectContainer;
using Atonement.Notifications;

public class ManaSystem : Aspect, IObserve {
	public const string ValueChangedNotification = "ManaSystem.ValueChangedNotification";

	public void Awake () {
		this.AddObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
		this.AddObserver (OnPerformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
		this.AddObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	public void Destroy () {
		this.RemoveObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
		this.RemoveObserver (OnPerformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
		this.RemoveObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	//Aumenta el maná permanente en 1 cada turno y reinicia sus atributos
	void OnPerformChangeTurn (object sender, object args) {
		var mana = container.GetMatch ().CurrentPlayer.mana;
		if (mana.permanent < Mana.MaxSlots)
			mana.permanent++;
		mana.spent = 0;
		mana.temporary = 0;
		this.PostNotification (ValueChangedNotification, mana);
	}

	//Añade el coste de la carta al maná gastado
	void OnPerformPlayCard (object sender, object args) {
		var action = args as PlayCardAction;
		var mana = container.GetMatch ().CurrentPlayer.mana;
		mana.spent += action.card.cost;
		this.PostNotification (ValueChangedNotification, mana);
	}

	//Si el coste de la carta es mayor al maná disponible se invalida
	void OnValidatePlayCard (object sender, object args) {
		var playCardAction = sender as PlayCardAction;
		var validator = args as Validator;
		var player = container.GetMatch().players[playCardAction.card.ownerIndex];
		if (player.mana.Available < playCardAction.card.cost)
			validator.Invalidate ();
	}
}
