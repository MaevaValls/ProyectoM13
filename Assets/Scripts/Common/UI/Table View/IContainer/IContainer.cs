using UnityEngine;
using System.Collections;

namespace Atonement.UI
{
	public interface IContainer
	{
		IFlow Flow { get; }
		void AutoSize ();
	}
}