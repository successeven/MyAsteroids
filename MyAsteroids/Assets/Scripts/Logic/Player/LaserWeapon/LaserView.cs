using Logic.Entities;
using Root.Core;
using UnityEngine;

namespace Logic.Player.LaserWeapon
{
    public class LaserView : BaseView
    {
        [SerializeField]
        private LineRenderer _laser;

        public LineRenderer Laser => _laser;
    }
}