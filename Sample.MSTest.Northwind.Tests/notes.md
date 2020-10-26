Needed to make appsettings.json Build Action: Content, Copy to output directory: If newer
** Can this be added as part of the NuGet package? ** https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package#add-a-readme-and-other-files
    See Product Backlog Item 82: As a developer I need the appsettings.json file to be automatically added (and included in build) when the nuget package is added so that I don't forget to add it

This is the message when a required property hasn't been set.  Need to include the template/table name: (maybe replace exception with Assert fail?  Would that make the stack trace cleaner?)

             Templates_CanAllBeAdded
               Source: TemplateTests.cs line 12
               Duration: 924 ms

              Message: 
                Test method NorthwindTests.Correctness.TemplateTests.Templates_CanAllBeAdded threw exception: 
                SQLConfirm.Core.Exceptions.RequiredPlaceholderIsNullException: The value for TerritoryID is required but has not been set
              Stack Trace: 
                IDictionaryExtensions.<ToSqlParameters>g__getValue|0_0(KeyValuePair`2 value)
                <>c.<ToSqlParameters>b__0_1(KeyValuePair`2 p)
                SelectEnumerableIterator`2.ToArray()
                Enumerable.ToArray[TSource](IEnumerable`1 source)
                IDictionaryExtensions.ToSqlParameters(IDictionary`2 dictionary)
                SQLServerTestRunner.InsertDataAsync(String tableName, DataSetRow defaultData, DataSetRow overrideData)
                SQLServerTestRunner.InsertTemplateAsync[T](T template)
                TemplateTests.Templates_CanAllBeAdded() line 19
                ThreadOperations.ExecuteWithAbortSafety(Action action)

    See Product Backlog Item 49: As a developer I need clear error messages when I have set up the data wrong so that I can quickly fix the issue

Maybe make template dictionary be the merged dictionary, so default data can be retrieved using template["Column"]

    See Product Backlog Item 85: As a developer I need to access the merged data by default when using template["column"] so that I can access the data used during insert more easily

If template value is an int, it needs a hard cast to be used in a SetValue method.  Either make template<T>["Column"], or something else

Need to add a custom Identity property, to generate a unique value.  Basically int, maybe cast as string.

    [Done] See Bug 80: Cannot insert multiple complex templates with non-identity primary keys

Add QueryResult assertion to assert single value on specific row.

    See Product Backlog Item 87: As a developer I need to assert that a single value exists on a specific row without calling ValidateRow first so that my tests are simpler

Need some way of creating a complex template where identifiers are not overwritten.

    [Done] See Bug 80: Cannot insert multiple complex templates with non-identity primary keys

Int comparisons need more work.  To compare Int32 (smallint) need to cast int to (short).  Decimal set as 100m.  Float as 100f.  Would be nice to convert and compare the values.  
Maybe create a new IComparison object to MatchNumeric, and handle the conversion from int/decimal to the target type.  Would need loads of unit tests.

    See Product Backlog Item 89: As a developer I need to write assertions using native INTs that match different SQL INT types so that I don't get test failures because 10 != 10m

Expose ExpectedData from TestRunner

    See Product Backlog Item 91: As a developer I need to access ExpectedData from TestRunner so that I don't need to look up which namespace that class belongs to