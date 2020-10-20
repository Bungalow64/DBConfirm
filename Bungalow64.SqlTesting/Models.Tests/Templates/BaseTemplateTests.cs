using Models.Data;
using Models.Templates;
using NUnit.Framework;
using System.Collections.Generic;

namespace Models.Tests.Templates
{
    [TestFixture]
    public class BaseTemplateTests
    {
        public class TestIdentityTemplate : BaseSimpleTemplate<TestIdentityTemplate>
        {
            public override string TableName => "dbo.Users";

            public override DataSetRow DefaultData => new DataSetRow
            {
                { "DefaultColumnA", 9001 }
            };

            public TestIdentityTemplate() : base() { }

            public TestIdentityTemplate(DataSetRow data) : base(data) { }
        }

        [Test]
        public void BaseTemplate_CustomData_RetrieveDataSetDuringSetup()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 },
                { "UserId", 2001 }
            };

            Assert.AreEqual(2001, template.CustomData["UserId"]);
        }

        [Test]
        public void BaseTemplate_DefaultCtor()
        {
            TestIdentityTemplate template = new TestIdentityTemplate();

            Assert.AreEqual(0, template.CustomData.Count);
        }

        [Test]
        public void BaseTemplate_Ctor_CanInstantiateFromExistingTemplate()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 },
                { "UserId", 2001 }
            };

            TestIdentityTemplate newTemplate = new TestIdentityTemplate(template);

            Assert.AreEqual(1001, newTemplate.CustomData["DomainId"]);
            Assert.AreEqual(2001, newTemplate.CustomData["UserId"]);
            Assert.AreEqual(9001, newTemplate.DefaultData["DefaultColumnA"]);
        }

        [Test]
        public void BaseTemplate_MergedDate_NoCustomData_ReturnDefaultOnly()
        {
            TestIdentityTemplate template = new TestIdentityTemplate();

            Assert.AreEqual(9001, template.MergedData["DefaultColumnA"]);
        }

        [Test]
        public void BaseTemplate_MergedDate_HasCustomData_ReturnDefaultAndCustom()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DomainId", 1001 },
                { "UserId", 2001 }
            };

            Assert.AreEqual(1001, template.MergedData["DomainId"]);
            Assert.AreEqual(2001, template.MergedData["UserId"]);
            Assert.AreEqual(9001, template.MergedData["DefaultColumnA"]);
        }

        [Test]
        public void BaseTemplate_MergedDate_HasCustomDataOverwritingDefaultData_ReturnCustom()
        {
            TestIdentityTemplate template = new TestIdentityTemplate
            {
                { "DefaultColumnA", 3001 }
            };

            Assert.AreEqual(3001, template.MergedData["DefaultColumnA"]);
        }
    }
}
