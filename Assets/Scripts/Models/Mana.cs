using UnityEngine;

public class Mana {
	public const int MaxSlots = 10;

	public int spent;
	public int permanent;
	public int temporary;

	public int Unlocked {
		get {
			//Devuelve el número menor entre el maná perma+temp y el max
			return Mathf.Min (permanent + temporary, MaxSlots);
		}
	}

	public int Available {
		get {
			//Devuelve el número menor entre el maná perma+temp-gastado y el max
			return Mathf.Min (permanent + temporary - spent, MaxSlots);
		}
	}
}