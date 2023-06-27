using System;
using UnityEngine;

namespace Client
{
    public class AboutWindow : BaseWindow
    {
        [SerializeField] private StartPanel _startPanel;
        [SerializeField] private ExitPanel _exitPanel;
        private void Update()
        {
            _startPanel.gameObject.SetActive(false);
            _exitPanel.gameObject.SetActive(false);
        }
    }
}

