﻿using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Aci.KeepYourDistance.ViewControllers
{
    public class ConsoleMessageViewController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_Label;
        
        [Zenject.Inject]
        public void Initialize(string sender, string message)
        {
            m_Label.text = $"{sender} ({DateTime.Now.ToString("HH:mm:ss")}): {message}";
        }

        public class Factory : PlaceholderFactory<string, string, ConsoleMessageViewController> { }
    }
}