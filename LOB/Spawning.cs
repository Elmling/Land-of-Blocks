package Spawning
{
	function serverCmdSetWrenchData(%client,%data)
	{
		parent::serverCmdSetWrenchData(%client,%data);
		
		%bn = strReplace(getField(%data,0),"N ","");
		%bn = trim(strReplace(%bn,"_"," "));
	
		%brick = %client.wrenchBrick;
		
		if(getWord(%bn,1) $= "spawn")
		{
			%area = getWord(%bn,0);
			%correctName = strReplace(%area,"SPACE"," ");
			$lob::correctName[%area] = %correctName;

			if(strStr($LOB::spawn[%area],%brick) >= 0)
				return false;
				
			addToList("$LOB::spawn["@%area@"]",%brick);
		}

		if(getWord(%bn,0) $= "Item")
		{
			%brick.item.isLOBItem = true;
		}
	}
	
	function gameConnection::spawnPlayer(%this)
	{
		
		if(%this.lob_guiOutdated)
		{
			commandtoclient(%this,'messageboxok',"Outdated","You need to download the GUI. You won't be able to do some things until you get it.\n\n" @ linkParser("landofblocks.weebly.com"));
		//	parent::spawnPlayer(%this);
		//	%this.controlToCamera("Point","16 2 4");
		//	%this.player.schedule(50,delete);
		//	return true;
		}
	
		parent::spawnPlayer(%this);
		
		if(%this.wild $= "")
			%this.wild = 0;
		else
		if(%this.wild $= "1")
			%this.wild = false;
		
		if(!isEventPending(%this.tipLoop))
		{
			%this.beginTips();
			%this.initClientGUI();
		}
			
		if(%this.status $= "loading")
		{
			%this.setstatus("idle",5000);
			commandToClient(%this,'playsong',$lob::song[%this.slo.area]);
		}	
		
		if(%this.slo.pkPoints >= 3 && %this.scrollUsed $= "")
		{
			%this.slo.area = "Jail";
		}
		else
		if(%this.slo.area $= "jail")
			%this.slo.area = "Ordunia";
			
		if(%this.scrollUsed $= "1")
			%this.scrollUsed = "";
			
		%area = capFirstLetter(%this.slo.Area);
			
		if($LOB::correctName[%area] !$= "")
			%correctArea = $Lob::correctName[%area];
		else
			%correctArea = %area;
		
		%this.bp();
		%b = getWord($LOB::spawn[%area],getRandom(0,getWordCount($LOB::spawn[%area])-1));
		if(isObject(%b))
			%this.player.setTransform(vectorAdd(%b.getTransform(),"0 0 0.2"));
		else
			echo("Uh oh, no spawn for "@%correctarea@".");
		
		%m = capFirstLetter(setKeyWords("\c6" @ %correctArea,%correctArea,"\c6"));
		%font = "<font:Gabriola:100>";
		commandToClient(%this,'centerprint',%font @ capFirstLetter(%m),3);	

		if((!%this.buildMode))
			%this.player.changeDataBlock(playerNoJet);

		if(%this.customAvatar !$= "")
		{
			dressPlayer(%this.player,%this.slo.donation.avatar[%this.customAvatar]);
		}
		else
			dressplayer(%this.player,"Default");
		//equiparmorstuff();
		//if(!%this.isAdmin)
		if(!%this.buildMode)
		{
			commandToClient(%this,'Lob_killBuildingHud');
			%this.clearTools();
		}
		else
			commandToCLient(%this,'lob_revivebuildinghud');
		
		for(%i=0;%i<%this.player.dataBlock.maxTools;%i++)
		{
			%dt = %this.deathTool[%i];
			if(isObject(%dt))
			{
				%this.addToDefaultInventory(%dt);
				%this.deathTool[%i] = "";
			}
		}
		
		serverCmdNewSong(%this);
		
		if(%this.oldPos !$= "")
		{
			%this.player.setTransform(%this.oldPos);
			%this.oldPos = "";
		}
		
		if(%this.randomEvent $= "1")
		{
			%b = "_antibot_spawn";
			%bp = %b.position;
			%this.player.setTransform(%b.position);
		}
		
		if(isObject(%this.slo.helmet) && %this.slo.itemCount[%this.slo.helmet.getname()] > 0)
		{
			%this.mountImage(%this.slo.helmet.getname(),0);
		}
		
		$lob::zonePlayerCount[%this.slo.zoneArea]--;
		$lob::zonePlayerCount[%this.slo.area]++;
		%this.slo.ZoneArea = %this.slo.area;
		
		if(%this.slo.customScale !$= "")
			%this.player.setScale(%this.slo.scalex SPC %this.slo.scaley SPC %this.slo.scaleZ);
		
		if(%this.dungeon)
		{
			%this.player.setTransform(vectorAdd(map_Generator.startBrick.position,"0" SPC "0" SPC "1.5"));
		}
		
		//somehowloadtools();
	}
	
	function fxDtsBrick::killBrick(%this)
	{
		%name = %this.getname();
		parent::killBrick(%this);
		
		%cn = trim(strReplace(%name,"_"," "));
		if(getWord(%cn,0) $= "zone")
			schedule(100,0,deleteStackName,%name);
	}
	
	function fxDtsBrickData::onTeleDoorEnter(%this,%a,%b)
	{
		parent::onTeleDoorEnter(%this,%a,%b);
		return;
		
		%brick = %a;
		%player = %b;
		%client = %player.client;
		
		%base = %a;

		%downcount = %brick.getNumDownBricks();
		%base = %brick.getDownBrick(%downCount-1).getname();
		%base.users--;
		if(%base.users <= 0)
		{

			//echo("killing");

			$killtime = getsimtime();
			$deletePend = %base.schedule(5000,killbrick); // <= change this to like a minute
		}
	}
	
	function fxDtsBrickData::onTeleDoorExit(%this,%a,%b)
	{
		parent::onTeleDoorExit(%this,%a,%b);
		return;
		
		%brick = %a;
		%player = %b;
		%client = %player.client;
		
		%downcount = %brick.getNumDownBricks();
		%base = %brick.getDownBrick(%downCount-1).getname();
		echo("base = "@%base);
		%base.users++;
		
	}
	
	function brickTeleDoorData::onPlayerTouch(%this,%a,%b)
	{
		parent::onPlayerTouch(%this,%a,%b);
		return;
		%name = %a.getname();
		%name = strReplace(%name,"_"," ");
		%name = trim(%name);
		
		%downcount = %a.getNumDownBricks();
		%base = %a.getDownBrick(%downCount-1).getname();
		%base = strReplace(%base,"_"," ");
		%base = trim(%base);
		%base = getWord(%base,1);

		%nn = getWords(%name,1,2);
		%nn = strReplace(%nn,%base,"");
		%nn = trim(%nn);
		
		%destination = %nn;
		
		//echo(isObject("_Zone_"@%destination) SPC isfile("saves/"@%destination@".bls") SPC "isObject(_Zone_"@%destination@");" SPC "saves/"@%destination@".bls");
		
		if(isEventPending($deletePend))
			cancel($deletePend);
			
		if(!isObject("_Zone_"@%destination) && isfile("saves/"@%destination@".bls"))
		{
			if($loadname !$= "")
			{
				commandtoClient(%client,'centerprint',"\c3Sorry, there is another zone being populated at the moment, please wait until that zone is finished, thanks.");
			}
			//load it let clients camera view it loading
			//hide player
			//serverDirectSaveFileLoad(path,3,map,0);
			echo("loading");
			%a.oldname = %a.getname();
			%a.setname("");
			%a.setNTObjectName("");
			echo("brick1 = "@%a);
			%loadname = %destination;
			$oldbrick = %a;
			$client = %b.client;
			$currrot = getWords(%b.getTransform(),3,6);
			echo("rot = "@$currrot @" | "@%b.gettransform());
			schedule(100,0,serverDirectSaveFileLoad,"saves/"@%loadname@".bls",3,"slate",0);
			%b.client.schedule(500,controlToCamera,"Point",vectorAdd(%b.position,"0 0 20"));
			%b.client.waitForBrickToView("_zone_"@%loadname);
		}
		
		parent::onPlayerTouch(%this,%a,%b);
	}
	
	function handleProcessComplete(%this,%data)
	{
		parent::handleProcessComplete(%this,%data);
		return;
		if(getWord(%this,1) $= "msgprocesscomplete")
		{
			$loadname = $oldbrick.oldname;
			%ln = $loadname;
			
			if(isObject(%ln))
			{
				echo("Brick2 = "@%ln.getid());
				%client = $client;
				%client.schedule(1000,endOrbit);
				
				$client = "";
				$loadname = "";
				
				$oldbrick.setname($oldbrick.oldname);
				//$oldbrick.setNTOBjectname($oldBrick.name);
				$oldBrick = "";
				
			}
		}
	}
	
	function FxDtsBrick::onRemove(%this)
	{
		%bn = %this.getName();
		%bn = trim(strReplace(%bn,"_"," "));
		
		if(getWord(%bn,1) $= "spawn")
		{
			%area = getWord(%bn,0);
			
			removeFromList("$LOB::spawn["@%area@"]",%this);
			
			//echo("Brick spawn group cleanup successful.");
		}
		
		parent::onRemove(%this);
	}
};
activatePackage(Spawning);

