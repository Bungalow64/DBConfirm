using DBConfirm.Core.DataResults.Abstract;
using DBConfirm.Core.TestFrameworks.Abstract;
using DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2017.Xml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2017
{
    /// <summary>
    /// The execution plan for a query run on a SQL Server 2017 database
    /// </summary>
    public class ExecutionPlan : IExecutionPlan
    {
        /// <summary>
        /// The execution plan, converted directly from XML
        /// </summary>
        public ShowPlanXML Data { get; }

        /// <summary>
        /// The collection of <see cref="RelOpBaseType"/> objects within the plan
        /// </summary>
        public ICollection<RelOpBaseType> PlanElements { get; private set; } = new List<RelOpBaseType>();

        private readonly string _rawXml;

        /// <summary>
        /// The test framework to use for assertions
        /// </summary>
        internal readonly ITestFramework TestFramework;

        /// <summary>
        /// Creates a new <see cref="ExecutionPlan"/> instance, populating Data with the xml provided
        /// </summary>
        /// <param name="testFramework">The test framework to use for assertions</param>
        /// <param name="xml">The XML of the execution plan</param>
        public ExecutionPlan(ITestFramework testFramework, string xml)
        {
            TestFramework = testFramework;
            _rawXml = xml;
            Data = Deserialize();
            ExtractPlanElements();
        }

        private ShowPlanXML Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ShowPlanXML));
            using (StringReader stringReader = new StringReader(_rawXml))
            {
                return (ShowPlanXML)serializer.Deserialize(stringReader);
            }
        }

        private void ExtractPlanElements()
        {
            void Add(RelOpBaseType item)
            {
                if (item == null)
                {
                    return;
                }

                PlanElements.Add(item);

                foreach (RelOpType inner in item.GetRelOp())
                {
                    Add(inner.Item);
                }
            }

            foreach (StmtBlockType[] batchStatementArray in Data.BatchSequence)
            {
                foreach (StmtBlockType batchStatement in batchStatementArray)
                {
                    foreach (BaseStmtInfoType item in batchStatement?.Items ?? new BaseStmtInfoType[0])
                    {
                        Add(item?.GetQueryPlan()?.RelOp?.Item);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IExecutionPlan AssertKeyLookups(int expectedTotal)
        {
            TestFramework.AreEqual(expectedTotal, PlanElements.OfType<IndexScanType>().Count(p => p.Lookup), "Key lookup count is incorrect");
            return this;
        }
    }
}
