package Building
{
	function ServerCmdDropCameraAtPlayer(%client)
	{
		if(%client.buildMode && !%client.isAdmin)
		{
			%client.isAdmin = true;
			%set = true;
		}
		
		Parent::ServerCmdDropCameraAtPlayer(%client);
		
		if(%set)
		{
			%client.isAdmin = false;
		}
	}
	
	function fxDTSbrick::onplant(%brick)
	{
		Parent::onPlant(%brick);
		
		%name = %brick.getname();
		
		%fw = trim(strReplace(%brick.getname(),"_"," "));
		%fw = getWord(%fw);
		if(%fw $= "tree" || %fw $= "rock")
		{
			if(!%brick.isRendering())
			{
				%brick.setRendering(1);
				%brick.setColliding(1);
				%brick.setRaycasting(1);
			}
		}
	}

	function ServerCmdDropPlayerAtCamera(%client)
	{
		if(%client.buildMode && !%client.isAdmin)
		{	
			%client.isAdmin = true;
			%set = true;
		}
		
		Parent::ServerCmdDropPlayerAtCamera(%client);
		
		if(%set)
		{
			%client.isAdmin = false;
		}
	}
		
	function serverCmdFillCan(%this)
	{
		if(!%this.isAdmin && !%this.buildMode)
			return false;
			
		parent::serverCmdFillCan(%this);
	}
	
	function fillcanprojectile::onCollision(%a,%b,%c,%d,%e,%f,%g)
	{
		if(isObject(%c) && (%c.getclassname() $= "player" || %c.getClassname() $= "aiplayer"))
			return false;
			
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f,%g);
	}
	
	function fxDtsBrick::onPlant(%this)
	{
		parent::onPlant(%this);
		
		%this.schedule(50,okToPlant);
	}
	
	function fxDtsBrick::onPlayerTouch(%a,%b,%c)
	{
		%bn = %a.getName();
		if(%bn !$= "" && getSubStr(%bn,0,strLen(%bn)-1) $= "_wildbrick")
		{
			if(%b.getClassName() $= "aiPlayer" && !%b.isHorse)
				return parent::onPlayerTouch(%a,%b,%c);
				
			if(%b.isHorse)
				%client = %b.owner;
			else
				%client = %b.client;
			
			%time = getSimTime();
			
			if(%time - %client.lastWildTime <= 200)
			{
				return parent::onPlayerTouch(%a,%b,%c);
			}
			
			%client.lastWildTime = %time;
			
			if(!%client.slo.robin_hastools)
			{
				commandToClient(%client,'centerPrint',"\c6It looks dangerous out there, I should probably speak with Robin first.",3);
			}
			else
			{
				if(%client.Wild $= "0")
				{
					if(%b.isHorse)
					{
						%b.setTransform(vectorAdd(%b.position,$lob::wildbrick[%bn,"out"]));
						%b.wild = true;
						%client.wild = true;
					}
					else
					{
						
						%client.player.setTransform(vectorAdd(%client.player.position,$lob::wildbrick[%bn,"out"]));
						%client.wild = true;
					}
					
					%client.wildsong = $lob::wildBrick[%bn,"outSong"];
					commandToclient(%client,'centerprint',$lob::wildbrick[%bn,"outmsg"],3);
					commandToClient(%client,'playSong',$lob::wildbrick[%bn,"outsong"]);
					%tip = "<font:impact:30><color:FF0000>Be careful, you will lose a random item in your inventory when you die in the wild.";

					commandToClient(%client,'showTip',%tip,20000);
					
					%client.enterZone(%bn);
				}
				else
				{
					if(%b.isHorse)
					{
						%b.setTransform(vectorAdd(%b.position,$lob::wildbrick[%bn,"in"]));
						%b.wild = false;
						%client.wild = false;
					}
					else
					{
						%client.player.setTransform(vectorAdd(%client.player.position,$lob::wildbrick[%bn,"in"]));
						%client.wild = false;
					}
					
					%client.wildsong = $lob::wildBrick[%bn,"outSong"];
					commandToclient(%client,'centerprint',$lob::wildbrick[%bn,"inmsg"],3);
					commandToClient(%client,'playSong',$lob::wildbrick[%bn,"insong"]);
					%client.slo.area = $lob::wildBrick[%bn,"inName"];
					
					%client.enterZone(%bn);
				}
			}
			
		}
		return parent::onPlayerTouch(%a,%b,%c);
	}
	
	function fxDtsBrick::onDeath(%a,%b,%c,%d,%e)
	{
		if(%this.rockOwner !$= "")
		{
			%this.rockOwner.rock = "";
			%this.rockOwner = "";
		}
		
		parent::onDeath(%a,%b,%c,%d,%e);
	}
	
	function brickImage::onMount(%a,%b,%c,%d,%e)
	{
		if(%b.client.buildMode $= "1")
		{
			%p = parent::onMount(%a,%b,%c,%d,%e);
			return %p;
		}
		
		%b.unmountimage(0);
	}
};
activatePackage(Building);

