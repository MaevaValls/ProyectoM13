using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Atonement.Notifications;
using Atonement.Pooling;
using Atonement.Extensions;
using Atonement.AspectContainer;
using Atonement.Animation;

public class TableView : MonoBehaviour {
	public List<MinionView> minions = new List<MinionView> ();
	SetPooler minionPooler;

	void Awake () {
		var board = GetComponentInParent<BoardView> ();
		minionPooler = board.minionPooler;
	}

	void OnEnable () {
		this.AddObserver (OnPrepareSummon, Global.PrepareNotification<SummonMinionAction> ());
		this.AddObserver (OnPrepareDeath, Global.PrepareNotification<DeathAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnPrepareSummon, Global.PrepareNotification<SummonMinionAction> ());
		this.RemoveObserver (OnPrepareDeath, Global.PrepareNotification<DeathAction> ());
	}

	void OnPrepareSummon (object sender, object args) {
		var action = args as SummonMinionAction;
		if (GetComponentInParent<PlayerView>().player.index == action.minion.ownerIndex)
			action.perform.viewer = SummonMinion;
	}

	void OnPrepareDeath (object sender, object args) {
		var action = args as DeathAction;
		if (GetComponentInParent<PlayerView> ().player.index == action.card.ownerIndex)
			action.perform.viewer = ReapMinion;
	}

	//Invoca un esbirro, elimina la vista de carta y sitúa la vista de esbirro en la mesa
	public IEnumerator SummonMinion (IContainer game, GameAction action) {
		var summon = action as SummonMinionAction;
		var playerView = GetComponentInParent<PlayerView> ();
		var cardView = playerView.hand.GetView (summon.minion);
		playerView.hand.Dismiss (cardView);
		StartCoroutine(playerView.hand.LayoutCards (true));

		var minionView = minionPooler.Dequeue ().GetComponent<MinionView> ();
		minionView.transform.ResetParent (transform);
		minions.Add (minionView);
		minionView.gameObject.SetActive (true);

		minionView.Display (summon.minion);
		var pos = GetComponentInParent<PlayerView> ().hand.activeHandle.position;
		minionView.transform.position = pos;

		//Animación de rebotar
		var tweener = LayoutMinions();
		tweener.duration = 0.5f;
		tweener.equation = EasingEquations.EaseOutBounce;
		while (tweener != null)
			yield return null;
	}

	public IEnumerator ReapMinion (IContainer game, GameAction action) {
		var reap = action as DeathAction;
		var view = GetMatch (reap.card);
		view.transform.ScaleTo (Vector3.zero);
		minions.Remove (view.GetComponent<MinionView> ());

		var tweener = LayoutMinions ();
		while (tweener != null)
			yield return null;

		view.SetActive (false);
		view.transform.localScale = Vector3.one;
		minionPooler.Enqueue(view.GetComponent<Poolable> ());
	}

	public GameObject GetMatch (Card card) {
		for (int i = minions.Count - 1; i >= 0; --i) {
			if (minions [i].minion == card)
				return minions [i].gameObject;
		}
		return null;
	}

	//Ajusta la posición dependiendo de la cantidad de esbirros en la mesa
	Tweener LayoutMinions () {
		var xPos = (minions.Count / -2f) + 0.5f;
		Tweener tweener = null;
		for (int i = 0; i < minions.Count; ++i) {
			tweener = minions [i].transform.MoveToLocal (new Vector3 (xPos, 0, 0), 0.25f);
			xPos += 1;
		}
		return tweener;
	}
}