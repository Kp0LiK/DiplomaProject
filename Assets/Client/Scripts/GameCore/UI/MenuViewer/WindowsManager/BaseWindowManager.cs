using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    public abstract class BaseWindowManager : MonoBehaviour
    {
        [field: SerializeField] public BaseWindow[] Windows { get; private set; }
    
        public BaseWindow CurrentWindow { get; set; }
    
        public Stack<BaseWindow> QueueWindow { get; set; }
            = new Stack<BaseWindow>();

        protected virtual void Start()
        {
            CurrentWindow = Windows[0];
            OpenWindow(CurrentWindow);
        }
    
        public void OpenWindow<T>() where T : BaseWindow
        {
            var window = Windows.FirstOrDefault(w => w is T);
            OpenWindow(window);
        }
    
        public void OpenWindow(BaseWindow window)
        {
            if (!ReferenceEquals(window, null))
                window.Open();
            else
                return;
    
            CurrentWindow = window;
            QueueWindow.Push(window);
    
        }
    
        public void CloseWindow<T>() where T : BaseWindow
        {
            var window = Windows.FirstOrDefault((w => w is T));
            if (!ReferenceEquals(window, null)) CloseWindow(window);
        }
    
        public void CloseWindow(BaseWindow window)
        {
            window.Close();
        }
    
        public void BackWindow()
        {
            if (QueueWindow.Count > 1)
            {
                var current = QueueWindow.Pop();
                var last = QueueWindow.First();
                current.Close();
                last.Open();
            }
        }
    
        public void CloseAllWindows()
        {
            if (!ReferenceEquals(Windows, null))
                foreach (var w in Windows)
                    w.Close();
        }
    
        [Button]
        private void InitWindows() => Windows = GetComponentsInChildren<BaseWindow>(true);
    }
}