$lob::wildbrick["_wildBrick0","out"] = "-1 -2 0";
$lob::wildbrick["_wildBrick0","in"] = "1 2 0";
$lob::wildbrick["_wildBrick0","inmsg"] = "<font:Gabriola:100>\c6Ordunia";
$lob::wildbrick["_wildbrick0","outmsg"] = "<font:Gabriola:100>\c6Ordunia's Wilderness (PVP)";
$lob::wildbrick["_wildBrick0","outsong"] = "lob_paperTurtles";
$lob::wildbrick["_wildBrick0","insong"] = "lob_inthemeadow";
$lob::wildbrick["_wildBrick0","inname"] = "Ordunia";
//for the zone manager
$lob::wildBrick["_wildBrick0","outName"] = "Ordunia Wild";

$lob::wildbrick["_wildBrick1","out"] = "1 3 0";
$lob::wildbrick["_wildBrick1","in"] = "-1 -3 0";
$lob::wildbrick["_wildBrick1","inmsg"] = "<font:Gabriola:100>\c6Ordunia";
$lob::wildbrick["_wildbrick1","outmsg"] = "<font:Gabriola:100>\c6Bandit Land (PVP)";
$lob::wildbrick["_wildBrick1","outsong"] = "lob_paperTurtles";
$lob::wildBrick["_wildBrick1","inSong"] = "lob_inthemeadow";
$loB::wildBrick["_wildBrick1","inName"] = "Ordunia";
//for the zone manager
$lob::wildBrick["_wildBrick1","outName"] = "Bandit Land Wild";

$lob::wildbrick["_wildBrick2","out"] = "-4 0 0";
$lob::wildbrick["_wildBrick2","in"] = "4 0 0";
$lob::wildbrick["_wildBrick2","inmsg"] = "<font:Gabriola:100>\c6Whitestone";
$lob::wildbrick["_wildbrick2","outmsg"] = "<font:Gabriola:100>\c6Ordunia's Wilderness (PVP)";
$lob::wildbrick["_wildBrick2","outsong"] = "lob_paperTurtles";
$lob::wildBrick["_wildBrick2","inSong"] = "lob_anotherrealm";
$lob::wildBrick["_wildBrick2","inName"] = "Whitestone";
//for the zone manager
$lob::wildBrick["_wildBrick2","outName"] = "Ordunia Wild";

$lob::wildbrick["_wildBrick3","out"] = "4 0 0";
$lob::wildbrick["_wildBrick3","in"] = "-4 0 0";
$lob::wildbrick["_wildBrick3","inmsg"] = "<font:Gabriola:100>\c6Whitestone";
$lob::wildbrick["_wildbrick3","outmsg"] = "<font:Gabriola:100>\c6Alyswell Forest (PVP)";
$lob::wildbrick["_wildBrick3","outsong"] = "lob_AuraOne";
$lob::wildbrick["_wildBrick3","insong"] = "lob_anotherRealm";
$lob::wildbrick["_wildBrick3","inname"] = "Whitestone";
//for the zone manager
$lob::wildBrick["_wildBrick3","outName"] = "alyswell forest Wild";

