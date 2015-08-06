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
$roam["Dungeoneer"] = 0;

//item to be heald
$equip["Dungeoneer"] = "adamantiteshortswordimage";

//datablock
$LOB::NPC["Dungeoneer","Datablock"] = playerStandardArmor;

$lob::isShopShopKeeper["Dungeoneer"] = true;

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
	if(!%ai.hasShop)
		%ai.lob_newShop("dungeoneer");	
	%client = %player.client;
	%c = %player.client;
	%name = %c.name;
	%head = "Dialogue with Dungeoneer";
	%m = "I'm the Dungeoneer, what would you like to do?";
	//%um1 = "#string I want to enter the dungeon. #command DungeoneerEnterDungeon";
	//%um2 = "#string What can I get from the Dungeon? #command DungeoneerWhatCanIGet";
	
	//%m = "I'm the Dungeoneer, the dungeon will be finished up soon, so be ready for it!";
	
	//%um1 = "#string I can't wait! #command";
	%m = "<font:comic sans ms:21>Hello " @ %name @ ". You can do the following:\n\n<div:1>Enter Dungeon\n<div:1>Equip Weapons\n<div:1>View Dungeon Shop";
	%um1 = "#string Send me to the dungeon. #command dungeoneerGoToDungeon";
	%um2 = "#string Let me equip my weapons. #command DungeoneerOpenInventory";
	%um3 = "#string View the Dungeon Shop. #command dungeoneerviewDungeonShop";
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3);
}

if(!isObject(dungeonDeploreObject))
	new simset(dungeonDeploreObject);
function serverCmdDungeoneerViewDungeonShop(%this)
{
	//talk("shop = " @ %this.lookingat.shop);
	serverCmdPopulateShop(%this);
	commandtoclient(%this,'OpenShopWindow');
}

function serverCmdDungeoneerOpenInventory(%this)
{
	commandToClient(%this,'openInventory');
}

function serverCmdDungeoneerGoToDungeon(%this)
{
	%name = %this.name;
	%head = "Dialogue with Dungeoneer";
	%time = getSimTime();
	if((map_generator.startbrick $= "" || !isObject(map_Generator.startbrick)))
	{
		if(!isObject(map_generator.startBrick()))
		{
			if(%time - map_generator.attempt <= 5000)
			{
				%m = "" @ %name @ ", the dungeon is loading.";
				%um1 = "#string Let me equip my weapons then. #command DungeoneerOpenInventory";
				commandToClient(%this,'setdlg',%head,%m,%um1,%um2);			
				return false;
			}
			else
			{
				map_generator.attempt = %time;
				%m = "" @ %name @ ", the dungeon is being created, it will be ready in a few seconds.";
				%um1 = "#string Let me equip my weapons then. #command DungeoneerOpenInventory";
				commandToClient(%this,'setdlg',%head,%m,%um1,%um2);
			}
			map_generator.attempt = %time;
		}
	}
	if(%this.lookingat.name !$= "Dungeoneer")
		return false;
	if(%this.dungeon $= "1")
	{
		messageClient(%this,'',"\c6You are already in the dungeon. Type /leaveDungeon to leave the dungeon!");
		return false;
	}

	if(isObject(map_generator.startBrick))
	{
		addClientToDungeon(%this);
		commandToClient(%this,'CloseDialogue');
	}
	else
	{
		map_generator.generate("0 0 1000");
		survival_game_onEndBrickTouch();
		//schedule(5000,0,addclientToDungeon,%this);
	}
	
}

function addClientToDungeon(%this)
{
	%this.dungeon = true;
	if(isObject(%this.horse))
	{
		if(%this.horse.getMountedObject(0) == %this.player)
			%this.horse.setTransform(vectorAdd(map_generator.startbrick.position,"0 0 1.5"));
		else
			%this.player.setTransform(vectorAdd(map_generator.startbrick.position,"0 0 1.5"));
	}
	else
		%this.player.setTransform(vectorAdd(map_generator.startbrick.position,"0 0 1.5"));
	messageClient(%this,'',"\c6Type /leaveDungeon to leave the dungeon!");
	messageAll('',"\c6" @ %this.name @ " has entered the dungeon!");
}

function serverCmdLeaveDungeon(%this)
{
	if(%this.dungeon)
	{
		%this.dungeon = "";
		%this.instantrespawn();
	}
}

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