using Atonement.AspectContainer;
using Atonement.Extensions;

public class EnemySystem : Aspect {

	public void TakeTurn () {
		//Si puede jugar cartas o atacar lo hace y cambia de turno
		if (PlayACard () || Attack ())
			return;
		container.GetAspect<MatchSystem> ().ChangeTurn ();
	}

	//Si tiene cartas jugables las juega
	bool PlayACard () {
		var system = container.GetAspect<CardSystem> ();
		if (system.playable.Count == 0)
			return false;
		var card = system.playable.Random ();
		var action = new PlayCardAction (card);
		container.Perform (action);
		return true;
	}

	//Si puede atacar ataca
	bool Attack () {
		var system = container.GetAspect<AttackSystem> ();
		if (system.validAttackers.Count == 0 || system.validTargets.Count == 0)
			return false;
		var attacker = system.validAttackers.Random ();
		var target = system.validTargets.Random ();
		var action = new AttackAction (attacker, target);
		container.Perform (action);
		return true;
	}
}
