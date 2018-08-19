﻿declare  @DB_Name NVARCHAR(200)
SET @DB_Name = db_name()

USE master 

DECLARE @KILL_ID int,
        @QUERY NVARCHAR(640)


DECLARE GETEXCLUSIVE_CURSOR CURSOR FOR 

SELECT A.SPID FROM sys.SYSPROCESSES A JOIN 
sys.SYSDATABASES B ON A.DBID=B.DBID 
WHERE B.NAME=@DB_Name 
AND A.SPID  > 50 And A.spid <> @@SPID

OPEN GETEXCLUSIVE_CURSOR 
FETCH NEXT FROM GETEXCLUSIVE_CURSOR INTO @KILL_ID 


WHILE(@@FETCH_STATUS =0) 
BEGIN 

SET @QUERY = 'KILL '+ CONVERT(VARCHAR,@KILL_ID) 

print  @query
EXEC (@QUERY) 
FETCH NEXT FROM GETEXCLUSIVE_CURSOR INTO @KILL_ID 

END 
CLOSE GETEXCLUSIVE_CURSOR 
DEALLOCATE GETEXCLUSIVE_CURSOR 