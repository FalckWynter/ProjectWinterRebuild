using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace FlappyBird
{
    public class UISystem : AbstractSystem
    {
        private Transform canvasTrans;
        private Dictionary<string, GameObject> openedWindow = new Dictionary<string, GameObject>();
        protected override void OnInit()
        {
            canvasTrans = Object.FindObjectOfType<Canvas>().transform;
        }

        public void OpenUI(string name)
        {
            if(openedWindow.ContainsKey(name)) return;
            var go = Object.Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), canvasTrans);
            openedWindow[name] = go;
        }

        public void CloseUI(string name)
        {
            if(!openedWindow.TryGetValue(name, out var window)) return;
            Object.Destroy(window);
            openedWindow.Remove(name);
        }
        
    }
}