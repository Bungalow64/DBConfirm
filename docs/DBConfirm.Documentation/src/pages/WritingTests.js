import React from 'react';
import { Link } from 'react-router-dom';

export default function WritingTests() {
    return (
        <>
            <h2 id="writing-tests">Writing Tests</h2>
            <p>The aim of DBConfirm is to make writing tests for database logic as simple and easy as possible.
                As with other unit tests, you can use the <a href="#arrange">Arrange</a>, <a href="#act">Act</a> and <a href="#assert">Assert</a> pattern,
                and here we will go through how each step is handled using DBConfirm.</p>

            <p>As a starting point, we will assume you have already installed the correct NuGet package, set up appsettings.json and
             created a test class inheriting from the correct base class.  If you haven't done this, see the <a href="/quickstart">Quick Start</a> guide.</p>

            <p>All interaction with DBConfirm is handled through the TestRunner property which is included in the base test class.
                For a full reference, see the <Link to={{ pathname: "/api", hash: "#testrunner" }}>API Reference</Link>.</p>

            <h3 id="arrange">Arrange - set up any prerequisite test data</h3>

            <p>Most tests will need some kind of prerequisite or existing state that needs to be set up.
                In DBConfirm this is usually handled by inserting <a href="/templates">templates</a>, but can be done manually.</p>

            <h4>Inserting a template</h4>

            <p>To insert data based on a template, call <code>InsertTemplateAsync</code>:</p>

            <pre>
                <code>
                    <span className="hljs-type">EmployeesTemplate</span> employee1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">EmployeesTemplate</span>());
                </code>
            </pre>

            <p>This will insert the data from the template into the database.
            Note that an instance of a template can only be inserted once - using the same
            instance in <code>InsertTemplateAsync</code> multiple times will only ever insert the data once.
                If multiple rows need to be inserted, create multiple instances of the template
                and call <code>InsertTemplateAsync</code> for each.</p>

            <p>An generic overload exists for <code>InsertTemplateAsync</code> which inserts the default data from a template:</p>
            <pre>
                <code>
                    <span className="hljs-type">EmployeesTemplate</span> employee1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>&lt;<span className="hljs-type">EmployeesTemplate</span>&gt;();
                </code>
            </pre>

            <p><Link to={{ pathname: "/templates", hash: "#complextemplates" }}>Complex templates</Link> are inserted in the same way.
            Since a template can only be inserted once, that means we can reuse the same template if we want to refer to the same row in the database.
            For example, say we have a complex template that inserts one Address row and one Country row.  If we wanted to add 2 Address rows for the same country,
            we can insert the data like this:</p>

            <pre>
                <code className="lang-csharp"><span className="hljs-type">CountryTemplate</span> countryGB = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">CountryTemplate</span>().<span className="hljs-title">WithName</span>(<span className="hljs-string">"en-GB"</span>));
                {"\n"}
                    {"\n"}<span className="hljs-type">AddressWithCountryTemplate </span>address1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">AddressWithCountryTemplate
                {"\n"}</span>{"{"}
                    {"\n"}    Country = countryGB
                {"\n"}{"}"});
                {"\n"}<span className="hljs-type">AddressWithCountryTemplate </span>address2 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">AddressWithCountryTemplate
                {"\n"}</span>{"{"}
                    {"\n"}    Country = countryGB
                {"\n"}{"}"});
            </code></pre>

            <p>Here, <code>countryGB</code> is only inserted once, and both <code>address1</code> and <code>address2</code> reference it.</p>

            <h4>Inserting data manually</h4>

            <p>If you want to insert data manually, call <code>InsertDataAsync</code>, passing in the name of the table and <code>DataSetRow</code> object containing the data to insert:</p>

            <pre>
                <code className="lang-csharp">
                    <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Employees"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                        {"\n"}{"{"}
                        {"\n"}    [<span className="hljs-string">"FirstName"</span>] = <span className="hljs-string">"Jamie"</span>,
                {"\n"}    [<span className="hljs-string">"LastName"</span>] = <span className="hljs-string">"Burns"</span>
                        {"\n"}{"}"})</span></span>;
