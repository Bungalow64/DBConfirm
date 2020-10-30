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
            <h3 id="assert">Assert - check the returned data, and check the state of data in the database</h3>

        </>
    );
}