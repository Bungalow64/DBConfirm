import React from 'react';

export default function FAQ() {
    return (
        <>
            <h2>Frequently Asked Questions</h2>
            <h3>Are there any limitations on what kind of project I can use DBConfirm in?</h3>
            <p>No.  Go for it.</p>
            <h3>Is DBConfirm free?</h3>
            <p>Yes.</p>
            <h3>Can I run the tests in parallel?</h3>
            <p>No.  Due to how the data in the database is used, it would be very easy to introduce deadlocks if the tests are run in parallel, so being able to run the tests in parallel has been disabled.</p>
            <h3>Do I need an instance of the database to test against?</h3>
            <p>Yes.  DBConfirm doesn't handle the deployment of the latest version of your database, it just requires configuration to point to the database to test.</p>
            <h3>Is this white-box, black-box or gray-box testing?</h3>
            <p>Gray-box testing.  You need to know how the tables in the database are structured, but you don't need to know how the rest of the database is put together.</p>
            <h3>Can I use data-driven tests?</h3>
            <p>Yes.  DBConfirm works really well with <code>[DataTestMethod]</code> (MSTest) and <code>[TestCase]</code> (NUnit).</p>
            <h3>Do I need to reset the database after a test run?</h3>
            <p>No.  DBConfirm rolls all changes back for you.  The only lasting effect will be that identity values will have increased - the only risk with this is after a <i>lot</i> of
            test runs, you might hit the limit of your <code>int</code> identity values - if/when you do, you'll need to manually reset the identity values back to 1.</p>
        </>
    );
}