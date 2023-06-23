using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class InGameMenuCommand : BaseCommand
    {
        private readonly Canvas _inGameMenu;
        private readonly Canvas _playerViewerMenu;

        private readonly float _defaultTime;

        public InGameMenuCommand(Canvas inGameMenu, Canvas playerViewerMenu)
        {
            _inGameMenu = inGameMenu;
            _playerViewerMenu = playerViewerMenu;

            _defaultTime = Time.timeScale;
        }

        public override void Execute()
        {
            Time.timeScale = 0;
            _inGameMenu.gameObject.SetActive(true);
            _playerViewerMenu.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            Time.timeScale = _defaultTime;
            _inGameMenu.gameObject.SetActive(false);
            _playerViewerMenu.gameObject.SetActive(true);
        }
    }
}
