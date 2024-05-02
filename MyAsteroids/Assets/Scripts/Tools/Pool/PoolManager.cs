using System.Collections.Generic;
using Tools.MyFramework;
using UnityEngine;

namespace Tools.Pool
{
	public class PoolManager : BaseDisposable, IPoolManager
	{
		private Dictionary<GameObject, GameObjectPool> _pools;
		public struct Ctx
		{
		}

		private readonly Ctx _ctx;
		private readonly Transform _poolRoot;

		public PoolManager(Ctx ctx)
		{
			_ctx = ctx;
			GameObject poolObj = AddComponent(new GameObject($"Pools"));
			Object.DontDestroyOnLoad(poolObj);
			_poolRoot = poolObj.transform;
			_pools = new Dictionary<GameObject, GameObjectPool>();
		}

		private GameObjectPool CreateOrGetPool(GameObject prefab)
		{
			if (_pools.TryGetValue(prefab, out GameObjectPool existedPool))
				return existedPool;
			GameObject poolObj = AddComponent(new GameObject($"pool [{prefab.name}]"));
			poolObj.transform.SetParent(_poolRoot);
			GameObjectPool createdPool = new GameObjectPool(new GameObjectPool.Ctx
			{
					prefab = prefab,
					parrent = poolObj.transform
			});
			_pools.Add(prefab, createdPool);
			return createdPool;
		}

		public GameObject Get(GameObject prefab)
		{
			if (isDisposed)
				return null;
			GameObjectPool pool = CreateOrGetPool(prefab);
			return pool?.Get();
		}
		public GameObject Get(GameObject prefab, Vector3 position)
		{
			if (isDisposed)
				return null;
			GameObjectPool pool = CreateOrGetPool(prefab);
			return pool?.Get(position);
		}
		public GameObject Get(GameObject prefab, Transform parent)
		{
			if (isDisposed)
				return null;
			GameObjectPool pool = CreateOrGetPool(prefab);
			return pool?.Get(parent);
		}
		public GameObject Get(GameObject prefab, Vector3 position, float rotateDig)
		{
			if (isDisposed)
				return null;
			GameObjectPool pool = CreateOrGetPool(prefab);
			return pool?.Get(position, rotateDig);
		}

		public void Return(GameObject prefab, GameObject obj)
		{
			if (isDisposed)
				return;
			GameObjectPool pool = CreateOrGetPool(prefab);
			pool?.Return(obj);
		}

	}
}