function deleteStackName(%name)
{
	cancel($dsn);
	
	if(isObject(%name))
	{
		%name.setname("");
		%name.setNTObjectName("");
		%name.killBrick();
	}
	else
	{
		return 0;
	}	
	$dsn = schedule(100,0,deleteStackName,%name);
}

function gameConnection::waitForBrickToView(%this,%brick)
{
	cancel(%this.wfbtv);
	%b = %brick.getID();
	
	if(isObject(%b))
	{
		echo("YES WE VIEWED NEW LOADING BRICK => "@%brick);
		%this.schedule(500,controlToCamera,"Point",vectorAdd(%b.position,"0 0 20"));
		return true;
	}
	
	%this.wfbtv = %this.schedule(100,waitForBrickToView,%brick);
}

function gameConnection::bp(%this)
{
	cancel(%this.bp);
	
	if(!isObject(%this))
		return 0;
	
	if(isObject(%this.player))
	{
		%time = getSimTime();
		if(%this.forestMinigame)
		{	
				//enable support for forest minigame gui here!!
				%m = "<color:40FF00>|||||||||||||||||";
				commandToClient(%this,'bottomPrint',%m,1);	
				%this.bp = %this.schedule(900,bp);
				return false;
				//------------------------------------
		}
		else
		if(%time - %this.lastVisionTime > 100)
		{
			commandToClient(%this,'catchHealthBarData',%this.slo.temphealth,%this.slo.health);
			//hook
			for(%i=0;%i<5;%i++)
			{
				%item = %this.player.tool[%i];
				if(%item.uiName $= "")
					continue;
					
				%name = convertToItemName(%item.uiName);
				
				if(mfloor(%this.slo.inventory.itemCount[%name]) <= 0)
				{
					if(!%this.buildmode)
					{
						%this.player.tool[%i] = 0;
						messageClient(%this, 'MsgItemPickup', '' , %i, 0);
					}
				}
				
				%tool = %this.player.getMountedImage(0);
				if(isObject(%tool))
				{
					if(!%this.buildMode)
					{
						%in = convertToItemName(%tool.item.uiName);
						//echo("in = "@ %in @" amount = " @ %
						if(mFloor(%this.slo.inventory.itemCount[%in]) <= 0)
							serverCmdUnuseTool(%this);

					}
				}
				
				commandToCLient(%this,'updateInventoryIcon',"weapon",%tool.item.iconName);
			}
			%EyeVector = %this.player.getEyeVector();
			%EyePoint = %this.player.getEyePoint();
			%Range = 100;
			%RangeScale = VectorScale(%EyeVector, %Range);
			%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
			%raycast = containerRayCast(%eyePoint,%rangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType , %this.player);
			%o = getWord(%raycast,0);
			if(isObject(%o))
			{
				%font = "<font:impact:20><just:left>";
				
				%this.lookingAt = %o;
				
				if(%o.getClassName() $= "player")
				{
					//%message = fairy.getVisionMessage("player",%o);
					//commandToClient(%this,'receiveVision',%message,3);
				}
				else
				if(%o.getClassName() $= "aiPlayer")
				{
					%level = %o.level;
					if(%level $= "")
						%level = "Unknown";
						
					if(isObject(%o) && isObject(%o.brick) && strStr(strLwr(%o.brick.getName()),"enemy") >= 0)
					{
						//%message = fairy.getVisionMessage("aienemy",%o);
						//commandToClient(%this,'receiveVision',%message,3);
					}
					else
					if(isObject(%o) && isObject(%o.brick) && strStr(strLwr(%o.brick.getName()),"npcspawn") >= 0)
					{
						//%message = fairy.getVisionMessage("ainpc",%o);
						//commandToClient(%this,'receiveVision',%message,3);
					}
					else
					{
						//%message = fairy.getVisionMessage("other",%o);
						//commandToClient(%this,'receiveVision',%message,3);
					}
					
				}
				else
				if(%o.getClassName() $= "fxDtsBrick")
				{
					if(strStr(strLwr(%o.getName()),"wildbrick") >= 0)
					{
						//if(
					}
					else
					if(strStr(strLwr(%o.getName()),"tree") >= 0)
					{
						%name = strReplace(strLwr(%o.getName()),"_tree_","");
						%o.name = %name;
						//%message = fairy.getVisionMessage("tree",%o);
						//commandToClient(%this,'receiveVision',%message,2);
					}
					else
					if(strStr(strLwr(%o.getName()),"rock") >= 0)
					{
						%name = strReplace(strLwr(%o.getName()),"_rock_","");
						%o.name = %name;	
						//%message = fairy.getVisionMessage("rock",%o);
						//commandToClient(%this,'receiveVision',%message,2);
					}
					else
					if(%o.getName() $= "_anvil")
					{
						%name = "Anvil";
						%o.name = %name;
						//%message = fairy.getVisionMessage("anvil",%o);
						//commandToClient(%this,'receiveVision',%message,2);
					}
					else
					if(%o.getName() $= "_furnace")
					{
						%name = "Anvil";
						%o.name = %name;
						//%message = fairy.getVisionMessage("furnace",%o);
						//commandToClient(%this,'receiveVision',%message,2);
					}
				}
			}
			%this.lastVisionTime = %time;
		}
		
		if(%this.status $= "PVP" && (%this.wild $= "0" || %this.wild $= ""))
			%this.status = "";
			
		if(%this.status $= "loading")
		{
			
		}
		else
		if(%this.status !$= "" && %this.status !$= "Idle" && %this.status !$= "Exploring")
		{
			%status = %this.status;
			%this.lastStatus = %status;
		}
		else
		if(vectorLen(%this.player.getVelocity()) >= 2)
		{
			%status = "Exploring";
		}
		else
			%status = "Idle";
			
		%this.status = %status;
		
		if($lob::correctName[%this.slo.area] $= "")
			%area = %this.slo.area;
		else
			%area = $lob::correctName[%this.slo.area];
			
		%m = setKeyWords("\c6 Status: " @ %status @ " | Town: " @ stripMlControlChars(%area) @ " | Combat Lvl: " @ %this.slo.combatLevel @ " | Health: " @ %this.slo.tempHealth @ "/" @ %this.slo.health, "Status Town Combat Lvl Health","\c6");
		
		if(%this.status $= "Mining")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Mining Lvl: " @ %this.slo.miningLevel @ " | Mining Exp: " @ %this.slo.miningExp @ "/" @ $lob::expNeeded["Mining",%this.slo.miningLevel],"Mining Lvl Mining Exp","\c6");
		}
		else
		if(%this.status $= "Wood Cutting")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.WoodCuttingLevel @ " | Exp: " @ %this.slo.woodCuttingExp @ "/" @ $lob::expNeeded["WoodCutting",%this.slo.woodCuttingLevel],"Status Lvl Exp","\c6");
		}
		else
		if(%this.status $= "Combat")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.combatLevel @ " | Exp: " @ %this.slo.combatExp @ "/" @ $lob::expNeeded["Combat",%this.slo.combatLevel] @ " | Health: \c0" @ %this.slo.tempHealth @ "\c6/" @ %this.slo.health@"","Status Lvl Exp Health","\c6");
		}
		else
		if(%this.status $= "PVP")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.combatLevel @ " | Exp: " @ %this.slo.combatExp @ "/" @ $lob::expNeeded["Combat",%this.slo.combatLevel] @ " | Health: " @ %this.slo.tempHealth @ "\c6/" @ %this.slo.health@"","Status Lvl Exp Health","\c6");
		}
		else
		if(%this.status $= "firemaking")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.fireMakingLevel @ " | Exp: " @ %this.slo.fireMakingExp @ "/" @ $lob::expNeeded["firemaking",%this.slo.firemakingLevel] @ "","Status Lvl Exp","\c6");
		}
		else
		if(%this.status $= "Smelting")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.smeltingLevel @ " | Exp: " @ %this.slo.smeltingExp @ "/" @ $lob::expNeeded["Smelting",%this.slo.smeltingLevel] @ "","Status Lvl Exp","\c6");
		}
		else
		if(%this.status $= "cooking")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.cookingLevel @ " | Exp: " @ %this.slo.cookingExp @ "/" @ $lob::expNeeded["cooking",%this.slo.cookingLevel] @ "","Status Lvl Exp","\c6");
		}
		else
		if(%this.status $= "Climbing")
		{
			%m = setKeyWords("\c6Status: " @ %this.status @ " | Lvl: " @ %this.slo.climbingLevel @ " | Exp: " @ %this.slo.climbingExp @ "/" @ $lob::expNeeded["climbing",%this.slo.climbingLevel] @ "","Status Lvl Exp","\c6");
		}
	}
	else
	{
		if(%this.hasSpawnedOnce)
			%status = "Dead";
		else
			%status = "Loading";
		%m = setKeyWords("\c6Status: " @ %status @ " Lvl: " @ %this.slo.combatLevel @ "", "Status Town Lvl","\c6");
	}
	
	if(!%this.wild && %this.slo.tempHealth <= %this.slo.health && %this.slo.tempHealth > 0)
	{
		if(!%this.rejuvenateHealth && %this.status !$= "Combat")
		{
			%this.rejuvenateHealth = true;
			%this.rejuvenateHealthLoopStart = %this.schedule(15000,rejuvenateHealth);
		}
	}
	
	if(%this.slo.pkPoints !$= "" && %this.slo.pkPoints >= $lob::wantedLevel)
		%m = %m @ " | \c0WANTED";	

	if(%this.player.tempBrick !$= "" && %this.buildMode !$= "1")
	{
		%this.player.tempBrick.delete();
				%this.player.tempBrick = "";
	}
	
	commandToClient(%this,'bottomPrint',%m,1);
	
	%this.bp = %this.schedule(900,bp);
}

function gameConnection::clearTools(%this)
{
	%p = %this.player;
	
	for(%i=0;%i<6;%i++)
	{
		%p.tool[%i] = 0;
		messageClient(%this,'MsgItemPickup','',%i,0);
	}
}

function gameConnection::clearToolByName(%this,%name,%multiple)
{
	%p = %this.player;
	
	for(%i=0;%i<6;%i++)
	{
		%t = %p.tool[%i];
		
		if(%t.getName() $= %name)
		{
			%p.tool[%i] = 0;
			messageClient(%this,'MsgItemPickup','',%i,0);
			if(!%multiple)
				break;
		}
	}
}

function gameConnection::addTool(%this,%weap)
{
	if(!isObject(%weap@"image"))
	{
		error(%weap@"image is not a valid object.");
		return 0;
	}
	
	%p = %this.player;
	
	for(%i=0;%i<6;%i++)
	{
		if(%p.tool[%i] $= "0")
		{
			%p.tool[%i] = %image;
			messageClient(%this,'MsgItemPickup','',%i,%weap@"image");
			%add = true;
		}
	}
	
	if(!%add)
		error("Player's inventory is full.");
}