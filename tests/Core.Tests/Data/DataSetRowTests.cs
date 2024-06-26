﻿using DBConfirm.Core.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Core.Tests.Data;

[TestFixture]
public class DataSetRowTests
{
    [Test]
    public void DataSetRow_Values_CanSetAndRetrieveValue()
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        DataSetRow row = new();
#pragma warning restore IDE0028 // Simplify collection initialization
        row["UserId"] = 123;

        Assert.AreEqual(123, row["UserId"]);
    }

    [Test]
    public void DataSetRow_Count_NoItems_0()
    {
        DataSetRow row = [];

        Assert.AreEqual(0, row.Count);
    }

    [Test]
    public void DataSetRow_Count_OneItem_1()
    {
        DataSetRow row = new()
        {
            { "UserId", 123 }
        };

        Assert.AreEqual(1, row.Count);
    }

    [Test]
    public void DataSetRow_Count_TwoItems_2()
    {
        DataSetRow row = new()
        {
            { "UserId", 123 },
            { "DomainId", 1001}
        };

        Assert.AreEqual(2, row.Count);
    }

    [Test]
    public void DataSetRow_Values_CanSetViaConstructorAndRetrieveValue()
    {
        DataSetRow row = new()
        {
            { "UserId", 123 }
        };

        Assert.AreEqual(123, row["UserId"]);
    }

    [Test]
    public void DataSetRow_Values_CanSetViaIndexInitialiserConstructorAndRetrieveValue()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123
        };

        Assert.AreEqual(123, row["UserId"]);
    }

    [Test]
    public void DataSetRow_Values_CanSetViaDictionaryConstructorAndRetrieveValue()
    {
        Dictionary<string, object> dictionary = new()
        {
            ["UserId"] = 123
        };

        DataSetRow row = new(dictionary);

        Assert.AreEqual(123, row["UserId"]);
    }

    [Test]
    public void DataSetRow_Ctor_NullDictionary_InitialiseNoValues()
    {
        DataSetRow row = new(null);

        Assert.AreEqual(0, row.Count);
    }

    [Test]
    public void DataSetRow_Values_RetrieveUnsetValue_ThrowException()
    {
        DataSetRow row = [];

        var exception = Assert.Throws<KeyNotFoundException>(() => { object result = row["UserId"]; });
        Assert.IsNotNull(exception);
        Assert.AreEqual("UserId was not found in the data set", exception.Message);
    }

    [Test]
    public void DataSetRow_ToString_NoValues_ReturnEmptyString()
    {
        DataSetRow row = [];

        Assert.AreEqual("", row.ToString());
    }

    [Test]
    public void DataSetRow_ToString_OneValue_ReturnValue()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123
        };

        Assert.AreEqual(@"
[UserId, 123]"
, row.ToString());
    }

    [Test]
    public void DataSetRow_ToString_TwoValues_ReturnValues()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        Assert.AreEqual(@"
[UserId, 123]
[DomainId, 1001]", row.ToString());
    }

    [Test]
    public void DataSetRow_Merge_WithNull_ReturnOriginal()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow result = row.Merge(null);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(1001, result["DomainId"]);
    }

    [Test]
    public void DataSetRow_Merge_WithEmpty_ReturnOriginal()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow result = row.Merge([]);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(1001, result["DomainId"]);
    }

    [Test]
    public void DataSetRow_Merge_WithNewKey_ReturnOriginalAndNew()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow result = row.Merge(new DataSetRow
        {
            ["AddressId"] = 2001
        });

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(1001, result["DomainId"]);
        Assert.AreEqual(2001, result["AddressId"]);
    }

    [Test]
    public void DataSetRow_Merge_WithNoOriginalWithNewKey_ReturnOriginalAndNew()
    {
        DataSetRow row = [];

        DataSetRow result = row.Merge(new DataSetRow
        {
            ["AddressId"] = 2001
        });

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(2001, result["AddressId"]);
    }

    [Test]
    public void DataSetRow_Merge_WithExistingKey_ReturnNew()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow result = row.Merge(new DataSetRow
        {
            ["DomainId"] = 2001
        });

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(2001, result["DomainId"]);
    }

    [Test]
    public void DataSetRow_Merge_WithExistingKeyDifferentType_ReturnNew()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow result = row.Merge(new DataSetRow
        {
            ["DomainId"] = DateTime.Parse("01-Mar-2020")
        });

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(DateTime.Parse("01-Mar-2020"), result["DomainId"]);
    }

    [Test]
    public void DataSetRow_Merge_WithExistingKeyNullValue_ReturnNew()
    {
        DataSetRow row = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow result = row.Merge(new DataSetRow
        {
            ["DomainId"] = null
        });

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(null, result["DomainId"]);
    }

    [Test]
    public void DataSetRow_Merge_ValueChanged_OriginalsNotChanged()
    {
        DataSetRow originalRow = new()
        {
            ["UserId"] = 123,
            ["DomainId"] = 1001
        };

        DataSetRow newRow = new()
        {
            ["DomainId"] = 2001
        };

        DataSetRow result = originalRow.Merge(newRow);

        Assert.AreEqual(2, originalRow.Count);
        Assert.AreEqual(123, originalRow["UserId"]);
        Assert.AreEqual(1001, originalRow["DomainId"]);

        Assert.AreEqual(1, newRow.Count);
        Assert.AreEqual(2001, newRow["DomainId"]);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(123, result["UserId"]);
        Assert.AreEqual(2001, result["DomainId"]);
    }
}
