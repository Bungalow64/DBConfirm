import React from 'react';

export default function NuGet() {
    return (
        <>
            <h2 id="nuget">NuGet Packages</h2>
            <p>There are currently 6 NuGet packages, and 1 NuGet tool.</p>
            <p>Most developers will only need to install the DBConfirm.Packages.* NuGet packages, since these contain everything you need to start testing.</p>

            <h3>DBConfirm.Packages.*</h3>

            <p>DBConfirm currently integrates with MSTest and NUnit as testing frameworks, and SQL Server as database engine.</p>
            <p>Therefore, there are 2 packages to choose from, one for MSTest and NUnit.  Install the one that you want, and you'll have everything you need.</p>

            <ul>
                <li><span>For integrating with MSTest and SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/">DBConfirm.Packages.SQLServer.MSTest</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/"><img src="https://img.shields.io/nuget/v/DBConfirm.Packages.SQLServer.MSTest" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Packages.SQLServer.MSTest" alt="" /></a></span>
                </li>
                <li><span>For integrating with NUnit and SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/">DBConfirm.Packages.SQLServer.NUnit</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/"><img src="https://img.shields.io/nuget/v/DBConfirm.Packages.SQLServer.NUnit" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Packages.SQLServer.NUnit" alt="" /></a></span>
                </li>
            </ul>

            <h3>DBConfirm.Core</h3>

            <p>DBConfirm.Core is the core library that handles all the DBConfirm logic.  This is common logic across all test frameworks and database engines.</p>

            <ul>
                <li><span><a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/">DBConfirm.Core</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/"><img src="https://img.shields.io/nuget/v/DBConfirm.Core" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Core/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Core" alt="" /></a></span>
                </li>
            </ul>

            <h3>DBConfirm.Frameworks.*</h3>

            <p>DBConfirm must run with a testing framework, currently either MSTest or NUnit.  These DBConfirm.Frameworks.* packages contain the integration logic.</p>

            <ul>
                <li><span>For integrating with MSTest, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Frameworks.MSTest/">DBConfirm.Frameworks.MSTest</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Frameworks.MSTest/"><img src="https://img.shields.io/nuget/v/DBConfirm.Frameworks.MSTest" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Frameworks.MSTest/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Frameworks.MSTest" alt="" /></a></span>
                </li>
                <li><span>For integrating with NUnit, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Frameworks.NUnit/">DBConfirm.Frameworks.NUnit</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Frameworks.NUnit/"><img src="https://img.shields.io/nuget/v/DBConfirm.Frameworks.NUnit" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Frameworks.NUnit/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Frameworks.NUnit" alt="" /></a></span>
                </li>
            </ul>

            <h3>DBConfirm.Databases.*</h3>

            <p>DBConfirm currently can only use SQL Server as a database engine.  This package handles the logic to insert and retrieve data from the database.</p>

            <ul>
                <li><span>For integrating with SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.SQLServer/">DBConfirm.Databases.SQLServer</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.SQLServer/"><img src="https://img.shields.io/nuget/v/DBConfirm.Databases.SQLServer" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.SQLServer/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Databases.SQLServer" alt="" /></a></span>
                </li>
            </ul>

            <h3>DBConfirm.TemplateGeneration</h3>

            <p>There is a TemplateGeneration tool that can be used to automatically generate template files based on the schema of your database tables.</p>
            <p>This tool outputs ready-to-go classes directly into your test project, referencing all columns that exist in the tables.</p>

            <ul>
                <li><span>To run the tool, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration/">DBConfirm.TemplateGeneration</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration/"><img src="https://img.shields.io/nuget/v/DBConfirm.TemplateGeneration" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration/"><img src="https://img.shields.io/nuget/dt/DBConfirm.TemplateGeneration" alt="" /></a></span>
                </li>
            </ul>
        </>
    );
}