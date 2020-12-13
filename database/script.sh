#sudo docker exec -it sql-server-db /opt/mssql-tools/bin/sqlcmd -S localhost \
 #  -U SA -P 'Ytrewq@321' \
 #  -Q 'RESTORE FILELISTONLY FROM DISK = "/var/opt/mssql/backup/Library.bak"' \
 #  | tr -s ' ' | cut -d ' ' -f 1-2


sudo docker exec -it sql-server-db /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U SA -P 'Ytrewq@321' \
   -Q 'RESTORE DATABASE Library_Db FROM DISK = "/var/opt/mssql/backup/Library.bak" WITH MOVE "Library_Db" TO "/var/opt/mssql/data/Library_Db.mdf", MOVE "Library_Db_log" TO "/var/opt/mssql/data/Library_Db_log.ldf"'
