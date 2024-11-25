using System.Linq;

namespace YogiGameCore.YogiGameCore.Utils.Factory
{
    internal abstract class AbilityBase : IFactoryProduct
    {
        public abstract string Name { get; }
        public virtual void Process()
        {
        }
    }
    internal static class AbilityFactory
    {
        static CommonFactory<AbilityBase> factory;
        public static void InitializeFactory()
        {
            if (factory != null)
                return;
            factory = new CommonFactory<AbilityBase>();
        }
        public static T GetAbility<T>(string name) where T : AbilityBase
        {
            InitializeFactory();
            return factory.GetProductByName<T>(name);
        }
        public static string[] GetAllAbilityNames()
        {
            InitializeFactory();
            return factory.GetNames().ToArray();
        }
    }
    internal class FireAbility : AbilityBase
    {
        private readonly string _name = "Fire";
        public override string Name { get => _name; }
        public override void Process()
        {
            base.Process();
            //Fire
        }
    }
    internal class HealingAbility : AbilityBase
    {
        private readonly string _name = "Heal";
        public override string Name { get => _name; }
        public override void Process()
        {
            base.Process();
            //self.hp++
        }
    }
    internal class TestMain
    {
        void Start()
        {
            var fireSkill = AbilityFactory.GetAbility<AbilityBase>("Fire");
            var healSkill = AbilityFactory.GetAbility<HealingAbility>("Heal");

            fireSkill.Process();
            healSkill.Process();

            //all ability Names
            var names = AbilityFactory.GetAllAbilityNames();
        }
    }
}
