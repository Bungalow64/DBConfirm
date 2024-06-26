import React from 'react';
import add_new_project from './images/add_new_project.png';

export default function QuickStart() {
    return (
        <>
            <h2 id="getting-started">Quick Start</h2>

            <p>The quickest (and recommended) way to get started is to install and run one of the DBConfirm template packages.  Alternatively, you can follow the <a href="/manualsetup">Manual Setup</a> guide.</p>
            <p>These template packages are provided to quickly set up new test projects, including all the depdendencies and required configuration.</p>
            <p>They are provided as NuGet packages, and need to be installed before new projects can be created.</p>

            <h3>Template package installation</h3>

            <div className="content-split">
                <div className="content-split-primary">
                    <p>To install the package (using MSTest as the test framework), execute this command from a command prompt:</p>
                    <pre>
                        <code>
                            dotnet new install DBConfirm.Templates.SQLServer.MSTest
                        </code>
                    </pre>
                </div>
                <aside>
                    <header>Package for NUnit</header>
                    <div className="aside-body">
                        <p>To use NUnit as the test framework, use <code>DBConfirm.Templates.SQLServer.NUnit</code> instead.</p>
                    </div>
                </aside>
            </div>

            <h3>Creating a new project</h3>

            <p>Once installed, within Visual Studio, add a new project and search for <strong>DBConfirm</strong>:</p>

            <p><img src={add_new_project}
                alt="The Add New Project window inside Visual Studio 2022, after searching for DBConfirm, showing the two available DBConfirm templates"
                style={{ width: '100%', maxWidth: '989px' }} /></p>

            <p>Select the framework you're using (either MSTest or NUnit), click Next and enter your project name.</p>

            <p>This will add a new project, with the required NuGet packages installed.  There will be a sample test class (inheriting from the correct base class),
            containing a single test method (already marked as async):
            </p>

            <div className="content-split">

                <div className="content-split-primary">
                    <pre><code className="lang-csharp"><span className="hljs-comment">{'//'} For MSTest</span>
                        {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Data;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.DataResults;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Parameters;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Packages.SQLServer.MSTest;
            {"\n"}<span className="hljs-keyword">using</span> Microsoft.VisualStudio.TestTools.UnitTesting;
            {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
            {"\n"}
                        {"\n"}[<span className="hljs-type">TestClass</span>]
                        {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">GetUserTests</span> : <span className="hljs-type">MSTestBase</span>
                        {"\n"}{"{"}
                        {"\n"}    [<span className="hljs-type">TestMethod</span>]
                        {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">TestMethod1</span>(<span className="hljs-params"></span>)
                        {"\n"}    {"{"}
                        {"\n"}
                        {"\n"}    {"}"}
                        {"\n"}{"}"}
                    </code></pre>
                </div>
                <aside>
                    <header>For NUnit</header>
                    <div className="aside-body">
                        <p>For NUnit, <code>[TestFixture]</code> is used instead of <code>[TestClass]</code>, and the class will inherit from <code>DBConfirm.Packages.SQLServer.NUnit.NUnitBase</code> instead of <code>MSTestBase</code>.</p>
                    </div>
                </aside>
            </div>

            <p>There will also be an appsettings.json file with a default connection string, which will need to be updated to point to your own database.</p>

            <h3>Upating the default connection string</h3>

            <p>DBConfirm requires a database instance to run its tests against.  This database needs to have the correct schema, but it is recommended to use a database
            that contains no data (or only static data that never changes) - all test data will be added as part of the tests themselves.
    </p>

            <p>In the root of your test project, find the <strong>appsettings.json</strong> file.  This file will contain
                    a connection string called '<strong>DefaultConnectionString</strong>', which is used to point to the database to be tested:</p>
            <pre><code className="lang-json">{"{"}
                {"\n"}  <span className="hljs-attr">"ConnectionStrings"</span>: {"{"}
                {"\n"}    <span className="hljs-attr">"DefaultConnectionString"</span>: <span className="hljs-string">"SERVER=(local);DATABASE=TestDatabaseName;Integrated Security=true;Connection Timeout=30;"</span>
                {"\n"}  {"}"}
                {"\n"}{"}"}
            </code></pre>

            <p>This connection string needs to be updated to point to the database that is to be used whilst running the tests.</p>

            <p>For more details on how to set the connection strings, see the <a href="/connectionstrings">Connection Strings</a> guide.</p>

            <h3>Writing your first test</h3>

            <p>Congratulations, you're now ready to start writing tests.</p>

            <p>For the full details of how to insert data, execute database logic and verify results, see the <a
                href="/writingtests">Writing Tests</a> guide.</p>
        </>
    );
}