$nodeColor["Gandalf","headskin"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gandalf","chest"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gandalf","pants"] = "0.9 0.9 0.9 1";
$nodeColor["Gandalf","larm"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gandalf","rarm"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gandalf","lhand"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gandalf","rhand"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gandalf","lshoe"] = "0.9 0.9 0.9 1";
$nodeColor["Gandalf","rshoe"] = $nodeColor["Gandalf","lshoe"];
//$nodeColor["Gandalf","pack"] = $nodeColor["Gandalf","pants"];
//$pack["Gandalf"] = "pack";
$smiley["Gandalf"] = "smileyevil1";
$decal["Gandalf"] = "worm_engineer";
$task["Gandalf"] = "mine";
$taskInner["gandalf"] = "copper";
$OnClickActionSet["Gandalf"] = "true";
$equip["gandalf"] = "bronzePickAxeImage";
$LOB::NPC["Gandalf","Datablock"] = playerStandardArmor;
$lob::vision["Gandalf"] = 10;

while(isObject(Gandalf))Gandalf.delete();

new scriptObject(Gandalf);

function Gandalf::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	cancel(%ai.mineloop);
	cancel(%ai.onClickLoop);
	
	%ai.setimageTrigger(1,0);
	
	%client = %player.client;
	%pp = %player.position;
	
	%mp = %ai.position;
	%ai.setMoveDestination(%mp);
	
	%vd = vectorDist(%mp,%pp);
	
	if(%vd >= 5)
	{
		%ai.messageBoxPlayer = "";
		%ai.mine();
		return true;
	}
	
	if(%ai.messageBoxPlayer != %player)
	{
		%ai.messageBoxPlayer = %player;
		//commandtoClient(%client,'messageBoxOk',"Dialogue with "@%ai.name,"Argg, hello there "@%client.name@". Cutting down these Acacia trees is hard work, but I'm a smither. Bring me 10 iron and I will make you an Iron Sword. Now leave me be.. Argg..");
		
		%um1 = "#string What are you doing? #command GandalfwhatAreYouDoing";
		%um2 = "#string Do you have any quests for me? #command GandalfQuestCheck";
		%um3 = "#string Goodbye #command GandalfgoodBye";
		
		commandToClient(%client,'setdlg',"Dialogue with " @ %ai.name,"Hey there " @ %client.name @ ", what can I do you for?",%um1,%um2,%um3);
	}
	
	if(%ai.getAimLocation() != %pp)
		%ai.setAimLocation(%pp);
		
	%ai.onClickLoop = %this.schedule(1000,onclick,%ai,%player);
}

function serverCmdGandalfWhatAreYouDoing(%client)
{
	commandToClient(%client,'setDlg',"Dialogue with Gandalf","I'm mining, you'll find out that around here, you have to work for what you want!","#string Okay #command gandalfOk");
}

function serverCmdGandalfQuestCheck(%client)
{
	commandToClient(%client,'setDlg',"Dialogue with Gandalf","No, but check back with me real soon, I'll work on finding something for you to do.","#string Okay #command gandalfOk");
}

function serverCmdGandalfGoodBye(%client)
{
	%npc = findbotbyName("gandalf");
	%npc.messageBoxPlayer = "";
	serverCmdGandalfDone(%client);
}

function serverCmdGandalfOk(%client)
{
	%npc = findBotByName("gandalf");
	%npc.messageBoxPlayer = "";
	gandalf.onClick(%npc,%client.player);
}

function serverCmdGandalfDone(%client)
{
	%npc = findBotByName("gandalf");
	commandToClient(%client,'closeDialogue');
	cancel(%npc.onClickLoop);
	%npc.mine();
	
}