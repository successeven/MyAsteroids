﻿using System.Collections.Generic;
using Logic.Entities;
using Logic.Entities.Core;
using Root.Core;
using Tools.MyFramework;
using UnityEngine;

namespace Logic.Scene
{
    public class EntitiesControllerPm : BaseDisposable, IEntitiesController
    {
        public struct Ctx
        {
        }

        public IReadOnlyDictionary<int, EntityInfo> AllEntities => _entities;
        public PlayerModel _playerModel;
        
        public EntitiesControllerPm(Ctx ctx)
        {
            _ctx = ctx;
            _entities = new Dictionary<int, EntityInfo>();
        }
        public bool TryGetEntityInfo(int id, out EntityInfo entityInfo)
        {
            return _entities.TryGetValue(id, out entityInfo);
        }
        public bool TryDestroyEntity(int id, int? killer = null)
        {
            if (!TryGetEntityInfo(id, out var entityInfo))
            {
                Debug.LogError($"Dont find entity with id = {id}");
                return false;
            }
            if (killer != null & killer == _playerModel.Id)
                _playerModel.Score.Value += entityInfo.Model.Reward;
            
            entityInfo?.Model.Destroy(killer);
            entityInfo?.Logic?.Dispose();
            return _entities.Remove(id);
        }
        public PlayerModel GetPlayerModel()
        {
            return _playerModel;
        }

        private readonly Ctx _ctx;
        private readonly Dictionary<int, EntityInfo> _entities;
        private int _indexator;


        public int GenerateId()
        {
            return _indexator++;
        }
        public void AddEntity(int Id, EntityInfo entityInfo)
        {
            _entities.Add(Id, entityInfo);
            if (entityInfo.Model.EntityType is EntityType.PlayerShip)
                _playerModel = (PlayerModel) entityInfo.Model;
        }
        protected override void OnDispose()
        {
            foreach (var entity in _entities)
                entity.Value.Logic?.Dispose();
            
            base.OnDispose();
        }

    }
}