</code></pre>

            <h4>Identities</h4>

            <p>If a table that is being inserted into contains an Identity column, you can either let the database generate the next
                value (the default behaviour), or you can specify an Identity value to use.</p>

            <p>If you want to set the value, just set a value as you would any other column.  Just be aware that if you try and add 2 rows with the same
            Identity value, the database will throw an error.
            </p>

            <p>If the database generates an Identity value, then the value used will be added to the dataset that is returned
                from <code>InsertTemplateAsync</code> and <code>InsertDataAsync</code>.</p>


            <p>For example, when <code>InsertTemplateAsync</code> or <code>InsertDataAsync</code> is called on a table with an Identity column called '<strong>EmployeeID</strong>', then the
        value can be accessed from the returned objects.</p>

            <pre>
                <code className="lang-csharp">
                    <span className="hljs-function">
                        <span className="hljs-type">EmployeesTemplate</span> employee1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>&gt;<span className="hljs-type">EmployeesTemplate</span>&gt;();
                    {"\n"}<span className="hljs-keyword">int</span> identity1 = employee1.Identity; <span className="hljs-comment">{'//'} The Identity value can be found in the Identity property of the template</span>
                        {"\n"}
                        {"\n"}<span className="hljs-type">DataSetRow</span> employee2 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertDataAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Employees"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                            {"\n"}{"{"}
                            {"\n"}    [<span className="hljs-string">"FirstName"</span>] = <span className="hljs-string">"Jamie"</span>,
                {"\n"}    [<span className="hljs-string">"LastName"</span>] = <span className="hljs-string">"Burns"</span>
                            {"\n"}{"}"})</span></span>;
                        {"\n"}<span className="hljs-keyword">object</span> identity2 = employee2[<span className="hljs-string">"EmployeeID"</span>]; <span className="hljs-comment">{'//'} The Identity value can be found in the DataSetRow under the Identity column name</span>
                </code></pre>

            <p>The Identity values are populated immediately after being inserted.  However there may be cases where you want to reference an Identity value before the template has
                been added.  For this, the <code>IdentityResolver</code> (from the base class <code>BaseIdentityTemplate&lt;T&gt;</code>) can be used, which only attempts to access the Identity value when itself is being inserted.
            </p>

            <pre><code className="lang-csharp"><span className="hljs-comment">{'//'} Create a country, without inserting it</span>
                {"\n"}<span className="hljs-type">CountryTemplate</span> countryGB = <span className="hljs-function"><span className="hljs-keyword">new</span> <span className="hljs-type">CountryTemplate</span>().<span className="hljs-title">WithName</span>(<span className="hljs-string">"en-GB"</span>);
            {"\n"}
                    {"\n"}<span className="hljs-comment">{'//'} Create templates that depend on the Identity value, using the IdentityResolver from BaseIdentityTemplate</span>
                    {"\n"}<span className="hljs-type">AddressTemplate</span> address1 = <span className="hljs-keyword">new</span> <span className="hljs-type">AddressTemplate</span>().<span className="hljs-title">WithCountryId</span>(countryGB.IdentityResolver);
            {"\n"}<span className="hljs-type">AddressTemplate</span> address2 = <span className="hljs-keyword">new</span> <span className="hljs-type">AddressTemplate</span>().<span className="hljs-title">WithCountryId</span>(countryGB.IdentityResolver);
            {"\n"}
                    {"\n"}<span className="hljs-comment">{'//'} Templates can be inserted, and the Identity populated correctly</span>
                    {"\n"}<span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(countryGB);
            {"\n"}<span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(address1);
            {"\n"}<span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(address2);</span>
            </code></pre>


            <h3 id="act">Act - run the SQL you want to test</h3>

            <p>The functionality of the database is then triggered during the Act step.  If you're testing a stored procedure, the stored procedure is
            executed.  If you're querying a view, you're selecting from that view.  You can even run arbitrary SQL statements to trigger anything you want.
            </p>

            <p>This is all handled via the TestRunner object, which exposes a number of methods you can use for this.</p>

            <h4>Executing a stored procedure</h4>

            <p>Stored procedures can be executed via the <code>ExecuteStoredProcedure*</code> methods, depending on what data you expect to be returned.  The options are:</p>
            <ul>
                <li><strong>ExecuteStoredProcedureQueryAsync</strong> - returns a table of data, wrapped in a <code>QueryResult</code> object</li>
                <li><strong>ExecuteStoredProcedureMultipleDataSetAsync</strong> - returns a list of tables, each wrapped in a <code>QueryResult</code> object</li>
                <li><strong>ExecuteStoredProcedureScalarAsync&lt;T&gt;</strong> - returns a single object, where <code>T</code> is the type of object</li>
                <li><strong>ExecuteStoredProcedureNonQueryAsync</strong> - returns nothing</li>
            </ul>

            <p>Each method above takes the name of the stored procedure to execute (as a <code>string</code>), and an optional list of parameters.  The
            parameters can either be defined as a dictionary (where the key is the parameter name and the value is the parameter value), or a list of <code>SqlQueryParameter</code> objects,
            which have constructors to set the name and value.</p>

            <p>Examples of calling stored procedure are:</p>

            <pre><code className="lang-csharp"><span className="hljs-comment">{'//'} A stored procedure executed with a single parameter, returning a data table</span>
                {"\n"}<span className="hljs-type">QueryResult</span> data1 = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.GetCustomerDetails"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                    {"\n"}{"}"});</span></span>
                {"\n"}
                {"\n"}<span className="hljs-comment">{'//'} A stored procedure executed with multiple parameters, returning a list of data tables</span>
                {"\n"}<span className="hljs-interface">IList</span>&lt;<span className="hljs-type">QueryResult</span>&gt; data2 = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureMultipleDataSetAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.GetCustomerOrders"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>,
            {"\n"}    [<span className="hljs-string">"OrderID"</span>] = <span className="hljs-number">234</span>
                    {"\n"}{"}"});</span></span>
                {"\n"}
                {"\n"}<span className="hljs-comment">{'//'} A stored procedure executed with multiple SqlQueryParameters returning an int</span>
                {"\n"}<span className="hljs-type">ScalarResult</span>&lt;<span className="hljs-keyword">int</span>&gt; data3 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureScalarAsync</span>&lt;<span className="hljs-keyword">int</span>&gt;(<span className="hljs-string">"dbo.GetCustomerCount"</span>,
            {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"MaxOrders"</span>, <span className="hljs-number">20</span>),
            {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"MaxValue"</span>, <span className="hljs-number">30</span>));
            {"\n"}
                {"\n"}<span className="hljs-comment">{'//'} A stored procedure executed with no parameters, returning nothing</span>
                {"\n"}<span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureNonQueryAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.ProcessPendingCustomerOrders"</span>)</span></span>;
