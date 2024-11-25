using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YogiGameCore.YogiGameCore.Utils.Factory
{
    public class CommonFactory<T> where T : IFactoryProduct
    {
        private Dictionary<string, Type> productsByName;
        public CommonFactory()
        {
            var productBaseType = typeof(T);
            var productsType = Assembly.GetAssembly(productBaseType)
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(productBaseType));
            productsByName = new Dictionary<string, Type>();
            foreach (var productType in productsType)
            {
                var productName = (Activator.CreateInstance(productType) as IFactoryProduct).Name;
                productsByName.Add(productName, productType);
            }
        }
        public virtual T GetProductByName<T>(string productName)
        {
            if (!productsByName.TryGetValue(productName, out var productType))
                return default(T);
            var instance = Activator.CreateInstance(productType);
            return (T)instance;
        }
        public virtual IEnumerable<string> GetNames()
        {
            return productsByName.Keys;
        }
    }
}
