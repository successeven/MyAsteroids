using Logic.Settings;
using Root.Loaders.SceneLoad.View;
using UnityEngine.Serialization;

namespace Logic.Scene
{
	public class MainSceneContextView : SceneContextView
	{

		public PlayerSettings PlayerSettings;
		public GameSettings GameSettings;
		public AsteroidSettings AsteroidSettings;
		public UFOSettings UFOSettings;
		public ProjectileSettings ProjectileSettings;
		public LaserSettings laserSettings;
		[FormerlySerializedAs("rewardSettings")]
		public RewardSettings RewardSettings;

	}
}