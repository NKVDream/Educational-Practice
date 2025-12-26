@echo off
set PGPASSWORD=postgres
set PG_BASEBACKUP="C:\Program Files\PostgreSQL\17\bin\pg_basebackup.exe"
set BACKUP_DIR=D:\PostgreSQL_Backups\WAL

if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%"
set BACKUP_FILE=%BACKUP_DIR%\base_%date:~-4%%date:~-7,2%%date:~-10,2%_%time:~0,2%%time:~3,2%

%PG_BASEBACKUP% -h localhost -U postgres -p 5432 -D "%BACKUP_FILE%" -Ft -z -P -v

echo Incremental backup completed: %BACKUP_FILE%
pause