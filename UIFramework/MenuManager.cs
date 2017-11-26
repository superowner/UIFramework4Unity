using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UIFramework {
	public class MenuManager : MonoBehaviour {

		public MenuDesign menuConf;

		[Tooltip("Destroy current menus gameobjects and add prefabs again")]
		public bool createOnAwake = true;

		[HideInInspector,SerializeField]
		public List<Menu> menus;
		[HideInInspector]
		public List<Transition> transitions= new List<Transition>();
		[HideInInspector]
		public Menu firstMenu;

		private Menu menuEditor;

		private Menu currentMenu;

		public class Transition{

			public Button button;
			public Menu toMenu;

			private MenuManager manager;

			public Transition(Connection data,List<Menu> menus){
				LinkTransition(menus,data);
			}

			public void AddListener(MenuManager m){
				manager = m;
				button.onClick.AddListener(Enter);
			}

			public void Enter(){
				manager.TransitionTo(toMenu);
			}

			public void LinkTransition(List<Menu> menus,Connection data){
				for (int i = 0; i < menus.Count; i++) {
					if(menus[i].name == data.menuOrigin.name){
						button = menus[i].transitionButtons[data.buttonOut];
					}else if(menus[i].name == data.menuTarget.name){
						toMenu = menus[i];
					}
				}
			}
		}

		void Awake(){
			if(menus.Count<=0 || createOnAwake)
				CreateMenu();
		}	

		void Start() {
			Init();
		}

		[ContextMenu("Create Menu")]
		public void CreateMenu(){
			foreach (var item in menus) {
				if(item != null) {
					#if UNITY_EDITOR
					if(Application.isPlaying){
						Destroy(item.gameObject);
					}else{
						DestroyImmediate(item.gameObject);
					}
					#else
					Destroy(item.gameObject);
					#endif
				}
			}
			menus.Clear();
			foreach (var item in menuConf.menuPrefabs) {
				#if UNITY_EDITOR
				if(Application.isPlaying){
					Menu m = Instantiate<Menu>(item,transform);
					m.name = item.name;
					menus.Add(m);
				}else{
					Menu m = PrefabUtility.InstantiatePrefab(item) as Menu;
					m.transform.SetParent(transform,false);
					menus.Add(m);
				}
				#else
				Menu m = Instantiate<Menu>(item,transform);
				m.name = item.name;
				menus.Add(m);
				#endif
			}
		}

		void LinkTransitions(){
			transitions.Clear();
			foreach (var item in menuConf.connections) {
				transitions.Add(new Transition(item,menus));
			}
		}

		public void Init(){
			firstMenu = menus[0];
			LinkTransitions();
			currentMenu = firstMenu;
			currentMenu.Enable();
			for (int i = 0; i < menus.Count; i++) {
				if(menus[i].Equals(currentMenu)) continue;
				menus[i].Disable();
			}
			for (int i = 0; i < transitions.Count; i++) {
				transitions[i].AddListener(this);
			}
		}

		public void TransitionTo(Menu m){
			currentMenu.Disable ();
			currentMenu = m;
			currentMenu.Enable ();
		}

		#if UNITY_EDITOR
		public void OnDrawGizmos(){
			if(menus.Count <=0) return;
			Menu m = null;
			if(Selection.activeGameObject)
				m = Selection.activeGameObject.GetComponent<Menu>();
			if(m) menuEditor = m;
			if(menuEditor == null && menus[0] != null)
				menuEditor = menus[0];
			foreach (var item in menus) {
				if(item == null) continue;
				if(item.Equals(menuEditor)){
					item.Enable();
				}else{
					item.Disable();
					EditorUtility.SetDirty(item.gameObject);
				}
			}
		}
		public void ApplyPrefabs(){
			for (int i = 0; i < menus.Count; i++) {
				if(!menus[i]) continue;
				PrefabUtility.ReplacePrefab(menus[i].gameObject,menuConf.menuPrefabs[i]);
			} 
		}
		#endif
	}	
}