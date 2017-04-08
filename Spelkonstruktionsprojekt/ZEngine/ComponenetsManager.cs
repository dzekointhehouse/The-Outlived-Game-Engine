using System;
using System.Collections.Generic;

namespace Systems
{
    public class ComponenetsManager
    {
        private readonly Dictionary<string, Type> components = new Dictionary<string, Type>()
        {
            { "Position", typeof(PositionComponent) }
        };

        public Type GetComponent(string componentName)
        {
            if (!components.ContainsKey(componentName))
            {
                throw new Exception("No such component.");
            }
            else
            {
                return components[componentName];
            }
        }
    }
}
