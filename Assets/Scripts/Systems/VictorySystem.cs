using Atonement.AspectContainer;

public class VictorySystem : Aspect {
	public bool IsGameOver () {
		//Comprueba si la vida de los jugadores és igual o menor a 0
		var match = container.GetMatch ();
		foreach (Player p in match.players) {
			Hero h = p.hero [0] as Hero;
			if (h.hitPoints <= 0) {
				return true;
			}
		}
		return false;
	}
}
