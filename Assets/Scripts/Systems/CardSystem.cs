using System.Collections.Generic;
using Atonement.AspectContainer;

public class CardSystem : Aspect {
	public List<Card> playable = new List<Card> ();

	//Permite saber qué cartas se pueden jugar desde la mano
	public void Refresh (ControlModes mode) {
		var match = container.GetMatch ();
		var targetSystem = container.GetAspect<TargetSystem> ();
		playable.Clear ();
		foreach (Card card in match.CurrentPlayer[Zones.Hand]) {
			targetSystem.AutoTarget (card, mode);
			var playAction = new PlayCardAction (card);
			if (playAction.Validate ())
            {
				playable.Add(card);
			}				
		}
	}

	//Cambia las zonas en las cuales se encuentran las cartas
	public void ChangeZone (Card card, Zones zone, Player toPlayer = null) {
		var fromPlayer = container.GetMatch ().players [card.ownerIndex];
		toPlayer = toPlayer ?? fromPlayer;
		fromPlayer [card.zone].Remove (card);
		toPlayer [zone].Add (card);
		card.zone = zone;
		card.ownerIndex = toPlayer.index;
	}
}
