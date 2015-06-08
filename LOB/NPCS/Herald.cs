$nodeColor["Herald","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Herald","chest"] = "0.2 0.2 0.2 1.000000";
$nodeColor["Herald","pants"] = "0.5 0.3 0 1.000000";
$nodeColor["Herald","larm"] = "0.2 0.2 0.2 1.000000";
$nodeColor["Herald","rarm"] = "0.2 0.2 0.2 1.000000";
$nodeColor["Herald","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Herald","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Herald","lshoe"] = "0.5 0.3 0 1";
$nodeColor["Herald","rshoe"] = $nodeColor["Herald","lshoe"];
$nodeColor["Herald","pack"] = $nodeColor["Herald","pants"];
$pack["Herald"] = "pack";
$smiley["Herald"] = "smiley";
$decal["Herald"] = "knight";
$task["Herald"] = "roam";
//$taskInner["Herald"] = "iron";
$OnClickActionSet["Herald"] = "1";
$roam["Herald"] = 0;
$equip["Herald"] = "goldImage";
$LOB::NPC["Herald","Datablock"] = playerStandardArmor;
$lob::roamMsgCount["Herald"] = -1;
$lob::roamMsg["Herald",$lob::roamMsgCount["Herald"]++] = "Here Ye'! Here Ye'! some vital information!";
$lob::roamMsg["Herald",$lob::roamMsgCount["Herald"]++] = "Oi' there adventurer, come chat with me for a bit!.";
$lob::vision["herald"] = 10;

while(isObject(Herald))Herald.delete();

new scriptObject(Herald);

function Herald::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	%client = %player.client;
	%slo = %client.slo;
	
	%m = "I'm busy.";
	
	%name = %c.name;
	%head = "Dialogue with Herald";
	%m = "Hello there " @ %client.name @ ", view my info!";
	%um1 = "#string View Bounty List #command heraldViewBountyList";
	%um2 = "#string Clear Bounty #command heraldClearWantedLevel";
	%um3 = "#string Nevermind, goodbye. #command";
	%client.herald = %ai;
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with Herald",%m);
}

function serverCmdHeraldViewBountyList(%client)
{
	%bountyList = lob_getWanted();
	
	if(%bountyList $= "0")
	{
		%list = "Nobody is currently wanted.";
		commandToClient(%client,'messageBoxOk',"Bounty List",%list);
		return false;
	}
	
	for(%i=0;%i<getWordCount(%bountyList);%i++)
	{
		%c = getWord(%bountyList,%i);
		
		
		%list = %list SPC %c.name SPC %c.slo.pkPoints SPC "Player Kills\n";
	}
	
	if(%list $= "")
		%list = "Nobody is currently wanted.";
		
	commandToClient(%client,'messageBoxOk',"Bounty List",%list);
}

function serverCmdHeraldClearWantedLevel(%client)
{
	if(%client.slo.pkPoints >= (0 + $lob::wantedLevel))
	{
		%cash = $lob::lowerBountyCost[%client.slo.pkPoints];
		if(%cash $= "0")
		{
			%head = "Uh Oh!";
			%m = "You aren't wanted " @ %client.name @".";
			%um1 = "#string Oh. #command";	
			commandToClient(%client,'setDlg',%head,%m,%um1,%um2);
		}
		else
		{
			%head = "Confirm";
			%m = "Do you want to clear your bounty by paying " @ %cash @ " Gold?";
			%um1 = "#string Yes I'll pay #command HeraldDoClearWantedLevel";
			%um2 = "#string No, never! #command";
			commandToClient(%client,'setDlg',%head,%m,%um1,%um2);
		}
	}
}

function serverCmdHeraldDoClearWantedLevel(%client)
{
	if(vectorDist(%client.player.position,%client.herald.position) <= 10)
	{
		if(%client.slo.pkPoints <= 0)
			return false;
			
		if(%client.slo.inventory.itemCount["gold"] >= $lob::lowerBountyCost[%client.slo.pkPoints])
		{
			%cash = $lob::lowerBountyCost[%client.slo.pkPoints];
			%client.removeFromInventory("gold",$lob::lowerBountyCost[%client.slo.pkPoints]);
			messageClient(%client,'',"\c6You've cleared your wanted level!");
			%client.slo.pkPoints = 0;
			if(%client.slo.area $= "Jail")
			{
				%client.slo.area = "Ordunia";
				%client.instantrespawn();
			}
			
			messageAll('',setKeyWords("\c6" @ %client.name @ " has paid " @ %cash @ " gold and is no longer wanted.",%client.name SPC "wanted","\c6"));
		}
		else
		{
			messageClient(%client,'',"You do not have enough gold!");
		}
	}
}
