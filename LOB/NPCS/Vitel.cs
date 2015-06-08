
//self explanatory
//--
$nodeColor["vitel","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["vitel","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["vitel","larm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["vitel","lhand"] = "0.388235 0 0.117647 1 1";
$nodeColor["vitel","lshoe"] = "0.388235 0 0.117647 1 1";
$nodeColor["vitel","pack"] = "0 0.435323 0.831776 1 1";
$nodeColor["vitel","rarm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["vitel","rhand"] = "0.388235 0 0.117647 1 1";
$nodeColor["vitel","rshoe"] = "0.388235 0 0.117647 1 1";
$nodeColor["vitel","chest"] = "0.105882 0.458824 0.768627 1 1";
$decal["vitel"] = "Chef";
$smiley["vitel"] = "smileyPirate2";
$pack["vitel"] = "pack";

$lob::vision["vitel"] = 10;
//--

//function the npc will use
$task["vitel"] = "roam";

//for some task, there is an inner task
//$taskInner["vitel"] = "iron";

//if we click the npc, are they ready for dialogue?
$OnClickActionSet["vitel"] = "1";

//roam range
$roam["vitel"] = 5;

//item to be heald
$equip["vitel"] = "bronzeAxeImage";

//datablock
$LOB::NPC["vitel","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the NPC's head
//--
$lob::roamMsgCount["vitel"] = -1;
$lob::roamMsg["vitel",$lob::roamMsgCount["vitel"]++] = "Greetings adventurer, I have some useful information for you.";
$lob::roamMsg["vitel",$lob::roamMsgCount["vitel"]++] = "Hey you, come over here.";
$lob::roamMsg["vitel",$lob::roamMsgCount["vitel"]++] = "I have something I need you to do.";
$lob::roamMsgCount++;
//--

while(isObject(vitel))vitel.delete();

//SO
new scriptObject(vitel);
vitel.quest1 = "Supplies For Eldria";

function vitel::onObjectSpawned(%this,%npc)
{
	//callback for when they first spawn
	
	//nothing
}

function vitel::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	
	%m = "I'm busy.";

	if(!%client.slo.hasGui)
	{
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to NPCS.");
		return false;
	}		
	%m = "";
	%name = %c.name;
	%head = "Dialogue with vitel";
	if(%client.slo.vitel_q1_complete $= "1")
	{
		%m = "So you want to change your scale?";
		%um1 = "#string Yes #command vitelChangeScale";
	}
	else
	if(%client.slo.woodcuttingLevel < 15 && %client.slo.cookingLevel < 15)
	{
		%m = "Cooking and woodcutting is a valuable skill to develop. Talk to me when you reach 15 in both skills.";
		%um1 = "#string Okay! #command";
	}
	else
	{
		if(%client.slo.vitel_q1_p1 $= "")
		{
			%m = "Times are tough, I cannot chat right now.";
			%um1 = "#string Maybe I can help you. #command vitelMaybeICanHelp";
			%um2 = "#string Oh alright, sorry. #command";
		}
		else if(%client.slo.vitel_q1_p2 $= "")
		{
			%oakLogsNeeded = 100;
			%pineLogsNeeded = 25;
			%steaksNeeded = 25;
			
			%clOakLogs = %client.slo.inventory.itemCountOakLogs;
			%clPineLogs = %client.slo.inventory.itemCountPineLogs;
			%clSteaks = %client.slo.inventory.itemCountCookedSteak;
			
			%oakMath = %oakLogsNeeded - %clOakLogs;
			%pineMath = %pineLogsNeeded - %clPineLogs;
			%steakMath = %steaksNeeded - %clSteaks;
			
			if(%oakMath > 0)
				%m = "I need " @ %oakMAth @ " more Oak logs. ";
			if(%pineMath > 0)
				%m = %m @ "I need " @ %pineMath @ " more Pine Logs. ";
			if(%steakMath > 0)
				%m = %m @ "I need " @ %steakMath @ " more cooked steaks.";
				
			if(%m $= "")
			{
				%m = "Thank you, you were sent by the gods!";
				%um1 = "#string Uhh thanks. #command vitelThanks";
			}
			else
			{
				%um1 = "#string Ok. #command";
			}
			//%um1 = "#string 
			//%um2 = 
		}
		else
		if(%client.slo.vitel_Q1_p3 $= "")
		{
			serverCmdVitelThanks(%client);
		}
		else
		if(%client.slo.vitel_q1_p4 $= "")
		{
			//check if client has sword
			if(%client.slo.vitel_q1_hasSword $= "1")
			{
				lob_finishQuest(%client,"Supplies For Eldria");
				%client.slo.vitel_q1_complete = 1;
				%client.slo.woodcuttingLevel+=5;
				%client.slo.cookingLevel+=5;
				lob_giveAvatar(%client,"EldriasClothes");
				%m = "Thank you! Here is a new avatar, +5 levels in cooking and woodcutting and the ability to change your scale. Talk with me to change it.";
				%um1 = "#string Thanks! #command";
			}
			else
			{
				%m = "Go find our sword, you're our only hope. It's somewhere in Frostbite.";
				%um1 = "#string Okay! #command";
			}
		}
	}
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with vitel",%m);
}

function serverCmdVitelMaybeICanHelp(%this)
{
	%npc = vitel;
	
	%name = %this.name;
	%head = "Dialogue with vitel";
	
	%m = "Well, " @ %name @ " my crew and I are trying to get to Eldria but we cannot get there because of the yetis..";
	%um1 = "#string Go on.. #command vitelGoOn";
	%um2 = "#string Nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdVitelGoOn(%this)
{
	%npc = vitel;
	
	%name = %this.name;
	%head = "Dialogue with vitel";
	
	%m = "Our only cook has died and we are out of food and cannot get back to our lovely town, Eldria.";
	%um1 = "#string Ok, what can i do? #command vitelWhatCanIDo";
	%um2 = "#string Nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdVitelWhatCanIDo(%this)
{
	%npc = vitel;
	
	%name = %this.name;
	%head = "Dialogue with vitel";
	
	%m = "We need supplies, do you think you can help?";
	%um1 = "#string Sure #command vitelSure";
	%um2 = "#string Nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);	
}

function serverCmdVitelSure(%this)
{
	if(%this.slo.vitel_q1_p1 $= "")
	{
		%this.slo.vitel_q1_p1 = 1;
		lob_startQuest(%this,"Supplies For Eldria");
	}
	
	%npc = vitel;
	
	%name = %this.name;
	%head = "Dialogue with vitel";
	
	%m = "Great, we need 100 oak logs, 25 pine logs, and 25 cooked steaks.";
	%um1 = "#string Ok I'll be back!! #command";
	%um2 = "#string Nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);		
}

function serverCmdVitelThanks(%this)
{	
	%this.slo.vitel_q1_p3 = 1;
	%npc = vitel;
	
	%name = %this.name;
	%head = "Dialogue with vitel";
	
	%m = "Lastly " @ %name @ " our crew member died trying to retrieve the Sword of Eldria back from the Yetis, can you please get it for us?";
	%um1 = "#string Sure, where is it? #command vitelwhereisit";
	%um2 = "#string Nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);	
}

function serverCmdVitelWhereIsIt(%this)
{
	%npc = vitel;
	
	%name = %this.name;
	%head = "Dialogue with vitel";
	
	%m = "It is in Frostbite somewhere, please be careful..";
	%um1 = "#string I shall return with it! #command vitelIshallreturn";
	%um2 = "#string Nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);	
}

function serverCmdVitelIshallreturn(%this)
{
	%this.slo.vitel_q1_p2 = "1";
}

function serverCmdVitelChangeScale(%this)
{
	if(%this.slo.vitel_Q1_complete)
	{
		%npc = vitel;
		
		%name = %this.name;
		%head = "Dialogue with vitel";
		
		%m = "Type in /scale INT INT INT. Minimum is 0.8 max is 1.2. This will cost you 10k.";
		%um1 = "#string Gotcha #command";
		%this.canChangeScale = true;
		commandToClient(%this,'setdlg',%head,%m,%um1,%um2);	
	}
}

function serverCmdScale(%this,%x,%y,%z)
{
	if(%this.canChangeScale)
	{
		if(%this.slo.inventory.itemCountGold <= 9999)
		{
			messageClient(%this,'',"\c6You need atleast 10k gold to change your scale!");
			return false;
		}
		
		if(%x >= 0.8 && %x <= 1.3)
		{
			if(%y >= 0.8 && %y <= 1.3)
			{
				if(%z >= 0.8 && %z <= 1.3)
				{
					%this.slo.customScale = true;
					%this.slo.scalex = %x;
					%this.slo.scaley = %y;
					%this.slo.scalez = %z;
					
					%this.player.setScale(%x SPC %y SPC %z);
					%this.slo.inventory.ItemCountGold -= 10000;
					%npc = vitel;
					
					%name = %this.name;
					%head = "Dialogue with vitel";
					
					%m = "Looking good! Thank you for the 10,000 gold!";
					%um1 = "#string Thanks and No problem! #command";
					%this.canChangeScale = "";
					commandToClient(%this,'setdlg',%head,%m,%um1,%um2);	
					
					return true;
				}
			}
		}
		
		messageClient(%this,'',"\c6Invalid paramaters, scale not changed!");
	}
}