$lob::wildbrick["_wildBrick4","out"] = "-1 -2 0";
$lob::wildbrick["_wildBrick4","in"] = "1 2 0";
$lob::wildbrick["_wildBrick4","inmsg"] = "<font:Gabriola:100>\c6Whitestone";
$lob::wildbrick["_wildbrick4","outmsg"] = "<font:Gabriola:100>\c6Whitestone's Cavern (PVP)";
$lob::wildbrick["_wildBrick4","outsong"] = "lob_crinitas";
$lob::wildbrick["_wildBrick4","insong"] = "lob_anotherRealm";
$lob::wildbrick["_wildBrick4","inname"] = "Whitestone";
//for the zone manager
$lob::wildBrick["_wildBrick4","outName"] = "whitestone cavern Wild";

$lob::wildbrick["_wildBrick5","out"] = "-4 0 0";
$lob::wildbrick["_wildBrick5","in"] = "4 0 0 ";
$lob::wildbrick["_wildBrick5","inmsg"] = "<font:Gabriola:100>\c6Alyswell";
$lob::wildbrick["_wildbrick5","outmsg"] = "<font:Gabriola:100>\c6Alyswell Forest (PVP)";
$lob::wildbrick["_wildBrick5","outsong"] = "lob_AuraOne";
$lob::wildbrick["_wildBrick5","insong"] = "lob_Anouk";
$lob::wildbrick["_wildBrick5","inname"] = "Alyswell";
//for the zone manager
$lob::wildBrick["_wildBrick5","outName"] = "alyswell forest Wild";

$lob::wildbrick["_wildBrick6","out"] = "0 2 0";
$lob::wildbrick["_wildBrick6","in"] = "0 -2 0 ";
$lob::wildbrick["_wildBrick6","inmsg"] = "<font:Gabriola:100>\c6Interpass";
$lob::wildbrick["_wildbrick6","outmsg"] = "<font:Gabriola:100>\c6Ordunia's Wilderness (PVP)";
$lob::wildbrick["_wildBrick6","outsong"] = "lob_paperturtles";
$lob::wildbrick["_wildBrick6","insong"] = "lob_humblebug";
$lob::wildbrick["_wildBrick6","inname"] = "Interpass";
//for the zone manager
$lob::wildBrick["_wildBrick6","outName"] = "Ordunia Wild";

$lob::wildbrick["_wildBrick7","out"] = "0 -2 0";
$lob::wildbrick["_wildBrick7","in"] = "0 2 0 ";
$lob::wildbrick["_wildBrick7","inmsg"] = "<font:Gabriola:100>\c6Interpass";
$lob::wildbrick["_wildbrick7","outmsg"] = "<font:Gabriola:100>\c6Frostbite (PVP)";
$lob::wildbrick["_wildBrick7","outsong"] = "lob_alostwish";
$lob::wildbrick["_wildBrick7","insong"] = "lob_humblebug";
$lob::wildbrick["_wildBrick7","inname"] = "Interpass";
//for the zone manager
$lob::wildBrick["_wildBrick7","outName"] = "frostbite";

$lob::wildbrick["_wildBrick8","out"] = "0 2 0";
$lob::wildbrick["_wildBrick8","in"] = "0 -2 0 ";
$lob::wildbrick["_wildBrick8","inmsg"] = "<font:Gabriola:100>\c6Eldria";
$lob::wildbrick["_wildbrick8","outmsg"] = "<font:Gabriola:100>\c6Frostbite (PVP)";
$lob::wildbrick["_wildBrick8","outsong"] = "lob_alostwish";
$lob::wildbrick["_wildBrick8","insong"] = "lob_bundtling";
$lob::wildbrick["_wildBrick8","inname"] = "Eldria";
//for the zone manager
$lob::wildBrick["_wildBrick8","outName"] = "frostbite";

