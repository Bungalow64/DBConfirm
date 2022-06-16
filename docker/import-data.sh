# This file needs to be encoded for Linux.  If you get a 'syntax error near unexpected token `$'\r'' error, then open this file in Notepad++, Edit-> EOL Conversion -> Unix (LF)

for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i northwind_setup.sql
    if [ $? -eq 0 ]
    then
        echo "northwind_setup.sql completed"
        break
    else
        echo "waiting..."
        sleep 1
    fi
done

for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i sampledb_setup.sql
    if [ $? -eq 0 ]
    then
        echo "sampledb_setup.sql completed"
        break
    else
        echo "waiting..."
        sleep 1
    fi
done