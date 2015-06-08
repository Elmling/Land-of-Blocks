@echo off
	call :main
goto :eof

::Main, handles user input
:main
	echo 1.NPC (Non Aggressive)
	echo 2.Enemy (Can be aggressive)
	::Get user input
	set /p choice=
	::Logic
	if %choice%==1 (
		::If user choice is 1, call :create with
		::The argument npc
		call :create npc
	) else if %choice%==2 (
		::If user choice is 2, call :create with
		::The argument enemy
		call :create enemy
	)
goto :eof

::Gets valid info to then create a new file
:create
	::Clear the debug screen
	cls
	
	::Setlocal is used because variables in block statements
	::Aren't updated. Notice we will use !varname! inside of
	::The block statements below
	::------------------------------
	setlocal enabledelayedexpansion
	::------------------------------
	
	::If user is creating a new Enemy
	if "%1%"=="enemy" (
		echo -Enter a name for your new Enemy-
		::Get user input for the enemy name
		set /p enemyName=
		::If user input is null
		if "!enemyName!"=="" (
			echo You need to enter a name.
			::Wait 3 seconds
			timeout 3
			::Call the function again
			call :create enemy
		) else (
			::Create the new file via copyFiles function
			call :copyFiles !enemyName! enemy
		)
	) else if "%1%"=="npc" (
		::^ If the user is creating a new NPC
		echo -Enter a name for your new NPC-
		::Get user input for the NPC name
		set /p npcName=
		::If user input is null
		if "!npcName!"=="" (
			echo You need to enter a name.
			::Wait 3 seconds
			timeout 3
			::Call the function again
			call :create npc
		) else (
			::Create the new file via copyFiles function
			call :copyFiles !npcName! npc
		)
	)
goto :eof

::Creates a new NPC/Enemy file with basic formatting for Land of Blocks
:copyFiles
	if "%2%"=="npc" (
		::Perform Copy
		copy %CD%\NPCformat.txt %CD%\%1%.cs
	) else if "%2%"=="enemy" (
		::Perform Copy
		copy %CD%\ENEMYformat.txt %CD%\%1%.cs
	)
	::Clear the debug screen
	cls
	echo File created, view the new file now? (y/n):
	set /p openFile=
	if "%openFile%"=="y" (
		::Open the file to let the user edit the new AI.
		start %CD%\%1%.cs
	)
goto :eof