function gameConnection::enterZone(%client,%brickName)
{
	if(%client.wild $= "")
		%client.wild = 0;
		
	%enter = $lob::wildBrick[%brickname,"inName"];
	%exit = $lob::wildBrick[%brickname,"outName"];
	
	if(%client.slo.zoneArea !$= "" && %client.slo.zoneArea !$= %enter)
		%exit = %client.slo.zoneArea;
	
	if($lob::zonePlayerCount[%enter] $= "" || $lob::zonePlayerCount[%enter] < 0)
		$lob::zonePlayerCount[%enter] = 0;
	if($lob::zonePlayerCount[%exit] $= "" || $lob::zonePlayerCount[%exit] < 0)
		$lob::zonePlayerCount[%exit] = 0;	
	
	//exit wild going into a town
	if(%client.wild $= "0")
	{
		//enter = town name
		//talk("1entering " @ %enter);
		$lob::zonePlayerCount[%enter]++;
		$lob::zonePlayerCount[%exit]--;
		if(%client.name $= "elm")
		{
		echo("INTO TOWN: " @ $lob::zonePlayerCount[%enter] @ " players in " @ %enter);
		echo("INTO TOWN: " @ $lob::zonePlayerCount[%exit] @ " players in " @ %exit);
		}

		spawnNpcsByArea(%enter);
		spawnEnemiesByArea(%enter);
		
		if($lob::zonePlayerCount[%exit] <= 0)
		{
			clearNpcsByArea(%exit);
			clearEnemiesByArea(%exit);
		}
		
		%client.slo.zoneArea = %enter;
	}
	//exiting a town going into wild
	else
	{
		//exit = wild name
		//talk("entering " @ %exit);
		$lob::zonePlayerCount[%exit]++;
		$lob::zonePlayerCount[%enter]--;
		if(%client.name $= "elm")
		{
		echo("INTO WILD: " @ $lob::zonePlayerCount[%enter] @ " players in " @ %enter);
		echo("INTO WILD: " @ $lob::zonePlayerCount[%exit] @ " players in " @ %exit);
		}
		spawnNpcsByArea(%exit);
		spawnEnemiesByArea(%exit);
		
		if($lob::zonePlayerCount[%enter] <= 0)
		{
			clearNpcsByArea(%enter);
			clearEnemiesByArea(%enter);
		}
		
		%client.slo.zoneArea = %exit;
	}
	
	if($lob::zonePlayerCount[%enter] $= "" || $lob::zonePlayerCount[%enter] < 0)
		$lob::zonePlayerCount[%enter] = 0;
	if($lob::zonePlayerCount[%exit] $= "" || $lob::zonePlayerCount[%exit] < 0)
		$lob::zonePlayerCount[%exit] = 0;	
}

function serverCmdSetBrickName(%client,%a,%b,%c,%d,%e,%f,%g)
{
	if(%client.buildMode $= "1" || %client.isAdmin)
	{
		%text = trim(%a SPC %b SPC %c SPC %d SPC %e SPC %f SPC %g);
		%text = strReplace(%text," ","_");
		if(%client.lookingAt.getClassname() $= "fxDtsBrick")
		{
			%client.lookingAt.setName("");
			%client.lookingAt.setName("_" @ %text);
			messageClient(%client,'',"\c6Set " @ %client.lookingAt @ "\'s brick name to " @ %text);
		}
	}
}

function spawnNpcsByArea(%areainput)
{
	%count = getWordCount($lob::npcSpawn["all"]);
	for(%i=0;%i<%count;%i++)
	{
		%spawn = getWord($lob::npcSpawn["All"],%i);
		if(isObject(%spawn))
		{
			%spawnName = strLwr(%spawn.getname());
			%area = strReplace(%spawnName,"npcspawn","");
			%area = getSubStr(%area,1,strLen(%area));
			%area = strReplace(%area,"_"," ");
			%area = getWords(%area,0,getWordCount(%area)-2);
			%area = trim(%area);
		}
		
		if(%area $= %areainput)
		{
			spawnAiPlayerOnBrick(%spawn);
		}
	}
}

