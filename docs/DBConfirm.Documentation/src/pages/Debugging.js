import React from 'react';

export default function Debugging() {
    return (
        <>
            <h2 id="templates">Debugging</h2>

            <p>Debugging DBConfirm tests is the same as debugging any other unit test - just set your breakpoint, and run the test as debug.</p>
            <p>However, if you want to investigate the state of the database whilst a test is running, you will need to change the isolation level of your query, otherwise
            your query will be blocked by the transaction of the test.
            </p>
            <p>So, if you want to run a query against the test database whilst a test is paused, you must execute this at the start of your SQL statement:</p>
            <pre><code className="lang-sql">SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;</code></pre>
            <p>You will then be able to query the data that has been set as part of the current test transaction.</p>
        </>
    );
}