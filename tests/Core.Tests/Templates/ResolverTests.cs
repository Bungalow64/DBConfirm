using DBConfirm.Core.Templates;
using NUnit.Framework;
using System;

namespace SqlConfirm.Core.Tests.Templates
{
    [TestFixture]
    public class ResolverTests
    {
        [Test]
        public void Resolver_Ctor_FunctionIsSet()
        {
            Func<int> function = () => 123;

            Resolver<int> resolver = new Resolver<int>(function);

            Assert.AreEqual(function, resolver.Function);
        }

        [Test]
        public void Resolver_Ctor_NullFunction_Error()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Resolver<int>(null));

            Assert.IsNotNull(exception);
            Assert.AreEqual("Value cannot be null. (Parameter 'function')", exception.Message);
        }

        [Test]
        public void Resolver_Resolve_ReturnValue()
        {
            Func<int> function = () => 123;

            Resolver<int> resolver = new Resolver<int>(function);

            Assert.AreEqual(123, resolver.Resolve());
        }
    }
}
