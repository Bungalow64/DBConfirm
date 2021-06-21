using DBConfirm.TemplateGeneration;
using DBConfirm.TemplateGeneration.Logic;
using DBConfirm.TemplateGeneration.Logic.Abstract;
using Moq;
using NUnit.Framework;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace TemplateGeneration.Tests.Logic
{
    public class GeneratorTests
    {
        #region Init

        private Mock<IFileHelper> _fileHelperMock;
        private Mock<IDatabaseHelper> _databaseHelperMock;
        private Mock<IConsoleLog> _consoleLogMock;

        [SetUp]
        public void Init()
        {
            _fileHelperMock = new Mock<IFileHelper>(MockBehavior.Strict);
            _databaseHelperMock = new Mock<IDatabaseHelper>(MockBehavior.Strict);
            _consoleLogMock = new Mock<IConsoleLog>(MockBehavior.Loose);
        }

        private Generator Create(Options options = null) => new Generator(options ?? new Options(), _fileHelperMock.Object, _databaseHelperMock.Object, _consoleLogMock.Object);

        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("TableName");
            table.Columns.Add("SchemaName");
            table.Columns.Add("ColumnName");
            table.Columns.Add("IsNullable", typeof(bool));
            table.Columns.Add("DefaultValue");
            table.Columns.Add("DataType");
            table.Columns.Add("MaxCharacterLength", typeof(int));
            table.Columns.Add("IsIdentity", typeof(bool));
            table.Columns.Add("IsForeignKey", typeof(bool));
            table.Columns.Add("ReferencedTableName");
            table.Columns.Add("ReferencesIdentity", typeof(bool));
            return table;
        }

        #endregion

        #region Ctor
        [Test]
        public void Generator_Ctor()
        {
            Assert.DoesNotThrow(() => Create(new Options()));
        }
        #endregion

        #region Connection Details
        [Test]
        public async Task Generator_NullConnectionStringAndDatabaseName_NoGeneration()
        {
            Options options = new Options
            {
                ConnectionString = null,
                DatabaseName = null
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Generator_EmptyConnectionStringAndDatabaseName_NoGeneration()
        {
            Options options = new Options
            {
                ConnectionString = string.Empty,
                DatabaseName = string.Empty
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region No Columns

        [Test]
        public async Task Generator_NoColumnsFound_NoGeneration()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "*"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(CreateTable());

            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region Table Identification

        [Test]
        public async Task Generator_WithTableName_UseDefaultSchema()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((connectionString, schemaName, tableName, script) =>
                {
                    Assert.AreEqual("SERVER=(local);DATABASE=TestDatabase1;Integrated Security=true;Connection Timeout=30;", connectionString);
                    Assert.AreEqual("dbo", schemaName);
                    Assert.AreEqual("Users", tableName);
                })
                .ReturnsAsync(CreateTable());

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_WithAllTables_UseDefaultSchema()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "*"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((connectionString, schemaName, tableName, script) =>
                {
                    Assert.AreEqual("SERVER=(local);DATABASE=TestDatabase1;Integrated Security=true;Connection Timeout=30;", connectionString);
                    Assert.AreEqual("dbo", schemaName);
                    Assert.AreEqual("%", tableName);
                })
                .ReturnsAsync(CreateTable());

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_WithPartialTables_UseDefaultSchema()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "User*"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((connectionString, schemaName, tableName, script) =>
                {
                    Assert.AreEqual("SERVER=(local);DATABASE=TestDatabase1;Integrated Security=true;Connection Timeout=30;", connectionString);
                    Assert.AreEqual("dbo", schemaName);
                    Assert.AreEqual("User%", tableName);
                })
                .ReturnsAsync(CreateTable());

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_WithTableAndSchema_UseProvidedDetails()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                SchemaName = "Fact",
                TableName = "Users"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((connectionString, schemaName, tableName, script) =>
                {
                    Assert.AreEqual("SERVER=(local);DATABASE=TestDatabase1;Integrated Security=true;Connection Timeout=30;", connectionString);
                    Assert.AreEqual("Fact", schemaName);
                    Assert.AreEqual("Users", tableName);
                })
                .ReturnsAsync(CreateTable());

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_WithTableAndSchemaInBrackets_UseProvidedDetailsStripBrackets()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                SchemaName = "[Fact]",
                TableName = "[Users]"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((connectionString, schemaName, tableName, script) =>
                {
                    Assert.AreEqual("SERVER=(local);DATABASE=TestDatabase1;Integrated Security=true;Connection Timeout=30;", connectionString);
                    Assert.AreEqual("Fact", schemaName);
                    Assert.AreEqual("Users", tableName);
                })
                .ReturnsAsync(CreateTable());

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_WithTableNameIncludingSchema_SchemaSplitOut()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "[Fact].[Users]"
            };

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((connectionString, schemaName, tableName, script) =>
                {
                    Assert.AreEqual("SERVER=(local);DATABASE=TestDatabase1;Integrated Security=true;Connection Timeout=30;", connectionString);
                    Assert.AreEqual("Fact", schemaName);
                    Assert.AreEqual("Users", tableName);
                })
                .ReturnsAsync(CreateTable());

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        #endregion

        #region File Generation

        private void AddPrimaryKeyRow(DataTable columns, string schemaName, string tableName, string columnName, bool isIdentity = true)
        {
            DataRow row = columns.NewRow();
            row["TableName"] = tableName;
            row["SchemaName"] = schemaName;
            row["ColumnName"] = columnName;
            row["IsNullable"] = false;
            row["DataType"] = "int";
            row["IsIdentity"] = isIdentity;
            row["IsForeignKey"] = false;
            row["ReferencesIdentity"] = false;
            columns.Rows.Add(row);
        }

        private void AddRequiredNVarcharRow(DataTable columns, string schemaName, string tableName, string columnName)
        {
            DataRow row = columns.NewRow();
            row["TableName"] = tableName;
            row["SchemaName"] = schemaName;
            row["ColumnName"] = columnName;
            row["IsNullable"] = false;
            row["DataType"] = "nvarchar";
            row["MaxCharacterLength"] = 50;
            row["IsIdentity"] = false;
            row["IsForeignKey"] = false;
            row["ReferencesIdentity"] = false;
            columns.Rows.Add(row);
        }
        
        private void AddRequiredNvarcharMaxRow(DataTable columns, string schemaName, string tableName, string columnName)
        {
            DataRow row = columns.NewRow();
            row["TableName"] = tableName;
            row["SchemaName"] = schemaName;
            row["ColumnName"] = columnName;
            row["IsNullable"] = false;
            row["DataType"] = "nvarchar";
            row["MaxCharacterLength"] = -1;
            row["IsIdentity"] = false;
            row["IsForeignKey"] = false;
            row["ReferencesIdentity"] = false;
            columns.Rows.Add(row);
        }

        private void AddNullableRow(DataTable columns, string schemaName, string tableName, string columnName, string dataType)
        {
            DataRow row = columns.NewRow();
            row["TableName"] = tableName;
            row["SchemaName"] = schemaName;
            row["ColumnName"] = columnName;
            row["IsNullable"] = true;
            row["DataType"] = dataType;
            row["IsIdentity"] = false;
            row["IsForeignKey"] = false;
            row["ReferencesIdentity"] = false;
            columns.Rows.Add(row);
        }

        private void AddRequiredForeignKey(DataTable columns, string schemaName, string tableName, string columnName, bool isIdentity = true, bool isNullable = false)
        {
            DataRow row = columns.NewRow();
            row["TableName"] = tableName;
            row["SchemaName"] = schemaName;
            row["ColumnName"] = columnName;
            row["IsNullable"] = isNullable;
            row["DataType"] = "int";
            row["IsIdentity"] = false;
            row["IsForeignKey"] = true;
            row["ReferencesIdentity"] = isIdentity;
            columns.Rows.Add(row);
        }

        private async Task<string> ReadResource(string filename)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TemplateGeneration.Tests.Logic.ExpectedClasses.{filename}.txt");
            using StreamReader reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        [Test]
        public async Task Generator_SingleTableWithColumns_DryRun_NoFileGenerated()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users",
                DryRun = true
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId");
            AddRequiredNVarcharRow(columns, "dbo", "Users", "UserId");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Generator_SingleIdentityTableWithColumns_FileGenerated()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId");
            AddRequiredNVarcharRow(columns, "dbo", "Users", "FirstName");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_SingleTableWithColumns_FileGenerated"), generatedFileText);
        }
        
        [Test]
        public async Task Generator_SingleIdentityTableWithRequiredNvarcharMaxColumn_FileGenerated()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId");
            AddRequiredNvarcharMaxRow(columns, "dbo", "Users", "Notes");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_SingleIdentityTableWithRequiredMaxNvarcharColumn_FileGenerated"), generatedFileText);
        }

        [Test]
        public async Task Generator_SingleNonIdentityTableWithColumns_FileGenerated()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId", isIdentity: false);
            AddRequiredNVarcharRow(columns, "dbo", "Users", "FirstName");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_SingleNonIdentityTableWithColumns_FileGenerated"), generatedFileText);
        }

        [Test]
        public async Task Generator_IncludeDateTimeColumn_FileGenerated_WithSystem()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId");
            AddRequiredNVarcharRow(columns, "dbo", "Users", "FirstName");
            AddNullableRow(columns, "dbo", "Users", "StartDate", "datetime2");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_IncludeDateTimeColumn_FileGenerated_WithSystem"), generatedFileText);
        }

        [Test]
        public async Task Generator_IncludeRequiredForeignKey_FileGenerated_WithPlaceholder()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId");
            AddRequiredForeignKey(columns, "dbo", "Users", "CountryId", isIdentity: false, isNullable: false);
            AddRequiredNVarcharRow(columns, "dbo", "Users", "FirstName");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_IncludeRequiredForeignKey_FileGenerated_WithPlaceholder"), generatedFileText);
        }

        [Test]
        public async Task Generator_IncludeRequiredForeignKeyAsIdentity_FileGenerated_WithPlaceholder()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "UserId");
            AddRequiredForeignKey(columns, "dbo", "Users", "CountryId", isIdentity: true, isNullable: false);
            AddRequiredNVarcharRow(columns, "dbo", "Users", "FirstName");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_IncludeRequiredForeignKeyAsIdentity_FileGenerated_WithPlaceholder"), generatedFileText);
        }

        [Test]
        public async Task Generator_SingleIdentityTableWithColumns_LowercaseNames_FileGeneratedWithPascalCaseColumns()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Users"
            };

            DataTable columns = CreateTable();
            AddPrimaryKeyRow(columns, "dbo", "Users", "userId");
            AddRequiredNVarcharRow(columns, "dbo", "Users", "firstName");

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            string generatedFileText = null;
            _fileHelperMock
                .Setup(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, contents) =>
                {
                    Assert.AreEqual(@"C:\Temp\UsersTemplate.cs", path);
                    generatedFileText = contents;
                });

            _fileHelperMock
                .Setup(p => p.GetCurrentDirectory())
                .Returns(@"C:\Temp");

            _fileHelperMock
                .Setup(p => p.Exists(It.IsAny<string>()))
                .Returns(false);

            await Create(options).GenerateFileAsync();

            _databaseHelperMock
                .Verify(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _fileHelperMock
                .Verify(p => p.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(await ReadResource("Generator_SingleIdentityTableWithColumns_LowercaseNames_FileGeneratedWithPascalCaseColumns"), generatedFileText);
        }

        [Test]
        public async Task Generator_AllTablesForSchema_NoTablesFound_ReturnCorrectMessage()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "*",
                SchemaName = "dbo2"
            };

            DataTable columns = CreateTable();

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            _consoleLogMock
                .Setup(p => p.WriteError(It.IsAny<string>()))
                .Callback<string>(p => Assert.AreEqual("Cannot find any tables in schema: dbo2", p));

            await Create(options).GenerateFileAsync();

            _consoleLogMock
                .Verify(p => p.WriteError(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_OneTableForSchema_NoColumnsFound_ReturnCorrectMessage()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "Table1",
                SchemaName = "dbo2"
            };

            DataTable columns = CreateTable();

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            _consoleLogMock
                .Setup(p => p.WriteError(It.IsAny<string>()))
                .Callback<string>(p => Assert.AreEqual("Cannot find table: dbo2.Table1", p));

            await Create(options).GenerateFileAsync();

            _consoleLogMock
                .Verify(p => p.WriteError(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Generator_PartialWildcardTableForSchema_NoColumnsFound_ReturnCorrectMessage()
        {
            Options options = new Options
            {
                DatabaseName = "TestDatabase1",
                TableName = "T*",
                SchemaName = "dbo2"
            };

            DataTable columns = CreateTable();

            _databaseHelperMock
                .Setup(p => p.GetColumnsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(columns);

            _consoleLogMock
                .Setup(p => p.WriteError(It.IsAny<string>()))
                .Callback<string>(p => Assert.AreEqual("Cannot find any tables that match: dbo2.T*", p));

            await Create(options).GenerateFileAsync();

            _consoleLogMock
                .Verify(p => p.WriteError(It.IsAny<string>()), Times.Once);
        }

        #endregion
    }
}