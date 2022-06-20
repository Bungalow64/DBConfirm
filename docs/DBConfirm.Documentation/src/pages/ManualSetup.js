import React from 'react';
import appsettings_properties from './images/appsettings_properties.png';

export default function ManualSetup() {
    return (
        <>
            <h2 id="manual-setup">Manual Setup</h2>

            <p>Here are the steps you need to take to manually add DBConfirm to a unit test project.  To use a dotnet template, see the <a href="/quickstart">Quick Start</a> guide.</p>

            <h3>Install packages</h3>
            <p>Install the NuGet package for the test framework you&apos;re using, either MSTest or NUnit and SQL Server or MySQL:</p>
            <ul>
                <li>Install-Package <a target="_blank" rel="noreferrer"
                    href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/">DBConfirm.Packages.SQLServer.MSTest</a>
                </li>
                <li>Install-Package <a target="_blank" rel="noreferrer"
                    href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/">DBConfirm.Packages.SQLServer.NUnit</a>
                </li>
                <li>Install-Package <a target="_blank" rel="noreferrer"
                    href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.MSTest/">DBConfirm.Packages.MySQL.MSTest</a>
                </li>
                <li>Install-Package <a target="_blank" rel="noreferrer"
                    href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.NUnit/">DBConfirm.Packages.MySQL.NUnit</a>
                </li>
            </ul>
            <h3>Set test database configuration</h3>

            <div className="content-split">

                <div className="content-split-primary">
                    <p>In the root of your test project, add an <strong>appsettings.json</strong> file with a connection
            string
            called '<strong>DefaultConnectionString</strong>' to
            point to the database to be tested:</p>
                    <pre><code className="lang-json">{"{"}
                        {"\n"}  <span className="hljs-attr">"ConnectionStrings"</span>: {"{"}
                        {"\n"}    <span className="hljs-attr">"DefaultConnectionString"</span>: <span className="hljs-string">"SERVER=(local);DATABASE=TestDB;Integrated Security=true;Connection Timeout=30;"</span>
                        {"\n"}  {"}"}
                        {"\n"}{"}"}
                    </code></pre>

                    <p>For more details on how to set the connection strings, see the <a href="/connectionstrings">Connection Strings</a> guide.</p>
                </div>
                <aside>
                    <header>appsettings.json</header>
                    <div className="aside-body">
                        <p>Note, this <strong>appsettings.json</strong> file needs to be copied to the output directory.</p>
                        <img src={appsettings_properties}
                            alt="appsettings.json file set with Build Action = Content, Copy to Output Directory = Copy if newer" />
                    </div>
                </aside>
            </div>

            <h3>Add test class</h3>
            <p>Add a new test file, and inherit from the base class for the framework (either <code>MSTestBase</code> or <code>NUnitBase</code>):</p>
            <div className="content-split">

                <div className="content-split-primary">
                    <pre><code className="lang-csharp"><span className="hljs-comment">{'//'} For MSTest</span>
                        {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
            {"\n"}<span className="hljs-keyword">using</span> Microsoft.VisualStudio.TestTools.UnitTesting;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Packages.SQLServer.MSTest;
            {"\n"}
                        {"\n"}[<span className="hljs-type">TestClass</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">GetUserTests</span> : <span className="hljs-type">MSTestBase</span>
                        {"\n"}{"{"}
                        {"\n"}    ...
            {"\n"}{"}"}
                    </code></pre>
                </div>
                <aside>
                    <header>For NUnit</header>
                    <div className="aside-body">
                        <p>For NUnit, use <code>[TestFixture]</code> instead of <code>[TestClass]</code>, and inherit from <code>DBConfirm.Packages.SQLServer.NUnit.NUnitBase</code> instead of <code>MSTestBase</code>.</p>
                        <p>When using MySQL, this base class is <code>DBConfirm.Packages.MySQL.NUnit.NUnitBase</code>.</p>
                    </div>
                </aside>
            </div>

            <h3>Add test method</h3>
            <p>Add a new test method, and start testing:</p>
            <div className="content-split">

                <div className="content-split-primary">
                    <pre><code className="lang-csharp"><span className="hljs-comment">{'//'} For MSTest</span>
                        {"\n"}[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">GetUsersView_ContainsFirstNameColumn</span>(<span className="hljs-params"></span>)
            {"\n"}</span>{"{"}
                        {"\n"}    <span className="hljs-keyword">var</span> results = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteTableAsync</span>(<span className="hljs-string">"dbo.GetUsers"</span>);
            {"\n"}
                        {"\n"}    results
            {"\n"}        .<span className="hljs-title">AssertColumnExists</span>(<span className="hljs-string">"FirstName"</span>);
            {"\n"}{"}"}
                    </code></pre>
                </div>
                <aside>
                    <header>For NUnit</header>
                    <div className="aside-body">
                        <p>For NUnit, use <code>[Test]</code> instead of <code>[TestMethod]</code>.</p>
                    </div>
                </aside>
            </div>

            <p>For the full details of how to insert data, execute database logic and verify results, see the <a
                href="/writingtests">Writing Tests</a> guide.</p>
        </>
    );
}