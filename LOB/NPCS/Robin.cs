
//self explanatory
//--
$nodeColor["Robin","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Robin","chest"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Robin","pants"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Robin","larm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Robin","rarm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Robin","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Robin","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Robin","lshoe"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Robin","rshoe"] = $nodeColor["Robin","lshoe"];
$nodeColor["Robin","pack"] = $nodeColor["Robin","pants"];
$pack["Robin"] = "pack";
$smiley["Robin"] = "smileyevil1";
$decal["Robin"] = "knight";
$lob::vision["robin"] = 10;
//--

//function the npc will use
$task["Robin"] = "roam";

//for some task, there is an inner task
//$taskInner["Robin"] = "iron";

//if we click the npc, are they ready for dialogue?
$OnClickActionSet["Robin"] = "1";

//roam range
$roam["Robin"] = 5;

//item to be heald
$equip["Robin"] = "bronzeAxeImage";

//datablock
$LOB::NPC["Robin","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the NPC's head
//--
$lob::roamMsgCount["Robin"] = -1;
$lob::roamMsg["Robin",$lob::roamMsgCount["Robin"]++] = "Greetings adventurer, I have some useful information for you.";
$lob::roamMsg["Robin",$lob::roamMsgCount["Robin"]++] = "Hey you, come over here.";
$lob::roamMsg["Robin",$lob::roamMsgCount["Robin"]++] = "I have something I need you to do.";
$lob::roamMsgCount++;
//--

while(isObject(Robin))Robin.delete();

//SO
new scriptObject(Robin);
robin.quest1 = "Robins Payback";
robin.quest2 = "Robins Plight";

function robin::onObjectSpawned(%this,%npc)
{
	//callback for when they first spawn
	
	//nothing
}

function Robin::onClick(%this,%ai,%player)
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

	%name = %c.name;
	%head = "Dialogue with Robin";
	%m = "My name is robin, how may I be of service to you?";
	%um1 = "#string What is this place? #command robinWhatIsThisPlace";
	%um2 = "#string What can I do? #command robinWhatCanIDo";
	%um3 = "#string Do you have any tools I can have? #command robinTools";
	%um4 = "#string Do you have any quests? #command robinQuests";
	
	if(%client.slo.robin_q1_Started && !%client.slo.robin_q1_completed)
	{
		%um5 = "#string Regarding the quest #command robinQuestCheck";
		%um6 = "#string Nevermind. #command";
	}
	else
		%um5 = "#string Nevermind. #command";
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with Robin",%m);
}

function serverCmdRobinWhatIsThisPlace(%this)
{
	commandToClient(%this,'setdlg',"Dialogue with Robin","This is the town Ordunia. There are a lot of things to do here in this town, have a look around " @ %this.name @ "!","#string Okay. #command robinOk");
}

function serverCmdRobinWhatCanIDo(%this)
{
	commandToClient(%this,'setDlg',"Dialogue with Robin","You can do lots, mine, woodcut, fight monsters, fish and much more!","#string Okay. #command robinOk");
}

function serverCmdRobinQuests(%this)
{
	if(!%this.slo.robin_q1_started)
	{
		commandToClient(%this,'setDlg',"Dialogue with Robin","As a matter of fact, I recently ran into bandits. They stole my grand fathers sword from me. If you can get it back, i'll be in debted to you!","#string Sure I'll go find them now. #command robinStartQuest","#string No way! #command");
	}
	else
	if(%this.slo.robin_q1_started && !%this.slo.robin_q1_completed)
	{
		//tell them they need to do the pending quest still
		commandToClient(%this,'setDlg',"Dialogue with Robin","You are already working on one " @ %this.name @". Remember? Retrieving my Grandfather's sword from those savage bandits!","#string Oh, right.. #command robinOk");
	}
	else
	if(%this.slo.robin_q1_completed && !%this.slo.robin_q2_started)
	{
		commandToClient(%this,'setDlg',"Dialogue with Robin","Something has been fooling around with our Well over there. Do you think you can get figure out what or who it is?","#string Yeah, I'll try. #command robinStartQuest","#string Not now. #command");
	}
	else
	{
		commandToClient(%this,'setdlg',"Dialogue with Robin","Sorry " @ %this.name @ ", I don't really have anything for you to do right now. Try going around and talking with the other people here in Ordunia.","#string Okay. #command robinOk");
	}
}

function serverCmdRobinStartQuest(%this)
{
	if(!%this.slo.robin_q1_started)
	{
		%this.slo.robin_q1_started = true;
		%m = setKeyWords("\c6" @ %this.name @ " has started the quest " @ robin.quest1 @ "!",%this.name SPC robin.quest1,"\c6");
		messageAll('',%m);
		commandToClient(%this,'closeDialogue');
	}
	else
	if(%this.slo.robin_q1_completed && !%this.slo.robin_q2_started)
	{
		%this.slo.robin_q2_started = true;
		%m = setKeyWords("\c6" @ %this.name @ " has started the quest " @ robin.quest2 @ "!",%this.name SPC robin.quest2,"\c6");
		messageAll('',%m);
		commandToclient(%this,'closeDialogue');
		messageClient(%this,'',"\c6[QUEST] I should first find Ordunia's Well.");
	}
}

function serverCmdRobinQuestCheck(%this)
{
	if(!%this.slo.robin_q1_completed)
	{
		if(%this.slo.robin_q1_hassword $= 1)
		{
			%this.slo.robin_q1_completed = true;
			%m = setKeyWords("\c6" @ %this.name @ " has finished the quest " @ robin.quest1 @ "!",%this.name SPC robin.quest1,"\c6");
			messageAll('',%m);
			
			%m = setKeyWords("\c6You've recieved 100 gold for completing the quest!","100","\c6");
			smartMessage(%this,%m,1000);
			%this.addToInventory("gold",100);
			%this.removeFromInventory("grandfather's Sword",1);
			
			//bring up the dlg saying thanks, letting player's choose a reward, etc
			
		}
		else
		{
			commandToClient(%this,'setdlg',"Dialogue with Robin","You still haven't found my Grandfather's sword " @ %this.name @ ". Please, go find the Bandits that took it from me and slay them!","#string Okay. #command robinOk");
		}
	}
}

function serverCmdRobinTools(%this)
{
	if(%this.slo.robin_hasTools)
	{
		commandToClient(%this,'setDlg',"Dialogue with Robin","I've already given you tools, " @ %this.name @ ". Don't beg!","#string Okay. #command robinOk");
	}
	else
	{
		%this.slo.robin_hasTools = true;
		%tc=-1;
		%t[%tc++] = "bronzeShortSword";
		%t[%tc++] = "bronzePickAxe";
		%t[%tc++] = "bronzeAxe";
		%tc++;
		
		for(%i=0;%i<%tc;%i++)
		{
			%item = %t[%i];
			%this.addToInventory(%item,1);
			commandToClient(%this,'showTip',"You've gotten some items. Press M and open your inventory.",10000);
			//%this.addToDefaultInventory(%item);
		}
		
		commandToClient(%this,'setDlg',"Dialogue with Robin","Yeah, here is a Short Sword, Pickaxe and an axe.");
	}
}

function serverCmdRobinOk(%this)
{
	%npc = findBotByName("robin");
	robin.onClick(%npc,%this.player);
}

function serverCmdRobinQuitMessingWithWell(%this)
{
	if(%this.slo.robin_q2_foundclue1 $= "")
		return false;
		
	if(%this.slo.robin_q2_ogreCount $= "")
		%this.slo.robin_q2_ogreCount = 0;
	
	if(%this.slo.robin_q2_ogreCount < 5)
		commandToClient(%this,'setDlg',"Dialogue with OgreGeneral","Until you've slayed atleast 5 of my crew, I will tell them to do nothing!","#string Alright, this'll be easy. #command RobinLetsSlay");
	else
	{
		if(%this.slo.inventory.itemCount["copperOres"] >= 100 && %this.slo.inventory.itemcount["tinOres"] >= 100)
		{
			%this.slo.robin_q2_takeGift = true;
			%this.removeFromInventory("copperOres",100);
			%this.removeFromInventory("tinOres",100);
			commandToClient(%this,'setDlg',"Dialogue with OgreGeneral","Thank you. I'll tell my crew not to attack you unless provoked. Also, here's a little gift.","#string *TAKE GIFT* #command robinTakeGift");
		}
		else
		if(%this.slo.robin_q2_slainFive)
		{
			commandToClient(%this,'setDlg',"Dialogue with OgreGeneral","I need " @ ( 100 - %this.slo.inventory.itemcount["copperOres"]) @ " more Copper ores and " @ ( 100 - %this.slo.inventory.itemcount["TinOres"]) @ " more Tin Ores.","#string I see. #command");	
		}
		else
		{
			%this.slo.robin_q2_slainFive = true;
			commandToClient(%this,'setDlg',"Dialogue with OgreGeneral","You've slain 5 of my best. For us to stop messing with your well, bring me 100 Tin and Copper.","#string Sure, since I'm a nice guy. #command");
		}
	}
}

function serverCmdRobinLetsSlay(%this)
{
	if(%this.slo.robin_q2_foundclue1 !$= "1")
		return false;
	commandTocLient(%this,'closedlg');
	%this.slo.robin_q2_readyToSlay = true;
	messageClient(%this,'',"\c6[QUEST] I need to slay " @  5 - %this.slo.robin_q2_ogreCount @ " Ogres since he thinks I'm weak.");
}

function serverCmdRobinTakeGift(%this)
{
	if(%this.slo.robin_q2_completed $= "1")
		return false;
		
	if(%this.slo.robin_q2_takeGift $= "1")
	{
		commandToClient(%this,'closeDialogue');
		%m = setKeyWords("\c6" @ %this.name @ " has finished the quest " @ robin.quest2 @ "!",%this.name SPC robin.quest2,"\c6");
		messageAll('',%m);
		%this.slo.robin_q2_completed = true;
		lob_giveAvatar(%this,"Fighter");
		%this.addToInventory("Gold",10000);
		messageClient(%this,'',"\c6[QUEST] You've recieved \c210k \c6and an \c2Avatar\c6!");
	
	}
}