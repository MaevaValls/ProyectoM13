using System.Collections.Generic;
using Atonement.AspectContainer;
using Atonement.Notifications;
using Atonement.Extensions;

public class TargetSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	public void Destroy () {
		this.RemoveObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	//Selecciona los posibles objetivos
	public void AutoTarget (Card card, ControlModes mode) {
		var target = card.GetAspect<Target> ();
		if (target == null)
			return;
		//En el modo IA marca los objetivos preferidos y en otro modo los permitidos
		var mark = mode == ControlModes.Computer ? target.preferred : target.allowed;
		var candidates = GetMarks (card, mark);
		//Si la lista de candidatos tiene más de 0 lo escoge al azar
		target.selected = candidates.Count > 0 ? candidates.Random() : null;
	}

	//Genera una lista con todos los posibles objetivos que coinciden con las marcas
	public List<Card> GetMarks (Card source, Mark mark) {
		var marks = new List<Card> ();
		var players = GetPlayers (source, mark);
		foreach (Player player in players) {
			var cards = GetCards (source, mark, player);
			marks.AddRange (cards);
		}
		return marks;
	}

	//Coge los jugadores que coinciden con la alianza de la marca
	List<Player> GetPlayers (Card source, Mark mark) {
		var dataSystem = container.GetAspect<DataSystem> ();
		var players = new List<Player> ();
		var pair = new Dictionary<Alliance, Player> () {
			{ Alliance.Ally, dataSystem.match.players[source.ownerIndex] }, 
			{ Alliance.Enemy, dataSystem.match.players[1 - source.ownerIndex] }
		};
		foreach (Alliance key in pair.Keys) {
			if (mark.alliance.Contains (key)) {
				players.Add (pair[key]);
			}
		}
		return players;
	}

	//Coge las cartas que coinciden con la zona de la marca
	List<Card> GetCards (Card source, Mark mark, Player player) {
		var cards = new List<Card> ();
		var zones = new Zones[] { 
			Zones.Hero, 
			Zones.Weapon, 
			Zones.Deck, 
			Zones.Hand, 
			Zones.Battlefield, 
			Zones.Graveyard 
		};
		foreach (Zones zone in zones) {
			if (mark.zones.Contains (zone)) {
				cards.AddRange (player[zone]);
			}
		}
		return cards;
	}

	//Valida si es posible jugar la carta, es decir si se encuentra entre los posibles objetivos
	void OnValidatePlayCard (object sender, object args) {
		var playCardAction = sender as PlayCardAction;
		var card = playCardAction.card;
		var target = card.GetAspect<Target> ();
		if (target == null || (target.required == false && target.selected == null))
			return;
		var validator = args as Validator;
		var candidates = GetMarks (card, target.allowed);
		if (!candidates.Contains(target.selected)) {
			validator.Invalidate ();
		}
	}
}