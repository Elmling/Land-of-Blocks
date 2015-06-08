
//self explanatory
//--
$nodeColor["Eldin","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["Eldin","pants"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["Eldin","larm"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Eldin","lhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Eldin","lshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Eldin","pack"] = "0.750 0.750 0.750 1.000 1";
$nodeColor["Eldin","rarm"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Eldin","rhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Eldin","rshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Eldin","chest"] = "0.392157 0.196078 0 1 1";
$nodeColor["Eldin","pack"] = $nodeColor["Eldin","pants"];
$pack["Eldin"] = "pack";
$smiley["Eldin"] = "smileyevil1";
$decal["Eldin"] = "knight";
//--

//function the Eldin will use
$task["Eldin"] = "roam";

//for some task, there is an inner task
//$taskInner["Eldin"] = "iron";

//if we click the Eldin, are they ready for dialogue?
$OnClickActionSet["Eldin"] = "1";

//roam range
$roam["Eldin"] = 5;

//item to be heald
$equip["Eldin"] = "bronzeAxeImage";

//datablock
$LOB::Eldin["Eldin","Datablock"] = playerStandardArmor;

$lob::vision["Eldin"] = "10";

//Stuff they say that users can see above the Eldin's head
//--
$lob::roamMsgCount["Eldin"] = -1;
$lob::roamMsg["Eldin",$lob::roamMsgCount["Eldin"]++] = "The Ogres are ruining the forest!";
$lob::roamMsg["Eldin",$lob::roamMsgCount["Eldin"]++] = "What am I going to do about these Ogres!";
$lob::roamMsg["Eldin",$lob::roamMsgCount["Eldin"]++] = "Argggghh, we need to stop those damned Ogres!!";
//--

while(isObject(Eldin))Eldin.delete();

//SO
new scriptObject(Eldin);
eldin.quest1 = "Ogres must fall";

function Eldin::onObjectSpawned(%this,%Eldin)
{

}

function Eldin::onClick(%this,%ai,%player)
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
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to Eldin.");
		return false;
	}		

	%name = %c.name;
	%head = "Dialogue with Eldin";
	if(%client.slo.eldin_q1_completed $= "1")
	{
		%m = "Hey there " @ %client.name @ "!";
		%um1 = "#string Goodbye #command";
	}
	else
	if(%client.slo.eldin_q1_started $= "")
	{
		%m = "The Ogres! THE Ogres!";
		%um1 = "#string Woah calm down, what about them? #command EldinCalmDown";
		%um2 = "#string Ogres are weak!  #command EldinOgresAreWeak";
		%um3 = "#string Nevermind #command";
	}
	else
	if(%client.slo.eldin_q1_completed $= "")
	{
		if(%client.slo.eldin_q1_hasMixture $= "")
		{
			serverCmdEldinHowCanIHelp(%client);
		}
		else
		{
			if(%client.slo.eldin_q1_hasToxicLeaf $= "")
				serverCmdEldinSoWhatNow(%client);
			else
				serverCmdEldinIHaveToxicLeaf(%client);
		}
		return true;
	}
	else
	{
	
	}
	
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3);
}

function serverCmdEldinCalmDown(%client)
{
	%head = "Dialogue with Eldin";
	%m = "I'm sorry " @ %client.name @ ". They are invading Alywell's Forest and we need to stop them!";
	%um1 = "#string No problem, how can I help? #command eldinHowCanIHelp";
	%um2 = "#string Quit talking then and get to killing! #command";
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdEldinOgresAreWeak(%client)
{
	%head = "Dialogue with Eldin";
	%m = "Well that's good to hear " @ %client.name @ ". I hate to ask this of you, but do you mind helping me fight them?";
	%um1 = "#string Yeah, sure thing Eldin #command eldinHowCanIHelp";
	%um2 = "#string No way, that sounds scary! #command";
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdEldinHowCanIHelp(%client)
{
	%head = "Dialogue with Eldin";
	%m = "Not too far, there is an abandoned house. There is a skull and I need what's under it - a mixture for a potion. Can you go find that for me?";
	%um1 = "#string Yes, I'll be back #command";
	%um2 = "#string Not now #command";
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
	if(%client.slo.eldin_q1_Started $= "")
	{
		lob_startQuest(%client,eldin.quest1);
		%client.slo.eldin_q1_started = true;
	}
}

function serverCmdEldinIHavePotion(%client)
{
	if(%client.slo.eldin_q1_hasMixture)
	{
		%head = "Dialogue with Eldin";
		%m = "Ahh thanks a lot " @ %client.name @ ". This mixture here will be the key to the downfall of the Ogres!";
		%um1 = "#string You sound crazy! #command";
		%um2 = "#string So what now? #command eldinSoWhatNow";
		commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
	}
}

function serverCmdEldinSoWhatNow(%client)
{
	%head = "Dialogue with Eldin";
	%m = "What we need now is part two of the solution. I hear Ogres carry toxic leaves with them which will allow me to create the solution. Can you go find one for me?";
	%um1 = "#string Yeah! I guess I can slay some Ogres. #command eldinSlaySomeOgres";
	%um2 = "#string I can't right now. #command";
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdEldinSlaySomeOgres(%client)
{
	%head = "Dialogue with Eldin";
	%m = "You have been sent by the gods " @ %client.name @ ", I give you many thanks. Come talk with me when you have one toxic leaf.";
	%um1 = "#string Okay. #command";
	commandToClient(%client,'setdlg',%head,%m,%um1);
	%client.slo.eldin_q1_toxicLeafPending = true;
}

function serverCmdEldinIHaveToxicLeaf(%client)
{
	if(%client.slo.eldin_q1_hasToxicLeaf)
	{
		%client.slo.eldin_q1_hasToxicLeaf = "";
		%expEarned = mfloor($lob::expNeeded["combat",%client.slo.combatLevel] * 0.75);
		giveExp(%client,"combat",%expearned);
		lob_giveAvatar(%client,"Noble");
		%head = "Dialogue with Eldin";
		%m = "Astounding " @ %client.name @ ". For your hard work you've earned the Noble avatar and " @ %expEarned @ " EXP in combat. I can't thank you enough.";
		%um1 = "#string Wow, thanks. #command";
		lob_finishQuest(%client,eldin.quest1);
		commandToClient(%client,'setdlg',%head,%m,%um1);	
		%client.slo.eldin_q1_completed = true;
	}
}