</code></pre>

            <h4>Executing a view</h4>

            <p>To select all data from a view, call <code>TestRunner.ExecuteViewAsync</code>, which returns the view data in a <code>QueryResult</code> object.</p>

            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteViewAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.ActiveCustomers"</span>);</span></span>
            </code></pre>

            <h4>Executing a arbitrary SQL statements</h4>

            <p>Executing arbitrary SQL statements work in the same way as executing stored procedures, the difference being instead of supplying the stored procedure name, the SQL statement is passed in instead.  The
            same overloads and response types are used.
            </p>

            <p>Arbitrary SQL statements can be executed via the <code>ExecuteCommand*</code> methods, depending on what data you expect to be returned.  The options are:</p>
            <ul>
                <li><strong>ExecuteCommandQueryAsync</strong> - returns a table of data, wrapped in a <code>QueryResult</code> object</li>
                <li><strong>ExecuteCommandMultipleDataSetAsync</strong> - returns a list of tables, each wrapped in a <code>QueryResult</code> object</li>
                <li><strong>ExecuteCommandScalarAsync&lt;T&gt;</strong> - returns a single object, where <code>T</code> is the type of object</li>
                <li><strong>ExecuteCommandNonQueryAsync</strong> - returns nothing</li>
            </ul>

            <pre><code className="lang-csharp"><span className="hljs-comment">{'//'} A command executed with a single parameter, returning a data table</span>
                {"\n"}<span className="hljs-type">QueryResult</span> data1 = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteCommandAsync</span><span className="hljs-params">(<span className="hljs-string">"SELECT * FROM dbo.Customers WHERE CustomerID = @CustomerID"</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>
                    {"\n"}{"}"});</span></span>
                {"\n"}
                {"\n"}<span className="hljs-comment">{'//'} A command executed with multiple parameters, returning a list of data tables</span>
                {"\n"}<span className="hljs-keyword">string</span> command = <span className="hljs-string">"UPDATE dbo.Orders SET StatusID = 3 WHERE OrderID = @OrderID;SELECT * FROM dbo.Customers WHERE CustomerID = @CustomerID;SELECT * FROM dbo.Orders WHERE OrderID = @OrderID"</span>;
                {"\n"}<span className="hljs-interface">IList</span>&lt;<span className="hljs-type">QueryResult</span>&gt; data2 = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteCommandMultipleDataSetAsync</span><span className="hljs-params">(command, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                    {"\n"}{"{"}
                    {"\n"}    [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-number">123</span>,
            {"\n"}    [<span className="hljs-string">"OrderID"</span>] = <span className="hljs-number">234</span>
                    {"\n"}{"}"});</span></span>
                {"\n"}
                {"\n"}<span className="hljs-comment">{'//'} A command executed with multiple SqlQueryParameters returning an int</span>
                {"\n"}<span className="hljs-type">ScalarResult</span>&lt;<span className="hljs-keyword">int</span>&gt; data3 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureScalarAsync</span>&lt;<span className="hljs-keyword">int</span>&gt;(<span className="hljs-string">"SELECT COUNT(*) FROM dbo.CustomerOrders WHERE OrderTotal &lt;= @MaxOrders AND OrderValue &lt;= @MaxValue"</span>,
            {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"MaxOrders"</span>, <span className="hljs-number">20</span>),
            {"\n"}    <span className="hljs-keyword">new</span> <span className="hljs-type">SqlQueryParameter</span>(<span className="hljs-string">"MaxValue"</span>, <span className="hljs-number">30</span>));
            {"\n"}
                {"\n"}<span className="hljs-comment">{'//'} A command executed with no parameters, returning nothing</span>
                {"\n"}<span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteCommandNoResultsAsync</span><span className="hljs-params">(<span className="hljs-string">"UPDATE dbo.Orders SET StatusID = 3"</span>)</span></span>;
</code></pre>

            <h3 id="assert">Assert - check the returned data, and check the state of data in the database</h3>

            <p>Once any test data has been set up, and the functionality triggered, you can assert that the test is successful.  Either you can make assertions on the data
                returned from a stored procedure or view (from the Act phase), or you can query the state of data in tables in the database.</p>

            <p>To get the state of data within a specific table, you can call <code>TestRunner.ExecuteTableAsync</code>, to return a <code>QueryResult</code> object containing all data from that table.</p>

            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-function"><span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteTableAsync</span><span className="hljs-params">(<span className="hljs-string">"dbo.Customers"</span>);</span></span>
            </code></pre>

            <p>Data returned from the database is either a <code>QueryResult</code> or <code>ScalarResult&lt;T&gt;</code> object, both of which have assertion methods which can be called.</p>

            <h4>ScalarResult&lt;T&gt; assertions</h4>

            <p><code>ScalarResult&lt;T&gt;</code> represents a single object, so there is just one assertion method, called <code>AssertValue</code>.  This is used to test whether the value is what you expect.</p>

            <pre><code className="lang-csharp"><span className="hljs-type">ScalarResult</span>&lt;<span className="hljs-keyword">int</span>&gt; result = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureScalarAsync</span>&lt;<span className="hljs-keyword">int</span>&gt;(<span className="hljs-string">"dbo.GetCount"</span>);
                {"\n"}<span className="hljs-comment">{'//'} Tests whether the value matches 100</span>
                {"\n"}<span className="hljs-literal">result</span>.<span className="hljs-type">AssertValue</span>(<span className="hljs-number">100</span>);
</code></pre>

            <p>To access the value itself, you can call <code>ScalarResult&lt;T&gt;.RawData</code>, which returns the value itself in case you need to do further assertions on it.</p>

            <pre><code className="lang-csharp"><span className="hljs-type">ScalarResult</span>&lt;<span className="hljs-keyword">int</span>&gt; result = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureScalarAsync</span>&lt;<span className="hljs-keyword">int</span>&gt;(<span className="hljs-string">"dbo.GetCount"</span>);
            {"\n"}<span className="hljs-comment">{'//'} Gets the value as an int</span>
                {"\n"}<span className="hljs-keyword">int</span> resultValue = result.RawData;
</code></pre>

            <h4>QueryResult assertions</h4>

            <p>A <code>QueryResult</code> object contains multiple rows of data, and there are many assertion methods to verify that the data is as you expect it to be.  The assertions can be split into
            3 sections - columns, rows and values.</p>

            <p>All assertion methods are fluent, so you can chain multiple assertions together.</p>

            <p>The columns within a <code>QueryResult</code> can be verified, to check that the correct columns have been returned from a given query.</p>

            <ul>
                <li><strong>AssertColumnCount</strong> - asserts the number of columns</li>
                <li><strong>AssertColumnExists</strong> - asserts that a specific column exists</li>
                <li><strong>AssertColumnNotExists</strong> - asserts that a specific column does not exist</li>
                <li><strong>AssertColumnsExist</strong> - given multiple columns, asserts that all exist</li>
                <li><strong>AssertColumnsNotExist</strong> - given multiple columns, asserts that non exist</li>
            </ul>

            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.GetCustomerData"</span>);
            {"\n"}
                {"\n"}data
            {"\n"}    .<span className="hljs-title">AssertColumnCount</span>(<span className="hljs-number">3</span>) <span className="hljs-comment">{'//'} Asserts that there are 3 columns</span>
                {"\n"}    .<span className="hljs-title">AssertColumnExists</span>(<span className="hljs-string">"CustomerID"</span>) <span className="hljs-comment">{'//'} Asserts that there is a column called 'CustomerID'</span>
                {"\n"}    .<span className="hljs-title">AssertColumnNotExists</span>(<span className="hljs-string">"OrderID"</span>) <span className="hljs-comment">{'//'} Asserts that there is no column called 'OrderID'</span>
                {"\n"}    .<span className="hljs-title">AssertColumnsExist</span>(<span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"LastName"</span>) <span className="hljs-comment">{'//'} Asserts that there are columns called 'FirstName' and 'LastName'</span>
                {"\n"}    .<span className="hljs-title">AssertColumnsNotExist</span>(<span className="hljs-string">"TotalValue"</span>, <span className="hljs-string">"Quantity"</span>); <span className="hljs-comment">{'//'} Asserts that there are no columns called 'TotalValue' and 'Quantity'</span>
            </code></pre>

            <p>The rows within a <code>QueryResult</code> can be verified, to check that the correct data is present.</p>

            <ul>
                <li><strong>AssertRowCount</strong> - asserts the number of rows</li>
                <li><strong>AssertRowExists</strong> - asserts that at least one row matches the values supplied</li>
                <li><strong>AssertRowDoesNotExist</strong> - asserts that no rows match the values supplied</li>
                <li><strong>AssertRowPositionExists</strong> - asserts that a row exists at the given position (zero-based)</li>
                <li><strong>AssertRowValues</strong> - asserts the values of a specific row match the values supplied</li>
                <li><strong>AssertValue</strong> - asserts the value of a specific column in a specific row matches the expected value</li>
            </ul>

            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> data = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span className="hljs-string">"dbo.GetCustomerData"</span>);
            {"\n"}
                {"\n"}data
            {"\n"}    .<span className="hljs-title">AssertRowCount</span>(<span className="hljs-number">3</span>)  <span className="hljs-comment">{'//'} Asserts that there are 3 rows</span>
                {"\n"}    .<span className="hljs-title">AssertRowExists</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}    {"{"}
                {"\n"}        {"{"} <span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"Jamie"</span> {"}"} <span className="hljs-comment">{'//'} Asserts that a row exists where the 'FirstName' column has a value of 'Jamie'</span>
                {"\n"}    {"}"})
            {"\n"}    .<span className="hljs-title">AssertRowDoesNotExist</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}    {"{"}
                {"\n"}        {"{"} <span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"Jimmy"</span> {"}"} <span className="hljs-comment">{'//'} Asserts that no rows have a 'FirstName' column with a value of 'Jimmy'</span>
                {"\n"}    {"}"})
            {"\n"}    .<span className="hljs-title">AssertRowPositionExists</span>(<span className="hljs-number">2</span>) <span className="hljs-comment">{'//'} Asserts that there is a row at position 2 (zero-based)</span>
                {"\n"}    .<span className="hljs-title">AssertRowValues</span>(<span className="hljs-number">2</span>, <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}    {"{"}
                {"\n"}        {"{"} <span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"Jamie"</span> {"}"} <span className="hljs-comment">{'//'} Asserts that the 'FirstName' column in row 2 has a value of 'Jamie'</span>
                {"\n"}    {"}"})
            {"\n"}    .<span className="hljs-title">AssertValue</span>(<span className="hljs-number">2</span>, <span className="hljs-string">"FirstName"</span>, <span className="hljs-string">"Jamie"</span>); <span className="hljs-comment">{'//'} Asserts that the 'FirstName' column in row 2 has a value of 'Jamie'</span>
            </code></pre>

            <p>The values within a specific row can also be asserted using <code>QueryResult.ValidateRow</code>, which returns a <code>RowResult</code> object containing
            assertion methods that are focussed on just that individual row.</p>

            <ul>
                <li><strong>AssertValue</strong> - asserts the value of a specific column matches the expected value</li>
                <li><strong>AssertValues</strong> - asserts the values of specific columns match the expected values</li>
            </ul>

            <pre><code class="lang-csharp"><span className="hljs-type">QueryResult</span> <span class="hljs-built_in">data</span> = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span class="hljs-string">"dbo.GetCustomerData"</span>);
            {"\n"}
                {"\n"}<span class="hljs-built_in">data</span>
                {"\n"}    .<span className="hljs-title">ValidateRow</span>(<span class="hljs-number">2</span>) <span className="hljs-comment">{'//'} Focusses the assertions on row number 2</span>
                {"\n"}        .<span className="hljs-title">AssertValue</span>(<span class="hljs-string">"FirstName"</span>, <span class="hljs-string">"Jamie"</span>)  <span className="hljs-comment">{'//'} Asserts that the 'FirstName' column has a value of 'Jamie'</span>
                {"\n"}        .<span className="hljs-title">AssertValues</span>(<span class="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}        {"{"}
                {"\n"}            {"{"} <span class="hljs-string">"FirstName"</span>, <span class="hljs-string">"Jamie"</span> {"}"},
            {"\n"}            {"{"} <span class="hljs-string">"LastName"</span>, <span class="hljs-string">"Burns"</span> {"}"}  <span className="hljs-comment">{'//'} Asserts that the 'FirstName' and 'LastName' columns have values of 'Jamie' and 'Burns'</span>
                {"\n"}        {"}"});
</code></pre>

            <p>To access the actual data within a <code>QueryResult</code> itself, you can call <code>RawData</code>, which returns the actual DataTable in case you need to do further assertions on it.</p>

            <pre><code class="lang-csharp"><span className="hljs-type">QueryResult</span> <span class="hljs-built_in">data</span> = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span class="hljs-string">"dbo.GetCustomerData"</span>);
            {"\n"}<span className="hljs-comment">{'//'} Gets the data as a DataTable</span>
                {"\n"}<span className="hljs-type">DataTable</span> actualData = data.RawData;