function spawnEnemiesByArea(%areaInput)
{
	%count = getWordCount($lob::EnemySpawn["all"]);
	for(%i=0;%i<%count;%i++)
	{
		%spawn = getWord($lob::EnemySpawn["All"],%i);
		
		if(isObject(%spawn))
		{
			%spawnName = strLwr(%spawn.getname());
			%area = strReplace(%spawnName,"enemyspawn","");
			%area = getSubStr(%area,1,strLen(%area));
			%area = strReplace(%area,"_"," ");
			%area = getWords(%area,0,getWordCount(%area)-2);
			%area = trim(%area);
		}

		if(%area $= %areainput)
		{
			spawnAiPlayerOnBrick(%spawn);
		}
	}
}

function clearNpcsByArea(%areainput)
{
	%count = getWordCount($lob::npcSpawn["all"]);
	for(%i=0;%i<%count;%i++)
	{
		%spawn = getWord($lob::npcSpawn["All"],%i);
		if(isObject(%spawn))
		{
			%spawnName = strLwr(%spawn.getname());
			%area = strReplace(%spawnName,"npcspawn","");
			%area = getSubStr(%area,1,strLen(%area));
			%area = strReplace(%area,"_"," ");
			%area = getWords(%area,0,getWordCount(%area)-2);
			%area = trim(%area);
		}
		
		if(%area $= %areainput)
		{
			if(isObject(%spawn.npc))
				%spawn.npc.delete();
			%spawn.npc = "";
		}
	}
}

function clearEnemiesByArea(%areainput)
{
	%count = getWordCount($lob::EnemySpawn["all"]);
	for(%i=0;%i<%count;%i++)
	{
		%spawn = getWord($lob::EnemySpawn["All"],%i);
		if(isObject(%spawn))
		{
			%spawnName = strLwr(%spawn.getname());
			%area = strReplace(%spawnName,"enemyspawn","");
			%area = getSubStr(%area,1,strLen(%area));
			%area = strReplace(%area,"_"," ");
			%area = getWords(%area,0,getWordCount(%area)-2);
			%area = trim(%area);
		}
		
		if(%area $= %areainput)
		{
			if(isObject(%spawn.npc))
				%spawn.npc.delete();
			%spawn.npc = "";
		}
	}
}

