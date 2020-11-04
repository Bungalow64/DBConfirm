;WITH Keys
AS
(
	SELECT
		schemas.name AS SchemaName
		,tables.name AS TableName
		,columns.[name] AS ColumnName
		,ReferencedTable.name AS ReferencedTableName
		,CASE WHEN identity_columns.column_id IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS ReferencesIdentity
	FROM
		sys.foreign_key_columns 
	INNER JOIN sys.columns ON foreign_key_columns.parent_object_id = columns.object_id
		AND foreign_key_columns.parent_column_id = columns.column_id
	INNER JOIN
		sys.tables ON tables.object_id = foreign_key_columns.parent_object_id
	INNER JOIN
		sys.schemas ON tables.schema_id = schemas.schema_id
	INNER JOIN
		sys.tables AS ReferencedTable ON ReferencedTable.object_id = foreign_key_columns.referenced_object_id
	LEFT JOIN
		sys.identity_columns ON identity_columns.object_id = foreign_key_columns.referenced_object_id AND identity_columns.column_id = foreign_key_columns.referenced_column_id
)
SELECT
	COLUMNS.COLUMN_NAME AS ColumnName,
	CASE WHEN COLUMNS.IS_NULLABLE = 'YES' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsNullable,
	COLUMNS.COLUMN_DEFAULT AS DefaultValue,
	COLUMNS.DATA_TYPE AS DataType,
	COLUMNS.CHARACTER_MAXIMUM_LENGTH AS MaxCharacterLength,
	COALESCE(is_identity, CAST(0 AS BIT)) AS IsIdentity,
	CASE WHEN Keys.ColumnName IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsForeignKey,
	COLUMNS.TABLE_SCHEMA AS SchemaName,
	COLUMNS.TABLE_NAME AS TableName,
	Keys.ReferencedTableName,
	COALESCE(Keys.ReferencesIdentity, CAST(0 AS BIT)) AS ReferencesIdentity
FROM INFORMATION_SCHEMA.COLUMNS
INNER JOIN INFORMATION_SCHEMA.TABLES ON COLUMNS.TABLE_NAME = TABLES.TABLE_NAME AND COLUMNS.TABLE_SCHEMA = TABLES.TABLE_SCHEMA AND TABLES.TABLE_TYPE = 'BASE TABLE'
LEFT JOIN sys.identity_columns
ON OBJECT_SCHEMA_NAME(identity_columns.object_id) = COLUMNS.TABLE_SCHEMA 
	AND OBJECT_NAME(identity_columns.object_id) = COLUMNS.TABLE_NAME
	AND identity_columns.name = COLUMNS.COLUMN_NAME
LEFT JOIN Keys
ON Keys.SchemaName = COLUMNS.TABLE_SCHEMA
AND Keys.TableName = COLUMNS.TABLE_NAME
AND Keys.ColumnName = COLUMNS.COLUMN_NAME
WHERE COLUMNS.TABLE_SCHEMA = @SchemaName AND COLUMNS.TABLE_NAME LIKE @TableName
ORDER BY
	COLUMNS.ORDINAL_POSITION ASC