using Atonement.AspectContainer;
using Atonement.Notifications;

public class MatchSystem : Aspect, IObserve {

	//Cambia el turno siguiendo el orden natural
	public void ChangeTurn () {
		var match = container.GetMatch ();
		var nextIndex = (1 - match.currentPlayerIndex);
		ChangeTurn (nextIndex);
	}

	//Cambia el turno recibiendo un index
	public void ChangeTurn (int index) {
		var action = new ChangeTurnAction (index);
		container.Perform (action);
	}

	public void Awake () {
		this.AddObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
	}

	//Aplica el cambio de turno a la partida
	void OnPerformChangeTurn (object sender, object args) {
		var action = args as ChangeTurnAction;
		var match = container.GetMatch ();
		match.currentPlayerIndex = action.targetPlayerIndex;
	}
}