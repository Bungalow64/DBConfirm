import React from 'react';
import ci_connectionstring from './images/ci_connectionstring.png';

export default function ConnectionStrings() {
    return (
        <>
            <h2>Connection Strings</h2>
            <p>There are a number of ways to configure the connection string used for the DBConfirm tests, to make it easy for local development and for running
            the tests are part of a CI build.
        </p>

            <p>There are 2 elements that can be configured - the <strong>connection string name</strong>, and the <strong>connection string value</strong>.</p>

            <p>By default, the <strong>connection string name</strong> is set as "DefaultConnectionString".  Unless this is overridden, the tests will
            search for a connection string using that name.  If you're only ever testing one database, then there's no need to change this value.  However,
            if you want to test multiple databases within the same test project or CI build, then you'll need to change this name so that you can refer to the different
            connection strings.</p>

            <p>For each <strong>connection string name</strong> (including the default name, if it's not overridden), there needs to be available a
            corresponding <strong>connection string value</strong>.</p>

            <ul>
                <li><a href="#connectionstringnames">Configuring the Connection String Name</a></li>
                <li><a href="#connectionstringvalues">Configuring the Connection String Value</a></li>
                <li><a href="#commonsetups">Common setups</a></li>
                <li><a href="#multipleconnections">Multiple database connections within a test</a></li>
            </ul>

            <h3 id="connectionstringnames">Configuring the Connection String Name</h3>

            <p>By default, the <strong>connection string name</strong> is "DefaultConnectionString".  This can be customised at a project level, or a test class level.</p>
            <p>To change this value for the <strong>entire test project</strong>, set the <code>DefaultConnectionStringName</code> property in the <code>appsettings.json</code> file.</p>

            <pre><code className="lang-json">{"{"}
                {"\n"}  <span className="hljs-attr">"ConnectionStrings"</span>: {"{"}
                {"\n"}    <span className="hljs-attr">"NorthwindConnection"</span>: <span className="hljs-string">"SERVER=(local);DATABASE=Northwind;Integrated Security=true;Connection Timeout=30;"</span>
                {"\n"}  {"}"},
            {"\n"}  <span className="hljs-attr">"DefaultConnectionStringName"</span>: <span className="hljs-string">"NorthwindConnection"</span>
                {"\n"}{"}"}
            </code></pre>

            <p>The value set here will override the default, and the tests will look for a connection string using that name.</p>

            <p>To change this value for a <strong>specific test class</strong>, you can either add the <code>ConnectionStringName</code> attribute to the class or override
            the <code>ParameterName</code> property.  Where both have been configured for the same class, the attribute value is used.</p>

            <p>To set the connection string name via an attribute, use the <code>ConnectionStringName</code> attribute on the class:</p>

            <div className="content-split">
                <div className="content-split-primary">
                    <pre><code className="lang-csharp"><span className="hljs-comment">{"//"} For MSTest</span>
                        {"\n"}<span className="hljs-keyword">using</span> Microsoft.VisualStudio.TestTools.UnitTesting;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Packages.SQLServer.MSTest;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Attributes;
            {"\n"}
                        {"\n"}[<span className="hljs-type">ConnectionStringName</span>(<span className="hljs-string">"NorthwindConnection"</span>)]
                        {"\n"}[<span className="hljs-type">TestClass</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">GetUserTests</span> : <span className="hljs-type">MSTestBase</span>
                        {"\n"}{"{"}
                        {"\n"}    ...
            {"\n"}{"}"}
                    </code></pre>
                </div>
                <aside>
                    <header>
                        For NUnit
                    </header>
                    <div className="aside-body">
                        <p>For NUnit, use <code>[TestFixture]</code> instead of <code>[TestClass]</code>, and inherit from <code>DBConfirm.Packages.SQLServer.NUnit.NUnitBase</code> instead of <code>MSTestBase</code>.</p>
                    </div>
                </aside>
            </div>

            <p>To set the connection string name via the property, override the <code>ParameterName</code> property:</p>

            <div className="content-split">
                <div className="content-split-primary">
                    <pre><code className="lang-csharp"><span className="hljs-comment">{"//"} For MSTest</span>
                        {"\n"}<span className="hljs-keyword">using</span> Microsoft.VisualStudio.TestTools.UnitTesting;
            {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Packages.SQLServer.MSTest;
            {"\n"}
                        {"\n"}[<span className="hljs-type">TestClass</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">GetUserTests</span> : <span className="hljs-type">MSTestBase</span>
                        {"\n"}{"{"}
                        {"\n"}    <span className="hljs-keyword">protected</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> ParameterName =&gt; <span className="hljs-string">"NorthwindConnection"</span>;
            {"\n"}
                        {"\n"}    ...
            {"\n"}{"}"}
                    </code></pre>
                </div>
                <aside>
                    <header>
                        For NUnit
                    </header>
                    <div className="aside-body">
                        <p>For NUnit, use <code>[TestFixture]</code> instead of <code>[TestClass]</code>, and inherit from <code>DBConfirm.Packages.SQLServer.NUnit.NUnitBase</code> instead of <code>MSTestBase</code>.</p>
                    </div>
                </aside>
            </div>

            <p>The order of precedence used for each of these methods (where the top method is used first) is:</p>
            <ol>
                <li>ConnectionStringName attribute</li>
                <li>ParameterName overridden property</li>
                <li>DefaultConnectionStringName in appsettings.json</li>
                <li>Default ("DefaultConnectionString")</li>
            </ol>

            <h3 id="connectionstringvalues">Configuring the Connection String Value</h3>

            <p>The <strong>connection string values</strong> can be stored in a number of places:</p>
            <ol>
                <li>Environment Variables</li>
                <li>.runsettings file</li>
                <li>appsettings.json file</li>
            </ol>

            <p>This is also the order of precedence for each of these locations.</p>

            <p>To set an environment variable (within an Azure DevOps CI build), add the connection string to the '<strong>Variables</strong>':</p>
            <p><img src={ci_connectionstring}
                alt="The ConnectionString environment variable added to a Pipeline build in Azure DevOps"
                style={{ width: '100%', minWidth: '700px', maxWidth: '1417px' }} /></p>

            <p>To set the values in a <code>*.runsettings</code> file, add the connection string name as a parameter within <code>TestRunParameters</code>:</p>

            <pre><code className="lang-xml"><span className="hljs-meta">&lt;?</span><span className="hljs-tag">xml</span> <span className="hljs-attr">version</span>="<span className="hljs-string">1.0</span>" <span className="hljs-attr">encoding</span>="<span className="hljs-string">utf-8</span>"<span className="hljs-meta">?&gt;</span>
                {"\n"}&lt;<span className="hljs-tag">RunSettings</span>&gt;
        {"\n"}    &lt;<span className="hljs-tag">TestRunParameters</span>&gt;
        {"\n"}        &lt;<span className="hljs-tag">Parameter</span> <span className="hljs-attr">name</span>="<span className="hljs-string">DefaultConnectionString</span>" <span className="hljs-attr">value</span>="<span className="hljs-string">SERVER=B64-BUILD-DB;DATABASE=Northwind;Integrated Security=true;Connection Timeout=30;</span>" /&gt;
        {"\n"}    &lt;/<span className="hljs-tag">TestRunParameters</span>&gt;
        {"\n"}&lt;/<span className="hljs-tag">RunSettings</span>&gt;
</code></pre>

            <p>To set the values in the <code>appsettings.json</code> file, add the connection string name to the <code>ConnectionStrings</code> property:</p>

            <pre><code className="lang-json">{"{"}
                {"\n"}    <span className="hljs-attr">"ConnectionStrings"</span>: {"{"}
                {"\n"}        <span className="hljs-attr">"DefaultConnectionString"</span>: <span className="hljs-string">"SERVER=B64-BUILD-DB;DATABASE=Northwind;Integrated Security=true;Connection Timeout=30;"</span>
                {"\n"}    {"}"}
                {"\n"}{"}"}
            </code></pre>

            <h3 id="commonsetups">Common setups</h3>
            <h4>1. Single database being tested</h4>
            <p>If you are only testing 1 database, then the setup is easy.  Leave the <strong>connection string name</strong> as default ("DefaultConnectionString"),
            and add an appsettings.json file containing a <strong>connection string value</strong> for "DefaultConnectionString" to get local development working.  For
            integration with a CI build, set an environment variable for "DefaultConnectionString" to be the <strong>connection string value</strong> to use during CI.</p>

            <h4>2. Multiple databases being tested, with one test project for each</h4>
            <p>Assuming each test project is only testing 1 database, then in the appsettings.json of each project change the <strong>connection string name</strong> (via
             the "DefaultConnectionStringName" property) to be different for each database that's being tested, and add a <strong>connection string value</strong> for each name.  For
             integration with a CI build, set an environment variable for each <strong>connection string name</strong> used.</p>

            <h3 id="multipleconnections">Multiple database connections within a test</h3>
            <p>If you're writing an integration-type test, you may want to connect to multiple databases within a single test.  By default, a single database
            is connected to during each test, however it's possible for you to open new connections - the only difference is that you must remember to close the connections
            when you're finished with them.
             </p>
            <p>To open a new connection, call <code>await NewConnectionByConnectionStringNameAsync</code> (or <code>await NewConnectionByConnectionStringAsync</code>).  These
             methods return a new instance of <code>ITestRunner</code>, so you can use this object to insert data and execute commands as you would in the
             rest of the test via <code>TestRunner</code>.  It's recommended to wrap <code>ITestRunner</code> in a <code>using</code> statement, so that the
             connection is automatically closed at the end of the test.</p>

            <p>To open a new connection using a <strong>connection string name</strong> (recommended) call <code>NewConnectionByConnectionStringNameAsync</code>:</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">GetAllUsers</span>(<span className="hljs-params"></span>)
            {"\n"}</span>{"{"}
                {"\n"}    <span className="hljs-comment">{"//"} Opens a connection using the 'Database2Connection' connection string</span>
                {"\n"}    <span className="hljs-keyword">using</span> <span className="hljs-interface">ITestRunner</span> database2Connection = <span className="hljs-keyword">await</span> <span className="hljs-title">NewConnectionByConnectionStringNameAsync</span>(<span className="hljs-string">"Database2Connection"</span>);
            {"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Opens a connection using the 'Database3Connection' connection string</span>
                {"\n"}    <span className="hljs-keyword">using</span> <span className="hljs-interface">ITestRunner</span> database3Connection = <span className="hljs-keyword">await</span> <span className="hljs-title">NewConnectionByConnectionStringNameAsync</span>(<span className="hljs-string">"Database3Connection"</span>);
            {"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Executes a command using the default database connection</span>
                {"\n"}    <span className="hljs-type">QueryResult</span> results1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteViewAsync</span>(<span className="hljs-string">"dbo.AllUsers"</span>);
            {"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Executes a command using the connection to Database2</span>
                {"\n"}    <span className="hljs-type">QueryResult</span> results2 = <span className="hljs-keyword">await</span> database2Connection.<span className="hljs-title">ExecuteViewAsync</span>(<span className="hljs-string">"dbo.AllUsers"</span>);
            {"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Executes a command using the connection to Database3</span>
                {"\n"}    <span className="hljs-type">QueryResult</span> results3 = <span className="hljs-keyword">await</span> database3Connection.<span className="hljs-title">ExecuteViewAsync</span>(<span className="hljs-string">"dbo.AllUsers"</span>);
            {"\n"}{"}"}
            </code></pre>

            <p>To open a new connection using a <strong>connection string value</strong> call <code>NewConnectionByConnectionStringAsync</code>:</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
{"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">GetAllUsers</span>(<span className="hljs-params"></span>)
{"\n"}</span>{"{"}
                {"\n"}    <span className="hljs-comment">{"//"} Opens a connection to Database2</span>
                {"\n"}    <span className="hljs-keyword">using</span> <span className="hljs-interface">ITestRunner</span> database2Connection = <span className="hljs-keyword">await</span> <span className="hljs-title">NewConnectionByConnectionStringAsync</span>(<span className="hljs-string">"SERVER=(local);DATABASE=Database2;Integrated Security=true;Connection Timeout=30;"</span>);
{"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Opens a connection to Database3</span>
                {"\n"}    <span className="hljs-keyword">using</span> <span className="hljs-interface">ITestRunner</span> database3Connection = <span className="hljs-keyword">await</span> <span className="hljs-title">NewConnectionByConnectionStringAsync</span>(<span className="hljs-string">"SERVER=(local);DATABASE=Database3;Integrated Security=true;Connection Timeout=30;"</span>);
{"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Executes a command using the default database connection</span>
                {"\n"}    <span className="hljs-type">QueryResult</span> results1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteViewAsync</span>(<span className="hljs-string">"dbo.AllUsers"</span>);
{"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Executes a command using the connection to Database2</span>
                {"\n"}    <span className="hljs-type">QueryResult</span> results2 = <span className="hljs-keyword">await</span> database2Connection.<span className="hljs-title">ExecuteViewAsync</span>(<span className="hljs-string">"dbo.AllUsers"</span>);
{"\n"}
                {"\n"}    <span className="hljs-comment">{"//"} Executes a command using the connection to Database3</span>
                {"\n"}    <span className="hljs-type">QueryResult</span> results3 = <span className="hljs-keyword">await</span> database3Connection.<span className="hljs-title">ExecuteViewAsync</span>(<span className="hljs-string">"dbo.AllUsers"</span>);
{"\n"}{"}"}
            </code></pre>

        </>
    );
}