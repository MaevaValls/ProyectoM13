﻿using System;

[Flags]
//Bit mask que asigna el valor en una posición diferente para cada elemento
public enum Alliance {
	None = 0,
	Ally = 1 << 0,
	Enemy = 1 << 1,
	Any = Ally | Enemy
}

public static class AllianceExtensions {
	public static bool Contains (this Alliance source, Alliance target) {
		return (source & target) == target;
	}
}