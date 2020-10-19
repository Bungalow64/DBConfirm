PROBLEM 1 - tests fail due to crash disposing of the temporary LocalDB instances (even with try/catch).  Also, slow to set up/start each instance for each class.  Still best to have a single DB instance that's used.

PROBLEM 2 - Where is the DB schema to be kept?  We can have .sql files, but they need to be accessible by the build.  Keep in same solution (i.e., tests alongside them), then local config to point to folder.  In CI, need to grab them from the build/repo itself.

If we want a single LocalDB instance, it can be created by the first test that needs it (problems with race conditions, or can that be solved by a lock?  Maybe an instance per assembly).  Do not know how to dispose of it.  Maybe both can be handled by assemblyinitialize/assemblycleanup (assuming they work), however this can't be handled in base class (in different assembly), so would need a hook in the assembly itself.


AssemblyInitialize
 -> setup/connect to instance
 -> set up database

ClassInitialize
 -> Set TestContext

TestInitialize
 -> Set up test runner and connection

TestCleanup
 -> Dispose connection, roll transaction back

AssemblyCleanup
 -> Remove database




OR....should database deployment be completely separate?  So the tests just need a connection string, and it's a different step to deploy the database.

If it's a different step, how will it work?  For now, can it be Powershell, running a tool from the Nuget package, pointing at the DB source files?  Connection string (or at least DB name) can be hard-coded (Variables).

Next step:
 - Find out how to bundle a tool (command line?) with Nuget
 - Use it to deploy a database (delete existing DB, add as new)
 - Configure CI build to run it.