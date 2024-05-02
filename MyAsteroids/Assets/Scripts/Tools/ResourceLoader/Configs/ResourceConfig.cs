using UnityEngine;

namespace Tools.ResourceLoader.Configs
{
	[CreateAssetMenu(fileName = "ResourceConfig.asset", menuName = "MyAsteroids/Configs/Create ResourceConfig")]
	public class ResourceConfig : ScriptableObject
	{
		public GameObject[] prefabs;
	}
}