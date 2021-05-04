using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Atonement.Notifications;

public class ManaView : MonoBehaviour {
	public List<Image> slots;
	public Sprite available;
	public Sprite unavailable;
	public Sprite slot;

	void OnEnable () {
		this.AddObserver (OnManaValueChangedNotification, ManaSystem.ValueChangedNotification);
	}

	void OnDisable () {
		this.RemoveObserver (OnManaValueChangedNotification, ManaSystem.ValueChangedNotification);
	}

	//Cambia la apariencia de la barra de maná dependiendo de los valores
	void OnManaValueChangedNotification (object sender, object args) {
		var mana = args as Mana;
		for (int i = 0; i < mana.Available; ++i) {
			SetSpriteForImageSlot (available, i);
		}
		for (int i = mana.Available; i < mana.Unlocked; ++i) {
			SetSpriteForImageSlot (unavailable, i);
		}
		for (int i = mana.Unlocked; i < Mana.MaxSlots; ++i) {
			SetSpriteForImageSlot (slot, i);
		}
	}

	//Cambia la imagen de cada espacio en la barra
	void SetSpriteForImageSlot(Sprite sprite, int slotIndex) {
		if (slotIndex >= 0 && slotIndex < slots.Count)
			slots [slotIndex].sprite = sprite;
	}
}