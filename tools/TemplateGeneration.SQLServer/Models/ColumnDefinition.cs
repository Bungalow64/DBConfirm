using System;
using System.Data;

namespace DBConfirm.TemplateGeneration.Models
{
    public class ColumnDefinition
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public bool IsNullable { get; set; }
        public object DefaultValue { get; set; }
        public string DataType { get; set; }
        public int? MaxCharacterLength { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsForeignKey { get; set; }
        public string ReferencedTableName { get; set; }
        public bool ReferencesIdentity { get; set; }

        public string SafeColumnName => ToUpperFirstLetter(ColumnName).Replace(' ', '_');

        public bool RequiresDefaultData => !IsNullable && (DefaultValue ?? DBNull.Value) == DBNull.Value && !IsIdentity;

        public string ActualType
        {
            get
            {
                switch (DataType)
                {
                    case "nvarchar":
                    case "varchar":
                    case "char":
                    case "nchar":
                    case "ntext":
                    case "text":
                        return "string";
                    case "int":
                    case "smallint":
                        return "int";
                    case "bigint":
                        return "long";
                    case "binary":
                    case "image":
                    case "rowversion":
                    case "timestamp":
                    case "varbinary":
                        return "byte[]";
                    case "bit":
                        return "bool";
                    case "decimal":
                    case "money":
                    case "numeric":
                    case "smallmoney":
                        return "decimal";
                    case "float":
                        return "double";
                    case "date":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                        return "DateTime";
                    case "time":
                        return "TimeSpan";
                    case "real":
                        return "float";
                    case "tinyint":
                        return "byte";
                    case "uniqueidentifier":
                        return "Guid";
                    case "datetimeoffset":
                    case "xml":
                    default:
                        return "object";
                }
            }
        }

        public bool TypeRequiresSystem
        {
            get
            {
                switch (ActualType)
                {
                    case "DateTime":
                    case "TimeSpan":
                    case "Guid":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool RequiredPlaceholder => IsForeignKey && !IsNullable;

        public ColumnDefinition(DataRow row)
        {
            TableName = row["TableName"].ToString();
            SchemaName = row["SchemaName"].ToString();
            ColumnName = row["ColumnName"].ToString();
            IsNullable = (bool)row["IsNullable"];
            DefaultValue = row["DefaultValue"];
            DataType = row["DataType"].ToString();
            MaxCharacterLength = ToInt(row["MaxCharacterLength"]);
            IsIdentity = (bool)row["IsIdentity"];
            IsForeignKey = (bool)row["IsForeignKey"];
            ReferencedTableName = row["ReferencedTableName"]?.ToString();
            ReferencesIdentity = (bool)row["ReferencesIdentity"];
        }

        private string GetDefaultText()
        {
            if (RequiredPlaceholder)
            {
                return "Placeholders.IsRequired()";
            }
            return ActualType switch
            {
                "string" => $"\"{TruncateLongString($"Sample{ColumnName}")}\"",
                "int" => "50",
                "long" => "50",
                "byte[]" => "new byte[] { 0x68, 0x65, 0x6c, 0x6c, 0x29 }",
                "bool" => "true",
                "decimal" => "50.5",
                "double" => "50.5",
                "DateTime" => "DateTime.Parse(\"22-Oct-2020\")",
                "TimeSpan" => "TimeSpan.FromMinutes(10)",
                "Single" => "50",
                "byte" => "0x65",
                "Guid" => "Guid.Parse(\"729e8f2e-5101-4ab7-a7ed-52e58c8cf9b1\")",
                "object" => "\"<object>\"",
                _ => string.Empty,
            };
        }

        private string TruncateLongString(string value)
        {
            return value.Substring(0, MaxCharacterLength == -1 ? Math.Min(value.Length, 100) : Math.Min(value.Length, MaxCharacterLength ?? 100));
        }

        private static int? ToInt(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return (int)value;
        }

        public string ToFluentRow(string indent, string className)
        {
            string root = $"{indent}public {className} With{SafeColumnName}({ActualType} value) => SetValue(\"{ColumnName}\", value);";
            if (ReferencesIdentity)
            {
                root += $"{Environment.NewLine}{indent}public {className} With{SafeColumnName}(IResolver resolver) => SetValue(\"{ColumnName}\", resolver);";
            }
            return root;
        }

        public string ToDefaultData()
        {
            return @$"[""{ColumnName}""] = {GetDefaultText()}";
        }

        public static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }
}
