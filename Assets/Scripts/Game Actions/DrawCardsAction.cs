using System.Collections.Generic;
using Atonement.AspectContainer;
using System;

public class DrawCardsAction : GameAction, IAbilityLoader {
	public int amount;
	public List<Card> cards;

	#region Constructors
	public DrawCardsAction() {
		
	}

	//Toma un jugador y una cantidad de cartas a robar
	public DrawCardsAction(Player player, int amount) {
		this.player = player;
		this.amount = amount;
	}
	#endregion

	#region IAbility
	//Configura una habilidad para robar cartas
	public void Load (IContainer game, Ability ability) {
		player = game.GetMatch ().players [ability.card.ownerIndex];
		amount = Convert.ToInt32 (ability.userInfo);
	}
	#endregion
}