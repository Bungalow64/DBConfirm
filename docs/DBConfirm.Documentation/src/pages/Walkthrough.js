import React from 'react';
import './Walkthrough.scss';
import dbtables from './images/walkthrough/db-tables.png';
import dbprocedures from './images/walkthrough/db-procedures.png';
import projectinstall from './images/walkthrough/project-install.png';
import projectaddedcmd from './images/walkthrough/project-added-cmd.png';
import projectaddedexplorer from './images/walkthrough/project-added-explorer.png';
import projectfilesexplorer from './images/walkthrough/project-files-explorer.png';
import projectaddedvs from './images/walkthrough/project-added-vs.png';
import settingsdefault from './images/walkthrough/settings-default.png';
import settingsupdated from './images/walkthrough/settings-updated.png';
import dbprocedure from './images/walkthrough/db-procedure.png';
import codedefaultclass from './images/walkthrough/code-default-class.png';
import coderenamedmethod from './images/walkthrough/code-renamed-method.png';
import vstest1 from './images/walkthrough/vs-test1.png';
import vstest1pass from './images/walkthrough/vs-test1-pass.png';
import vstest1fail from './images/walkthrough/vs-test1-fail.png';
import vstest2pass from './images/walkthrough/vs-test2-pass.png';
import vstest2fail from './images/walkthrough/vs-test2-fail.png';
import projectnewtemplate from './images/walkthrough/project-new-template.png';
import templatetoolinstalled from './images/walkthrough/template-tool-installed.png';
import templatetoolexecuted from './images/walkthrough/template-tool-executed.png';
import vstemplatesadded from './images/walkthrough/vs-templates-added.png';
import vstemplateaddedclass from './images/walkthrough/vs-template-added-class.png';
import vscomplextemplateadd from './images/walkthrough/vs-complextemplate-add.png';
import vsdatadriventestpass from './images/walkthrough/vs-data-driven-test-pass.png';
import azurecipass from './images/walkthrough/azure-ci-pass.png';

