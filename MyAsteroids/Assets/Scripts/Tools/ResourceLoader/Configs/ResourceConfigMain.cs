using UnityEngine;

namespace Tools.ResourceLoader.Configs
{
	[CreateAssetMenu(fileName = "ResourceConfigMain.asset", menuName = "MyAsteroids/Configs/Create ResourceConfigMain")]
	public class ResourceConfigMain : ScriptableObject
	{
		public ResourceConfig[] contentConfigs;
		public ResourceConfigSprite[] spriteConfigs;
	}
}