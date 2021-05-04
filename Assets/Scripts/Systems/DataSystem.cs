using Atonement.AspectContainer;

//Clase que contiene toda la partida
public class DataSystem : Aspect {
	public Match match = new Match();
}

public static class DataSystemExtensions {
	public static Match GetMatch (this IContainer game) {
		var dataSystem = game.GetAspect<DataSystem> ();
		return dataSystem.match;
	}
}