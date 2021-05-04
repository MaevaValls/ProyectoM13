using Atonement.AspectContainer;
using Atonement.Notifications;
using System;

public class AbilitySystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnPerformAbilityAction, Global.PerformNotification<AbilityAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPerformAbilityAction, Global.PerformNotification<AbilityAction> (), container);
	}

	//Al realizar la acción aplica el efecto de la habilidad
	void OnPerformAbilityAction (object sender, object args) {
		var action = args as AbilityAction;
		var type = Type.GetType (action.ability.actionName);
		var instance = Activator.CreateInstance (type) as GameAction;
		var loader = instance as IAbilityLoader;
		if (loader != null)
			loader.Load (container, action.ability);
		container.AddReaction (instance);
	}
}
