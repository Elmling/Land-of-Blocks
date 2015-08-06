@echo off
	call :updateLocalGitRepository
	echo Local github directory updated.
	pause
goto :eof

:updateLocalGitRepository
	setLocal EnableDelayedExpansion
	call :doFileCopy "C:\Users\Marc\Documents\Blockland\base\LOB" "C:\Users\Marc\Desktop\Land-of-Blocks\LOB"
goto :eof

:fileCopy
	setLocal EnableDelayedExpansion
	if Exist %1% (		
		cls
		echo Pull and Push file to local github.
		echo %1%
		echo %2%
		call :doFileCopy %%1 %%2
	) else (
		echo File does not exist:
		echo %2%
	)
	endLocal
	pause
goto :eof

:doFileCopy
	setLocal EnableDelayedExpansion
	echo LOB\saves\ > exclusionList.txt
	echo LOB\required addons\ >> exclusionList.txt
	echo LOB\Generated_HTML_stats\ >> exclusionList.txt
	echo LOB\Function Formatter\ >> exclusionList.txt
	echo LOB\Backup\ >> exclusionList.txt
	echo LOB\Client\ >> exclusionList.txt
	xcopy "%1" "%2" /D /E /C /R /I /K /Y /EXCLUDE:exclusionList.txt
	del exclusionList.txt
	del C:\Users\Marc\Desktop\Land-of-Blocks\LOB\exclusionList.txt
	::copy /y "C:\Users\Marc\Desktop\Land-of-Blocks\test.bat" "C:\Users\Marc\Desktop\Land-of-Blocks\LOB\test.bat"

goto :eof


set scanPeriodAdd=.
//scanningMSG msg count
:scanningMSG
	set scanMSG=Scanning
	cls
	SET /a result=%2%%400
	SET /a res=%2%%100
	if %result%==0 (
		set scanPeriodAdd=.
	) else if %res%==0 (
		set scanPeriodAdd=%scanPeriodAdd%.
	)
	echo %scanMSG%%scanPeriodAdd%
goto :eof