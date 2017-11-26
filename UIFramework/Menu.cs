using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
namespace UIFramework {
	public class Menu : MonoBehaviour {

		public GameObject firstSelected;

		public Button[] transitionButtons;
		public UnityEvent onEnter,onEntered,onLeave,onLeft;

		public void Disable(){
			onLeave.Invoke ();
			gameObject.SetActive(false);
			onLeft.Invoke ();
		}

		public void Enable(){
			onEnter.Invoke ();
			gameObject.SetActive(true);
			if(EventSystem.current)
				EventSystem.current.SetSelectedGameObject(firstSelected);
			onEntered.Invoke ();
		}
	}	
}