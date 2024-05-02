using UnityEngine;

namespace Tools.ResourceLoader.Configs
{
	[CreateAssetMenu(fileName = "ResourceConfigSprite.asset", menuName = "MyAsteroids/Configs/Create ResourceConfigSprite")]
	public class ResourceConfigSprite : ScriptableObject
	{
		public Sprite[] sprites;
		public Sprite[] atlases;
	}
}