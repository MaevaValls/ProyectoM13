using Atonement.AspectContainer;
using Atonement.Notifications;

public class PlayerIdleState : BaseState {
	public const string EnterNotification = "PlayerIdleState.EnterNotification";
	public const string ExitNotification = "PlayerIdleState.ExitNotification";

	//Recibe los sistemas que tiene el jugador cuando está en estado Idle
	public override void Enter () {
		var mode = container.GetMatch ().CurrentPlayer.mode;
		container.GetAspect<AttackSystem> ().Refresh ();
		container.GetAspect<CardSystem> ().Refresh (mode);
		//Si modo de control es IA realiza las acciones automáticamente
		if (mode == ControlModes.Computer)
			container.GetAspect<EnemySystem> ().TakeTurn ();
		this.PostNotification (EnterNotification);
	}

	public override void Exit () {
		this.PostNotification (ExitNotification);
	}
}