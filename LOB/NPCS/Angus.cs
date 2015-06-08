$nodeColor["Angus","headskin"] = "0.803922 0.666667 0.486275 1.000000";
$nodeColor["Angus","chest"] = "0.803922 0.666667 0.486275 1.000000";
$nodeColor["Angus","pants"] = "0.239216 0.349020 0.666667 1.000000";
$nodeColor["Angus","larm"] = "0.803922 0.666667 0.486275 1.000000";
$nodeColor["Angus","rarm"] = "0.803922 0.666667 0.486275 1.000000";
$nodeColor["Angus","lhand"] = "0.078431 0.078431 0.070588 1.000000";
$nodeColor["Angus","rhand"] = "0.078431 0.078431 0.070588 1.000000";
$nodeColor["Angus","lshoe"] = "0.078431 0.078431 0.070588 1.000000";
$nodeColor["Angus","rshoe"] = $nodeColor["Angus","lshoe"];
//$nodeColor["Angus","pack"] = $nodeColor["Angus","pants"];
//$pack["Angus"] = "pack";
$smiley["Angus"] = "male07smiley";
$decal["Angus"] = "";
$task["Angus"] = "smithing";
$equip["Angus"] = "hammerImage";
$OnClickActionSet["Angus"] = true;
$roam["Angus"] = 0;
$LOB::NPC["Angus","Datablock"] = playerStandardArmor;
$lob::vision["angus"] = 10;

$lob::roamMsgCount["Angus"] = -1;
$lob::roamMsg["Angus",$lob::roamMsgCount["Angus"]++] = "Sell or buy some stuff.";
$lob::roamMsg["Angus",$lob::roamMsgCount["Angus"]++] = "I have some weapons for you!";
$lob::roamMsg["Angus",$lob::roamMsgCount["Angus"]++] = "Need some tools?";

while(isObject(Angus))Angus.delete();

new scriptObject(Angus);

function Angus::onClick(%this,%ai,%player)
{
	cancel(%ai.smithloop);
	cancel(%ai.onClickLoop);
	
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%ai.setimageTrigger(1,0);
	
	%client = %player.client;
	%pp = %player.position;
	
	%mp = %ai.position;
	%ai.setMoveDestination(%mp);
	
	%vd = vectorDist(%mp,%pp);
	
	if(%vd >= 5)
	{
		%ai.messageBoxPlayer = "";
		%ai.smithing();
		%ai.setAimLocation(_angusAim.position);
		return true;
	}
	
	if(%ai.messageBoxPlayer != %player)
	{
		%ai.messageBoxPlayer = %player;
		//commandtoClient(%client,'messageBoxOk',"Dialogue with "@%ai.name,"Argg, hello there "@%client.name@". Cutting down these Acacia trees is hard work, but I'm a smither. Bring me 10 iron and I will make you an Iron Sword. Now leave me be.. Argg..");
		
		%um1 = "#string What are you doing? #command AngusWhatAreYouDoing";
		%um2 = "#string How do you smith and smelt stuff? #command AngusSmithAndSmelt";
		%um3 = "#string Goodbye #command AngusGoodBye";
		
		commandToClient(%client,'setdlg',"Dialogue with " @ %ai.name,"Hey there " @ %client.name @ ", what can I do you for?",%um1,%um2,%um3);
	}
	
	if(%ai.getAimLocation() != %pp)
		%ai.setAimLocation(%pp);
		
	%ai.onClickLoop = %this.schedule(1000,onclick,%ai,%player);
}

function serverCmdAngusWhatAreYouDoing(%client)
{
	commandToClient(%client,'setDlg',"Dialogue with Angus","Why I am smithing. Get yourself some bars and join in on the fun, adventurer!","#string Ok #command angusOk");
}

function serverCmdAngusSmithAndSmelt(%client)
{
	commandToClient(%client,'setDlg',"Dialogue with Angus","Go mine some ores and then interact with the furnace window over there. Once you create the bars you want, come interact with an anvil and make some items!","#string Wow, Thanks. #command angusOk");
}

function serverCmdAngusOk(%client)
{
	%npc = findBotByName("Angus");
	%npc.messageBoxPlayer = "";
	angus.onClick(%npc,%client.player);
}

function serverCmdAngusGoodBye(%client)
{
	serverCmdAngusDone(%client);
}

function serverCmdAngusDone(%client)
{
	%npc = findBotByName("angus");
	%npc.messageBoxPlayer = "";
	cancel(%npc.onClickLoop);
	%npc.smithing();
	commandToClient(%client,'closeDialogue');
	%npc.setAimLocation(_angusAim.position);
}