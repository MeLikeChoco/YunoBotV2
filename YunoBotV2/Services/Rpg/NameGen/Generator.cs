using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.Rpg.NameGen;

namespace YunoBotV2.Services.Rpg.NameGen
{
    public static class Generator
    {

        /// <summary>
        /// Generates a given amount of names
        /// </summary>
        /// <param name="name">The name of the generator</param>
        /// <param name="amount">The amount of names to generate</param>
        /// <param name="type">The type of names to generate</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<string> GenerateNames(string name, int amount, int type)
        {

            if (amount > 10 || amount < 1)
                throw new ArgumentOutOfRangeException("amount", amount, "Please ask for at least 1 name and no more than 10 at a time!");

            var generator = Assembly.GetEntryAssembly()
                .GetTypes()
                .FirstOrDefault(t => (t.GetTypeInfo().GetCustomAttribute<GenNameAttribute>() as GenNameAttribute)?.Name == name);

            if (generator == null)
                return null;

            IGenerator names = Activator.CreateInstance(generator, new object[] { amount, type }) as IGenerator;

            return names.GeneratedResults;

        }

    }
}