</code></pre>

        <p>By default, all assertions use object equality comparisons, which are case sensitive and type dependent.  For example, when asserting that a value matches a specific string, the assertion
            will fail if the case is different.  Also, when asserting that a value matches an integer, the assertion will fail if the data type returned from the database is any other type than an 
            integer (e.g., decimal).  In these cases, make sure the value you are asserting with is of the same type (e.g., use a decimal value when comparing a value returned as a decimal).
        </p>

        <p>You can customise how a comparison is made, by using an instance of an object that inherits from <code>DBConfirm.Core.Comparisons.Abstract.IComparison</code>.  This interface
        specifies an <code>Assert</code> and <code>Validate</code> method, which should perform the desired comparison.  The <code>Assert</code> method should trigger the framework-dependent Assert methods, so
        that the test fails immediately if the comparison conditions are not met.  The <code>Validate</code> method should not fail the test, but instead return <code>true</code>/<code>false</code> depending
        on whether the conditions are met.</p>

        <p>DBConfirm provides a number of comparisons that can be used directly, accessed via the static class <code>DBConfirm.Core.Comparisons.ExpectedData</code>.  these
        comparison objects can be used in any assertion method, including within <code>DataSetRow</code> objects.</p>

        <ul>
            <li><strong>HasLength</strong> - Asserts the length of a string value</li>
            <li><strong>IsDay</strong> - Asserts that a date matches a specific day, ignoring time.  There's one overload for a <code>DateTime</code> object, and one for a <code>string</code> which is parsed using <code>DateTime.Parse</code></li>
            <li><strong>IsDateTime</strong> - Asserts that a date matches a specific day, including time.  A default precision of 1 second is used, which can be overridden, which provides room for variance.  There's one overload for a <code>DateTime</code> object, and one for a <code>string</code> which is parsed using <code>DateTime.Parse</code></li>
            <li><strong>IsUtcNow</strong> - Asserts that a date matches the current UTC date and time.  A default precision of 1 second is used, which can be overridden, which provides room for variance.</li>
            <li><strong>IsNull</strong> - Asserts the value is null</li>
            <li><strong>IsNotNull</strong> - Asserts the value is not null</li>
            <li><strong>MatchesRegex</strong> - Asserts the value matches the Regex value</li>
            <li><strong>NotMatchesRegex</strong> - Asserts that the value does not match the Regex value</li>
        </ul>

        <pre><code class="lang-csharp"><span class="hljs-type">QueryResult</span> data = <span class="hljs-keyword">await</span> TestRunner.<span class="hljs-title">ExecuteStoredProcedureQueryAsync</span>(<span class="hljs-string">"dbo.GetCount"</span>);
        {"\n"}
        {"\n"}data
        {"\n"}    .<span class="hljs-title">ValidateRow</span>(<span class="hljs-number">2</span>) <span class="hljs-comment">{'//'} Focusses the assertions on row number 2</span>
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the value is 5 characters long</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"FirstName"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">HasLength</span>(<span class="hljs-number">5</span>))
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the date is 01-Feb-2020 (ignoring time)</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"StartDate"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsDay</span>(<span class="hljs-type">DateTime</span>.<span class="hljs-title">Parse</span>(<span class="hljs-string">"01-Feb-2020"</span>)))
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the date is 01-Feb-2020 at 9am, with a default precision of 1 second</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"StartDate"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsDateTime</span>(<span class="hljs-type">DateTime</span>.<span class="hljs-title">Parse</span>(<span class="hljs-string">"01-Feb-2020 09:00:00"</span>)))
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the date is 01-Feb-2020 at 9am, with a precision of 10 seconds</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"StartDate"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsDateTime</span>(<span class="hljs-type">DateTime</span>.<span class="hljs-title">Parse</span>(<span class="hljs-string">"01-Feb-2020 09:00:00"</span>), <span class="hljs-type">TimeSpan</span>.<span class="hljs-title">FromSeconds</span>(<span class="hljs-number">10</span>)))
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the date matches UtcNow, with a default precision of 1 second</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"StartDate"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsUtcNow</span>())
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the date matches UtcNow, with a precision of 10 seconds</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"StartDate"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsUtcNow</span>(<span class="hljs-type">TimeSpan</span>.<span class="hljs-title">FromSeconds</span>(<span class="hljs-number">10</span>)))
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the value is null</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"FirstName"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsNull</span>())
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the value is not null</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"FirstName"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">IsNotNull</span>())
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the value matches the Regex value</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"EmailAddress"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">MatchesRegex</span>(<span class="hljs-string">".*@.*"</span>))
        {"\n"}    <span class="hljs-comment">{'//'} Asserts that the value does not match the Regex value</span>
        {"\n"}    .<span class="hljs-title">AssertValue</span>(<span class="hljs-string">"FirstName"</span>, <span class="hljs-type">ExpectedData</span>.<span class="hljs-title">NotMatchesRegex</span>(<span class="hljs-string">".*@.*"</span>));
</code></pre>

        </>
    );
}