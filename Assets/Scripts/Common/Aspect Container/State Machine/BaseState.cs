
namespace Atonement.AspectContainer
{
	public interface IState : IAspect {
		void Enter ();
		bool CanTransition (IState other);
		void Exit ();
	}

	public abstract class BaseState : Aspect, IState {
		//Estado actual
		public virtual void Enter () {}
		//Puede transicionar
		public virtual bool CanTransition (IState other) { return true; }
		//Salir del estado actual
		public virtual void Exit () {}
	}
}