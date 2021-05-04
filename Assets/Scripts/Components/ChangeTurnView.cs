using System.Collections;
using UnityEngine;
using Atonement.AspectContainer;
using Atonement.Notifications;
using Atonement.Animation;

public class ChangeTurnView : MonoBehaviour {
	[SerializeField] Transform yourTurnBanner;
	[SerializeField] ChangeTurnButtonView buttonView;
	IContainer game;

	//Cambia de turno al pulsar el botón
	public void ChangeTurnButtonPressed () {
		if (CanChangeTurn ()) {
			var system = game.GetAspect<MatchSystem> ();
			system.ChangeTurn ();
		} 
	}

	//Comprueba si se puede cambiar el turno
	bool CanChangeTurn () {
		var stateMachine = game.GetAspect<StateMachine> ();
		if (!(stateMachine.currentState is PlayerIdleState))
			return false;

		var player = game.GetMatch ().CurrentPlayer;
		if (player.mode != ControlModes.Local)
			return false;

		return true;
	}

	void Awake () {
		game = GetComponentInParent<GameViewSystem> ().container;
	}

	void OnEnable () {
		this.AddObserver (OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction> (), game);
	}

	void OnDisable () {
		this.RemoveObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction> (), game);
	}

	void OnPrepareChangeTurn (object sender, object args) {
		var action = args as ChangeTurnAction;
		action.perform.viewer = ChangeTurnViewer;
	}

	//Gestiona las animaciones de cambio de turno
	IEnumerator ChangeTurnViewer (IContainer game, GameAction action) {
		var dataSystem = game.GetAspect<DataSystem> ();
		var changeTurnAction = action as ChangeTurnAction;
		var targetPlayer = dataSystem.match.players [changeTurnAction.targetPlayerIndex];

		var banner = ShowBanner (targetPlayer);
		var button = FlipButton (targetPlayer);
        bool isAnimating;
        do {
			var bannerOn = banner.MoveNext ();
			var buttonOn = button.MoveNext ();
			isAnimating = bannerOn || buttonOn;
			yield return null;
		} while (isAnimating);
	}

	//Muestra el texto de 'Your Turn' animado en el turno del jugador
	IEnumerator ShowBanner (Player targetPlayer) {
		if (targetPlayer.mode != ControlModes.Local)
			yield break;

		var tweener = yourTurnBanner.ScaleTo(Vector3.one, 0.25f, EasingEquations.EaseOutBack);
		while (tweener.IsPlaying) { yield return null; }

		tweener = yourTurnBanner.Wait (1f);
		while (tweener.IsPlaying) { yield return null; }

		tweener = yourTurnBanner.ScaleTo (Vector3.zero, 0.25f, EasingEquations.EaseInBack);
		while (tweener.IsPlaying) { yield return null; }
	}

	//Gira el botón dependiendo del turno
	IEnumerator FlipButton (Player targetPlayer) {
		var up = Quaternion.identity;
		var down = Quaternion.Euler (new Vector3(180, 0, 0));
		var targetRotation = targetPlayer.mode == ControlModes.Local ? up : down;
		var tweener = buttonView.rotationHandle.RotateTo(targetRotation, 0.5f, EasingEquations.EaseOutBack);
		while (tweener.IsPlaying) { yield return null; }
	}
}