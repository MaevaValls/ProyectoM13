using System.Collections.Generic;
using Atonement.AspectContainer;
using Atonement.Notifications;

public class DestructableSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> (), container);
		this.AddObserver (OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> (), container);
		this.RemoveObserver (OnFilterAttackTargets, AttackSystem.FilterTargetsNotification, container);
	}
	
	//Por cada objetivo en la lista resta el daño a la vida
	void OnPerformDamageAction (object sender, object args) {
		var action = args as DamageAction;
		foreach (IDestructable target in action.targets) {
			target.hitPoints -= action.amount;
		}
	}

	//Filtra los objetivos dependiendo de si tienen la interfaz IDestructable
	void OnFilterAttackTargets (object sender, object args) {
		var candidates = args as List<Card>;
		for (int i = candidates.Count - 1; i >= 0; --i) {
			var destructable = candidates [i] as IDestructable;
			if (destructable == null)
				candidates.RemoveAt (i);
		}
	}
}