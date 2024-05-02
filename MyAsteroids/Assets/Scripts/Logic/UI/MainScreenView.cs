﻿using Root.Core;
using TMPro;
using Tools.MyFramework;
using UnityEngine;

namespace Logic.UI
{
    public class MainScreenView : BaseMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _score;
        
        [SerializeField]
        private TextMeshProUGUI _speed;
        
        [SerializeField]
        private TextMeshProUGUI _angle;
        
        [SerializeField]
        private TextMeshProUGUI _posX;
        
        [SerializeField]
        private TextMeshProUGUI _posY;
        
        [SerializeField]
        private RectTransform _laserChargeHolder;

        public TextMeshProUGUI Score => _score;
        public TextMeshProUGUI Speed => _speed;
        public TextMeshProUGUI Angle => _angle;
        public TextMeshProUGUI PosX => _posX;
        public TextMeshProUGUI PosY => _posY;
        public RectTransform LaserChargeHolder => _laserChargeHolder;
    }
}