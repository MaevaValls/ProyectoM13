using System.Collections.Generic;
using UnityEngine;
using Atonement.Pooling;

public class BoardView : MonoBehaviour {
	public GameObject damageMarkPrefab;
	public List<PlayerView> playerViews;
	public SetPooler cardPooler;
	public SetPooler minionPooler;

	//Se asignan las vistas a cada jugador
	void Start () {
		var match = GetComponentInParent<GameViewSystem> ().container.GetMatch ();
		for (int i = 0; i < match.players.Count; ++i) {
			playerViews [i].SetPlayer (match.players[i]);
		}
	}

	public GameObject GetMatch (Card card) {
		var playerView = playerViews [card.ownerIndex];
		return playerView.GetMatch (card);
	}
}