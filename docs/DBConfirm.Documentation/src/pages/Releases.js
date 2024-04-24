import React from 'react';
import './Releases.scss';

export default function Releases() {
    return (
        <div className="image-content">
            <h2 id="Releases">Releases</h2>

            <ul>
                <li><a href="#v1_1_0">v1.1.0</a> - 01 May 2024</li>
                <li><a href="#v1_0_1">v1.0.1</a> - 25 Jan 2023</li>
                <li><a href="#v1_0_0">v1.0.0</a> - 18 Aug 2021</li>
            </ul>


            <h3 id="v1_1_0">v1.1.0</h3>
            <p>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/1.1.0"><img
                src="https://img.shields.io/badge/nuget-v1.1.0-blue" alt="" /></a>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/1.1.0"><img
                src="https://img.shields.io/badge/release-may_2024-4ABD1E" alt="" /></a></p>
                <p>This release adds more functionality when making assertions and fixes a few issues.</p>
            <p>This release also upgrades the version of .NET used, and confirms compatibility with SQL Server 2022.</p>

            <h4>Enhancements</h4>
            <section>
                <header><span>Added a new assertion to allow for numeric values to be asserted, regardless of the format of the number</span>
                <a target="_blank" rel="noreferrer" href="https://github.com/Bungalow64/DBConfirm/issues/52"><img
                        src="https://img.shields.io/badge/GitHub_issue-%2352-4ABD1E" alt="" /></a></header>
                <div className="aside-body">
                    <p>When asserting numbers, we previously got a failure if the type of the number didn't match what was coming 
                        back from the database (e.g., we were expecting an int and recieved a float), even if the numbers 
                        numerically matched (e.g., we expected 1 and recieved 1.0):
                    </p>
                    <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> <span className="hljs-built_in">data</span> = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.GetCount"</span>);
                    {"\n"}
                        {"\n"}<span className="hljs-built_in">data</span>
                        {"\n"}    .<span className="hljs-title">ValidateRow</span>(<span className="hljs-number">2</span>)
                {"\n"}    <span className="hljs-comment">{'//'} This assertion only passes if the data type of the 'Total' column is an int</span>
                        {"\n"}    .<span className="hljs-title">AssertValue</span>(<span className="hljs-string">"Total"</span>, <span className="hljs-number">5</span>);
        </code></pre>
                    <p>This release introduces a new <strong><code>IComparison.MatchesNumber</code></strong> method, which attempts to assert the numeric value 
                        regardless of the type.
                    </p>
                    <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.GetCount"</span>);
                {"\n"}
                        {"\n"}data
                {"\n"}    .<span className="hljs-title">ValidateRow</span>(<span className="hljs-number">2</span>)
                {"\n"}    <span className="hljs-comment">{'//'} Asserts that the value numerically matches, regardless of type</span>
                        {"\n"}    .<span className="hljs-title">AssertValue</span>(<span className="hljs-string">"Total"</span>, Comparisons.<span className="hljs-title">MatchesNumber</span>(<span className="hljs-number">5</span>));
        </code></pre>
                    <p>If actual the numeric value cannot be converted to the expected type (e.g., 1.5 into an int) then the assertion falls back to the original assertion logic, and will fail as expected.</p>
                </div>
            </section>

            <section>
            <header><span>Added a new column assertion to check for duplicate data</span>
            <a target="_blank" rel="noreferrer" href="https://github.com/Bungalow64/DBConfirm/issues/96"><img
                src="https://img.shields.io/badge/GitHub_issue-%2396-4ABD1E" alt="" /></a></header>
                <div className="aside-body">
            <p>There is now a column-based assertion to verify that the data in the specified columns are unique.</p>
            <p>We can use <strong><code>AssertColumnValuesUnique</code></strong> to check this:</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.GetCustomerData"</span>);
            {"\n"}
                {"\n"}Data
        {"\n"}    <span className="hljs-comment">{'//'} Asserts that the data in the 'FirstName' and 'LastName' columns does not exist in multiple rows</span>
                {"\n"}    .<span className="hljs-title">AssertColumnValuesUnique</span>(<span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"LastName"</span>);
            </code></pre>
            </div>
            </section>

            <section>
            <header><span>Added assertions to check for expected errors</span>
            <a target="_blank" rel="noreferrer" href="https://github.com/Bungalow64/DBConfirm/issues/54"><img
                src="https://img.shields.io/badge/GitHub_issue-%2354-4ABD1E" alt="" /></a>
            </header>
            <div className="aside-body">
            <p>We now have dedicated methods that can be used to assert that a given command/stored procedure throws the expected exception.</p>
            <p>We can assert that any error was found, the error's message and the error's type:</p>

            <pre><code className="lang-csharp"><span className="hljs-type">ErrorResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureErrorAsync</span>(<span className="hljs-string">"dbo.GetCount"</span>);
        {"\n"}
                {"\n"}data
                {"\n"}    <span className="hljs-comment">{'//'} Asserts that the error was found</span>
                {"\n"}    .<span className="hljs-title">AssertError</span>()
        {"\n"}    <span className="hljs-comment">{'//'} Asserts that the message is 'Cannot insert the value NULL into column 'FirstName''</span>
                {"\n"}    .<span className="hljs-title">AssertMessage</span>(<span className="hljs-string">"Cannot insert the value NULL into column 'FirstName'"</span>)
        {"\n"}    <span className="hljs-comment">{'//'} Asserts that the message starts with 'Cannot insert the value NULL'</span>
                {"\n"}    .<span className="hljs-title">AssertMessage</span>(Comparisons.<span className="hljs-title">StartsWith</span>(<span className="hljs-string">"Cannot insert the value NULL"</span>))
        {"\n"}    <span className="hljs-comment">{'//'} Asserts that the exception type is 'SqlException'</span>
                {"\n"}    .<span className="hljs-title">AssertType</span>(<span className="hljs-keyword">typeof</span>(<span className="hljs-type">SqlException</span>));
</code></pre>

        <p>There is also <strong><code>TestRunner.ExecuteCommandErrorAsync</code></strong> which works in the same way, but with a command rather than a stored procedure.</p>
        </div>
        </section>
            <section>
            <header><span>Confirmed compatibility with SQL Server 2022</span>
            <a target="_blank" rel="noreferrer" href="https://github.com/Bungalow64/DBConfirm/issues/99"><img
                src="https://img.shields.io/badge/GitHub_issue-%2399-4ABD1E" alt="" /></a></header>
            <div className="aside-body">
        <p>We've tested DBConfirm against SQL Server 2022, and it all works as expected so we've added SQL Server 2022 to our automated tests.</p>
</div>
</section>
        <h4>Issues Resolved</h4>

        <p>This release includes a number of fixes:</p>
        <ul>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/51" target="_blank" rel="noreferrer">'Inconsistent Line Endings' warning when opening generated template files in VS2019</a></li>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/57" target="_blank" rel="noreferrer">Generating a template for a table name that contains an apostrophe results in an invalid class</a></li>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/97" target="_blank" rel="noreferrer">Upgrade Microsoft.Data.SqlClient/System.Data.SqlClient to non-vulnerable versions</a></li>
        </ul>
        <h4>Internal Fixes</h4>

        <p>This release includes a number of internal fixes:</p>
        <ul>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/98" target="_blank" rel="noreferrer">Upgrade test projects to .NET 8</a></li>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/100" target="_blank" rel="noreferrer">Remove SQL setup script from root</a></li>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/101" target="_blank" rel="noreferrer">Add negative tests, asserting that verifications should fail when expected</a></li>
        </ul>

        <h4>Breaking Changes</h4>
        <p>There are no breaking changes between this version (1.1.0) and the previous version (1.0.1).</p>

            <h3 id="v1_0_1">v1.0.1</h3>
            <p>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/1.0.1"><img
                src="https://img.shields.io/badge/nuget-v1.0.1-blue" alt="" /></a>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/1.0.1"><img
                src="https://img.shields.io/badge/release-jan_2023-F48041" alt="" /></a></p>
            <p>This release fixes an issue with an assertion.</p>
            
            <h4>Issues Resolved</h4>

        <p>This release includes one fix:</p>
        <ul>
            <li><a href="https://github.com/Bungalow64/DBConfirm/issues/87" target="_blank" rel="noreferrer">Using AssertRowDoesNotExist when using NUnit results in an incorrect test failure</a></li>
        </ul>

<h4>Breaking Changes</h4>
<p>There are no breaking changes between this version (1.0.1) and the previous version (1.0.0).</p>

            <h3 id="v1_0_0">v1.0.0</h3>
            <p>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/1.0.0"><img
                src="https://img.shields.io/badge/nuget-v1.0.0-blue" alt="" /></a>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/1.0.0"><img
                src="https://img.shields.io/badge/release-aug_2021-F48041" alt="" /></a></p>
            <p>The initial release.</p>
        </div >
    );
}