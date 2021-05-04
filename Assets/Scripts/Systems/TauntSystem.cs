using System.Collections.Generic;
using Atonement.AspectContainer;
using Atonement.Notifications;

public class TauntSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
	}

	public void Destroy () {
		this.RemoveObserver (OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
	}

	//Filtra los objetivos y elimina los que no tienen provocar
	void OnFilterAttackTargets (object sender, object args) {
		var candidates = args as List<Card>;
		if (TargetsContainTaunt (candidates) == false)
			return;
		
		for (int i = candidates.Count - 1; i >= 0; --i) {
			if (candidates [i].GetAspect<Taunt> () == null)
				candidates.RemoveAt (i);
		}
	}

	//Comprueba si en una lista de cartas alguna tiene provocar
	bool TargetsContainTaunt (List<Card> cards) {
		foreach (Card card in cards) {
			if (card.GetAspect<Taunt> () != null)
				return true;
		}
		return false;
	}
}
