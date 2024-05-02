using System;
using UnityEngine;

namespace Tools.ResourceLoader
{
	public interface IResourceLoader: IDisposable
	{
		// загрузка префаба
		void LoadPrefab(string prefabName, Action<GameObject> onComplete);

		// загрузка картинки
		void LoadSprite(string spriteName, Action<Sprite> onComplete);

	}
}