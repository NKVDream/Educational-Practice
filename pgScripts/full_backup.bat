@echo off
set PGDUMP="C:\Program Files\PostgreSQL\17\bin\pg_dump.exe"
set BACKUP_DIR="D:\PostgreSQL_Backups"
set DATE=%date:~-4%-%date:~3,2%-%date:~0,2%
set TIME=%time:~0,2%-%time:~3,2%-%time:~6,2%
set BACKUP_FILE=%BACKUP_DIR%\NewsApp_backup_%DATE%_%TIME%.backup
set PGPASSWORD=postgres

echo Creating backup of NewsApp...
if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%"
%PGDUMP% -h localhost -p 5432 -U postgres -F c -b -v -f "%BACKUP_FILE%" NewsApp
if %ERRORLEVEL% EQU 0 (
    echo Backup successfully created: %BACKUP_FILE%
) else (
    echo Backup FAILED!
)
pause