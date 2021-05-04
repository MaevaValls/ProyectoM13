using Atonement.AspectContainer;

public static class GameFactory {

	public static Container Create () {
		Container game = new Container ();

		//Añade sistemas
		game.AddAspect<AbilitySystem> ();
		game.AddAspect<ActionSystem> ();
		game.AddAspect<AttackSystem> ();
		game.AddAspect<CardSystem> ();
		game.AddAspect<CombatantSystem> ();
		game.AddAspect<DataSystem> ();
		game.AddAspect<DeathSystem> ();
		game.AddAspect<DestructableSystem> ();
		game.AddAspect<EnemySystem> ();
		game.AddAspect<ManaSystem> ();
		game.AddAspect<MatchSystem> ();
		game.AddAspect<MinionSystem> ();
		game.AddAspect<PlayerSystem> ();
		game.AddAspect<SpellSystem> ();
		game.AddAspect<TargetSystem> ();
		game.AddAspect<TauntSystem> ();
		game.AddAspect<VictorySystem> ();

		//Añade estados
		game.AddAspect<StateMachine> ();
		game.AddAspect<GlobalGameState> ();

		return game;
	}
}