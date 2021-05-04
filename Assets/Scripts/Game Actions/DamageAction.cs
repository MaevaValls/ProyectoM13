using System.Collections.Generic;
using Atonement.AspectContainer;
using System;

public class DamageAction : GameAction, IAbilityLoader {
	public List<IDestructable> targets;
	public int amount;

	#region Constructors
	public DamageAction() {
		
	}

	//Lista con un solo objetivo que recibe daño
	public DamageAction(IDestructable target, int amount) {
		targets = new List<IDestructable> (1);
		targets.Add (target);
		this.amount = amount;
	}

	//Lista con varios objetivos que reciben daño
	public DamageAction(List<IDestructable> targets, int amount) {
		this.targets = targets;
		this.amount = amount;
	}
	#endregion

	#region IAbility
	//Comprueba si las cartas tienen la interfaz destruible para añadirlos a la lista de objetivos para habilidad
	public void Load (IContainer game, Ability ability) {
		var targetSelector = ability.GetAspect<ITargetSelector> ();
		var cards = targetSelector.SelectTargets (game);
		targets = new List<IDestructable> ();
		foreach (Card card in cards) {
			var destructable = card as IDestructable;
			if (destructable != null)
				targets.Add (destructable);
		}
		amount = Convert.ToInt32 (ability.userInfo);
	}
	#endregion
}