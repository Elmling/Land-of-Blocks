function serverCmdDuel(%this,%name)
{
	if(%name $= "")return false;
	%cl = findclientbyname(%name);
	
	%cl.duel[%this.name] = true;
	
	if(%this.duel[%cl.name])
	{
		messageClient(%this,'',"\c6You are dueling " @ %cl.name);
		messageClient(%cl,'',"\c6You are dueling " @ %this.name);
		%this.duel = true;
		%cl.duel = true;
		%this.wild = true;
		%cl.wild = true;
		%this.dueling = %cl;
		%cl.dueling = %this;
	}
	else
	{
		messageClient(%this,'',"\c6Duel request sent");
		messageClient(%cl,'',"\c6" @ %this.name @" wants to duel with you, type /duel " @ %this.name@".");
	}
}

