import React from 'react';

export default function NuGet() {
    return (
        <>
            <h2 id="nuget">NuGet Packages</h2>
            <p>There are currently 13 NuGet packages, and 2 NuGet tools.</p>
            <p>Most developers will only need to install the DBConfirm.Packages.* NuGet packages, or use one of the DBConfirm.Templates.* packages, since
                these contain everything you need to start testing.</p>

            <h3>DBConfirm.Templates</h3>

            <p>DBConfirm provides template packages to quickly set up new test projects, including all the depdendencies and required configuration.</p>
            <p>These packages need to be installed using <code>dotnet new -i [package name]</code>, and new 
            projects created using <code>dotnet new [package command]</code>.  See the <a href="/quickstart">Quick Start</a> guide.</p>

            <p>There is a template for MSTest and one for NUnit, for using both SQL Server and MySQL as the database engine.</p>

            <ul>
                <li><span>For integrating with MSTest and SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.MSTest/">DBConfirm.Templates.SQLServer.MSTest</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.MSTest/"><img src="https://img.shields.io/nuget/v/DBConfirm.Templates.SQLServer.MSTest" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.MSTest/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Templates.SQLServer.MSTest" alt="" /></a></span>
                </li>
                <li><span>For integrating with NUnit and SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.NUnit/">DBConfirm.Templates.SQLServer.NUnit</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.NUnit/"><img src="https://img.shields.io/nuget/v/DBConfirm.Templates.SQLServer.NUnit" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.SQLServer.NUnit/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Templates.SQLServer.NUnit" alt="" /></a></span>
                </li>
                <li><span>For integrating with MSTest and MySQL, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.MySQL.MSTest/">DBConfirm.Templates.MySQL.MSTest</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.MySQL.MSTest/"><img src="https://img.shields.io/nuget/v/DBConfirm.Templates.MySQL.MSTest" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.MySQL.MSTest/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Templates.MySQL.MSTest" alt="" /></a></span>
                </li>
                <li><span>For integrating with NUnit and MySQL, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.MySQL.NUnit/">DBConfirm.Templates.MySQL.NUnit</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.MySQL.NUnit/"><img src="https://img.shields.io/nuget/v/DBConfirm.Templates.MySQL.NUnit" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Templates.MySQL.NUnit/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Templates.MySQL.NUnit" alt="" /></a></span>
                </li>
            </ul>

            <h3>DBConfirm.Packages.*</h3>

            <p>DBConfirm currently integrates with MSTest and NUnit as testing frameworks, and SQL Server and MySQL as database engine.</p>
            <p>To install DBConfirm to an existing project, use one of the DBConfirm.Packages.* packages, which adds the depdendencies for the test framework and the database engine.</p>
            <p>Therefore, there are 4 packages to choose from, one for MSTest and NUnit.  Install the one that you want, and you'll have everything you need.</p>

            <ul>
                <li><span>For integrating with MSTest and SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/">DBConfirm.Packages.SQLServer.MSTest</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/"><img src="https://img.shields.io/nuget/v/DBConfirm.Packages.SQLServer.MSTest" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.MSTest/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Packages.SQLServer.MSTest" alt="" /></a></span>
                </li>
                <li><span>For integrating with NUnit and SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/">DBConfirm.Packages.SQLServer.NUnit</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/"><img src="https://img.shields.io/nuget/v/DBConfirm.Packages.SQLServer.NUnit" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.SQLServer.NUnit/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Packages.SQLServer.NUnit" alt="" /></a></span>
                </li>
                <li><span>For integrating with MSTest and MySQL, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.MSTest/">DBConfirm.Packages.MySQL.MSTest</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.MSTest/"><img src="https://img.shields.io/nuget/v/DBConfirm.Packages.MySQL.MSTest" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.MSTest/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Packages.MySQL.MSTest" alt="" /></a></span>
                </li>
                <li><span>For integrating with NUnit and MySQL, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.NUnit/">DBConfirm.Packages.MySQL.NUnit</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.NUnit/"><img src="https://img.shields.io/nuget/v/DBConfirm.Packages.MySQL.NUnit" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Packages.MySQL.NUnit/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Packages.MySQL.NUnit" alt="" /></a></span>
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

            <p>DBConfirm currently can only use SQL Server and MySQL as a database engine.  These packages handle the logic to insert and retrieve data from the database.</p>

            <ul>
                <li><span>For integrating with SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.SQLServer/">DBConfirm.Databases.SQLServer</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.SQLServer/"><img src="https://img.shields.io/nuget/v/DBConfirm.Databases.SQLServer" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.SQLServer/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Databases.SQLServer" alt="" /></a></span>
                </li>
                <li><span>For integrating with MySQL, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.MySQL/">DBConfirm.Databases.MySQL</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.MySQL/"><img src="https://img.shields.io/nuget/v/DBConfirm.Databases.MySQL" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.Databases.MySQL/"><img src="https://img.shields.io/nuget/dt/DBConfirm.Databases.MySQL" alt="" /></a></span>
                </li>
            </ul>

            <h3>DBConfirm.TemplateGeneration</h3>

            <p>There is a TemplateGeneration tool that can be used to automatically generate template files based on the schema of your database tables.</p>
            <p>This tool outputs ready-to-go classes directly into your test project, referencing all columns that exist in the tables.</p>

            <ul>
                <li><span>To run the tool for SQL Server, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration.SQLServer/">DBConfirm.TemplateGeneration.SQLServer</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration.SQLServer/"><img src="https://img.shields.io/nuget/v/DBConfirm.TemplateGeneration.SQLServer" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration.SQLServer/"><img src="https://img.shields.io/nuget/dt/DBConfirm.TemplateGeneration.SQLServer" alt="" /></a></span>
                </li>
                <li><span>To run the tool for MySQL, use&nbsp;<a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration.MySQL/">DBConfirm.TemplateGeneration.MySQL</a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration.MySQL/"><img src="https://img.shields.io/nuget/v/DBConfirm.TemplateGeneration.MySQL" alt="" /></a>
                    <a target="_blank" rel="noreferrer" href="https://www.nuget.org/packages/DBConfirm.TemplateGeneration.MySQL/"><img src="https://img.shields.io/nuget/dt/DBConfirm.TemplateGeneration.MySQL" alt="" /></a></span>
                </li>
            </ul>
        </>
    );
}