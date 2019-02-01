using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IntegrationTests.Infrastructure
{
    /// <summary>
    ///     Provides runnable integration tests with the required infrastructure.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        ///     Gets the local test types.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetLocalTestTypes()
        {
            var testType = typeof(TestBase);
            var assembly = Assembly.GetAssembly(testType);
            return assembly.GetTypes().Where(t => t.IsSubclassOf(testType));
        }

        /// <summary>
        ///     Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static TestBase CreateInstance(Type type)
        {
            if (!type.IsSubclassOf(typeof(TestBase)))
            {
                throw new ArgumentException("Given type does not derive from " + nameof(TestBase));
            }

            return (TestBase) Activator.CreateInstance(type);
        }

        /// <summary>
        ///     Creates the instance.
        /// </summary>
        /// <typeparam name="TTestType">The type of the test type.</typeparam>
        /// <returns></returns>
        public static TTestType CreateInstance<TTestType>() where TTestType : TestBase
        {
            return Activator.CreateInstance<TTestType>();
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public abstract void Run();
    }
}
