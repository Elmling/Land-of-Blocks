function serverCmdSuggestApprove(%client,%a,%b,%c,%d)
{
	if(%client.isAdmin)
	{
		%name = stripTrailingSpaces(trim(%a SPC %b SPC %c SPC %d));
		
		if(isObject(%o = findclientbyname(%name)))
		{
			%o.suggestApproved = true;
			messageClient(%client,'',"\c6" @ %o.name @ " has been approved for a suggestion.");
			messageClient(%o,'',"\c6" @ %client.name @ " has allowed you to add a suggestion. Use /suggest your suggestion");
		}
	}
}

function serverCmdSuggest(%client,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r,%s,%t,%u,%v,%w,%x,%y,%z)
{
	if(%client.suggestApproved)
	{
		echo("ye");
		%m = stripTrailingSpaces(trim(%a SPC %b SPC %c SPC %d SPC %e SPC %f SPC %g SPC %h SPC %i SPC %j SPC %k SPC %l SPC %m SPC %n SPC %o SPC %p SPC %q SPC %r SPC %s SPC %t SPC %u SPC %v SPC %w SPC %x SPC %y SPC %z));

		
		
		if(%m $= "")
			return false;
		
		%f = new fileObject();
		%f.openForAppend("base/lob/Suggestions/Suggestions.txt");	
		%f.writeLine(%client.name @ ": " @ %m);
		%f.close();
		%f.delete();
		
		messageClient(%client,'',"\c6Thanks, your suggestion will be stored and read.");
		lob_msgAdmins(%client.name @ " has logged a suggestion in /suggestions/suggestions.txt");
		%client.suggestApproved=0;
	}
	
}