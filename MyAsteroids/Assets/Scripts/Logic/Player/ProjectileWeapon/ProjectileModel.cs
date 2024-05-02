using Logic.Entities.Core;
using Root.Core.RX;

namespace Logic.Player.ProjectileWeapon
{
    public class ProjectileModel : BaseModel
    {
        public int OwnerId;
        public ReactiveProperty<float> Speed;
    }
}