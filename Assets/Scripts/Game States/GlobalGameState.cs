using Atonement.AspectContainer;
using Atonement.Notifications;

public class GlobalGameState : Aspect, IObserve {

	public void Awake () {
		this.AddObserver (OnBeginSequence, ActionSystem.beginSequenceNotification);
		this.AddObserver (OnCompleteAllActions, ActionSystem.completeNotification);
	}

	public void Destroy () {
		this.RemoveObserver (OnBeginSequence, ActionSystem.beginSequenceNotification);
		this.RemoveObserver (OnCompleteAllActions, ActionSystem.completeNotification);
	}

	//Cuando la secuencia se inicia cambia el estado a secuencia
	void OnBeginSequence (object sender, object args) {
		container.ChangeState<SequenceState> ();
	}

	//Cuando la secuencia finaliza cambia el estado a Idle o a partida terminada
	void OnCompleteAllActions (object sender, object args) {
		if (container.GetAspect<VictorySystem> ().IsGameOver ()) {
			container.ChangeState<GameOverState> ();
		} else {
			container.ChangeState<PlayerIdleState> ();
		}
	}
}