using UnityEngine;

public class PlayerView : MonoBehaviour {
	public DeckView deck;
	public HandView hand;
	public TableView table;
	public HeroView hero;
	public GameObject cardPrefab;
	public Player player { get; private set; }

	//Asigna el héroe al jugador
	public void SetPlayer (Player player) {
		this.player = player;
		var heroCard = player.hero [0] as Hero;
		hero.SetHero (heroCard);
	}

	//Devuelve los elementos mesa y héroe del juagdor
	public GameObject GetMatch (Card card) {
		switch (card.zone) {
		case Zones.Battlefield:
			return table.GetMatch (card);
		case Zones.Hero:
			return hero.gameObject;
		default:
			Debug.Log ("No Implementation for zone");
			return null;
		}
	}
}