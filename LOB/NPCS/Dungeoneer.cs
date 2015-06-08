$nodeColor["Dungeoneer","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["Dungeoneer","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Dungeoneer","larm"] = "0.200 0.000 0.800 1.000 1";
$nodeColor["Dungeoneer","lhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Dungeoneer","lshoe"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["Dungeoneer","pack"] = "0.750 0.750 0.750 1.000 1";
$nodeColor["Dungeoneer","rarm"] = "0.200 0.000 0.800 1.000 1";
$nodeColor["Dungeoneer","rhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Dungeoneer","rshoe"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["Dungeoneer","chest"] = "0.0784314 0.0784314 0.0784314 1 1";
$decal["Dungeoneer"] = "LinkTunic";
$smiley["Dungeoneer"] = "smileyRedBeard";
$pack["Dungeoneer"] = "cape";
$chest["Dungeoneer"] = "armor";
$hat["Dungeoneer"] = "pointyhelmet";
$roam["Dungeoneer"] = 0;
$lob::vision["Dungeoneer"] = 10;

//--

//function the npc will use
$task["Dungeoneer"] = "roam";

//for some task, there is an inner task
//$taskInner["Dungeoneer"] = "iron";

//if we click the npc, are they ready for dialogue?
$OnClickActionSet["Dungeoneer"] = "1";

//roam range
$roam["Dungeoneer"] = 5;

//item to be heald
$equip["Dungeoneer"] = "adamantiteshortswordimage";

//datablock
$LOB::NPC["Dungeoneer","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the NPC's head
//--
$lob::roamMsgCount["Dungeoneer"] = -1;
$lob::roamMsg["Dungeoneer",$lob::roamMsgCount["Dungeoneer"]++] = "Let us kill some enemies in the dungeons!";
$lob::roamMsg["Dungeoneer",$lob::roamMsgCount["Dungeoneer"]++] = "What are you chicken?! Let's head to the Dungeon";
$lob::roamMsgCount++;
//--

while(isObject(Dungeoneer))Dungeoneer.delete();

//SO
new scriptObject(Dungeoneer);

function Dungeoneer::onObjectSpawned(%this,%npc)
{
	//callback for when they first spawn
	
	//nothing
}

function Dungeoneer::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%c = %player.client;
	%name = %c.name;
	%head = "Dialogue with Dungeoneer";
	%m = "I'm the Dungeoneer, what would you like to do?";
	//%um1 = "#string I want to enter the dungeon. #command DungeoneerEnterDungeon";
	//%um2 = "#string What can I get from the Dungeon? #command DungeoneerWhatCanIGet";
	
	%m = "I'm the Dungeoneer, the dungeon will be finished up soon, so be ready for it!";
	
	%um1 = "#string I can't wait! #command";
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
}

if(!isObject(dungeonDeploreObject))
	new simset(dungeonDeploreObject);

function serverCmdDungeoneerEnterDungeon(%this)
{
	//size = 1 need to work on dynamics. for now we'll only use dungeon size 1
	%size = 1;
	if($lob::dungeonActive[%size] $= "1")
	{
		messageClient(%this,'',"\c5There is already a group in the dungeon, please wait.");
		return false;
	}
	
	if(%this.waitForGroup $= "")
	{
		dungeonDeploreObject.add(%this);
		%this.waitForGroup = 1;
		messageAll('',"\c5" @ %this.name @ " is waiting to enter the Dungeon.");
		
		if(dungeonDeploreObject.getCount() == 1)
		{
			messageAll('',"\c5Dungeon Group deploring in 10 seconds.");
			schedule(10000,0,lob_deploreDungeonGroup);
		}
	}
	else	
		messageClient(%this,'',"\c5Please be patient.");
}

function lob_deploreDungeonGroup()
{
	if(dungeonDeploreObject.getCount() < 1)
	{
		messageAll('',"\c5Dungeon deploration canceled due to lack of atleast 2 players.");
		return false;
	}
	
	messageAll('',"\c5The Dungeon Party has been deployed.");
	
	%c = clientGroup.getCount();
	%group = lob_newGroup(1);
	
	%x = "_dungeon_size_1_x";
	%xpos = %x.position;
	%xposx = getWord(%xpos,0);
	%xposy = getWord(%xpos,1);
	%xposz = getWord(%xpos,2);
	
	%y = "_dungeon_size_1_y";
	%ypos = %y.position;
	%yposx = getWord(%ypos,0);
	%yposy = getWord(%ypos,1);
	%yposz = getWord(%ypos,2);
	
	if(%xposx > %yposx)
	{
		%lowx = %yposx;
		%highx = %xposx;
	}
	else
	{
		%lowx = %xposx;
		%highx = %yposx;
	}
	if(%xposy > %yposy)
	{
		%lowy = %yposy;
		%highy = %xposy;
	}
	else
	{
		%lowy = %xposy;
		%highy = %yposy;
	}
	
	for(%i=0;%i<%c;%i++)
	{
		%cl = clientGroup.getObject(%i);
		
		if(%cl.waitforGroup $= "1")
		{
			%pos = getRandom(%lowx,%highx) SPC getRandom(%lowy,%highy) SPC "4.2";
			%cl.player.setTransform(%pos);
			%cl.waitForGroup = "";
			%group.script.addPlayer(%cl);
			dungeonDeploreObject.remove(%cl);
		}
	}
	
	$lob::dungeonActive[%group.size] = true;
	%group.script.initialize();
}