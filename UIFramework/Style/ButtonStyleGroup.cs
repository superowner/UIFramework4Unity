using UnityEngine;
using UnityEngine.UI;

namespace UIFramework {
	public class ButtonStyleGroup : ButtonStyleBase {

		private Button[] button;

		[ContextMenu("SetStyleGroup")]
		public override void SetNewStyle(){
			base.SetNewStyle();
			button = gameObject.GetComponentsInChildren<Button>(true);
			foreach (var item in button) {
				if(item.GetComponent<ButtonStyle>()) continue;
				item.colors = data.colors;
				Button2 b2 = item as Button2;
				if(b2){
					b2.scaleMultiplier = data.scaleMultiplier;
				}
			}
		}
	}	
}