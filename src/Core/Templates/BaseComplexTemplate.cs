﻿using DBConfirm.Core.Runners.Abstract;
using DBConfirm.Core.Templates.Abstract;
using System.Threading.Tasks;

namespace DBConfirm.Core.Templates
{
    /// <summary>
    /// The abstract template class used as the base for complex templates.  Complex templates are used to build templates that insert data into multiple tables, including setting foreign keys
    /// </summary>
    public abstract class BaseComplexTemplate : ITemplate
    {
        /// <inheritdoc/>
        public bool IsInserted { get; private set; }

        /// <inheritdoc/>
        public void RecordInsertion() => IsInserted = true;

        /// <summary>
        /// Generates the next identity for the test
        /// </summary>
        /// <returns>Returns the next identity value</returns>
        protected int GetNextIdentity() => CustomIdentityService.GenerateNextIdentity();

        /// <inheritdoc/>
        public abstract Task InsertAsync(ITestRunner testRunner);
    }
}
