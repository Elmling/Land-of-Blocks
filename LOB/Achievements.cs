$achievements::lineCount = -1;
$achievements::line[$achievements::lineCount++] = "//Achievement #achievementName";
$achievements::line[$achievements::lineCount++] = "//------------------------------";
$achievements::line[$achievements::lineCount++] = "//IMPORTANT: When declaring a package it must follow this syntax: Achievement_AchievementNameOfThisFile without the extension of course (.cs)";
$achievements::line[$achievements::lineCount++] = "//Here, you will write the code necessary to complete an achievement.";
$achievements::line[$achievements::lineCount++] = "//You will use the achievements.onAchieved(%client,AchievementName); function when a user has successfully achieved a landmark / goal.";
$achievements::line[$achievements::lineCount++] = "//You can either call this function with the first two arguments or use as the function like so:";
$achievements::line[$achievements::lineCount++] = "//achievements.onAchieved(%client,%nameOfAchievement,%alertAll,\"Message\",\"SoundDataBlock\");";
$achievements::line[$achievements::lineCount++] = "//Any field you leave blank will use the default settings (besides the first and second argument which are required).";
$achievements::line[$achievements::lineCount++] = "//--------- WRITE CODE BELOW---------";
$achievements::line[$achievements::lineCount++] = "";

if(!isFile("base/lob/achievements/manager/manager.cs"))
{
	new scriptObject(achievements)
	{
		count = -1;
	};
	
	achievements.save("base/lob/achievements/manager/manager.cs");
}
else
	exec("base/lob/achievements/manager/manager.cs");
	
function achievements::registerAll(%this)
{
	//put achievements to be registered on start up here:
	achievements.register("a gift to ingvar");
	achievements.register("Just Starting Out");
}

achievements.registerAll();

function achievements::newAchievement(%this,%name,%noDelete)
{
	if(%name $= "")
	{
		warn("Missing required field(s) %name!");
		return 0;
	}
	
	%name = strReplace(%name," ","_");
	%path = "base/lob/achievements/" @ %name @ "/" @ %name @ ".cs";
	
	if(isFile(%path) && !%noDelete)
	{
		messageBoxYesNo("Warning","There is already an achievement with this name, override?","achievements.deleteAchievement(%path);","achievements.newAchievement(%name,1);");
		return 0;
	}
	
	%f = new fileObject();
	%f.openForWrite(%path);
	
	for(%i=0;%i<$achievements::lineCount+1;%i++)
	{
		%f.writeLine(strReplace($achievements::line[%i],"#achievementName",%name));
	}
	
	%f.close();
	%f.delete();
	
	achievements.save("base/lob/achievements/Manager/Manager.cs");
	
	return true;
}

function achievements::register(%this,%name)
{
	%name = strReplace(%name," ","_");
	
	if(!isFile(%path = "base/lob/achievements/" @ %name @ "/" @ %name @ ".cs"))
		return 0;
	
	exec(%path);
}

function achievements::unRegister(%this,%name)
{
	%name = strReplace(%name," ","_");
	
	if(!isFile(%path = "base/lob/achievements/" @ %name @ "/" @ %name @ ".cs"))
		return 0;
	
	if(!isPackage("Achievement_"@%name))
	{
		error("You've set up your package incorrectly!");
		return 0;
	}
	
	eval("deactivatePackage(Achievement_"@%name@");");
	
	return true;
}

function achievements::onAchieved(%this,%client,%name,%alertAll,%message,%soundDataBlock)
{
	%sound = false;
	
	if(%name $= "")
	{
		warn("Name field is missing for an achievement, halting function!");
		return 0;
	}
	
	if(%alertAll $= "" || %alertAll == 0)
		%alertAll = true;
		
	if(%message $= "")
		%message = setKeyWords("\c6" @ %client.name @ " has completed the " @ strReplace(%name,"_"," ") @ " achievement.",%client.name SPC strReplace(%name,"_"," "),"\c6");
		
	if(isObject(%soundDataBlock))
		%sound = true;
		
	if(%alertAll)
	{
		messageAll('',%message);
		if(%sound)
			serverPlay2D(%soundDataBlock);
	}
	else
	{
		messageClient(%client,'',strReplace(%message,%client.name@" has","You've"));
	}
	
	return true;
}