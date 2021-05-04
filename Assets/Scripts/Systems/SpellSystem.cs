using Atonement.AspectContainer;
using Atonement.Notifications;

public class SpellSystem : Aspect, IObserve {

	public void Awake () {
		this.AddObserver (OnPeformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
		this.AddObserver (OnPrepareCastSpell, Global.PrepareNotification<CastSpellAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPeformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
		this.RemoveObserver (OnPrepareCastSpell, Global.PrepareNotification<CastSpellAction> (), container);
	}

	//Comprueba si la carta que se ha jugado es un hechizo y si lo es lo lanza
	void OnPeformPlayCard (object sender, object args) {
		var action = args as PlayCardAction;
		var spell = action.card as Spell;
		if (spell != null) {
			var reaction = new CastSpellAction (spell);
			container.AddReaction (reaction);
		}
	}

	//Al lanzar el hechizo aplica la acción de la habilidad
	void OnPrepareCastSpell (object sender, object args) {
		var action = args as CastSpellAction;
		var ability = action.spell.GetAspect<Ability> ();
		var reaction = new AbilityAction (ability);
		container.AddReaction (reaction);
	}
}