function spawnAiPlayerOnBrick(%brick)
{	
	if(isEventPending(%brick.respawnevent))
		return false;
	
	if(isObject(%brick))
	{
		%bn = %brick.getname();
		%bn = trim(strReplace(%bn,"_"," "));
	}
	if(strStr(strLwr(%bn),"npcspawn") >= 0)
	{
		%area = getWord(%bn,0);
		
		%name = getWord(%bn,getWordCount(%bn) - 1);
		%name = strUpr(getSubStr(%name,0,1)) @ getSubStr(%name,1,strLen(%name));
		
		if(%brick.NPC $= "")
		{
			%datablock = $LOB::NPC[%name,"Datablock"];
			
			%o = new aiPlayer()
			{
				onClickAction = $LOB::OnClickAction[%area,%name];
				datablock = playerStandardArmor;
				position = vectorAdd(%brick.position,"0 0 0.2");
				name = %name;
			};
			
			%o.brick = %brick;
			%brick.NPC = %o;
			
			if($nodeColor[%name,"chest"] $= "")
				dressplayer(%o,"citizen");
			else
				dressplayer(%o,%name);
			
			//echo("EQUIP NAME = " @ $equip[%name]);
			
			if($equip[%name] !$= "")
			{
				equipPlayer(%o,$equip[%name]);
			}
			
			if($task[%name] $= "")
			{
				%o.schedule(200,roam);
			}
			else
				eval("%o.schedule(200,"@$task[%name]@");");
				
			addToList("$LOB::Npcspawn[\"All\"]",%brick);
			
			if($lob::isShopNpc[%name])
				%o.lob_newShop();
			
			if(isFunction(%o.name,"onObjectSpawned"))
			{
				eval("" @ %o.name @ ".onObjectSpawned(" @ %o @ ");");
			}
				
			//echo("Ai Player registered to brick complete.");
		}
	}
	else
	if(strStr(strLwr(%bn),"enemyspawn") >= 0)
	{
		%area = getWord(%bn,0);
		%name = getWord(%bn,getWordCount(%bn) - 1);
		
		if(%brick.NPC $= "" || %brick.npc $= "0")
		{
			%datablock = $LOB::Enemy[%name,"Datablock"];
			%aggressive = $LOB::Enemy[%name,"Aggressive"];
			%health = $LOB::Enemy[%name,"Health"];
			%o = new aiPlayer()
			{
				onClickAction = $LOB::OnClickAction[%area,%name];
				datablock = %datablock;
				position = vectorAdd(%brick.position,"0 0 0.2");
				name = %name;
				Level = getRandom(getWord($LOB::Enemy[%name,"Level"],0),getWord($LOB::Enemy[%name,"Level"],1));
				Aggressive = %aggressive;
				tempHealth = %health;
			};
			
			%o.brick = %brick;
			%brick.NPC = %o;
			
			newCombatDataFromLevel(%o,%o.level);
			
			if($nodeColor[%name,"chest"] $= "" && $roam[%name] $= "")
				dressplayer(%o,"citizen");
			else
			if($nodeColor[%name,"chest"] !$= "")
				dressplayer(%o,%name);
				
			if($equip[%name] !$= "")
			{
				equipPlayer(%o,$equip[%name]);
			}
			
			if($task[%name] $= "")
			{
				%o.schedule(200,roam);
			}
			else
				eval("%o.schedule(200,"@$task[%name]@");");
			
		
			addToList("$LOB::EnemySpawn[\"all\"]",%brick);
			%newArea = getWords(%bn,1,getWordCount(%bn) - 2);
			addToList("$LOB::enemySpawn[\"" @ %newarea @ "\"]",%brick);
			
			if(isFunction(%o.name,"onObjectSpawned"))
			{
				eval("" @ %o.name @ ".onObjectSpawned(" @ %o @ ");");
			}
			
			//echo("Ai Player registered to brick complete.");
		}
	}
}

function listZoneInfo(%this)
{
	if(%this $= "")
		return false;
		
	%info = $lob::zonePlayerCount[%this.slo.zoneArea];
	
	echo(%info);
	
	return %info;
}

function serverCmdZoneCounts(%this)
{
	if(%this.isAdmin)
	{
		%ac=-1;
		%a[%ac++] = "Ordunia";
		%a[%ac++] = "Ordunia Wild";
		%a[%ac++] = "Bandit Land Wild";
		%a[%ac++] = "alyswell forest wild";
		%a[%ac++] = "whitestone cavern wild";
		%a[%ac++] = "Interpass";
		%a[%ac++] = "Frostbite";
		%a[%ac++] = "Eldria";
		
		for(%i=0;%i<%ac;%i++)
			%list = %list @ "" @ %a[%i] @ " = " @ $lob::zonePlayerCount[%a[%i]] @ "\n";
			//messageClient(%this,'',%a[%i] @ " = " @ $lob::zonePlayerCount[%a[%i]]);
			
		//%list = trim(%list);
			
		commandToClient(%this,'messageBoxOk',"List",%list);
	}
}

function fxDtsBrick::okToPlant(%this)
{
	if(%this.client.bl_id $= "1337" || %this.client $= "-1" || %this.client.isAdmin || %this.isFire || %this.client.buildmode $= "1" || isObject(%this.map_generator))
	{
		return true;
	}
	else
	{
		%this.killBrick();
		if(isObject(%this.client.player))
		{
			echo(%this.name @ " has a real temp brick");
			%this.client.player.tempbrick.delete();
		}
		return false;
	}
}