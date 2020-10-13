using Models.Templates;
using NUnit.Framework;
using System.Collections.Generic;

namespace Models.Tests.Templates
{
    [TestFixture]
    public class BaseIdentityTemplateTests
    {
        public class TestIdentityTemplate : BaseIdentityTemplate
        {
            public override string IdentityColumnName => "UserId";

            public override string TableName => "dbo.Users";

            public override DataSetRow DefaultData => new DataSetRow();
        }

        [Test]
        public void BaseIdentityTemplate_Identity_RetrieveCorrectValue()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 },
                { "UserId", 2001 }
            };

            Assert.AreEqual(2001, template.Identity);
        }

        [Test]
        public void BaseIdentityTemplate_IdentityColumnNotSet_Error()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 }
            };

            var exception = Assert.Throws<KeyNotFoundException>(() => { int value = template.Identity; });

            Assert.AreEqual("UserId was not found in the data set", exception.Message);
        }

        [Test]
        public void BaseIdentityTemplate_IdentityResolver_RetrieveCorrectValue()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 },
                { "UserId", 2001 }
            };

            Resolver<int> resolver = template.IdentityResolver;
            Assert.IsNotNull(resolver);

            Assert.AreEqual(2001, resolver.Resolve());
        }

        [Test]
        public void BaseIdentityTemplate_IdentityColumnNotSet_IdentityResolver_ErrorDuringResolveOnly()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 }
            };

            Resolver<int> resolver = template.IdentityResolver;
            Assert.IsNotNull(resolver);

            var exception = Assert.Throws<KeyNotFoundException>(() => { object value = resolver.Resolve(); });

            Assert.AreEqual("UserId was not found in the data set", exception.Message);
        }
    }
}
