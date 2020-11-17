import React from 'react';

export default function Templates() {
    return (
        <>
            <h2 id="templates">Templates</h2>
            <h3>What are templates?</h3>
            <p>A template is a pattern used by DBConfirm to easily insert data into a database as part of a test.</p>
            <p>Since each test runs in isolation, all test data needs to be set up at the start of each test.  With large and complex
            databases, this could easily become unmanageable.  Templates are a way of abstracting the setup of data into strongly-typed and reusable components,
            which can be used by multiple tests.  This helps keep the tests focussed, and makes maintenance easier when changes are made to the database.
        </p>
            <p>Basic tests may require single rows of data to be inserted into tables, however as the complexity of a
            database grows,
            it may be useful to be able to represent a specific scenario or state.  This may require data to be inserted into
            multiple tables, with dependencies and foreign keys
            set.</p>
            <p>Therefore, there are two kinds of templates:</p>
            <ul>
                <li>A <a href="#simpletemplates">simple template</a> represents a single table, and allows for rows to be
                inserted into
                that
                table.</li>
                <li>A <a href="#complextemplates">complex template</a> represents a combination of other templates, allowing
                for multiple
                rows
                to be inserted across many tables at the same time.</li>
            </ul>

            <p>Simple templates can also be automatically generated based on an existing database by using <a href="#templategeneration">DBConfirm.TemplateGeneration</a>.</p>

            <h3 id="simpletemplates">Simple Templates</h3>

            <p>A simple template, representing a single table (which contains an Identity column) looks like this:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp"><span className="hljs-keyword">using</span> DBConfirm.Core.Data;
                {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                {"\n"}
                        {"\n"}<span className="hljs-keyword">namespace</span> Sample.Tests.Templates
                {"\n"}{"{"}
                        {"\n"}    <span className="hljs-comment">{'//'} The class inherits from BaseIdentityTemplate, indicating the table has an identity column</span>
                        {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">EmployeesTemplate</span> : <span className="hljs-type">BaseIdentityTemplate</span>&lt;<span className="hljs-type">EmployeesTemplate</span>&gt;
                {"\n"}    {"{"}
                        {"\n"}        <span className="hljs-comment">{'//'} Sets the schema and name of the table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> TableName =&gt; <span className="hljs-string">"[dbo].[Employees]"</span>;
                {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Sets the name of the identity column</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> IdentityColumnName =&gt; <span className="hljs-string">"EmployeeID"</span>;
                {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Defines default data that's used when this template is inserted</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-type">DataSetRow</span> DefaultData =&gt; <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                        {"\n"}        {"{"}
                        {"\n"}            [<span className="hljs-string">"FirstName"</span>] = <span className="hljs-string">"DefaultFirstName"</span>,
                {"\n"}            [<span className="hljs-string">"LastName"</span>] = <span className="hljs-string">"DefaultLastName"</span>
                        {"\n"}        {"}"};
                {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Optional fluent methods to make it easier to override specific values in the template</span>
                        {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-type">EmployeesTemplate</span> <span className="hljs-title">WithEmployeeID</span><span className="hljs-params">(<span className="hljs-keyword">int</span> value)</span> </span>=&gt; <span className="hljs-title">SetValue</span>(<span className="hljs-string">"EmployeeID"</span>, value);
                {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-type">EmployeesTemplate</span> <span className="hljs-title">WithFirstName</span><span className="hljs-params">(<span className="hljs-keyword">string</span> value)</span> </span>=&gt; <span className="hljs-title">SetValue</span>(<span className="hljs-string">"FirstName"</span>, value);
                {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-type">EmployeesTemplate</span> <span className="hljs-title">WithLastName</span><span className="hljs-params">(<span className="hljs-keyword">string</span> value)</span> </span>=&gt; <span className="hljs-title">SetValue</span>(<span className="hljs-string">"LastName"</span>, value);
                {"\n"}    {"}"}
                        {"\n"}{"}"}
                    </code></pre>

                </div>
                <div className="content-split-secondary">
                    <aside>
                        <header>Original SQL table</header>
                        <div className="aside-body">
                            <p>This example represents the <strong>dbo.Employees</strong> table in the SQL database:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Column Name</th>
                                        <th>Data Type</th>
                                        <th>Properties</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>EmployeeID</td>
                                        <td>int</td>
                                        <td>Primary Key, Identity</td>
                                    </tr>
                                    <tr>
                                        <td>FirstName</td>
                                        <td>nvarchar(50)</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>LastName</td>
                                        <td>nvarchar(50)</td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </aside>
                    <aside>
                        <header>Automatic template generation</header>
                        <div className="aside-body">
                            <p>Note that these templates can be automatically generated for you.  See <a href="#templategeneration">DBConfirm.TemplateGeneration</a> below.</p>
                        </div>
                    </aside>
                </div>
            </div>


            <p>This template can then be used in a test to insert data:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
                {"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertEmployeesTest</span>(<span className="hljs-params"></span>)
                {"\n"}</span>{"{"}
                        {"\n"}    <span className="hljs-comment">{'//'} Inserts the template, using the default data</span>
                        {"\n"}    <span className="hljs-type">EmployeesTemplate</span> defaultEmployee = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">EmployeesTemplate</span>());
                {"\n"}
                        {"\n"}    <span className="hljs-comment">{'//'} Inserts the template, overriding the FirstName value in the constructor</span>
                        {"\n"}    <span className="hljs-type">EmployeesTemplate</span> customEmployee1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">EmployeesTemplate</span>
                        {"\n"}    {"{"}
                        {"\n"}        [<span className="hljs-string">"FirstName"</span>] = <span className="hljs-string">"Custom1"</span>
                        {"\n"}    {"}"});
                {"\n"}
                        {"\n"}    <span className="hljs-comment">{'//'} Inserts the template, overriding the FirstName value using a fluent method</span>
                        {"\n"}    <span className="hljs-type">EmployeesTemplate</span> customEmployee2 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">EmployeesTemplate</span>()
                {"\n"}        .<span className="hljs-title">WithLastName</span>(<span className="hljs-string">"Custom2"</span>));
                {"\n"}    {"}"}
                    </code></pre>

                </div>
                <div className="content-split-secondary">
                    <aside>
                        <header>Inserted SQL data</header>
                        <div className="aside-body">
                            <p>Running this test would insert these rows into the <strong>dbo.Employees</strong> table:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>EmployeeID</th>
                                        <th>FirstName</th>
                                        <th>LastName</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>1</td>
                                        <td>DefaultFirstName</td>
                                        <td>DefaultLastName</td>
                                    </tr>
                                    <tr>
                                        <td>2</td>
                                        <td>Custom1</td>
                                        <td>DefaultLastName</td>
                                    </tr>
                                    <tr>
                                        <td>3</td>
                                        <td>DefaultFirstName</td>
                                        <td>Custom2</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </aside>
                    <aside>
                        <header>Note: Identities</header>
                        <div className="aside-body">
                            <p>By default, the next Identity value is generated by the SQL database, so running this test a
                            second time would result in the EmployeeIDs being 4, 5 and 6.</p>
                            <p>When a template is inserted, the generated Identity value is added back into the template, so
                            you can reference it in the rest of the test (e.g., <code>defaultEmployee.Identity</code>).
                        </p>
                            <p>Alternative, you can specify an Identity value in the test itself, and that value will be
                            used instead of generating one.</p>
                        </div>
                    </aside>
                </div>
            </div>

            <h4>Default Data</h4>

            <p>The <strong>DefaultData</strong> property is intended to provide enough default data to allow the template to be added, without any extra
            data being provided.  The only exception to this is when the table has required foreign keys to other tables - these related rows will have
            to be added before this temmplate can be inserted (either by other simple templates, or via a complex template).  In situations like this, the
            required foreign key can be marked as <coode>IsRequired</coode>, so that it is obvious what the dependent data is.  Also, by using
            the <code>IsRequired</code> placeholder, if a template is attempted to be inserted without a value having been set, you will get a specific
            test failure showing you which table and which column still requires a value.</p>

            <p>The placeholders can be found in the <code>DBConfirm.Core.Templates.Placeholders</code> namespace, and
            to mark a column as required is <code>Placeholders.IsRequired()</code>:</p>

            <pre><code className="lang-csharp"><span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-type">DataSetRow</span> DefaultData =&gt; <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                {"\n"}{"{"}
                {"\n"}    [<span className="hljs-string">"OrderID"</span>] = <span className="hljs-type">Placeholders</span>.<span className="hljs-title">IsRequired</span>(), <span className="hljs-comment">{'//'} Sets the OrderID column as required</span>
                {"\n"}    [<span className="hljs-string">"UnitPrice"</span>] = <span className="hljs-string">"123.45"</span>
                {"\n"}{"}"};
</code></pre>

            <p>Once the template has been inserted, all values that were used from <strong>DefaultData</strong> are applied to the main
template dictionary, so they can be accessed via <code>insertedTemplate["DefaultDataColumn"]</code>.  If this column has been
overridden, then the latest value will be returned, not the original default.</p>

            <h4>Non-Identity templates</h4>

            <p>If a table does not contain an Identity column, then the template class should inherit from <code>BaseSimpleTemplate{"<"}T{">"}</code>, and doesn't have an <code>IdentityColumnName</code> property:</p>


            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp"><span className="hljs-keyword">using</span> DBConfirm.Core.Data;
    {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
    {"\n"}
                        {"\n"}<span className="hljs-keyword">namespace</span> Sample.Tests.Templates
    {"\n"}{"{"}
                        {"\n"}    <span className="hljs-comment">{'//'} The class inherits from BaseSimpleTemplate</span>
                        {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">CustomersTemplate</span> : <span className="hljs-type">BaseSimpleTemplate</span>&lt;<span className="hljs-type">CustomersTemplate</span>&gt;
    {"\n"}    {"{"}
                        {"\n"}        <span className="hljs-comment">{'//'} Sets the schema and name of the table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> TableName =&gt; <span className="hljs-string">"[dbo].[Customers]"</span>;
    {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Defines default data that's used when this template is inserted</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-type">DataSetRow</span> DefaultData =&gt; <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                        {"\n"}        {"{"}
                        {"\n"}            [<span className="hljs-string">"CustomerID"</span>] = <span className="hljs-string">"C1234"</span>,
    {"\n"}            [<span className="hljs-string">"CompanyName"</span>] = <span className="hljs-string">"SampleCompanyName"</span>
                        {"\n"}        {"}"};
    {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Optional fluent methods to make it easier to override specific values in the template</span>
                        {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-type">CustomersTemplate</span> <span className="hljs-title">WithCustomerID</span><span className="hljs-params">(<span className="hljs-keyword">string</span> value)</span> </span>=&gt; <span className="hljs-title">SetValue</span>(<span className="hljs-string">"CustomerID"</span>, value);
                {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-type">CustomersTemplate</span> <span className="hljs-title">WithCompanyName</span><span className="hljs-params">(<span className="hljs-keyword">string</span> value)</span> </span>=&gt; <span className="hljs-title">SetValue</span>(<span className="hljs-string">"CompanyName"</span>, value);
                {"\n"}    {"}"}
                        {"\n"}{"}"}
                    </code></pre>

                </div>
                <div className="content-split-secondary">
                    <aside>
                        <header>Original SQL table</header>
                        <div className="aside-body">
                            <p>This example represents the <strong>dbo.Customers</strong> table in the SQL database:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Column Name</th>
                                        <th>Data Type</th>
                                        <th>Properties</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>CustomerID</td>
                                        <td>nchar(5)</td>
                                        <td>Primary Key</td>
                                    </tr>
                                    <tr>
                                        <td>CompanyName</td>
                                        <td>nvarchar(50)</td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </aside>
                </div>
            </div>


            <h3 id="complextemplates">Complex Templates</h3>

            <p>Complex templates are an easy way of combining multiple templates together, so that dependent tables can be
            inserted into at the same time.</p>

            <p>A good use case for complex templates is inserting into a table that has non-nullable foreign keys, so that
            the dependent tables don't need to be set up in each individual test.</p>

            <p>The <code>InsertAsync</code> method is used to control how the data is inserted.  Typically, a complex template is
            used when there are dependencies between the tables being used, so this method is used to make sure the tables are inserted into
            in the correct order, and the foreign keys set accordingly.</p>

            <p>A complex template, representing a scenario containing multiple tables looks like this:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp"><span className="hljs-keyword">using</span> DBConfirm.Core.Runners.Abstract;
                {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
                {"\n"}
                        {"\n"}<span className="hljs-keyword">namespace</span> Sample.Tests.Templates.Complex
                {"\n"}{"{"}
                        {"\n"}    <span className="hljs-comment">{'//'} The class inherits from BaseComplexTemplate</span>
                        {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">UserWithAddressTemplate</span> : <span className="hljs-type">BaseComplexTemplate</span>
                        {"\n"}    {"{"}
                        {"\n"}        <span className="hljs-comment">{'//'} Defines a template for the User table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">UserTemplate</span> User {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">UserTemplate</span>();
                {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Defines a template for the UserAddress table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">UserAddressTemplate</span> UserAddress {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">UserAddressTemplate</span>();
                {"\n"}
                        {"\n"}        <span className="hljs-comment">{'//'} Inserts the data based on the templates</span>
                        {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertAsync</span>(<span className="hljs-params"><span className="hljs-interface">ITestRunner</span> testRunner</span>)
                {"\n"}        </span>{"{"}
                        {"\n"}            <span className="hljs-comment">{'//'} Inserts the User template</span>
                        {"\n"}            <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(User);
                {"\n"}
                        {"\n"}            <span className="hljs-comment">{'//'} Sets the User foreign key (UserId) in the UserAddress template</span>
                        {"\n"}            <span className="hljs-comment">{'//'} with the Identity in the User template (User.Identity)</span>
                        {"\n"}            UserAddress.<span className="hljs-title">WithUserId</span>(User.Identity);
                {"\n"}
                        {"\n"}            <span className="hljs-comment">{'//'} Inserts the UserAddress template</span>
                        {"\n"}            <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(UserAddress);
                {"\n"}        {"}"}
                        {"\n"}    {"}"}
                        {"\n"}{"}"}
                    </code></pre>
                </div>
                <div className="content-split-secondary">
                    <aside>
                        <header>Original SQL tables</header>
                        <div className="aside-body">
                            <p>This example represents inserting data into 2 tables, where <strong>dbo.UserAddress</strong> has
                         a required foreign key to the <strong>dbo.Users</strong> table.</p>
                            <p>The <strong>dbo.Users</strong> table in the SQL database looks like this:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Column Name</th>
                                        <th>Data Type</th>
                                        <th>Properties</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>UserId</td>
                                        <td>int</td>
                                        <td>Primary Key, Identity</td>
                                    </tr>
                                    <tr>
                                        <td>Username</td>
                                        <td>nvarchar(50)</td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                            <p>The <strong>dbo.UserAddresses</strong> table in the SQL database looks like this:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Column Name</th>
                                        <th>Data Type</th>
                                        <th>Properties</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>UserAddressId</td>
                                        <td>int</td>
                                        <td>Primary Key, Identity</td>
                                    </tr>
                                    <tr>
                                        <td>UserId</td>
                                        <td>int</td>
                                        <td>Foreign Key, Required</td>
                                    </tr>
                                    <tr>
                                        <td>Address1</td>
                                        <td>nvarchar(50)</td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </aside>
                </div>
            </div>


            <p>This template can then be used in a test to insert data:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
                {"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertUserAddressesTest</span>(<span className="hljs-params"></span>)
                {"\n"}</span>{"{"}
                        {"\n"}    <span className="hljs-comment">{'//'} Inserts the template, using the default data for dbo.Users and dbo.UserAddresses</span>
                        {"\n"}    <span className="hljs-type">UserWithAddressTemplate</span> defaultAddress =
                {"\n"}        <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">UserWithAddressTemplate</span>());
                {"\n"}
                        {"\n"}    <span className="hljs-comment">{'//'} Inserts the template, with custom templates</span>
                        {"\n"}    <span className="hljs-type">UserWithAddressTemplate</span> customAddress =
                {"\n"}        <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">UserWithAddressTemplate</span>
                        {"\n"}        {"{"}
                        {"\n"}            User = <span className="hljs-keyword">new</span> <span className="hljs-type">UserTemplate</span>().<span className="hljs-title">WithUsername</span>(<span className="hljs-string">"CustomUsername1"</span>),
                {"\n"}            UserAddress = <span className="hljs-keyword">new</span> <span className="hljs-type">UserAddressTemplate</span>().<span className="hljs-title">WithAddress1</span>(<span className="hljs-string">"CustomAddress1"</span>)
                {"\n"}        {"}"});
                {"\n"}{"}"}
                    </code></pre>


                </div>
                <div className="content-split-secondary">
                    <aside>
                        <header>Inserted SQL data</header>
                        <div className="aside-body">
                            <p>Running this test would insert these rows into the <strong>dbo.Users</strong> table:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>UserId</th>
                                        <th>Username</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>1</td>
                                        <td>DefaultUsername</td>
                                    </tr>
                                    <tr>
                                        <td>2</td>
                                        <td>CustomUsername1</td>
                                    </tr>
                                </tbody>
                            </table>
                            <p>Also, this test would insert these rows into the <strong>dbo.UserAddresses</strong> table:
                        </p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>UserAddressId</th>
                                        <th>UserId</th>
                                        <th>Address1</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>1</td>
                                        <td>1</td>
                                        <td>DefaultAddress1</td>
                                    </tr>
                                    <tr>
                                        <td>2</td>
                                        <td>2</td>
                                        <td>CustomAddress1</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </aside>
                </div>
            </div>

            <h3 id="templategeneration">Template Generation</h3>

            <p>Simple templates are designed to exactly match their corresponding tables in the database,
                so the dotnet tool <a href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration/" target="_black">DBConfirm.TemplateGeneration</a> can
                be used to generate the C# classes for you.  These classes include all required properties for the table, and add a fluent method for each column found.</p>

            <p>To get started, you need to have created your test project, and added the relevant DBConfirm.Packages.* NuGet package.</p>
            <p>Next, using the Package Manager Console (or command line) install the tool by executing the following:</p>
            <pre><code>dotnet tool install --global DBConfirm.TemplateGeneration</code></pre>
            <p>Once installed, you can run the tool by executing <strong>GenerateTemplatesSQLServer</strong>.  There are a number of parameters you can set:</p>
            <table>
                <thead>
                    <tr>
                        <th style={{ width: "170px" }}>Property</th>
                        <th style={{ width: "70px" }}>Data Type</th>
                        <th style={{ width: "500px" }}>Description</th>
                        <th>Remarks</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>--databaseName (or -d)</td>
                        <td>String</td>
                        <td>The name of the database to use</td>
                        <td>Optional, if not specified, --connectionString must be used</td>
                    </tr>
                    <tr>
                        <td>--connectionString (or -c)</td>
                        <td>String</td>
                        <td>The connection string to use</td>
                        <td>Optional, if not specified, --databaseName is used, pointing to (local) and integrated security</td>
                    </tr>
                    <tr>
                        <td>--tableName (or -t)</td>
                        <td>String</td>
                        <td>The name of the table to process.  The name can contain wildcard characters (*) to match multiple tables within the same schema</td>
                        <td>Required</td>
                    </tr>
                    <tr>
                        <td>--schemaName (or -s)</td>
                        <td>String</td>
                        <td>The schema of the table to process</td>
                        <td>Optional, defaults to <strong>dbo</strong></td>
                    </tr>
                    <tr>
                        <td>--namespace (or -n)</td>
                        <td>String</td>
                        <td>The namespace to use for the generated class</td>
                        <td>Optional, defaults to <strong>DBConfirm.Templates</strong></td>
                    </tr>
                    <tr>
                        <td>--destination</td>
                        <td>string</td>
                        <td>The path to where the file is to be saved</td>
                        <td>Optional, defaults to the current location</td>
                    </tr>
                    <tr>
                        <td>--overwrite (or -o)</td>
                        <td>Boolean</td>
                        <td>Sets whether the target file can be overwritten if it already exists</td>
                        <td>Optional, defaults to <strong>false</strong></td>
                    </tr>
                    <tr>
                        <td>--dry-run</td>
                        <td>Boolean</td>
                        <td>Outputs the generated file to the console instead of creating a file</td>
                        <td>Optional, defaults to <strong>false</strong></td>
                    </tr>
                </tbody>
            </table>

            <h4>Example commands</h4>
            <p>To generate all tables for the local database called <strong>Northwind</strong>, outputting all files in the same location, execute:</p>
            <pre><code>GenerateTemplatesSQLServer --databaseName "Northwind" --tableName "*"</code></pre>

            <p>To generate the file for the <strong>dbo.Users</strong> table, using a custom connection string, execute:</p>
            <pre><code>GenerateTemplatesSQLServer --connectionString "SERVER=(local);DATABASE=Northwind;Integrated Security=true;Connection Timeout=30;" --tableName "Users"</code></pre>

            <p>To generate all tables into a specific directory with a custom namespace, but not overwriting existing templates, execute:</p>
            <pre><code>GenerateTemplatesSQLServer --databaseName "Northwind" --tableName "*" --destination "C:\git\SQLTests\Templates" --namespace "SQLTests.Templates" --overwrite false</code></pre>

            <h4>Updating the tool</h4>
            <p>To update the tool to the latest version, execute:</p>
            <pre><code>dotnet tool update --global DBConfirm.TemplateGeneration</code></pre>

            <h4>Uninstalling the tool</h4>
            <p>To uninstall the tool, execute:</p>
            <pre><code>dotnet tool uninstall --global DBConfirm.TemplateGeneration</code></pre>
        </>
    );
}