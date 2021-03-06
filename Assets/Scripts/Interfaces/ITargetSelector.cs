using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Atonement.AspectContainer;

public interface ITargetSelector : IAspect {
	List<Card> SelectTargets (IContainer game);
	void Load(Dictionary<string, object> data);
}
