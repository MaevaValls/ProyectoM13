using System.Collections.Generic;

public class Match {
	public const int PlayerCount = 2;

	//Lista de dos jugadores
	public List<Player> players = new List<Player> (PlayerCount);
	public int currentPlayerIndex;

	//Jugador
	public Player CurrentPlayer {
		get {
			return players [currentPlayerIndex]; 
		}
	}

	//Oponente
	public Player OpponentPlayer {
		get {
			return players [1 - currentPlayerIndex];
		}
	}

	public Match () {
		//Añade a los dos jugadores a uan partida
		for (int i = 0; i < PlayerCount; ++i) {
			players.Add (new Player (i));
		}
	}
}