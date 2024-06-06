## DBConfirm 1.1.0 - 1-May-2024

[![](https://img.shields.io/badge/nuget-v1.1.0-blue)](https://www.nuget.org/packages/DBConfirm.Core/1.1.0)

This release adds more functionality when making assertions and fixes a few issues.

This release also upgrades the version of .NET used, and confirms compatibility with SQL Server 2022.

### Enhancements

* [Assertions for numbers are type-specific, which can be too restrictive and slow development down](https://github.com/Bungalow64/DBConfirm/issues/52)
* [add a function to assert Uniqueness](https://github.com/Bungalow64/DBConfirm/issues/96)
* [Add helper methods to make it easy to test for exceptions/errors coming back from executing stored procedures](https://github.com/Bungalow64/DBConfirm/issues/54)
* [Confirm compatibility with SQL Server 2022](https://github.com/Bungalow64/DBConfirm/issues/99)

### Issues Resolved

* ['Inconsistent Line Endings' warning when opening generated template files in VS2019](https://github.com/Bungalow64/DBConfirm/issues/51)
* [Generating a template for a table name that contains an apostrophe results in an invalid class](https://github.com/Bungalow64/DBConfirm/issues/57)
* [Upgrade Microsoft.Data.SqlClient/System.Data.SqlClient to non-vulnerable versions](https://github.com/Bungalow64/DBConfirm/issues/97)

## Internal Fixes

* [Upgrade test projects to .NET 8](https://github.com/Bungalow64/DBConfirm/issues/98)
* [Remove SQL setup script from root](https://github.com/Bungalow64/DBConfirm/issues/100)
* [Add negative tests, asserting that verifications should fail when expected](https://github.com/Bungalow64/DBConfirm/issues/101)

### Breaking Changes

There are no breaking changes between this version (1.1.0) and the previous version (1.0.1).



## DBConfirm 1.0.1 - 25-Jan-2023

[![](https://img.shields.io/badge/nuget-v1.0.1-blue)](https://www.nuget.org/packages/DBConfirm.Core/1.0.1)

This release fixes an issue with an assertion.

### Issues Resolved

* [Using AssertRowDoesNotExist when using NUnit results in an incorrect test failure](https://github.com/Bungalow64/DBConfirm/issues/87)

### Breaking Changes

There are no breaking changes between this version (1.0.1) and the previous version (1.0.0).


## DBConfirm 1.0.0 - 18-Aug-2021

[![](https://img.shields.io/badge/nuget-v1.0.0-blue)](https://www.nuget.org/packages/DBConfirm.Core/1.0.0)

The initial release.