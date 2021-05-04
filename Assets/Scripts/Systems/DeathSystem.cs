using Atonement.AspectContainer;
using Atonement.Notifications;

public class DeathSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnDeathReaperNotification, ActionSystem.deathReaperNotification);
		this.AddObserver (OnPerformDeath, Global.PerformNotification<DeathAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnDeathReaperNotification, ActionSystem.deathReaperNotification);
		this.RemoveObserver (OnPerformDeath, Global.PerformNotification<DeathAction> (), container);
	}

	//Al recibir la notificación de muerte se elimina la carta
	void OnDeathReaperNotification (object sender, object args) {
		var match = container.GetMatch ();
		foreach (Player player in match.players) {
			foreach (Card card in player[Zones.Battlefield]) {
				if (ShouldReap (card))
					TriggerReap (card);
			}
		}
	}

	//Mueve la carta eliminada de la mesa al cementerio
	void OnPerformDeath (object sender, object args) {
		var action = args as DeathAction;
		var cardSystem = container.GetAspect<CardSystem> ();
		cardSystem.ChangeZone (action.card, Zones.Graveyard);
	}

	//Comprueba que la carta sea IDestructable y su vida sea igual o menor de 0
	bool ShouldReap (Card card) {
		var target = card as IDestructable;
		return target != null && target.hitPoints <= 0;
	}

	//Muere la carta
	void TriggerReap (Card card) {
		var action = new DeathAction (card);
		container.AddReaction (action);
	}
}