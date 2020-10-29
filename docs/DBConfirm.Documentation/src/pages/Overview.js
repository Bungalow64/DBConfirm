import React from 'react';

export default function Overview() {
    return (
        <>
            <h2 id="introduction">Introduction</h2>
            <p>A C#-based testing framework to write and run tests for logic within SQL Server</p>
            <p><a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/"><img
                src="https://img.shields.io/nuget/v/DBConfirm.Core" alt="" /></a>
                <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/"><img
                    src="https://img.shields.io/nuget/dt/DBConfirm.Core" alt="" /></a>
                <a target="_blank" rel="noreferrer"
                    href="https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_build/latest?definitionId=1"><img
                        src="https://dev.azure.com/bungalow64/Bungalow64.SqlTesting/_apis/build/status/Bungalow64.SqlTesting/Sprint-CI"
                        alt="Build Status" /></a></p>
            <h2 id="what-is-DBConfirm-">What is DBConfirm?</h2>
            <p>DBConfirm is a unit testing framework for SQL databases from within .Net projects.</p>
            <h2 id="why-">Why?</h2>
            <p>Developers are pretty good at writing unit tests for their application logic already, but sometimes database
            logic (stored procedures, views, etc.) can be overlooked. A big reason is that traditionally SQL unit tests
            are
            difficult to write, or have a very steep learning curve. DBConfirm aims to solve this by allowing SQL tests
            to
            be written in the same way that all other unit tests are written, so that they are easy to write, easy to
                        maintain, and easy to run.</p>
            <h2 id="how-">How?</h2>
            <p>The DBConfirm framework is designed to execute tests against a physical instance of the database under test,
            and
            ensures that each test run is accurate and repeatable by making sure all effects of a test are rolled back
            when
                        the test has finished.</p>

            <h2 id="what-does-a-DBConfirm-test-look-like-">What does a DBConfirm test look like?</h2>
            <div className="content-split">
                <div className="content-split-primary">
                    <p>A simple test (in MSTest) to call a stored procedure then verify that the data has been added, looks
                    like
                    this:
                            </p>
                    <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
                    {"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">AddUserProcedure_UserIsAdded</span>(<span className="hljs-params"></span>)
                    {"\n"}</span>{"{"}
                    {"\n"}    <span className="hljs-comment">{'//'} Call a stored procedure with some parameters</span>
                    {"\n"}    <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureNonQueryAsync</span>(<span className="hljs-string">"dbo.AddUser"</span>,
                    {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"Jamie"</span>),
                    {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"LastName"</span>, <span className="hljs-string">"Burns"</span>),
                    {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"EmailAddress"</span>, <span className="hljs-string">"jamie@example.com"</span>));
                    {"\n"}
                    {"\n"}    <span className="hljs-comment">{'//'} Get all the data in a table</span>
                    {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteTableAsync</span>(<span className="hljs-string">"dbo.Users"</span>);
                    {"\n"}
                    {"\n"}    <span className="hljs-comment">{'//'} Make some assertions on the data</span>
                    {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>) <span className="hljs-comment">{'//'} Asserts that there is only 1 row</span>
                    {"\n"}        .<span className="hljs-title">AssertValue</span>(<span className="hljs-number">0</span>, <span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"Jamie"</span>); <span className="hljs-comment">{'//'} Asserts that "FirstName" is "Jamie" in the first row</span>
                    {"\n"}{"}"}
                    </code></pre>
                </div>
                <aside>
                    <header id="the-fundamentals-of-a-DBConfirm-test">The fundamentals of a DBConfirm test</header>
                    <div className="aside-body">
                        <h3 id="arrange-set-up-any-prerequisite-test-data">1. Arrange - set up any prerequisite test data</h3>
                        <p>Data can be inserted into any table in the database using either the DBConfirm API or by creating
                                templates that are used to set up complex scenarios across multiple tests.  See <a href="/templates">templates</a> for more details.</p>
                        <h3 id="act-run-the-sql-you-want-to-test">2. Act - run the SQL you want to test</h3>
                        <p>Using the DBConfirm API you can trigger any stored procedure, query any view, or run any arbitrary
                                SQL statement.</p>
                        <h3 id="assert-check-the-returned-data-and-check-the-state-of-data-in-the-database">3. Assert - check
                                the returned data, and check the state of data in the database</h3>
                        <p>The DBConfirm API provides a whole bunch of assertion methods that you can run on the returned data,
                        including checking specific columns are present, that values are what you expect them to be, and that at least
                                one row meets your conditions, etc.</p>
                        <p>You can also use the DBConfirm API to query the data in tables, and run the same assertions, to make
                                sure the data is in the exact state that you expect.</p>
                    </div>
                </aside>
            </div>

            <h2 id="getting-started">Getting started</h2>
            <p>See the <a href="/quickstart">Quick Start</a> guide for getting started.</p>
        </>
    );
}