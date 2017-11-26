using UnityEngine;
using UnityEngine.UI;

namespace UIFramework {
	[CreateAssetMenu]
	public class ButtonStyleData : ScriptableObject {

		public ColorBlock colors;
		[Range(1,5)]
		public float scaleMultiplier=1.1f;
	}	
}