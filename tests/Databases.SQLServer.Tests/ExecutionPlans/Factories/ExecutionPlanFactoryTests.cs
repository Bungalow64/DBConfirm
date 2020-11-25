using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Databases.SQLServer.ExecutionPlans.Factories;
using SQLServer2017 = DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2017;
using SQLServer2019 = DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2019;
using FastMember;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace Databases.SQLServer.Tests.ExecutionPlans.Factories
{
    [TestFixture]
    public class ExecutionPlanFactoryTests
    {
        #region Init

        private Mock<ITestFramework> _testFrameworkMock;

        [SetUp]
        public void SetUp()
        {
            _testFrameworkMock = new Mock<ITestFramework>(MockBehavior.Strict);
        }

        private IExecutionPlan Build(DataSet dataSet) => new ExecutionPlanFactory().Build(_testFrameworkMock.Object, dataSet);

        private class DataClass
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public DataClass(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        private class ExecutionPlanClass
        {
            public string Xml { get; set; }

            public ExecutionPlanClass(string xmlFileName)
            {
                Xml = ReadResource(xmlFileName);
            }
        }

        private void LoadTable<T>(DataSet dataSet, params T[] data)
        {
            DataTable table = new DataTable();
            using (ObjectReader reader = ObjectReader.Create(data))
            {
                table.Load(reader);
            }
            dataSet.Tables.Add(table);
        }
        private static string ReadResource(string resourceName)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Databases.SQLServer.Tests.ExecutionPlans.Factories.SampleExecutionPlans.{resourceName}");
            using StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd().Replace(Environment.NewLine, "");
        }
        #endregion

        [Test]
        public void ExecutionPlanFactory_NullDataSet_ReturnNull()
        {
            DataSet dataSet = null;

            IExecutionPlan result = Build(dataSet);

            Assert.IsNull(result);
        }

        [Test]
        public void ExecutionPlanFactory_EmptyDataSet_ReturnNull()
        {
            DataSet dataSet = new DataSet();

            IExecutionPlan result = Build(dataSet);

            Assert.IsNull(result);
        }

        [Test]
        public void ExecutionPlanFactory_DataSetWithNoPlanTables_ReturnNull()
        {
            DataSet dataSet = new DataSet();
            LoadTable(dataSet, new DataClass(100, "Jamie"));

            IExecutionPlan result = Build(dataSet);

            Assert.IsNull(result);
        }

        [Test]
        public void ExecutionPlanFactory_DataSetWithMultipleDataTablesNoPlanTables_ReturnNull()
        {
            DataSet dataSet = new DataSet();
            LoadTable(dataSet, new DataClass(100, "Jamie"));
            LoadTable(dataSet, new DataClass(101, "Sarah"));

            IExecutionPlan result = Build(dataSet);

            Assert.IsNull(result);
        }

        [Test]
        public void ExecutionPlanFactory_DataSetWithSingle2017PlanTables_ReturnPlan()
        {
            DataSet dataSet = new DataSet();
            LoadTable(dataSet, new DataClass(100, "Jamie"));
            LoadTable(dataSet, new ExecutionPlanClass("ExecutionPlan2017_Sample1.xml"));

            IExecutionPlan result = Build(dataSet);

            Assert.IsInstanceOf<SQLServer2017.ExecutionPlan>(result);
        }

        [Test]
        public void ExecutionPlanFactory_DataSetWithSingle2019PlanTables_ReturnPlan()
        {
            DataSet dataSet = new DataSet();
            LoadTable(dataSet, new DataClass(100, "Jamie"));
            LoadTable(dataSet, new ExecutionPlanClass("ExecutionPlan2019_Sample1.xml"));

            IExecutionPlan result = Build(dataSet);

            Assert.IsInstanceOf<SQLServer2019.ExecutionPlan>(result);
        }

        [Test]
        public void ExecutionPlanFactory_DataSetWithMultiple2017PlanTables_ReturnPlanSet()
        {
            DataSet dataSet = new DataSet();
            LoadTable(dataSet, new DataClass(100, "Jamie"));
            LoadTable(dataSet, new ExecutionPlanClass("ExecutionPlan2017_Sample1.xml"));
            LoadTable(dataSet, new ExecutionPlanClass("ExecutionPlan2017_Sample2.xml"));

            IExecutionPlan result = Build(dataSet);

            Assert.IsInstanceOf<SQLServer2017.ExecutionPlanSet>(result);
        }

        [Test]
        public void ExecutionPlanFactory_DataSetWithMultiple2019PlanTables_ReturnPlanSet()
        {
            DataSet dataSet = new DataSet();
            LoadTable(dataSet, new DataClass(100, "Jamie"));
            LoadTable(dataSet, new ExecutionPlanClass("ExecutionPlan2019_Sample1.xml"));
            LoadTable(dataSet, new ExecutionPlanClass("ExecutionPlan2019_Sample2.xml"));

            IExecutionPlan result = Build(dataSet);

            Assert.IsInstanceOf<SQLServer2019.ExecutionPlanSet>(result);
        }
    }
}
