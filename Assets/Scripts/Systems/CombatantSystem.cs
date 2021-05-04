using System.Collections.Generic;
using Atonement.AspectContainer;
using Atonement.Notifications;

public class CombatantSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnFilterAttackers, AttackSystem.FilterAttackersNotification, container);
		this.AddObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnFilterAttackers, AttackSystem.FilterAttackersNotification, container);
		this.RemoveObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
	}

	//Filtra la lista de atacantes según si tienen la interfaz ICombatant
	void OnFilterAttackers (object sender, object args) {
		var candidates = args as List<Card>;
		for (int i = candidates.Count - 1; i >= 0; --i) {
			var combatant = candidates [i] as ICombatant;
			if (!CanAttack(combatant)) {
				candidates.RemoveAt (i);
			}
		}
	}

	//Al cambiar de turno devolvemos los ataques restantes a los ataques permitidos inicialmente
	void OnPerformChangeTurn (object sender, object args) {
		var action = args as ChangeTurnAction;
		var player = container.GetMatch ().players [action.targetPlayerIndex];
		var active = container.GetAspect<AttackSystem> ().GetActive (player);
		foreach (Card card in active) {
			var combatant = card as ICombatant;
			if (combatant == null)
				continue;
			combatant.remainingAttacks = combatant.allowedAttacks;
		}
	}

	//Comprueba que el atacante sea válido, haga más de 0 daño y le queden ataques disponibles
	bool CanAttack (ICombatant combatant) {
		return combatant != null && combatant.attack > 0 && combatant.remainingAttacks > 0;
	}
}