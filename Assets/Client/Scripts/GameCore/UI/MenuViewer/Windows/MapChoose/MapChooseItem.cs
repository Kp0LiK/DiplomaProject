using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class MapChooseItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Constructor(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        

        public void SetSceneLoader(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
    }
}
