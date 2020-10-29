import React from 'react';
import ci_connectionstring from './images/ci_connectionstring.png';

export default function ContinuousIntegration(){
    return (
        <>
        <h2 id="continuousintegration">Continuous Integration</h2>
        <p>DBConfirm tests are integrated with existing test frameworks, so tests will be run by any Continuous
            Integration (CI) build that
            can run tests in that framework.</p>
        <p>The only different between a standard unit test suite and a DBConfirm test suite is that DBConfirm requires
            an actual database to test against,
            so this database needs to be accessible by the build agent.</p>
        <p>There are 3 ways to configure the connection string used by the tests, which are checked in this order:
        </p>
        <ul>
            <li><a href="#environmentvariables">1. Environment Variables</a></li>
            <li><a href="#runsettings">2. .runsettings</a></li>
            <li><a href="#appsettings">3. appsettings.json</a></li>
        </ul>
        <h3 id="environmentvariables">Configure using Environment Variables</h3>
        <p>This is the recommended approach to configuring the CI build.</p>
        <p>During the test initialisation, the current environment variables are checked for a variable called
            '<strong>ConnectionString</strong>', and if
            a value is found, this connection string is used.</p>
        <p>To set this environment variable, see the configuration for the CI build itself.</p>
        <p>For example, in Azure DevOps, the environment variables are listed as '<strong>Variables</strong>':
        </p>
        <p><img src={ci_connectionstring}
                alt="The ConnectionString environment variable added to a Pipeline build in Azure DevOps"
                style={{width: '100%', minWidth: '700px', maxWidth: '1481px'}} /></p>


        <h3 id="runsettings">Configure using a .runsettings file</h3>
        <p>During test initialisation, if no environment variable is found, DBConfirm will next check for a
            .runsettings file.</p>
        <p>If a .runsettings file is found, a parameter called '<strong>ConnectionString</strong>' is checked, and used
            if found.</p>
        <p>An example .runsettings file is:</p>
        <pre><code className="lang-xml"><span className="hljs-meta">&lt;?</span><span className="hljs-tag">xml</span> <span className="hljs-attr">version</span>="<span className="hljs-string">1.0</span>" <span className="hljs-attr">encoding</span>="<span className="hljs-string">utf-8</span>"<span className="hljs-meta">?&gt;</span>
        {"\n"}&lt;<span className="hljs-tag">RunSettings</span>&gt;
        {"\n"}    &lt;<span className="hljs-tag">TestRunParameters</span>&gt;
        {"\n"}        &lt;<span className="hljs-tag">Parameter</span> <span className="hljs-attr">name</span>="<span className="hljs-string">ConnectionString</span>" <span className="hljs-attr">value</span>="<span className="hljs-string">SERVER=B64-BUILD-DB;DATABASE=Northwind;Integrated Security=true;Connection Timeout=30;</span>" /&gt;
        {"\n"}    &lt;/<span className="hljs-tag">TestRunParameters</span>&gt;
        {"\n"}&lt;/<span className="hljs-tag">RunSettings</span>&gt;
</code></pre>

        <h3 id="appsettings">Configure using appsettings.json</h3>
        <p>This method is recommended for local development and testing, but since it can't easily be changed as part of a CI build, it should only be used as a last resort for builds.</p>
        <p>During test initialisation, if no environment variable or .runsettings variable is found, then the appsettings.json file is checked for a connection string 
            with a name of '<strong>TestDatabase</strong>'.
        </p>
        <p>An example appsettings.json file is:</p>
        <pre><code className="lang-json">{"{"}
        {"\n"}    <span className="hljs-attr">"ConnectionStrings"</span>: {"{"}
        {"\n"}        <span className="hljs-attr">"TestDatabase"</span>: <span className="hljs-string">"SERVER=(local);DATABASE=SampleDB;Integrated Security=true;Connection Timeout=30;"</span>
        {"\n"}    {"}"}
        {"\n"}{"}"}
</code></pre>
        </>
    );
}