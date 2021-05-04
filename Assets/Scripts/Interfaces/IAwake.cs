using Atonement.AspectContainer;

public interface IAwake {
	void Awake();
}

public static class AwakeExtensions {
	//Añade la interfaz IAwake a todos los Aspects e invoca el método
	public static void Awake (this IContainer container) {
		foreach (IAspect aspect in container.Aspects()) {
			var item = aspect as IAwake;
			if (item != null)
				item.Awake ();
		}
	}
}