import React from 'react';

export default function Api() {
    return (
        <>
            <ul>
                <li><a href="#testrunner">TestRunner API</a></li>
                <li><a href="#queryresult">QueryResult object</a></li>
                <li><a href="#rowresult">RowResult object</a></li>
                <li><a href="#scalarresult">ScalarResult object</a></li>
            </ul>

            <h2 id="testrunner">TestRunner API</h2>
            <p>The DBConfirm API is accessed via the <code>TestRunner</code> property, on the test&#39;s base class. Most
            of
            the API methods are awaitable, so make sure your test is marked as <strong>async</strong> and return a <code>Task</code>.</p>
            <h3 id="countrowsintableasync">CountRowsInTableAsync</h3>
            <p>Returns the total number of rows in the table</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-keyword">int</span>&gt; CountRowsInTableAsync(<span className="hljs-keyword">string</span> tableName)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>tableName</strong> - the name of the table, including schema</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>the total number of rows</li>
            </ul>
            <h3 id="countrowsinviewasync">CountRowsInViewAsync</h3>
            <p>Returns the total number of rows in the view</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-keyword">int</span>&gt; CountRowsInViewAsync(<span className="hljs-keyword">string</span> viewName)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>viewName</strong> - the name of the view, including schema</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>the total number of rows</li>
            </ul>
            <h3 id="executecommandasync">ExecuteCommandAsync</h3>
            <p>Executes a command, returning a single data table</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-type">QueryResult</span>&gt; ExecuteCommandAsync(<span className="hljs-keyword">string</span> commandText, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-type">QueryResult</span>&gt; ExecuteCommandAsync(<span className="hljs-keyword">string</span> commandText, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>commandText</strong> - The command to execute</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Where the command returns a data set, the first table is returned, otherwise an empty data set is
                returned
            </li>
            </ul>
            <h3 id="executecommandmultipledatasetasync">ExecuteCommandMultipleDataSetAsync</h3>
            <p>Executes a command, returning all data tables</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-interface">IList</span>&lt;<span className="hljs-type">QueryResult</span>&gt;&gt; ExecuteCommandMultipleDataSetAsync(<span className="hljs-keyword">string</span> commandText, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-interface">IList</span>&lt;<span className="hljs-type">QueryResult</span>&gt;&gt; ExecuteCommandMultipleDataSetAsync(<span className="hljs-keyword">string</span> commandText, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>commandText</strong> - The command to execute</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns all tables returned from the command</li>
            </ul>
            <h3 id="executecommandnoresultsasync">ExecuteCommandNoResultsAsync</h3>
            <p>Executes a command, returning no data</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span> ExecuteCommandNoResultsAsync(<span className="hljs-keyword">string</span> commandText, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span> ExecuteCommandNoResultsAsync(<span className="hljs-keyword">string</span> commandText, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>commandText</strong> - The command to execute</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Nothing</li>
            </ul>
            <h3 id="executecommandscalarasync">ExecuteCommandScalarAsync</h3>
            <p>Executes a command, returning a single object</p>
            <pre><code className="lang-csharp"><span className="hljs-type">Task</span>&lt;<span className="hljs-type">ScalarResult</span><span className="hljs-symbol">&lt;T&gt;</span>&gt; ExecuteCommandScalarAsync<span className="hljs-symbol">&lt;T&gt;</span>(<span className="hljs-keyword">string</span> commandText, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-type">Task</span>&lt;<span className="hljs-type">ScalarResult</span><span className="hljs-symbol">&lt;T&gt;</span>&gt; ExecuteCommandScalarAsync<span className="hljs-symbol">&lt;T&gt;</span>(<span className="hljs-keyword">string</span> commandText, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Types:</p>
            <ul>
                <li><strong>T</strong> - The type of the object to return</li>
            </ul>
            <p>Parameters:</p>
            <ul>
                <li><strong>commandText</strong> - The command to execute</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the object returned from the command</li>
            </ul>
            <h3 id="executestoredproceduremultipledatasetasync">ExecuteStoredProcedureMultipleDataSetAsync</h3>
            <p>Executes a stored procedure, returning all data tables</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-interface">IList</span>&lt;<span className="hljs-type">QueryResult</span>&gt;&gt; ExecuteStoredProcedureMultipleDataSetAsync(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-interface">IList</span>&lt;<span className="hljs-type">QueryResult</span>&gt;&gt; ExecuteStoredProcedureMultipleDataSetAsync(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>procedureName</strong> - The name of the stored procedure, including schema</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns all tables returned from the stored procedure</li>
            </ul>
            <h3 id="executestoredprocedurenonqueryasync">ExecuteStoredProcedureNonQueryAsync</h3>
            <p>Executes a stored procedure, returning nothing</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span> ExecuteStoredProcedureNonQueryAsync(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span> ExecuteStoredProcedureNonQueryAsync(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>procedureName</strong> - The name of the stored procedure, including schema</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Nothing</li>
            </ul>
            <h3 id="executestoredprocedurequeryasync">ExecuteStoredProcedureQueryAsync</h3>
            <p>Executes a stored procedure, returning a single data table</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-type">QueryResult</span>&gt; ExecuteStoredProcedureQueryAsync(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-type">QueryResult</span>&gt; ExecuteStoredProcedureQueryAsync(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>procedureName</strong> - The name of the stored procedure, including schema</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Where the stored procedure returns a data set, the first table is returned, otherwise an empty data set
                is
                returned</li>
            </ul>
            <h3 id="executestoredprocedurescalarasync">ExecuteStoredProcedureScalarAsync</h3>
            <p>Executes a stored procedure, returning a single object</p>
            <pre><code className="lang-csharp"><span className="hljs-type">Task</span>&lt;<span className="hljs-type">ScalarResult</span><span className="hljs-symbol">&lt;T&gt;</span>&gt; ExecuteStoredProcedureScalarAsync<span className="hljs-symbol">&lt;T&gt;</span>(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-interface">IDictionary</span>&lt;<span className="hljs-keyword">string</span>, <span className="hljs-keyword">object</span>&gt; parameters)
        {"\n"}
                {"\n"}<span className="hljs-type">Task</span>&lt;<span className="hljs-type">ScalarResult</span><span className="hljs-symbol">&lt;T&gt;</span>&gt; ExecuteStoredProcedureScalarAsync<span className="hljs-symbol">&lt;T&gt;</span>(<span className="hljs-keyword">string</span> procedureName, <span className="hljs-keyword">params</span> <span className="hljs-type">SqlQueryParameter</span>[] parameters)
</code></pre>
            <p>Types:</p>
            <ul>
                <li><strong>T</strong> - The type of the object to return</li>
            </ul>
            <p>Parameters:</p>
            <ul>
                <li><strong>procedureName</strong> - The name of the stored procedure, including schema</li>
                <li><strong>parameters</strong> - The parameters to be used (when an IDictionary is used, the Key is used as
                the
                parameter name, and the Value used as the parameter value)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the object returned from the stored procedure</li>
            </ul>
            <h3 id="executetableasync">ExecuteTableAsync</h3>
            <p>Returns all data for a specific table</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-type">QueryResult</span>&gt; ExecuteTableAsync(<span className="hljs-keyword">string</span> tableName)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>tableName</strong> - The name of the table, including schema</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns all columns and values found</li>
            </ul>
            <h3 id="executeviewasync">ExecuteViewAsync</h3>
            <p>Returns all data for a specific view</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-type">QueryResult</span>&gt; ExecuteViewAsync(<span className="hljs-keyword">string</span> viewName)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>viewName</strong> - The name of the view, including schema</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns all columns and values found</li>
            </ul>
            <h3 id="insertdataasync">InsertDataAsync</h3>
            <p>Inserts data into a table.</p>
            <p>Where the table has an identity column, and is not set as part of the input data, then the identity value
            used is
            added to the returned data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">Task</span><span className="hljs-variable">&lt;<span className="hljs-type">DataSetRow</span>&gt;</span> InsertDataAsync(<span className="hljs-keyword">string</span> tableName, <span className="hljs-type">DataSetRow</span> data)
        {"\n"}
                {"\n"}<span className="hljs-type">Task</span><span className="hljs-variable">&lt;<span className="hljs-type">DataSetRow</span>&gt;</span> InsertDataAsync(<span className="hljs-keyword">string</span> tableName, <span className="hljs-type">DataSetRow</span> defaultData, <span className="hljs-type">DataSetRow</span> overrideData)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>tableName</strong> - The name of the table to insert into, including schema</li>
                <li><strong>data</strong> - The data to insert</li>
                <li><strong>defaultData</strong> - The default data to insert</li>
                <li><strong>overrideData</strong> - The data to insert, overriding the data provided in defaultData</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the data inserted, including the identity value (if applicable)</li>
            </ul>
            <h3 id="inserttemplateasync-default-values-">InsertTemplateAsync (default values)</h3>
            <p>Inserts data based on the default values defined in the template</p>
            <p>Where the table has an identity column, and is not set as part of the input data, then the identity value
            used is
            added to the returned data set</p>
            <pre><code className="lang-csharp"><span className="hljs-keyword">Task</span>&lt;T&gt; InsertTemplateAsync&lt;T&gt;() <span className="hljs-keyword">where</span> T : <span className="hljs-interface">ITemplate</span>, <span className="hljs-keyword">new</span>()
</code></pre>
            <p>Types:</p>
            <ul>
                <li><strong>T</strong> - The type of template to insert</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the template object, including the identity value (if applicable)</li>
            </ul>
            <h3 id="inserttemplateasync">InsertTemplateAsync</h3>
            <p>Inserts data based on the supplied template</p>
            <p>Where the table has an identity column, and is not set as part of the input data, then the identity value
            used is
            added to the returned data set</p>
            <pre><code className="lang-csharp"><span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;T&gt; InsertTemplateAsync&lt;T&gt;(T template) <span className="hljs-keyword">string</span> T : <span className="hljs-interface">ITemplate</span>
                {"\n"}
                {"\n"}<span className="hljs-built_in"><span className="hljs-type">Task</span></span>&lt;<span className="hljs-interface">ITemplate</span>&gt; InsertTemplateAsync(<span className="hljs-interface">ITemplate</span> template)
</code></pre>
            <p>Types:</p>
            <ul>
                <li><strong>T</strong> - The type of template to insert</li>
            </ul>
            <p>Parameters:</p>
            <ul>
                <li><strong>template</strong> - The template containing the data to add</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the template object, including the identity value (if applicable)</li>
            </ul>
            <h2 id="queryresult">QueryResult</h2>
            <p>A set of data (columns and rows) is returned in a <code>QueryResult</code> object. This object has a number
            of
            properties and assertion methods which can be used to test the data. Alternatively, the data itself can be
            accessed vua the <code>RawData</code> property, to return the data as a <code>DataTable</code>.</p>
            <h3 id="totalrows">TotalRows</h3>
            <p>The total number of rows in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-keyword">int</span> TotalRows {"{"} <span className="hljs-keyword">get</span>; {"}"}
            </code></pre>
            <p>Returns:</p>
            <ul>
                <li>Returns the total number of rows in the data set</li>
            </ul>
            <h3 id="totalcolumns">TotalColumns</h3>
            <p>The total number of columns in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-keyword">int</span> TotalColumns {"{"} <span className="hljs-keyword">get</span>; {"}"}
            </code></pre>
            <p>Returns:</p>
            <ul>
                <li>Returns the total number of columns in the data set</li>
            </ul>
            <h3 id="columnnames">ColumnNames</h3>
            <p>The collection of columns in the data set, in the order they appear in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-interface">ICollection</span>&lt;<span className="hljs-keyword">string</span>&gt; ColumnNames {"{"} <span className="hljs-keyword">get</span>; {"}"}
            </code></pre>
            <p>Returns:</p>
            <ul>
                <li>Returns the collection of columns in the data set, in the order they appear in the data set</li>
            </ul>
            <h3 id="assertrowcount">AssertRowCount</h3>
            <p>Asserts the number of rows</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertRowCount(<span className="hljs-keyword">int</span> expected)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expected</strong> - The expected number of rows</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertrowcount">AssertRowCount</h3>
            <p>Asserts the number of columns</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertColumnCount(<span className="hljs-keyword">int</span> expected)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expected</strong> - The expected number of columns</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertcolumnexists">AssertColumnExists</h3>
            <p>Asserts that a specific column exists in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertColumnExists(<span className="hljs-keyword">string</span> expectedColumnName)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expectedColumnName</strong> - The column name (case-sensitive)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertcolumnnotexists">AssertColumnNotExists</h3>
            <p>Asserts that a specific column does not exist in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertColumnNotExists(<span className="hljs-keyword">string</span> notExpectedColumnName)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>notExpectedColumnName</strong> - The column name (case-sensitive)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertcolumnsexist">AssertColumnsExist</h3>
            <p>Asserts that a number of columns all exist in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertColumnsExist(<span className="hljs-keyword">params</span> <span className="hljs-keyword">string</span>[] expectedColumnNames)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expectedColumnNames</strong> - The column names (case-sensitive)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertcolumnsnotexist">AssertColumnsNotExist</h3>
            <p>Asserts that a number of columns all do not exist in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertColumnsNotExist(<span className="hljs-keyword">params</span> <span className="hljs-keyword">string</span>[] notExpectedColumnNames)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>notExpectedColumnNames</strong> - The column names (case-sensitive)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertrowpositionexists">AssertRowPositionExists</h3>
            <p>Asserts that a row exists at a specific position (zero-based)</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertRowPositionExists(<span className="hljs-keyword">int</span> expectedRowPosition)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expectedRowPosition</strong> - The row position (zero-based)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertvalue">AssertValue</h3>
            <p>Asserts that a specific value exists for the given row and column. Also asserts that the row and column
            exists
        </p>
            <pre><code className="lang-csharp"><span className="hljs-function"><span className="hljs-type">QueryResult</span> <span className="hljs-title">AssertValue</span>(<span className="hljs-params"><span className="hljs-keyword">int</span> rowNumber, <span className="hljs-keyword">string</span> columnName, <span className="hljs-keyword">object</span> expectedValue</span>)</span>
            </code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>rowNumber</strong> - The row position (zero-based)</li>
                <li><strong>columnName</strong> - The column name (case-sensitive)</li>
                <li><strong>expectedValue</strong> - The expected value. Respects <code>IComparison</code> objects</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="validaterow">ValidateRow</h3>
            <p>Returns a <code>RowResult</code> object, representing the specific row on which further assertions can be
            made.
            Validates that the row number exists in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">RowResult</span> ValidateRow(<span className="hljs-keyword">int</span> rowNumber)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>rowNumber</strong> - The row number (zero-based)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the <code>RowResult</code> for the row</li>
            </ul>
            <h3 id="assertrowvalues">AssertRowValues</h3>
            <p>Asserts that the row at the given position matches the expected data. Also asserts that all columns in the
            expected data exist</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertRowValues(<span className="hljs-keyword">int</span> rowNumber, <span className="hljs-type">DataSetRow</span> expectedData)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>rowNumber</strong> - The row number (zero-based)</li>
                <li><strong>expectedData</strong> - The expected data to match. Respects <code>IComparison</code> objects
            </li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertrowexists">AssertRowExists</h3>
            <p>Asserts that at least one row matches the expected data. Also asserts that all columns in the expected data
            exist
        </p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertRowExists(<span className="hljs-type">DataSetRow</span> expectedData)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expectedData</strong> - The expected data to match. Respects <code>IComparison</code> objects
            </li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h3 id="assertrowdoesnotexist">AssertRowDoesNotExist</h3>
            <p>Asserts that no rows match the supplied data. Also asserts that all columns in the supplied data exist</p>
            <pre><code className="lang-csharp"><span className="hljs-type">QueryResult</span> AssertRowDoesNotExist(<span className="hljs-type">DataSetRow</span> unexpectedData)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>unexpectedData</strong> - The unexpected data. Respects <code>IComparison</code> objects</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>QueryResult</code> object</li>
            </ul>
            <h2 id="rowresult">RowResult</h2>
            <p>A single row of values is returned in a <code>RowResult</code> object, accessed via the <code>QueryResult.ValidateRow</code> method. This object has a number of assertion methods which can be used
            to
            test the data in that specific row.</p>
            <h3 id="assertvalue">AssertValue</h3>
            <p>Asserts that a specific value exists for the given column. Also asserts that the column exists</p>
            <pre><code className="lang-csharp"><span className="hljs-function"><span className="hljs-type">RowResult</span> <span className="hljs-title">AssertValue</span>(<span className="hljs-params"><span className="hljs-keyword">string</span> columnName, <span className="hljs-keyword">object</span> expectedValue</span>)</span>
            </code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>columnName</strong> - The column name (case-sensitive)</li>
                <li><strong>expectedValue</strong> - The expected value. Respects <code>IComparison</code> objects</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>RowResult</code> object</li>
            </ul>
            <h3 id="assertvalues">AssertValues</h3>
            <p>Asserts that the row matches the expected data. Also asserts that all columns in the expected data exist</p>
            <pre><code className="lang-csharp"><span className="hljs-type">RowResult</span> AssertValues(<span className="hljs-type">DataSetRow</span> expectedData)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expectedData</strong> - The expected data to match. Respects <code>IComparison</code> objects
            </li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>RowResult</code> object</li>
            </ul>
            <h3 id="validaterow">ValidateRow</h3>
            <p>Returns a <code>RowResult</code> object, representing the specific row on which further assertions can be
            made.
            Validates that the row number exists in the data set</p>
            <pre><code className="lang-csharp"><span className="hljs-type">RowResult</span> ValidateRow(<span className="hljs-keyword">int</span> rowNumber)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>rowNumber</strong> - The row number (zero-based)</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the <code>RowResult</code> for the row</li>
            </ul>
            <h2 id="scalarresult">ScalarResult</h2>
            <p>A single value is returned in a <code>ScalarResult&lt;T&gt;</code> object, accessed via a scalar method in <code>TestRunner</code>. This object has an assertion method which can be used to test the value.</p>
            <h3 id="assertvalue">AssertValue</h3>
            <p>Asserts that the value matches the expected value</p>
            <pre><code className="lang-csharp"><span className="hljs-type">ScalarResult</span>&lt;T&gt; AssertValue(<span className="hljs-keyword">object</span> expectedValue)
</code></pre>
            <p>Parameters:</p>
            <ul>
                <li><strong>expectedValue</strong> - The expected value. Respects <code>IComparison</code> objects</li>
            </ul>
            <p>Returns:</p>
            <ul>
                <li>Returns the same <code>ScalarResult&lt;T&gt;</code> object</li>
            </ul>
        </>
    );
}