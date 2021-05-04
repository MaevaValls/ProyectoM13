
public class ChangeTurnAction : GameAction {
	public int targetPlayerIndex;

	//Cambia el turno del jugador al introducido
	public ChangeTurnAction (int targetPlayerIndex) {
		this.targetPlayerIndex = targetPlayerIndex;
	}
}