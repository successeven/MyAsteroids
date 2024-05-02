﻿using System;
using Logic.Entities.Core;
using Root.Core;
using Tools.MyFramework;
using UnityEngine;

namespace Logic.Entities
{
    public class BaseView : BaseMonoBehaviour, IEntityView
    {
        public struct Ctx
        {
            public BaseModel model;
        }

        protected Ctx _ctx;
        public BaseModel Model => _ctx.model;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            AfterSetCtx();
        }

        protected virtual void AfterSetCtx()
        {
            
        }

        public event Action<CollidedInfo> Collided;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ( other.gameObject.TryGetComponent<IEntityView>( out var view ) )
            {
                var info = new CollidedInfo()
                {
                    ownerId = _ctx.model.Id,
                    defenderId = view.Model.Id
                };

                Collided?.Invoke( info );
            }
        }

    }
}