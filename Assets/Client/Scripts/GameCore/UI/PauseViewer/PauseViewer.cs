using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
 public class PauseViewer : MonoBehaviour
 {
     [Scene, SerializeField] private string _firstScene;
     [SerializeField] private Button _continueButton;
     [SerializeField] private Button _tryAgainButton;
     [SerializeField] private Button _mainMenuButton;
 
     private CommandRecorder _commandRecorder;
     private GameSession _gameSession;
     private SceneLoader _sceneLoader;
 
     [Inject]
     public void Constructor(CommandRecorder commandRecorder, SceneLoader sceneLoader, GameSession gameSession)
     {
         _commandRecorder = commandRecorder;
         _gameSession = gameSession;
         _sceneLoader = sceneLoader;
     }
 
     private void Start() => gameObject.SetActive(false);
 
     private void OnEnable()
     {
         _continueButton.onClick.AddListener(OnContinueButton);
         _mainMenuButton.onClick.AddListener(OnMainMenuButton);
     }
 
     private void OnDisable()
     {
         _continueButton.onClick.RemoveListener(OnContinueButton);
         _mainMenuButton.onClick.RemoveListener(OnMainMenuButton);
     }
 
     private void OnContinueButton()
     {
         _commandRecorder.Rewind();
     }
     
 
     private void OnMainMenuButton()
     {
         _sceneLoader.LoadFullScene(_firstScene);
     }
 }   
}
