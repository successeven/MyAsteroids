using System.Collections.Generic;
using Logic.Entities.Core;

namespace Logic.Entities
{
    public interface IEntitiesController
    {
        public int GenerateId();
        public void AddEntity(int Id, EntityInfo entityInfo);
        public IReadOnlyDictionary<int, EntityInfo> AllEntities { get; }

        public bool TryGetEntityInfo(int id, out EntityInfo entityInfo);

        public bool TryDestroyEntity(int id, int? killer = null);

        public PlayerModel GetPlayerModel();
    }
}