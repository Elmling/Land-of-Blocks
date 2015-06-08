function servercmdreportBug(%client,%title,%text)
{
	%time = getSimTime();
	
	if(%time - %client.lastBugReportTime > 60000)
	{
		messageClient(%client,'',"\c6You can only report bugs every 60 seconds.");
		return false;
	}
	
	%client.lastBugReportTime = %time;
	
	%f = new fileObject();
	%f.openForWrite("base/lob/Bugs/" @ %client.name @ " - " @ %title @ ".txt");
	%f.writeLine(%text);
	%f.close();
	%f.delete();
	
	messageclient(%client,'',"\c6Thank you, your Bug Report will be reviewed.");
}