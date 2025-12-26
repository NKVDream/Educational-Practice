@echo off
set PGPASSWORD=postgres
set PGRESTORE="C:\Program Files\PostgreSQL\17\bin\pg_restore.exe"
set BACKUP_DIR=D:\PostgreSQL_Backups

echo Available backups:
dir /B "%BACKUP_DIR%\*.backup"
echo.

set /p BACKUP_NAME=Enter backup filename (without path): 
set BACKUP_FILE=%BACKUP_DIR%\%BACKUP_NAME%

%PGRESTORE% -h localhost -p 5432 -U postgres -d NewsApp -v -c "%BACKUP_FILE%"

echo Restore completed!
pause