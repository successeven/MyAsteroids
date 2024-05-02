using Root.Core.RX;

namespace Logic.Entities.Core
{
    public class AsteroidModel : BaseModel
    {
        public ReactiveProperty<bool> CanCollapse;

        public AsteroidModel() : base()
        {
            CanCollapse = new ReactiveProperty<bool>(true);
        }

    }
}