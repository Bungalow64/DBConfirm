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


            <h3 id="simpletemplates">Simple Templates</h3>

            <p>A simple template, representing a single table looks like this:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp"><span className="hljs-keyword">using</span> DBConfirm.Core.Data;
                {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                {"\n"}
                        {"\n"}<span className="hljs-keyword">namespace</span> Sample.Tests.Templates
                {"\n"}{"{"}
                        {"\n"}    <span className="hljs-comment">// The class inherits from BaseIdentityTemplate, indicating the table has an identity column</span>
                        {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">EmployeesTemplate</span> : <span className="hljs-type">BaseIdentityTemplate</span>&lt;<span className="hljs-type">EmployeesTemplate</span>&gt;
                {"\n"}    {"{"}
                        {"\n"}        <span className="hljs-comment">// Sets the schema and name of the table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> TableName =&gt; <span className="hljs-string">"[dbo].[Employees]"</span>;
                {"\n"}
                        {"\n"}        <span className="hljs-comment">// Sets the name of the identity column</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">string</span> IdentityColumnName =&gt; <span className="hljs-string">"EmployeeID"</span>;
                {"\n"}
                        {"\n"}        <span className="hljs-comment">// Defines default data that's used when this template is inserted</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-type">DataSetRow</span> DefaultData =&gt; <span className="hljs-keyword">new</span> <span className="hljs-type">DataSetRow</span>
                        {"\n"}        {"{"}
                        {"\n"}            [<span className="hljs-string">"FirstName"</span>] = <span className="hljs-string">"DefaultFirstName"</span>,
                {"\n"}            [<span className="hljs-string">"LastName"</span>] = <span className="hljs-string">"DefaultLastName"</span>
                        {"\n"}        {"}"};
                {"\n"}
                        {"\n"}        <span className="hljs-comment">// Optional fluent methods to make it easier to override specific values in the template</span>
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
                </div>
            </div>

            <p>This template can then be used in a test to insert data:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp">[<span className="hljs-type">TestMethod</span>]
                {"\n"}<span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertEmployeesTest</span>(<span className="hljs-params"></span>)
                {"\n"}</span>{"{"}
                        {"\n"}    <span className="hljs-comment">// Inserts the template, using the default data</span>
                        {"\n"}    <span className="hljs-type">EmployeesTemplate</span> defaultEmployee = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">EmployeesTemplate</span>());
                {"\n"}
                        {"\n"}    <span className="hljs-comment">// Inserts the template, overriding the FirstName value in the constructor</span>
                        {"\n"}    <span className="hljs-type">EmployeesTemplate</span> customEmployee1 = <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">EmployeesTemplate</span>
                        {"\n"}    {"{"}
                        {"\n"}        [<span className="hljs-string">"FirstName"</span>] = <span className="hljs-string">"Custom1"</span>
                        {"\n"}    {"}"});
                {"\n"}
                        {"\n"}    <span className="hljs-comment">// Inserts the template, overriding the FirstName value using a fluent method</span>
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

            <h3 id="complextemplates">Complex Templates</h3>

            <p>Complex templates are an easy way of combining multiple templates together, so that dependent tables can be
            inserted into at the same time.</p>

            <p>A good use case for complex templates is inserting into a table that has non-nullable foreign keys, so that
            the dependent tables don't need to be set up in each individual test.</p>

            <p>A complex template, representing a scenario containing multiple tables looks like this:</p>

            <div className="content-split">
                <div className="content-split-primary">

                    <pre><code className="lang-csharp"><span className="hljs-keyword">using</span> DBConfirm.Core.Runners.Abstract;
                {"\n"}<span className="hljs-keyword">using</span> DBConfirm.Core.Templates;
                {"\n"}<span className="hljs-keyword">using</span> System.Threading.Tasks;
                {"\n"}
                        {"\n"}<span className="hljs-keyword">namespace</span> Sample.Tests.Templates.Complex
                {"\n"}{"{"}
                        {"\n"}    <span className="hljs-comment">// The class inherits from BaseComplexTemplate</span>
                        {"\n"}    <span className="hljs-keyword">public</span> <span className="hljs-keyword">class</span> <span className="hljs-type">UserWithAddressTemplate</span> : <span className="hljs-type">BaseComplexTemplate</span>
                        {"\n"}    {"{"}
                        {"\n"}        <span className="hljs-comment">// Defines a template for the User table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">UserTemplate</span> User {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">UserTemplate</span>();
                {"\n"}
                        {"\n"}        <span className="hljs-comment">// Defines a template for the UserAddress table</span>
                        {"\n"}        <span className="hljs-keyword">public</span> <span className="hljs-type">UserAddressTemplate</span> UserAddress {"{"} <span className="hljs-keyword">get</span>; <span className="hljs-keyword">set</span>; {"}"} = <span className="hljs-keyword">new</span> <span className="hljs-type">UserAddressTemplate</span>();
                {"\n"}
                        {"\n"}        <span className="hljs-comment">// Inserts the data based on the templates</span>
                        {"\n"}        <span className="hljs-function"><span className="hljs-keyword">public</span> <span className="hljs-keyword">override</span> <span className="hljs-keyword">async</span> <span className="hljs-type">Task</span> <span className="hljs-title">InsertAsync</span>(<span className="hljs-params"><span className="hljs-interface">ITestRunner</span> testRunner</span>)
                {"\n"}        </span>{"{"}
                        {"\n"}            <span className="hljs-comment">// Inserts the User template</span>
                        {"\n"}            <span className="hljs-keyword">await</span> testRunner.<span className="hljs-title">InsertTemplateAsync</span>(User);
                {"\n"}
                        {"\n"}            <span className="hljs-comment">// Sets the User foreign key (UserId) in the UserAddress template</span>
                        {"\n"}            <span className="hljs-comment">// with the Identity in the User template (User.Identity)</span>
                        {"\n"}            UserAddress.<span className="hljs-title">WithUserId</span>(User.Identity);
                {"\n"}
                        {"\n"}            <span className="hljs-comment">// Inserts the UserAddress template</span>
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
                        {"\n"}    <span className="hljs-comment">// Inserts the template, using the default data for dbo.Users and dbo.UserAddresses</span>
                        {"\n"}    <span className="hljs-type">UserWithAddressTemplate</span> defaultAddress =
                {"\n"}        <span className="hljs-keyword">await</span> TestRunner.<span className="hljs-title">InsertTemplateAsync</span>(<span className="hljs-keyword">new</span> <span className="hljs-type">UserWithAddressTemplate</span>());
                {"\n"}
                        {"\n"}    <span className="hljs-comment">// Inserts the template, with custom templates</span>
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

        </>
    );
}