export default function Walkthrough() {
    return (
        <div class="image-content">
            <h2 id="Walkthrough">Walkthrough</h2>

            <p>If you want to see how all the different parts of DBConfirm work together, this walkthrough should help.  We're going to go through every from setting up
            the database to set, to adding the test projects, all the way to writing tests and getting it set up in Azure DevOps.
            </p>

            <p>This walkthrough is going to cover:</p>
            <ul>
                <li><a href="#the-database-to-test">The database to test</a></li>
                <li><a href="#creating-the-test-project">Creating the test project</a></li>
                <li><a href="#writing-a-test">Writing a test</a></li>
                <li><a href="#add-test-data-manually">Adding test data (manually)</a></li>
                <li><a href="#add-test-data-simple-templates">Adding test data (with simple templates)</a></li>
                <li><a href="#automatically-generating-templates">Automatically generating templates</a></li>
                <li><a href="#add-test-data-complex-templates">Adding test data (with complex templates)</a></li>
                <li><a href="#data-driven-tests">Adding a data-driven test</a></li>
                <li><a href="#including-in-ci">Including in a Continuous Integration (CI) build</a></li>
            </ul>
            <h3 id="the-database-to-test">The database to test</h3>

            <p>We're going to be testing the sample Northwind database.  If you want to code along with this walkthrough, you can install the
                database using the <a href="https://github.com/Bungalow64/DBConfirm/blob/sprint/tests/SampleDatabases/northwind_schema_setup.sql">Northwind set up script</a>.  This
                script has no dependencies, you just need to open a new query window, copy that script in, and execute it.
            </p>

            <div className="content-split">

                <div className="content-split-primary">
                    <p>This database has a number of stored procedures, and we'll be writing tests to check that these all work as expected.</p>
                    <img src={dbprocedures} alt="The stored procedures within the Northwind database" />
                </div>
                <aside>
                    <header>Northwind tables</header>
                    <div className="aside-body">
                        <p>This database has a number of tables, and we'll be inserting into these to set up the different test scenarios.</p>
                        <img src={dbtables} alt="The tables within the Northwind database" />
                    </div>
                </aside>
            </div>

            <h3 id="creating-the-test-project">Creating the test project</h3>

            <p>Now that we've got the database set up locally, we can create the test project.</p>
            <p>Usually you'll be adding this project to an existing solution, but for this walkthrough we'll assume you have nothing set up.</p>

            <p>In your file explorer, create a new folder for your solution (I'll call it NorthwindApp), and open up a command prompt there.</p>

            <p>Now we're going to use a DBConfirm template package to create the project for us, so that we don't need to set everything up by hand.  For
            this, you need to first install the template locally.  We need to decide whether we use MSTest or NUnit, so for this project, we'll use
            MSTest.  To install this, run this command:
            </p>

            <pre>
                <code>
                    dotnet new -i DBConfirm.Templates.SQLServer.MSTest
                        </code>
            </pre>

            <p>You should see the latest version has been installed:</p>
            <img src={projectinstall} alt="The project template being installed" />

            <p>We'll now create the test project called NorthwindApp.Tests, by running this command:</p>

            <pre>
                <code>
                    dotnet new dbconfirm-sqlserver-mstest -n "NorthwindApp.Tests"
                </code>
            </pre>

            <p>You should be told that the project has been created:</p>
            <img src={projectaddedcmd} alt="The command to add the project was successful" />

            <p>And if you look in the file explorer, a folder called 'NorthindApp.Tests' has been created:</p>
            <img src={projectaddedexplorer} alt="The project folder has been added" />

            <p>Inside this folder there will be a NorthwindApp.Tests.csproj file:</p>
            <img src={projectfilesexplorer} alt="The default files added to the new project" />

            <p>Let's open that project file in Visual Studio:</p>
            <img src={projectaddedvs} alt="The project opened in Visual Studio" />

            <p>The first thing we need to do is update the connetion string to point to our own database.  Open up appsettings.json, and you'll see a connection string:</p>
            <img src={settingsdefault} alt="The default connection string" />

            <p>Our database is in (local), but is called Northwind, so update the connection string so that our database will be found:</p>
            <img src={settingsupdated} alt="The connection string updated to use the Northwind database" />

            <p>Now we can start looking at the tests themselves.</p>

            <h3 id="writing-a-test">Writing a test</h3>

            <p>We'll start off by writing some tests for the <code>dbo.CustOrderHist</code> procedure, which looks like this:</p>
            <img src={dbprocedure} alt="The dbo.CustOrderHist procedure to be tested" />

            <p>This procedure lists all products purchased by a specific customer, with the total quantity shown for each.  It'll involve data from
                the <code>dbo.Products</code>, <code>dbo.[Order Details]</code>, <code>dbo.Orders</code> and <code>dbo.Customers</code> tables.
            </p>

            <p>Our first test will be to check what happens if we just run that procedure with no test data set up.  We'd expect nothing to be returned, so let's check that.</p>

            <p>Looking at the project, we have a UnitTest1.cs file.  We'll rename this so we've got a test class we can work with for this procedure.  We like to
                have a different test class for each procedure, so let's rename it to be <code>CustOrderHistTests.cs</code>, and open it up:
            </p>

            <img src={codedefaultclass} alt="The default test class and method created" />

            <p>We can see that we've already got the <code>[TestClass]</code> attribute on the class
            (for MSTest), and the class inherits from <code>MSTestBase</code>.  There's a test already
            created, with the <code>[TestMethod]</code> attribute, and set up to be async
            with <code>async Task</code>.</p>

            <p>We'll rename this test to be our first test for checking what happens when the procedure
            is called with no existing data.  Let's call it NoData_ReturnNoRows:
            </p>
            <img src={coderenamedmethod} alt="Default method renamed to be NoData_ReturnNoRows" />

            <div className="content-split">

                <div className="content-split-primary">

                    <p>Now, for this test, we don't need to set any data up, so we can skip the Arrange phase and
                    go straight to Act.  We need to execute the stored procedure, with
                a <code>CustomerId</code> parameter, and get the data returned:
            </p>

                    <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                        {"\n"}{"{"}
                        {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                        {"\n"}{"}"});</span></span>
                    </code>
                    </pre>

                </div>
                <aside>
                    <header>Import namespaces</header>
                    <div className="aside-body">

                        <p>Note that you'll need to import a couple of namespaces, to be able to use <code>QueryResult</code> and <code>DataSetRow</code>.  If Visual Studio doesn't
            automatically add them for you, just add <code>using DBConfirm.Core.Data;</code> and <code>using DBConfirm.Core.DataResults;</code> to the top of your class.</p>

                    </div>
                </aside>
            </div>

            <p>This means we're executing the <code>dbo.CustOrderHist</code> procedure, where we expect
            a single table of data to be returned.  The procedure also is given a parameter called
            'CustomerID', set with a dummy value of 123.  Because the <code>ExecuteStoredProcedureQueryAsync</code> method
            is <code>async</code> we need to use <code>await</code>, and it returns a <code>QueryResult</code> object, which
            contains the data returned from the procedure.  Next, we need to run some
            assertions on this data.</p>

            <p>We expect no data to be returned from the procedure, so we can use one of the assertion methods
                on <code>QueryResult</code> to check this:
            </p>

            <pre><code className="lang-csharp">data
            {"\n"}    .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">0</span>);
            </code></pre>

            <p>This will check that the returned data contains 0 rows.  If it finds 1 or more rows, then
            the test will fail.
            </p>

            <p>Putting this all together, we have a test that looks like this:</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">NoData_ReturnNoRows</span>()
            {"\n"}{"{"}
                {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">0</span>);
                    {"\n"}{"}"}
            </code>
            </pre>

            <p>We'll now run this test, and make sure it passes.  Open up the Test Explorer in Visual
                Studio, and expand the test sections:</p>
            <img src={vstest1} alt="Our first test shown in the Test Explorer" />

            <p>Run the test, and we'll see it pass:</p>
            <img src={vstest1pass} alt="Our first test has passed" />

            <p>Now, just to double check that our test isn't just passing regardless, change the test
                assertion to check that there is 1 row, by calling <code>.AssertRowCount(1);</code>.  When
                we run the test now, it should fail, with a message telling us what's wrong:
            </p>
            <img src={vstest1fail} alt="Our first test has failed" />
            <p>Great, the test tells us that it is expecting a total row count of 1, but it found 0.  Now
                revert that last change, so we're calling <code>.AssertRowCount(0);</code>, and
            check the test passes.</p>

            <p>Next, we need to test what happens when there's some data for the procedure to
                return.  We'll add this data manually for this test.</p>

            <h3 id="add-test-data-manually">Adding test data (manually)</h3>

            <p>Let's add a new test in the same class called SingleOrder_ReturnOrderDetails, to test
            what happens when a customer has 1 order.  We'll start with this:
            </p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">SingleOrder_ReturnOrderDetails</span>()
            {"\n"}{"{"}
                {"\n"}
                {"\n"}{"}"}
            </code>
            </pre>

            <p>Firstly, we need to set up some test data.  This procedure needs a row in 4 different tables, and there's a couple of
                foreign keys in there, so we need to do these inserts in the correct order.</p>

            <p>We'll start with the <code>dbo.Customers</code> table.  This table only has 2 required columns - <code>CustomerID</code> and <code>CompanyName</code>, so
            we'll populate these:</p>

            <pre><code className="lang-csharp"><span className="hljs-function"><span className="hljs-type">DataSetRow</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Customers"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}{"{"}
                {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>,
                    {"\n"}    [<span className="hljs-string">"CompanyName"</span>] = <span className="hljs-string">"Company1"</span>
                {"\n"}{"}"});</span></span>
            </code>
            </pre>

            <p>This will insert 1 row into the <code>dbo.Customers</code> table, with the <code>CustomerID</code>
            and <code>CompanyName</code> set.</p>
            <p>We can do the same with <code>dbo.Products</code>, which only requires a <code>ProductName</code> column:</p>

            <pre><code className="lang-csharp"><span className="hljs-function"><span className="hljs-type">DataSetRow</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Products"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}{"{"}
                {"\n"}    [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>
                {"\n"}{"}"});</span></span>
            </code>
            </pre>

            <p>Next is the <code>dbo.Orders</code> table, and whilst this doesn't have any columns that need to be populated, we
            want to set the <code>CustomerID</code> column, so that it's linked to the customer we've just added:</p>

            <pre><code className="lang-csharp"><span className="hljs-function"><span className="hljs-type">DataSetRow</span> order = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Orders"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}{"{"}
                {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-string">"123"</span>
                {"\n"}{"}"});</span></span>
            </code>
            </pre>

            <p>Finally we need to add the <code>dbo.[Order Details]</code> row.  This table requires the <code>OrderID</code> and <code>ProductID</code> columns
            to be set, but these need to be the Identity values used in the <code>dbo.Orders</code> and <code>dbo.Products</code> tables.  Fortunately, when DBConfirm inserts into
            a table that has an Identity column, the Identity value used is returned, so we can use that.  We also want to set the <code>Quantity</code> column, since we'll be
            verifying that this number is correct when the stored procedure is executed:</p>

            <pre><code className="lang-csharp"><span className="hljs-function"><span className="hljs-type">DataSetRow</span> orderDetail = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.[Order Details]"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}{"{"}
                {"\n"}    [<span className="hljs-string">"OrderID"</span>] = order[<span className="hljs-string">"OrderID"</span>],
                {"\n"}    [<span className="hljs-string">"ProductID"</span>] = product[<span className="hljs-string">"ProductID"</span>],
                {"\n"}    [<span className="hljs-string">"Quantity"</span>] = <span className="hljs-number">5</span>
                {"\n"}{"}"});</span></span>
            </code>
            </pre>

            <p>The pre-condition data has now been set up, so we can execute the stored procedure and check that the correct data is returned.  What we've expect is when we execute the procedure
                for the customer with a <code>CustomerID</code> of 123, that we get 1 row back, for the 'Product1' product, and a quantity of 5:
            </p>

            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}{"{"}
                {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                {"\n"}{"}"});</span></span>
                {"\n"}
                {"\n"}data
                    {"\n"}    .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                    {"\n"}    .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">0</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}    {"{"}
                {"\n"}        [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>,
                    {"\n"}        [<span className="hljs-string">"Total"</span>] = <span className="hljs-number">5</span>
                {"\n"}    {"}"});
            </code>
            </pre>

            <p>This will check that there's exactly 1 row returned from the procedure, and that the <code>ProductName</code> and <code>Total</code> columns have the correct values.</p>
            <p>Putting all this together, we have a test that looks like this;</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">SingleOrder_ReturnOrderDetails</span>()
            {"\n"}{"{"}

                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Customers"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>,
                    {"\n"}        [<span className="hljs-string">"CompanyName"</span>] = <span className="hljs-string">"Company1"</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Products"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> order = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Orders"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-string">"123"</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> orderDetail = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.[Order Details]"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"OrderID"</span>] = order[<span className="hljs-string">"OrderID"</span>],
                {"\n"}        [<span className="hljs-string">"ProductID"</span>] = product[<span className="hljs-string">"ProductID"</span>],
                {"\n"}        [<span className="hljs-string">"Quantity"</span>] = <span className="hljs-number">5</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                    {"\n"}        .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">0</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}        {"{"}
                {"\n"}            [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>,
                    {"\n"}            [<span className="hljs-string">"Total"</span>] = <span className="hljs-number">5</span>
                {"\n"}        {"}"});

                {"\n"}{"}"}
            </code>
            </pre>

            <p>Run this test, and we'll see it passes:</p>
            <img src={vstest2pass} alt="The second test is passing" />

            <p>Again, to check that the test is not passing regardless, change the expected <code>Total</code> to be 6 instead of 5, by setting <code>["Total"] = 6</code>, and we'll see
            the test fail:</p>
            <img src={vstest2fail} alt="The second test is failing" />
            <p>Here you can see the failure message indicating that the 'Total' column in row 0 has an unexpected value, where it expected 6 but found 5.  Great.  Now revert this change back to 5, and verify
                that the test passes again.</p>

            <p>Looking at the test that we've just written though, if we had to set up the same kind of data for other tests, there'd be a lot of duplicated code in our tests, and it would be a real pain
            to maintain going forward.  Fortunately, DBConfirm has a solution to this, and it's to use Templates to add data to tables.  Next, we'll update this test to use simple templates
            to reduce the amount of code needed in each test, and make the tests a little more readable.
            </p>

            <h3 id="add-test-data-simple-templates">Adding test data (with simple templates)</h3>

            <p>Simple templates are designed to represent one table each, so for this test, we'll write 4 templates.</p>
            <p>We'll start with <code>dbo.Customer</code>.</p>
            <p>Create a new folder in your test project called 'Templates', and add a new class called CustomersTemplate.</p>
            <img src={projectnewtemplate} alt="New template file has been added to the project, under a Templates folder" />

            <p>Open the class file.  Since this <code>dbo.Customers</code> table does not have an Identity column, we can inherit from the <code>BaseSimpleTemplate</code> class, in the <code>DBConfirm.Core.Templates</code> namespace.  This
            base class is generic, so we need to pass in the type of the current class.</p>

            <pre>
                <code>
                    <span className="hljs-keyword">using</span> DBConfirm.Core.Data;
                    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                    {"\n"}
                    {"\n"}<span className="hljs-keyword">namespace</span> NorthwindApp.Tests.Templates
                    {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">CustomersTemplate</span> : <span className="hljs-type">BaseSimpleTemplate</span>&lt;<span className="hljs-type">CustomersTemplate</span>&gt;
                    {"\n"}    {"{"}
                    {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>There are 2 abstract methods that we need to override.  The first is <code>TableName</code>, and this is simply the schema and name of the table that is to be inserted into.  For this
            template, that's just <code>dbo.Customers</code>:</p>
            <pre>
                <code>
                    <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> TableName =&gt; <span className="hljs-string">"[dbo].[Customers]"</span>;
                </code>
            </pre>

            <p>The next is <code>DefaultData</code>, where we specify the minimum default values that are needed to insert the template.  This would include any columns that are non-nullable, or columns that
            make sense to always be populated, depending on what the table is.  For this table, we must populate the <code>CustomerId</code> and <code>CompanyName</code> columns - everything else is fine as null.</p>

            <p>This default data is set up by creating a new <code>DataSetRow</code> object:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-type">DataSetRow</span> DefaultData =&gt; <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-string">"123"</span>,
                {"\n"}    [<span className="hljs-string">"CompanyName"</span>] = <span className="hljs-string">"Company1"</span>
                    {"\n"}{"}"};
                </code>
            </pre>

            <p>Putting this all together, results in a class that looks like this:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">using</span> DBConfirm.Core.Data;
                    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                    {"\n"}
                    {"\n"}<span className="hljs-keyword">namespace</span> NorthwindApp.Tests.Templates
                    {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">CustomersTemplate</span> : <span className="hljs-type">BaseSimpleTemplate</span>&lt;<span className="hljs-type">CustomersTemplate</span>&gt;
                    {"\n"}    {"{"}

                    {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> TableName =&gt; <span className="hljs-string">"[dbo].[Customers]"</span>;
                    {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-type">DataSetRow</span> DefaultData =&gt; <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}        {"{"}
                    {"\n"}            [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-string">"123"</span>,
                    {"\n"}            [<span className="hljs-string">"CompanyName"</span>] = <span className="hljs-string">"Company1"</span>
                    {"\n"}        {"}"};

                    {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>This template means that when it is added, a new row is inserted into the <code>dbo.Customers</code> table, with a <code>CustomerID</code> of "123" and a <code>CompanyName</code> of "Company1".</p>

            <p>Within a test, we can use this template by calling <code>TestRunner.InsertTemplateAsync</code>:</p>

            <pre>
                <code className="lang-csharp">
                    <span className="hljs-type">CustomersTemplate</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>{"<"}<span className="hljs-type">CustomersTemplate</span>{">"}();
                </code>
            </pre>

            <p>So, adding this into our test results in:</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">SingleOrder_ReturnOrderDetails</span>()
            {"\n"}{"{"}

                {"\n"}    <span className="hljs-type">CustomersTemplate</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>{"<"}<span className="hljs-type">CustomersTemplate</span>{">"}();
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Products"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> order = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Orders"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-string">"123"</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> orderDetail = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.[Order Details]"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"OrderID"</span>] = order[<span className="hljs-string">"OrderID"</span>],
                {"\n"}        [<span className="hljs-string">"ProductID"</span>] = product[<span className="hljs-string">"ProductID"</span>],
                {"\n"}        [<span className="hljs-string">"Quantity"</span>] = <span className="hljs-number">5</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                    {"\n"}        .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">0</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}        {"{"}
                {"\n"}            [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>,
                    {"\n"}            [<span className="hljs-string">"Total"</span>] = <span className="hljs-number">5</span>
                {"\n"}        {"}"});

                {"\n"}{"}"}
            </code>
            </pre>

            <p>That looks a bit better - we now don't need to set the <code>CompanyName</code> in the test itself, and just let the default value be used.  However, now that we're not setting
            the <code>CustomerID</code> in the test, it's not entirely clear what the <code>["CustomerID"] = "123"</code> line is doing when creating the order, and why the <code>CustomerID</code> parameter is set as 123 when executing the
            procedure.  Since that "123" value is supposed to match the customer that's been added, it's best to make that intention clear.  We also don't want this test to start failing if the default values used in
            the template is changed.  So, we have 2 options.  We can either hardcode the <code>CustomerID</code> value in the customers template from within this test itself, or we can reference the value of <code>CustomerID</code> that
            has been set in the template.  In this test, since we don't care what the actual value is to be used, as long as the relationships work, we'll reference the template value.  Therefore, we'll
            update everywhere that references the <code>CustomerID</code> of "123" with <code>customer["CustomerID"]</code>:</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">SingleOrder_ReturnOrderDetails</span>()
            {"\n"}{"{"}

                {"\n"}    <span className="hljs-type">CustomersTemplate</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>{"<"}<span className="hljs-type">CustomersTemplate</span>{">"}();
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Products"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> order = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Orders"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = customer[<span className="hljs-string">"CustomerID"</span>]
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-function"><span className="hljs-type">DataSetRow</span> orderDetail = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.[Order Details]"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"OrderID"</span>] = order[<span className="hljs-string">"OrderID"</span>],
                {"\n"}        [<span className="hljs-string">"ProductID"</span>] = product[<span className="hljs-string">"ProductID"</span>],
                {"\n"}        [<span className="hljs-string">"Quantity"</span>] = <span className="hljs-number">5</span>
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = customer[<span className="hljs-string">"CustomerID"</span>]
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                    {"\n"}        .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">0</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}        {"{"}
                {"\n"}            [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>,
                    {"\n"}            [<span className="hljs-string">"Total"</span>] = <span className="hljs-number">5</span>
                {"\n"}        {"}"});

                {"\n"}{"}"}
            </code>
            </pre>

            <p>Great, we've now got fewer variables in the test, and it's clear what tables are referencing what.  Next, we'll want to create templates for the other tables.</p>
            <p>However, instead of adding them manually, we can use a DBConfirm tool to generate them for us, which would be much quicker.</p>

            <h3 id="automatically-generating-templates">Automatically generating templates</h3>

            <p>This tool is a donet tool, so it needs to be installed before it can be used.  So, back in the command line, execute this command:</p>

            <pre>
                <code>
                    dotnet tool install --global DBConfirm.TemplateGeneration
                </code>
            </pre>

            <p>You should see a confirmation that the tool has been installed, telling you that the tool can be run by executing the 'GenerateTemplatesSQLServer' command:</p>

            <img src={templatetoolinstalled} alt="Confirmation that the template generation tool has been successful" />

            <p>This tool can be used to generate a single template, or templates for all tables found in the database.  We might as well generate all of them.  We also have
            the option to overwrite any templates that we have already written - we might as well do that as well, so that the one we've just written is made consistent with the
            new templates.
            </p>

            <p>There are a few options we need to set to run the tool.  <code>--databaseName</code> is the name of the database.  If we set this, the tool will look for
            a database in (local) with that name.  We'll set this to "Northwind".  We need to set <code>--tableName</code> to be "*" to find all tables.  We need to set <code>--namespace</code>
            to be "NorthwindApp.Tests.Templates" so that it matches the rest of our project, and we need to set <code>--destination</code> to be "C:\Git\NorthwindApp\NorthwindApp.Tests\Templates" so that the
            files are added to our project.  Finally, we need to set <code>--overwrite</code> to be true so that our existing template is overwritten.</p>

            <p>This results in a command like this:</p>
            <pre><code>GenerateTemplatesSQLServer --databaseName "Northwind" --tableName "*" --namespace "NorthwindApp.Tests.Templates" --destination "C:\Git\NorthwindApp\NorthwindApp.Tests\Templates" --overwrite true</code></pre>

            <p>Execute this command, and the templates will be generated:</p>

            <img src={templatetoolexecuted} alt="Templates have been generated" />

            <p>And if you take a look in your 'Templates' folder in your project, a template has been added for each table in the Northwind database:</p>

            <img src={vstemplatesadded} alt="Template classes added to the project" />

            <p>If you open <code>CustomersTemplate.cs</code>, you'll see what a generated template looks like:</p>

            <img src={vstemplateaddedclass} alt="A generated template" />

            <p>You'll see that the <code>TableName</code> property is the same, and the <code>DefaultData</code> has been set up to populate
            the same columns, but with different default data.  You'll also see that there are methods for each column in the table, which allows you to set these columns fluently, and avoids
            having to hardcode the column name as a string in the test itself.  Let's update our test to use these new templates.</p>
            <p>First up is <code>dbo.Products</code>.  We'll use the <code>ProductsTemplate</code> template to add this, but we'll also hardcode the <code>ProductName</code> to be "Product1", since
            this data is actually something we're interested in.</p>

            <pre>
                <code>
                    <span className="hljs-type">ProductsTemplate</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                    {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>()
                    {"\n"}        .<span className="hljs-title">WithProductName</span>(<span className="hljs-string">"Product1"</span>));
                </code>
            </pre>

            <p>Next, is <code>dbo.Orders</code>:</p>

            <pre>
                <code>
                    <span className="hljs-type">OrdersTemplate</span> order = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
        {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
        {"\n"}        .<span className="hljs-title">WithCustomerID</span>(customer[<span className="hljs-string">"CustomerID"</span>] <span className="hljs-keyword">as string</span>));
    </code>
            </pre>

            <p>Then, finally, <code>dbo.[Order Details]</code>:</p>

            <pre>
                <code>
                    <span className="hljs-type">Order_DetailsTemplate</span> orderDetail = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
{"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
{"\n"}        .<span className="hljs-title">WithOrderID</span>(order.Identity)
{"\n"}        .<span className="hljs-title">WithProductID</span>(product.Identity)
{"\n"}        .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">5</span>));
</code>
            </pre>

            <p>Note that with the template for <code>dbo.[Order Details]</code>, the Identity values for <code>dbo.Orders</code> and <code>dbo.Products</code> can be accessed by the <code>.Identity</code> property.</p>

            <p>Putting all this together results in a test that looks like this:</p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">SingleOrder_ReturnOrderDetails</span>()
            {"\n"}{"{"}

                {"\n"}    <span className="hljs-type">CustomersTemplate</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>{"<"}<span className="hljs-type">CustomersTemplate</span>{">"}();
                {"\n"}
                {"\n"}    <span className="hljs-type">ProductsTemplate</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>()
                {"\n"}            .<span className="hljs-title">WithProductName</span>(<span className="hljs-string">"Product1"</span>));
                {"\n"}
                {"\n"}    <span className="hljs-type">OrdersTemplate</span> order = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
                {"\n"}            .<span className="hljs-title">WithCustomerID</span>(customer[<span className="hljs-string">"CustomerID"</span>] <span className="hljs-keyword">as string</span>));
                {"\n"}
                {"\n"}    <span className="hljs-type">Order_DetailsTemplate</span> orderDetail = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
                {"\n"}            .<span className="hljs-title">WithOrderID</span>(order.Identity)
                {"\n"}            .<span className="hljs-title">WithProductID</span>(product.Identity)
                {"\n"}            .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">5</span>));
                {"\n"}
                {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = customer[<span className="hljs-string">"CustomerID"</span>]
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                    {"\n"}        .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">0</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}        {"{"}
                {"\n"}            [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>,
                    {"\n"}            [<span className="hljs-string">"Total"</span>] = <span className="hljs-number">5</span>
                {"\n"}        {"}"});

                {"\n"}{"}"}
            </code>
            </pre>

            <p>That's getting better, there are fewer hardcoded column names in there, and only the values relevant to the test have been set.  However, if we were to write another test that involves this kind
            of data, there's still quite a bit of code that would need to be duplicated.  This is where complex templates come in.  Next, we'll create a complex template that represents an entire order (including related
            data), to minimise what we need to write in each test.
            </p>

            <h3 id="add-test-data-complex-templates">Adding test data (with complex templates)</h3>

            <p>Firstly, we'll create a new folder within 'Templates' called 'CustomTemplates', and add a new class called 'CompleteOrderForCustomerTemplate.cs':</p>

            <img src={vscomplextemplateadd} alt="Added class for the complex template" />

            <p>The purpose of this template is to insert the data for the 4 tables involved in an order (<code>dbo.Customers</code>, <code>dbo.Products</code>, <code>dbo.Orders</code> and <code>dbo.[Order Details]</code>), and
            make sure the related columns are populated correctly.</p>

            <p>So we'll start off by inheriting the class from the <code>BaseComplexTemplate</code> class, in the <code>DBConfirm.Core.Templates</code> namespace, and including a couple of other namespaces we'll need:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">using</span> DBConfirm.Core.Runners.Abstract;
                    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                    {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
                    {"\n"}
                    {"\n"}<span className="hljs-keyword">namespace</span> NorthwindApp.Tests.Templates.CustomTemplates
                    {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span> : <span className="hljs-type">BaseComplexTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>Next, let's add a property for each simple template we want to deal with, instantiating each to a new instance of the template:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">using</span> DBConfirm.Core.Runners.Abstract;
                    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                    {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
                    {"\n"}
                    {"\n"}<span className="hljs-keyword">namespace</span> NorthwindApp.Tests.Templates.CustomTemplates
                    {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span> : <span className="hljs-type">BaseComplexTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">CustomersTemplate</span> CustomersTemplate {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">CustomersTemplate</span>();
                    {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">ProductsTemplate</span> ProductsTemplate {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>();
                    {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">OrdersTemplate</span> OrdersTemplate {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>();
                    {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">Order_DetailsTemplate</span> Order_DetailsTemplate {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>();
                    {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>There is only one method that we need to implement - <code>InsertAsync</code>.  This method is run when the data is being inserted, so it will contain the logic of the template:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertAsync</span>(<span className="hljs-interface">ITestRunner</span> testRunner)
                {"\n"}{"{"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>Firstly, we'll insert the <code>dbo.Customers</code> and <code>dbo.Products</code> data:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertAsync</span>(<span className="hljs-interface">ITestRunner</span> testRunner)
                {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(CustomersTemplate);
                {"\n"}    <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(ProductsTemplate);
                {"\n"}{"}"}
                </code>
            </pre>

            <p>Next, we need to insert <code>dbo.Orders</code> data.  This template requires the <code>CustomerID</code> column to be set, so we need to grab the <code>CustomerID</code> value
            from the inserted <code>dbo.Customers</code> template.  However, one of the principles of a complex template is that the same simple template can be re-used across multiple complex
            templates (for example, the same <code>CustomersTemplate</code> instance used multiple times so that the same customer is re-used). So, we only want to set the <code>CustomerID</code> if
            the template has not already been added.  We can do this by checking the <code>.IsInserted</code> property on the template:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertAsync</span>(<span className="hljs-interface">ITestRunner</span> testRunner)
                {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(CustomersTemplate);
                {"\n"}    <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(ProductsTemplate);
                {"\n"}
                    {"\n"}    <span className="hljs-logic">if</span> (!OrdersTemplate.IsInserted)
                {"\n"}    {"{"}
                    {"\n"}        OrdersTemplate
                {"\n"}            .<span className="hljs-title">WithCustomerID</span>(CustomersTemplate[<span className="hljs-string">"CustomerID"</span>] <span className="hljs-keyword">as string</span>);
                {"\n"}
                    {"\n"}        <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(OrdersTemplate);
                {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>Finally, we can do include <code>dbo.[Order Details]</code> in the same way:</p>

            <pre>
                <code>
                    <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertAsync</span>(<span className="hljs-interface">ITestRunner</span> testRunner)
                {"\n"}{"{"}
                    {"\n"}    <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(CustomersTemplate);
                {"\n"}    <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(ProductsTemplate);
                {"\n"}
                    {"\n"}    <span className="hljs-logic">if</span> (!OrdersTemplate.IsInserted)
                {"\n"}    {"{"}
                    {"\n"}        OrdersTemplate
                {"\n"}            .<span className="hljs-title">WithCustomerID</span>(CustomersTemplate[<span className="hljs-string">"CustomerID"</span>] <span className="hljs-keyword">as string</span>);
                {"\n"}
                    {"\n"}        <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(OrdersTemplate);
                {"\n"}    {"}"}
                    {"\n"}
                    {"\n"}    <span className="hljs-logic">if</span> (!Order_DetailsTemplate.IsInserted)
                {"\n"}    {"{"}
                    {"\n"}        Order_DetailsTemplate
                {"\n"}            .<span className="hljs-title">WithOrderID</span>(OrdersTemplate.Identity)
                {"\n"}            .<span className="hljs-title">WithProductID</span>(ProductsTemplate.Identity);
                {"\n"}
                    {"\n"}        <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(Order_DetailsTemplate);
                {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>We now have a complex template that we can use and customise within a test.  Let's add this complex template to the start of our test:</p>

            <pre>
                <code>
                    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> completeOrder = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                    {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>());
                </code>
            </pre>

            <p>This will use default values for the 4 simple templates inside it, so we want to set the custom data that we need for this test:</p>

            <pre>
                <code>
                    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> completeOrder = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                    {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}        ProductsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>().<span className="hljs-title">WithProductName</span>(<span className="hljs-string">"Product1"</span>),
                {"\n"}        Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>().<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">5</span>)
                {"\n"}    {"}"});
                </code>
            </pre>

            <p>Since all the relationships are handled within the complex template, we don't need to set anything like that in the test.</p>
            <p>We can now remove the insertions of the simple templates from our test.  The last change we need to make is to change where the parameter value for the procedure comes from.  We
                need to update this to come from the complex template, so replace <code>["CustomerID"] = customer["CustomerID"]</code> with <code>["CustomerID"] = completeOrder.CustomersTemplate["CustomerID"]</code>.  Our
                final test should now look like this:
            </p>

            <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
            {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">SingleOrder_ReturnOrderDetails</span>()
            {"\n"}{"{"}

                {"\n"}    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> completeOrder = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(
                {"\n"}        <span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                {"\n"}        {"{"}
                {"\n"}            ProductsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>().<span className="hljs-title">WithProductName</span>(<span className="hljs-string">"Product1"</span>),
                {"\n"}            Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>().<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">5</span>)
                {"\n"}        {"}"});
                {"\n"}
                {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.CustOrderHist"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CustomerID"</span>] = completeOrder.CustomersTemplate[<span className="hljs-string">"CustomerID"</span>]
                    {"\n"}    {"}"});</span></span>
                {"\n"}
                {"\n"}    data
                    {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                    {"\n"}        .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">0</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}        {"{"}
                {"\n"}            [<span className="hljs-string">"ProductName"</span>] = <span className="hljs-string">"Product1"</span>,
                    {"\n"}            [<span className="hljs-string">"Total"</span>] = <span className="hljs-number">5</span>
                {"\n"}        {"}"});

                {"\n"}{"}"}
            </code>
            </pre>

            <p>This is now much cleaner, and is keeping the test focussed on what the test is dealing with, and everything else is centralised within the templates.</p>

            <p>Next, we'll see how we can use what we've learnt to add a more complicated test, with some data-driven inputs.</p>

            <h3 id="data-drive-tests">Adding a data-driven test</h3>
            <div className="content-split">

                <div className="content-split-primary">

                    <p>Next, we want to test one of the other procedures - <code>dbo.SalesByCategory</code>.  This procedure calculates the total purchase cost of orders, after
            applying discounts.
            </p>
                    <p>This procedure is ideal for a data-driven test, if we set up a specific data set, and test the results of the procedure depending on the parameters used.</p>

                </div>
                <aside>
                    <header>Years</header>
                    <div className="aside-body">

                        <p>Note that there's some logic in this procedure around dates (this database is old), so it really only works with dates between 1996 and 1998.  Hands up
            if you weren't even born then.</p>

                    </div>
                </aside>
            </div>

            <p>Add a new test class called 'SalesByCategoryTests.cs', and add a new test called 'OrdersAcrossYears_TotalPurchaseIsCorrect':</p>

            <pre>
                <code>
                    <span className="hljs-keyword">using</span> DBConfirm.Core.Data;
                    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.DataResults;
                    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Packages.SQLServer.MSTest;
                {"\n"}<span className="hljs-keyword">using</span> Microsoft.VisualStudio.TestTools.UnitTesting;
                {"\n"}<span className="hljs-keyword">using</span> NorthwindApp.Tests.Templates;
                {"\n"}<span className="hljs-keyword">using</span> NorthwindApp.Tests.Templates.CustomTemplates;
                {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
                {"\n"}<span className="hljs-keyword">using</span> System;
                {"\n"}
                    {"\n"}<span className="hljs-keyword">namespace</span> NorthwindApp.Tests
                {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-type">TestClass</span>]
                {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">SalesByCategoryTests</span> : <span className="hljs-type">MSTestBase</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-type">TestMethod</span>]
                {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">OrdersAcrossYears_TotalPurchaseIsCorrect</span>()
                {"\n"}        {"{"}
                    {"\n"}
                    {"\n"}        {"}"}
                    {"\n"}    {"}"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>To make this a data test, we need to use <code>[DataTestMethod]</code> instead of <code>[TestMethod]</code>, and add the variables into the method signature.  For this
            test, we'll have 2 parameters - one for the year, and another for the expected total:</p>

            <pre>
                <code>[<span className="hljs-type">DataTestMethod</span>]
                {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">OrdersAcrossYears_TotalPurchaseIsCorrect</span>(<span className="hljs-keyword">int</span> year, <span className="hljs-keyword">int</span> expectedTotal)
                {"\n"}{"{"}
                    {"\n"}
                    {"\n"}{"}"}
                </code>
            </pre>

            <p>Next, we need to insert a product with a category, and a customer.  We're going to use this same customer and product for 3 different orders (one in each year), so we'll insert these using simple templates:</p>

            <pre>
                <code>[<span className="hljs-type">DataTestMethod</span>]
                {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">OrdersAcrossYears_TotalPurchaseIsCorrect</span>(<span className="hljs-keyword">int</span> year, <span className="hljs-keyword">int</span> expectedTotal)
                {"\n"}{"{"}
                    {"\n"}    <span className="hljs-type">CustomersTemplate</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>&lt;<span className="hljs-type">CustomersTemplate</span>&gt;();
                {"\n"}
                    {"\n"}    <span className="hljs-type">CategoriesTemplate</span> category = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CategoriesTemplate</span>()
                {"\n"}        .<span className="hljs-title">WithCategoryName</span>(<span className="hljs-string">"Category1"</span>));
                {"\n"}
                    {"\n"}    <span className="hljs-type">ProductsTemplate</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>()
                {"\n"}        .<span className="hljs-title">WithProductName</span>(<span className="hljs-string">"Product1"</span>)
                {"\n"}        .<span className="hljs-title">WithCategoryID</span>(category.Identity));
                {"\n"}{"}"}
                </code>
            </pre>

            <p>Next, we can use our complex template to insert some orders, using these templates.  We'll add 4 orders:</p>
            <ul>
                <li>One for 1996, with a unit price of 10 and quantity of 10</li>
                <li>One for 1997, with a unit price of 10 and quantity of 5</li>
                <li>One for 1998, with a unit price of 10 and quantity of 8</li>
                <li>Another one for 1998, with a unit price of 10 and quantity of 8 but with a discount of 50%</li>
            </ul>

            <pre>
                <code><span className="hljs-type">CompleteOrderForCustomerTemplate</span> order1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}{"{"}
                    {"\n"}    CustomersTemplate = customer,
                    {"\n"}    ProductsTemplate = product,
            {"\n"}    OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1996"</span>)), <span className="hljs-comment">{"//"} Set the date as 1996</span>
                    {"\n"}    Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}        .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the quantity as 10</span>
                    {"\n"}        .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0</span>)
            {"\n"}{"}"});
                {"\n"}
                    {"\n"}<span className="hljs-type">CompleteOrderForCustomerTemplate</span> order2 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}{"{"}
                    {"\n"}    CustomersTemplate = customer,
                    {"\n"}    ProductsTemplate = product,
            {"\n"}    OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1997"</span>)), <span className="hljs-comment">{"//"} Set the date as 1997</span>
                    {"\n"}    Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}        .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">5</span>) <span className="hljs-comment">{"//"} Set the quantity as 5</span>
                    {"\n"}        .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0</span>)
            {"\n"}{"}"});
                {"\n"}
                    {"\n"}<span className="hljs-type">CompleteOrderForCustomerTemplate</span> order3 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}{"{"}
                    {"\n"}    CustomersTemplate = customer,
                    {"\n"}    ProductsTemplate = product,
            {"\n"}    OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1998"</span>)), <span className="hljs-comment">{"//"} Set the date as 1998</span>
                    {"\n"}    Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}        .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">8</span>) <span className="hljs-comment">{"//"} Set the quantity as 8</span>
                    {"\n"}        .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0</span>)
            {"\n"}{"}"});
                {"\n"}
                    {"\n"}<span className="hljs-type">CompleteOrderForCustomerTemplate</span> order4 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}{"{"}
                    {"\n"}    CustomersTemplate = customer,
                    {"\n"}    ProductsTemplate = product,
            {"\n"}    OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1998"</span>)), <span className="hljs-comment">{"//"} Set the date as 1998</span>
                    {"\n"}    Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}        .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}        .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">8</span>) <span className="hljs-comment">{"//"} Set the quantity as 8</span>
                    {"\n"}        .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0.5f</span>) <span className="hljs-comment">{"//"} Set the discount as 50% (set as a float)</span>
                    {"\n"}{"}"});
                </code>
            </pre>

            <p>Now the test data is set up, we can execute the procedure, using the <code>year</code> parameter:</p>

            <pre>
                <code>
                    <span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.[SalesByCategory]"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-string">"CategoryName"</span>] = <span className="hljs-string">"Category1"</span>,
                {"\n"}    [<span className="hljs-string">"OrdYear"</span>] = year
                {"\n"}{"}"});
                </code>
            </pre>

            <div className="content-split">

                <div className="content-split-primary">

                    <p>Next, we can add the assertion, this time based on the <code>expectedTotal</code> parameter:</p>

                    <pre>
                        <code>
                            data
                {"\n"}    .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                {"\n"}    .<span className="hljs-title">AssertValue</span>(<span className="hljs-number">0</span>, <span className="hljs-string">"TotalPurchase"</span>, <span className="hljs-type">Convert</span>.<span className="hljs-title">ToDecimal</span>(expectedTotal));
                </code>
                    </pre>
                </div>
                <aside>
                    <header>Asserting data types</header>
                    <div className="aside-body">

                        <p>Note that the value of <code>expectedTotal</code> needs to be converted to a decimal to be asserted correctly.  If the type of <code>expectedTotal</code> was
                        left as an <code>int</code> then the assertion would fail because the database is returning a decimal number, and all assertions are type specific.</p>

                    </div>
                </aside>
            </div>

            <p>Finally, we can add the actual test cases.  We want to assert that:</p>
            <ul>
                <li>Given the year 1996, the total is 100</li>
                <li>Given the year 1997, the total is 50</li>
                <li>Given the year 1998, the total is 120</li>
            </ul>

            <p>We do that by adding <code>[DataRow]</code> attributes to the test:</p>

            <pre>
                <code>[<span className="hljs-type">DataTestMethod</span>]
                {"\n"}[<span className="hljs-type">DataRow</span>(<span className="hljs-number">1996</span>, <span className="hljs-number">100</span>)]
                {"\n"}[<span className="hljs-type">DataRow</span>(<span className="hljs-number">1997</span>, <span className="hljs-number">50</span>)]
                {"\n"}[<span className="hljs-type">DataRow</span>(<span className="hljs-number">1998</span>, <span className="hljs-number">120</span>)]
                {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">OrdersAcrossYears_TotalPurchaseIsCorrect</span>(<span className="hljs-keyword">int</span> year, <span className="hljs-keyword">int</span> expectedTotal)
                {"\n"}{"{"}
                    {"\n"}    ...
                {"\n"}{"{"}
                </code>
            </pre>

            <p>The test should now look like this:</p>

            <pre>
                <code>[<span className="hljs-type">DataTestMethod</span>]
                {"\n"}[<span className="hljs-type">DataRow</span>(<span className="hljs-number">1996</span>, <span className="hljs-number">100</span>)]
                {"\n"}[<span className="hljs-type">DataRow</span>(<span className="hljs-number">1997</span>, <span className="hljs-number">50</span>)]
                {"\n"}[<span className="hljs-type">DataRow</span>(<span className="hljs-number">1998</span>, <span className="hljs-number">120</span>)]
                {"\n"}<span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">OrdersAcrossYears_TotalPurchaseIsCorrect</span>(<span className="hljs-keyword">int</span> year, <span className="hljs-keyword">int</span> expectedTotal)
                {"\n"}{"{"}

                    {"\n"}    <span className="hljs-type">CustomersTemplate</span> customer = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>&lt;<span className="hljs-type">CustomersTemplate</span>&gt;();
                {"\n"}
                    {"\n"}    <span className="hljs-type">CategoriesTemplate</span> category = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CategoriesTemplate</span>()
                {"\n"}        .<span className="hljs-title">WithCategoryName</span>(<span className="hljs-string">"Category1"</span>));
                {"\n"}
                    {"\n"}    <span className="hljs-type">ProductsTemplate</span> product = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">ProductsTemplate</span>()
                {"\n"}        .<span className="hljs-title">WithProductName</span>(<span className="hljs-string">"Product1"</span>)
                {"\n"}        .<span className="hljs-title">WithCategoryID</span>(category.Identity));
                {"\n"}
                    {"\n"}    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> order1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}        CustomersTemplate = customer,
                    {"\n"}        ProductsTemplate = product,
            {"\n"}        OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1996"</span>)), <span className="hljs-comment">{"//"} Set the date as 1996</span>
                    {"\n"}        Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}            .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the quantity as 10</span>
                    {"\n"}            .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0</span>)
            {"\n"}    {"}"});
                {"\n"}
                    {"\n"}    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> order2 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}        CustomersTemplate = customer,
                    {"\n"}        ProductsTemplate = product,
            {"\n"}        OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1997"</span>)), <span className="hljs-comment">{"//"} Set the date as 1997</span>
                    {"\n"}        Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}            .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">5</span>) <span className="hljs-comment">{"//"} Set the quantity as 5</span>
                    {"\n"}            .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0</span>)
            {"\n"}    {"}"});
                {"\n"}
                    {"\n"}    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> order3 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}        CustomersTemplate = customer,
                    {"\n"}        ProductsTemplate = product,
            {"\n"}        OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1998"</span>)), <span className="hljs-comment">{"//"} Set the date as 1998</span>
                    {"\n"}        Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}            .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">8</span>) <span className="hljs-comment">{"//"} Set the quantity as 8</span>
                    {"\n"}            .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0</span>)
            {"\n"}    {"}"});
                {"\n"}
                    {"\n"}    <span className="hljs-type">CompleteOrderForCustomerTemplate</span> order4 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CompleteOrderForCustomerTemplate</span>
                    {"\n"}    {"{"}
                    {"\n"}        CustomersTemplate = customer,
                    {"\n"}        ProductsTemplate = product,
            {"\n"}        OrdersTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">OrdersTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithOrderDate</span>(<span className="hljs-type">DateTime</span>.<span className="hljs-title">Parse</span>(<span className="hljs-string">"05-Mar-1998"</span>)), <span className="hljs-comment">{"//"} Set the date as 1998</span>
                    {"\n"}        Order_DetailsTemplate = <span className="hljs-keyword">new</span> <span className="hljs-type">Order_DetailsTemplate</span>()
            {"\n"}            .<span className="hljs-title">WithUnitPrice</span>(<span className="hljs-number">10</span>) <span className="hljs-comment">{"//"} Set the unit price as 10</span>
                    {"\n"}            .<span className="hljs-title">WithQuantity</span>(<span className="hljs-number">8</span>) <span className="hljs-comment">{"//"} Set the quantity as 8</span>
                    {"\n"}            .<span className="hljs-title">WithDiscount</span>(<span className="hljs-number">0.5f</span>) <span className="hljs-comment">{"//"} Set the discount as 50% (set as a float)</span>
                    {"\n"}    {"}"});
                    {"\n"}
                    {"\n"}    <span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.[SalesByCategory]"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}    {"{"}
                    {"\n"}        [<span className="hljs-string">"CategoryName"</span>] = <span className="hljs-string">"Category1"</span>,
                {"\n"}        [<span className="hljs-string">"OrdYear"</span>] = year
                {"\n"}    {"}"});
                {"\n"}
                    {"\n"}    data
                {"\n"}        .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">1</span>)
                {"\n"}        .<span className="hljs-title">AssertValue</span>(<span className="hljs-number">0</span>, <span className="hljs-string">"TotalPurchase"</span>, <span className="hljs-type">Convert</span>.<span className="hljs-title">ToDecimal</span>(expectedTotal));
               {"\n"}{"}"}
                </code>
            </pre>

            <p>Now, if you run the tests, you should see the test pass all 4 scenarios:</p>

            <img src={vsdatadriventestpass} alt="Data-driven tests have passed" />

            <p>Next, we're going to want to run these tests as part of an Azure DevOps CI build.</p>

            <h3 id="including-in-ci">Including in a Continuous Integration (CI) build</h3>

            <p>Since the DBConfirm tests we've written so far are all using MSTest, a CI build will run the tests during a standard .NET Core test task.  However, we will
                still need to configure the build to use the correct connection string.
            </p>
            <p>For this walkthrough I'm using Azure DevOps, so I'll go ahead and commit this testing project in.</p>
            <p>Now, I'm going to set up a CI build purely to build and run this test project.  I'll use a YAML build:</p>

            <pre>
                <code className="lang-yaml">
                <span className="hljs-attr">trigger</span>:
                {"\n"}- <span className="hljs-value">master</span>
                {"\n"}
                {"\n"}<span className="hljs-attr">variables</span>:
                {"\n"}  <span className="hljs-attr">DefaultConnectionString</span>: <span className="hljs-value">'SERVER=B64-VM-BUILD;DATABASE=Northwind;Integrated Security=true;Connection Timeout=30;'</span>
                {"\n"}  <span className="hljs-attr">BuildConfiguration</span>: <span className="hljs-value">'Debug'</span>
                {"\n"}
                {"\n"}<span className="hljs-attr">pool</span>: <span className="hljs-value">'Default'</span>
                {"\n"}
                {"\n"}<span className="hljs-attr">steps</span>:
                {"\n"}- <span className="hljs-attr">task</span>: <span className="hljs-value">DotNetCoreCLI@2</span>
                {"\n"}  <span className="hljs-attr">displayName</span>: <span className="hljs-value">Build</span>
                {"\n"}  <span className="hljs-attr">inputs</span>:
                {"\n"}    <span className="hljs-attr">projects</span>: <span className="hljs-value">'**/*.csproj'</span>
                {"\n"}    <span className="hljs-attr">arguments</span>: <span className="hljs-value">'--configuration $(BuildConfiguration)'</span>
                {"\n"}
                {"\n"}- <span className="hljs-attr">task</span>: <span className="hljs-value">DotNetCoreCLI@2</span>
                {"\n"}  <span className="hljs-attr">displayName</span>: <span className="hljs-value">Test</span>
                {"\n"}  <span className="hljs-attr">inputs</span>:
                {"\n"}    <span className="hljs-attr">command</span>: <span className="hljs-value">test</span>
                {"\n"}    <span className="hljs-attr">projects</span>: <span className="hljs-value">'**/*Tests/*.csproj'</span>
                {"\n"}    <span className="hljs-attr">arguments</span>: <span className="hljs-value">'--configuration $(BuildConfiguration)'</span>
                </code>
            </pre>

            <p>The key part of this build is the <code>DefaultConnectionString</code> variable.  This tells DBConfirm where it can find the database to test against.  In
            this case, it's looking for the Northwind database on the B64-VM-BUILD server.  Running this build executes all tests, and they pass:</p>

            <img src={azurecipass} alt="CI build has completed, passing 3 tests"/>

            <p>And that's it!  We've set a testing project up, added tests that have arrange, act and assert steps, and included the tests as part of a CI build.</p>
            <p>Hopefully this will give you an idea of how you can approach database testing using DBConfirm.</p>
        </div >
    );
}