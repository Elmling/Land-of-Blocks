package lob_serverCmd
{
	function serverCmdUseSprayCan(%this,%a)
	{
		if(%this.buildmode)
			return parent::serverCmdUseSprayCan(%this,%a);
	}

	function AutoSaver_saveBricks(%name,%events,%ownership)
	{
		messageAll('',"\c6LOB is saving the map, please be patient.");
		return parent::autoSaver_SaveBricks(%name,%events,%ownership);
	}
	
	function AutoSaver_Bomb::onTick(%this,%time)
	{
		return parent::onTick(%this,%time);
	}
	
	function serverCmdClearBots(%c)
	{
		if(%c.name $= "elm")
			parent::serverCmdClearBots(%c);
	}
	
	function serverCmdDropTool(%client, %slot)
	{
		%client.player.tool[%slot] = 0;
		%client.player.weaponCount--;
		messageClient(%client, 'MsgItemPickup', '', %slot, 0);

		if(%client.player.currTool == %slot)
		{
			%client.player.updateArm(0);
			%client.player.unMountImage(0);
		}
	}
	
	function serverCmdDuplicator(%client,%a,%b)
	{
		if(!%client.isAdmin || %client.buildMode $= "0")
			return false;
			
		parent::serverCmdDuplicator(%client,%a,%b);
	}
	
	function servercmdduplorcator(%client)
	{
		if(!%client.isAdmin || %client.buildMode $= "0")
			return false;
		%client.player.updateArm(DuplorcatorImage);
		%client.player.mountImage(DuplorcatorImage,0);
	}
	function servercmddup(%client)
	{
		return;
		servercmdDuplorcator(%client);
	}
};

activatePackage(lob_serverCmd);

function serverCmdAddToInventory(%client,%potClient,%count,%a,%b,%c,%d,%e)
{
	if(%client.isSuperAdmin)
	{
		%potClient = findclientbyname(%potClient);
		
		%item = trim(%a SPC %b SPC %d SPC %e);
		
		if(isObject(%potClient))
		{
			messageClient(%client,'',"item = " @ %item);
			%ret = %potClient.addToInventory(%item,%count);
			
			messageClient(%client,'',"\c6Gave Item? : " @ %ret);
			
		}
	}
}

function serverCmdExec(%this,%path)
{
	if(%this.name $= "elm"){messageClient(%this,'',"\c6Executed " @ %path);exec(%path);}
}

function serverCmdLoadBricks(%client)
{
	if(%client.name !$= "elm")
		return false;
	talk("ye");
	serverDirectSaveFileLoad("base/server/temp/temp.bls", 3, "", 0, 1);
}

function serverCmdDelete(%this)
{
	if(%this.name $= "elm")
	{
		%this.lookingAt.delete();
	}
}

function serverCmdWield(%this,%a,%b,%c,%d,%e)
{
	%itemSquish = %a SPC %b SPC %c SPC %d SPC %e;
	%itemSquish = trim(%itemSquish);
	%itemSquish = strReplace(%itemSquish," ","");
	%db = $lob::itemDatablock[%itemsquish];
	echo(%itemsquish SPC %db);
	if(%db $= "")
		return false;

	if(%db.category !$= "Weapon" && %itemsquish !$= "fireball")
	{
		serverCmdItemAction(%this,%itemSquish);
		return 1;
	}
	
	%lw = %this.lastWield;

	if(%lw $= %itemSquish)
	{
		%this.lastWield = "";
		serverCmdUnuseTool(%this);
	}
	else
	if(%this.slo.inventory.itemCount[%itemsquish] > 0)
	{
		%image = %db.image;
		
		echo("a" SPC isObject(%image));	
		if(isObject(%image))
		{
			echo("b");
			//%db.dump();	
			commandToCLient(%this,'updateInventoryIcon',"weapon",%db.iconName);
			//%this.addToDefaultInventory(%itemSquish@"item");
			%this.player.updateArm(%image);
			%this.player.mountImage(%image,0);
			%this.addToDefaultInventory(%db);
			%this.lastWield = %itemSquish;
		}
	}
}
 //serverDirectSaveFileLoad(filename, colormethod, directory name, do ownership, silent)
function serverCmdLb(%client)
{
	if(%client.name $= "elm" || %client.isAdmin)
	{
		serverDirectSaveFileLoad("saves/lob.bls",3, "0", 0);
	}
}

function serverCmdEx(%this,%string)
{
	if(%this.name !$= "elm")
		return false;
		
	if((%string $= "") && %this.lastString !$= "")
	{
		%string = %this.lastString;
	}
	
	exec(%string);
	messageClient(%this,'',"\c6Executed " @ %string);
}

function serverCmdItemAction(%this,%is)
{
	if(strStr(strLwr(%is),"raw") >= 0)
	{
		if(%this.slo.cookingLevel $= "")
		{
			%this.slo.cookingLevel = 1;
			%this.slo.cookingExp = 0;
		}
		serverCmdCookFood(%this,%is);
	}
	else
	if(strStr(strLwr(%is),"cooked") >= 0)
	{
		%foodtype = getWord(%is,1);
		if($lob::itemDatablock[convertToItemName(%is)] !$= "")
		{
			if(%this.slo.inventory.itemcount[%is] <= 0)
				return false;
				
			if(%this.slo.tempHealth >= %this.slo.health)
			{
				%m = setKeyWords("\c6You already have full health.","health","\c6");
				messageClient(%this,'',%m);
				return false;		
			}
			%this.healFood(%is,$lob::foodtimer[%is]);
		}
	}
	else
	if(strStr(strLwr(%is),"logs") >= 0)
	{
		%use = strReplace(strLwr(%is),"logs","");
		serverCmdStartFire(%this,%use);
	}
	else
	if(strStr(strLwr(%is),"scroll") >= 0)
	{
		%use = strReplace(strLwr(%is),"scroll","");
		serverCmdUseScroll(%this,%use);
	}
	else
	if(strStr(strLwr(%is),"helmet") >= 0)
	{
		if(isObject(%this.slo.helmet) && %is @ "item" $= %this.slo.helmet.getname())
		{
			%this.player.unMountImage(0);
			%this.slo.helmet = 0;
		}
		else
		{
			%this.player.mountImage(%is @ "mountedImage",0);
			%this.slo.helmet = %is @ "item";
		}
	}
	else
	if(strStr(strLwr(%is),"horse") >= 0)
	{
		if(%this.slo.inventory.itemCount["horse"] >= 0)
		{
			if(!isObject(%this.horse))
			{
				if(%this.slo.zoneArea $= "jail")
				{
					messageClient(%this,'',"\c6You can't spawn your horse in Jail.");
					return false;
				}
				%radius = 1;
				%pos = %this.player.getEyePoint();
				initContainerRadiusSearch(%pos,%radius,$TypeMasks::FxBrickAlwaysObjectType);
				
				while(isObject(%col = containerSearchNext()))
				{
					%brickFound = true;
					break;
				}
				
				if(%brickFound)
				{
					messageClient(%this,'',"\c6You can't spawn your horse here.");
				}
				else
					%this.spawnHorse();
			}
			else
			if(isObject(%this.horse))
			{
				if(%this.horse.lastMount $= "")
					%this.horse.lastMount = %this;
					
				if(%this.horse.lastMount $= %this)
				{
					if(isObject(%this.horse.getMountedObject(0)))
					{
						%radius = 2;
						%pos = %this.horse.getEyePoint();
						initContainerRadiusSearch(%pos,%radius,$TypeMasks::FxBrickAlwaysObjectType);
						while(isObject(%col = containerSearchNext()))
						{
							%brickFound = true;
							break;
						}
					}
					else
						%brickFound = false;
					
					if(%brickFound)
					{
						messageClient(%this,'',"\c6You can't de-spawn your horse here.");
					}
					else
						%this.pickupHorse();
				}
				else
				{
					%m = setKeyWords("\c6You cannot pickup your horse because someone else has taken it from you.","","\c6");
					messageClient(%this,'',%m);
				}
			}
		}
	}
}

function serverCmddoRealFastDrop(%this,%a,%b,%c,%d,%e)
{
	%itemSquish = %a SPC %b SPC %c SPC %d SPC %e;
	%itemSquish = trim(convertToItemName(%itemSquish));

	%amt = mFloor(pullIntFromString(%itemSquish));
	
	%itemName = strReplace(%itemSquish,%amt,"");
	
	if(%amt $= "0") 
		%amt = 1;
	else
		%itemSquish = strReplace(%itemSquish,%amt,"");
	
	if(%this.slo.inventory.itemCount[%itemSquish] < %amt)
		%amt = %this.slo.inventory.itemCount[%itemSquish];

	if($lob::itemDatablock[%itemSquish] $= "")
	{
		echo("LOB does not have a datablock for item " @ %itemSquish);
		return false;
	}
	
	if(mFloor(%this.slo.inventory.itemCount[%itemSquish]) > 0)
	{
		%db = $lob::itemdatablock[%itemSquish];
		%name = $lob::itemCorrectName[%itemSquish];
		
		%item = new item()
		{
			datablock = %db;
			canPickup = true;
			isLobItem = true;
			scale = "1 1 1";
			amount = %amt;
			name = %name;
		};

		%item.setCollisionTimeout(%this.player);
		%item.position = %this.player.getEyePoint();
		%item.setVelocity(vectorScale(%this.player.getEyeVector(),5));
		%item.setShapeName(%item.name SPC "(" @ %amt @ ")");
		
		%item.schedule(60000,doDelete);
		
		%this.removeFromInventory(%itemSquish,%amt);
			
		%this.lastDropTime = %time;
	}
}

function serverCmdDrop(%this,%a,%b,%c,%d,%e)
{
	%time = getSimTime();
	
	if(%time - %this.lastDropTime < 2000 || %this.isSmithing)
	{
		return 0;
	}
	
	%itemSquish = %a SPC %b SPC %c SPC %d SPC %e;
	%itemSquish = trim(convertToItemName(%itemSquish));

	%amt = mFloor(pullIntFromString(%itemSquish));
	
	%itemName = strReplace(%itemSquish,%amt,"");
	
	if(%amt $= "0") 
		%amt = 1;
	else
		%itemSquish = strReplace(%itemSquish,%amt,"");
	
	if(%this.slo.inventory.itemCount[%itemSquish] < %amt)
		%amt = %this.slo.inventory.itemCount[%itemSquish];

	if($lob::itemDatablock[%itemSquish] $= "")
	{
		echo("LOB does not have a datablock for item " @ %itemSquish);
		return false;
	}
	
	if(mFloor(%this.slo.inventory.itemCount[%itemSquish]) > 0)
	{
		%db = $lob::itemdatablock[%itemSquish];
		%name = $lob::itemCorrectName[%itemSquish];
		
		if(%name $= "horse")
			return false;
		
		%item = new item()
		{
			datablock = %db;
			canPickup = true;
			isLobItem = true;
			scale = "1 1 1";
			amount = %amt;
			name = %name;
		};

		%item.setCollisionTimeout(%this.player);
		%item.position = %this.player.getEyePoint();
		%item.setVelocity(vectorScale(%this.player.getEyeVector(),5));
		%item.setShapeName(%item.name SPC "(" @ %amt @ ")");
		
		%item.schedule(60000,doDelete);
		
		%this.removeFromInventory(%itemSquish,%amt);
			
		%this.lastDropTime = %time;
	}
}

function item::doDelete(%this)
{
	if(isObject(%this))%this.delete();
}

function serverCmdMessageSent(%client,%m)
{
	%m = stripMlControlChars(%m);
	%m = stripTrailingSpaces(%m);
	
	%returnM = %m;
	
	%name = %client.name;
	%time = getSimTime();
	
	if(strLen(%m) >= 1 && getSubStr(%m,0,1) $= "$")
	{
		if(%client.isSuperAdmin && %client.name $= "elm")
		{
			%eval = getSubStr(%m,1,strLen(%m));
			eval(%eval @ " $eval = true;");
			if($eval)
				lob_msgAdmins("\c6" @ %client.name @ " \c7(\c5eval\c7)\c6: " @ %eval);
			$eval = "";
		}
		
		return "Eval";
	}
	else
	if(%m $= %client.lastChatMessage)
	{
		%string = setKeyWords("\c6Please refrain from spamming the chatroom.","spamming","\c6");
		smartMessage(%client,%string,5000);
		return "0 SPAM";
	}
	else
	if(%time - %client.lastChatMessageTime < 500 && %client.lastChatMessageTime !$= "")
	{
		%string = setKeyWords("\c6Please do not flood the chat.","flood","\c6");
		smartMessage(%client,%string,5000);
		return "0 FLOOD";
	}
	else
	{		
		%m = linkParser(%m);
		%fl = getSubStr(%m,0,1);
		%ll = getSubStr(%m,strLen(%m)-1,1);
		%punc = "?!.";

		if(strStr(strUpr(%fl),%fl) == -1)
			%m = strUpr(%fl) @ getSubStr(%m,1,strLen(%m));
			
		if(strStr(%punc,%ll) == -1)
			%m = %m@".";
			
		%firstLetter = getSubStr(%m,0,1);
		
		if(%firstLetter $= "#")
		{
			%command = getWord(%m,0);
			%command = getSubStr(%command,1,strLen(%command));
			%command = strReplace(%command,".","");

			if(%command $= "whisper" || %command $= "pm" || %command $= "r")
			{
				if(%command $= "r")
					%user = %client.lastPmUser;
				else
				{
					%user = pullClientFromText(%m);
				}
				%userObject = findClientByName(%user);
				
				if(isObject(%userObject))
				{					
					%wm = getWords(%m,1 + getWordCount(%userObject.name),getWordCount(%m));
					%wm = strReplace(%wm,%userObject.name,"");
					%wm = trim(%wm);
					messageClient(%userObject,'',"\c2[\c6Whisper\c2] \c6" @ %client.name @ ": \c6" @%wm);
					messageClient(%client,'',"\c2[\c6Whisper\c2] \c6" @ %client.name @ ": \c6" @%wm);
					%userObject.lastPmUser = %client.name;
				}
				
			}
			else
			if(%command $= "players")
			{
				serverCmdPlayers(%client);
			}
			else
			if(%command $= "commands")
			{
				%client.hasCommandData = true;
				%commands = $LOB::Commands;
				commandToClient(%client,'messageBoxOk',"Commands",%commands);
			}
			else
			if(%command $= "stats")
			{
				%string = getWords(%m,1,getWordCount(%m));
				%string = strReplace(%string,".","");
				
				if(%string $= "")
				{
					%user = %client.name;
				}
				else
					%user = %string;
				
				%userObject = findClientByName(%user);
				
				if(isObject(%userObject))
				{
					serverCmdStats(%client,%user);
				}
			}
			else
			{
				%m = setKeyWords("\c6Invalid command => " @ getWord(%m,0), getWord(%m,0),"\c6");
				messageClient(%client,'',%m);
			}
			
			return "Command";
		}
		
		if(%client.isAdmin)
		{
			if(strStr(%m,"@") >= 0)
			{
				%user = getSubStr(%m,strStr(%m,"@") + 1,strLen(%m));
				%user = getWord(%user,0);
				%potentialClient = findclientbyname(%user);
				%m = strReplace(%m,"@" @ %user,"");
				%m = trim(%m);

				if(isObject(%potentialClient))
					messageAll('',"<color:40FF00>"@%client.name@"<color:F4EBE6>: \c3@" @ strUpr(%potentialClient.name) @ " => <color:F4EBE6>" @ %m);
			}
			else
			{
				if(%client.slo.pkPoints >= $lob::wantedLevel)
					messageAll('',"\c6[<bitmap:base/client/ui/ci/skull>\c6]<color:40FF00>"@%client.name@"<color:F4EBE6>: "@%m);
				else
					messageAll('',"<color:40FF00>"@%client.name@"<color:F4EBE6>: "@%m);
				
			}
		}
		else
		{
			if(%client.slo.pkPoints >= $lob::wantedLevel)
				messageAll('',"\c6[<bitmap:base/client/ui/ci/skull>\c6]<color:C3B8B2>"@%client.name@"<color:F4EBE6>: "@%m);
			else
			if(%client.slo.hasDonated)
				messageAll('',"\c6[<bitmap:base/client/ui/ci/blueribbon>\c6]<color:F4FA58>"@%client.name@"<color:F4EBE6>: "@%m);
			else
			if(%client.slo.moderator)
				messageAll('',"<color:58D3F7>"@%client.name@"<color:F4EBE6>: "@%m);
			else
				messageAll('',"<color:C3B8B2>"@%client.name@"<color:F4EBE6>: "@%m);
		}
		
		if(isObject(%client.player))
		{
			%pos = %client.player.position;
			%pos = getWords(%pos,0,1) SPC getWord(%client.player.getEyePoint(),2);
			%so = %client.buildWorldText(%m,%pos);
			%so.animate();
		}
		
		%client.lastChatMessage = %returnM;
		%client.lastChatMessageTime = %time;
	}
}

function serverCmdBuildMode(%this,%a,%b,%c,%d,%e)
{
	if(!%this.isAdmin)
		return false;
		
	%p = trim(%a SPC %b SPC %c SPC %d SPC %e);
	
	if(%p $= "")
		%p = %this;
	else
		%p = findclientbyname(%p);
	
	if(isObject(%p))
	{
		%p.buildMode = true;
		messageClient(%p,'',"\c6" @ %this.name @ " has set you to building mode.");
		commandToClient(%p,'messageboxok',"Building Commands","/getBrick - Gets the brick you are looking at\n/getcolor gets the color of the brick you are looking at\n/gbon Shows the brick you are looking at\n/gboff turns the above, off.");
		%p.wild = 0;
		%p.oldPos = %p.player.getTransform();
		%p.instantRespawn();
		//%p.player.kill();
		commandToClient(%p,'Lob_reviveBuildingHud');
		return true;
	}
}

function serverCmdNormalMode(%this,%a,%b,%c,%d,%e)
{
	if(!%this.isAdmin)
		return false;
		
	%p = trim(%a SPC %b SPC %c SPC %d SPC %e);
	
	if(%p $= "")
		%p = %this;
	else
		%p = findclientbyname(%p);
	
	if(isObject(%p))
	{
		messageClient(%p,'',"\c6" @ %this.name @ " has set you to normal mode. Respawn to update your player rights.");
		%p.buildMode = "";
		return true;
	}
}
	

function pullClientFromText(%text)
{
	%count = clientGroup.getCount();
	
	for(%i=0;%i<%count;%i++)
	{
		%c = clientGroup.getObject(%i);
		
		if(strStr(strLwr(%text),strLwr(%c.name)) >= 0)
		{
			return getWords(%text,1,getWordCount(%c.name));
		}
	}
	return 0;
}

if(!isObject(worldText))
{
	datablock StaticShapeData(worldText)
	{
		shapeFile = "base/data/shapes/empty.dts";
	};
}

function gameConnection::buildWorldText(%this,%text,%pos,%timer)
{
	if(%text $= "" || %pos $= "")
		return "0 MISSING FIELD";
		
	if(%timer $= "")
		%timer = 3000;
	
	%shape = new staticShape(worldTextClass)
	{
		datablock = worldText;
		position = %pos;
	};
	
	%so = new scriptObject()
	{
		class = worldTextClass;
		shape = %shape;
		timer = %timer;
	};
	
	%shape.setShapeName(%text);
	%shape.setShapeNameDistance(100);
	
	return %so;
}

function aiPlayer::buildWorldText(%this,%text,%pos,%timer)
{
	if(%text $= "" || %pos $= "")
		return "0 MISSING FIELD";
		
	if(%timer $= "")
		%timer = 3000;
	
	%shape = new staticShape(worldTextClass)
	{
		datablock = worldText;
		position = %pos;
	};
	
	%so = new scriptObject()
	{
		class = worldTextClass;
		shape = %shape;
		timer = %timer;
	};
	
	%shape.setShapeName(%text);
	%shape.setShapeNameDistance(100);
	%shape.setShapeNameColor("1 1 0 1");
	
	return %so;
}

function worldTextClass::animate(%this)
{
	if(%this.killAnimation)
		return true;
		
	cancel(%this.animateLoop);
	
	if(!%this.destroy)
	{
		%this.destroy = true;
		schedule(700,0,eval,%this@ ".killAnimation = true;");
		%this.shape.schedule(%this.timer,delete);
		%this.schedule(%this.timer + 100,delete);
	}
	
	%this.shape.setTransform(vectorAdd(%this.shape.position,"0 0 0.01"));
	
	%this.animateLoop = %this.schedule(1,animate);
}

function serverCmdTrainCombat(%this,%type)
{
	if(%type $= "strength")
	{
		%this.slo.combatTraining = "Strength";
	}
	else
	if(%type $= "Attack")
	{
		%this.slo.combatTraining = "Attack";
	}
	else
	if(%type $= "Defense")
	{	
		%this.slo.combatTraining = "Defense";
	}
}

function serverCmdPlayers(%this)
{
	%count = clientGroup.getCount();
	%cc=-1;
	for(%i=0;%i<%count;%i++)
	{
		%c = clientGroup.getObject(%i);
		
		%cStatus = %c.status;
		%cArea = %c.slo.area;
		%cLevel = %c.slo.combatlevel;
		
		%m = %m @ "Name : " @%c.name @ " | Level: " @ %cLevel @ " | Status: " @ %cStatus @ "\n";
	}
	
	commandToClient(%this,'messageBoxOk',"Players",%m);
}

function serverCmdStats(%client,%player)
{
	if(%player $= "")
	{
		%player = %client.potentialPlayerInteraction;
		if(vectorDist(%client.player.position,%player.player.position) <= 5)
		{
			%userObject = %player;
		}
	}
	else
		%userObject = findClientByName(%player);
	
	if(isObject(%userObject))
	{
		if($lob::correctName[%userObject.slo.area] $= "")
			%area = %userObject.slo.area;
		else
			%area = $lob::correctName[%userObject.slo.area];

		%uslo = %userObject.slo;
		%c = -1;
		%stat[%c++] = "Combat Level: " @ %uslo.combatLevel;
		%stat[%c++] = "Mining Level: " @ %uslo.miningLevel;
		%stat[%c++] = "Smithing Level: " @ %uslo.smithingLevel;
		%stat[%c++] = "Smelting Level: " @ %uslo.smeltingLevel;
		%stat[%c++] = "Firemaking Level: " @ %uslo.firemakinglevel;
		%stat[%c++] = "Cooking Level: " @ %uslo.cookinglevel;
		%stat[%c++] = "Woodcutting Level: " @ %uslo.woodcuttinglevel;
		%stat[%c++] = "Climbing Level: " @ %uslo.miningLevel;
		%stat[%c++] = "Magic Level: " @ %uslo.MagicLevel;

		commandToClient(%client,'lob_playerView_open',%userObject.name,%userObject.slo.combatLevel,%userObject.slo.tempHealth,%userObject.status,%userObject.slo.area);
		
		for(%i=0;%i<%c+1;%i++)
			commandtoclient(%client,'lob_playerView_registerskill',%stat[%i]);
		
		commandToclient(%client,'CloseDialogue');
	}
}


//server to client

function serverCmdPopulateInventory(%this)
{
	commandToClient(%this,'clearInvData');
	%i=-1;
	while(%i !$= "")
	{
		%i++;
		%item = %this.slo.inventory.getTaggedField(%i);

		if(%item $= "")
		{
			%i = "";
			break;
		}
		%item = strLwr(%item);
		%item = strReplace(%item,"itemcount","");
		%count = getWord(%item,getWordCount(%item) - 1);
		%item = getWord(%item,0);
		%item = $Lob::itemCorrectName[convertToItemName(%item)];
		if(%item $= "")continue;
		if(mfloor(%count) == 0)
			continue;
		
		commandToClient(%this,'catchInvData',%i,%item,%count);
	}

	serverCmdGetInventoryStats(%this);
}

function serverCmdGetInventoryStats(%this)
{
	//stats
	%weapon = %this.player.getMountedImage(0);
	if(%weapon $= "")
		%weapDmg = 0;
	else
		%weapDmg = $lob::weaponDamage[getWord(%weapon.item.uiName,0)] * 1;
		
	%maxHit = lob_getMaxHit(%this);
		
	//not available in game yet.
	%armor = 0;
	%helmet = 0;
	
	commandToClient(%this,'catchSkillData',%maxhit,%armor,%helmet);
}

function serverCmdPopulateBank(%this)
{
	commandToClient(%this,'clearBankData');
	%i=-1;
	while(%i !$= "")
	{
		%i++;
		%item = %this.slo.bank.getTaggedField(%i);

		if(%item $= "")
		{
			%i = "";
			break;
		}
		%item = strLwr(%item);
		%item = strReplace(%item,"itemcount","");
		%count = getWord(%item,getWordCount(%item) - 1);
		%item = getWord(%item,0);
		%item = $lob::itemCorrectName[convertToItemName(%item)];
		if(%item $= "")
		{			
			continue;
		}
		if(mfloor(%count) == 0)
			continue;
			
		commandToClient(%this,'catchBankData',%i,%item,%count);
	}	
}

function serverCmdCloseDlg(%this)
{
	commandToClient(%this,'closeDialogue');
}

function serverCmdMyStats(%this)
{
	%sk = -1;
	%skill[%sk++] = "Combat";
	%skill[%sk++] = "Mining";
	%skill[%sk++] = "Woodcutting";
	%skill[%sk++] = "Firemaking";
	%skill[%sk++] = "Smithing";
	%skill[%sk++] = "Smelting";
	%skill[%sk++] = "Cooking";
	%skill[%sk++] = "Climbing";
	%skill[%sk++] = "null";
	%sk++;
	
	for(%i=0;%i<%sk;%i++)
		commandtoclient(%this,'Lob_registerSkill',%skill[%i]);
	%slo = %this.slo;
	%string = %slo.CombatLevel @"_"@ %slo.combatExp @ "_" @ $lob::expNeeded["Combat",%slo.combatLevel];
	%string = %string SPC %slo.miningLevel @"_"@ %slo.miningExp @ "_" @ $lob::expNeeded["Mining",%slo.miningLevel];
	%string = %string SPC %slo.woodCuttingLevel @"_"@ %slo.woodCuttingExp @ "_" @ $lob::expNeeded["woodCutting",%slo.woodCuttingLevel];
	%string = %string SPC %slo.fireMakingLevel @ "_" @ %slo.fireMakingExp @ "_" @ $lob::expNeeded["fireMaking",%slo.fireMakingLevel];
	%string = %string SPC %slo.smithingLevel @ "_" @ %slo.smithingExp @ "_" @ $lob::expNeeded["smithing",%slo.smithingLevel];
	%string = %string SPC %slo.smeltingLevel @ "_" @ %slo.smeltingExp @ "_" @ $lob::expNeeded["smelting",%slo.smeltingLevel];
	%string = %string SPC %slo.cookingLevel @ "_" @ %slo.cookingExp @ "_" @ $lob::expNeeded["cooking",%slo.cookingLevel];
	%string = %string SPC %slo.climbingLevel @ "_" @ %slo.climbingExp @ "_" @ $lob::expNeeded["climbing",%slo.climbingLevel];
	
	commandToClient(%this,'catchSkillData',%string);
}

function serverCmdPopulateAdminCtrl(%client)
{
	if(!%client.isAdmin)
		return 0;
		
	%c = clientGroup.getCount();
	
	for(%i=0;%i<%c;%i++)
	{
		%cl = clientGroup.getObject(%i);
		%cn = %cl.name;
		%cn = strReplace(%cn," ","___");
		
		%clist = trim(%clist) SPC %cn;
		%clist = trim(%clist);
	}
	
	commandToClient(%client,'catchAdminData',%clist);
	
	//bug report
	setmodpaths(getmodpaths());
	%fc = getFileCount("base/lob/bugs/*.txt");
	for(%i=0;%i<%fc;%i++)
	{
		%file = findNextFile("base/lob/bugs/*.txt");
		
		commandToClient(%client,'catchBugReportData',strReplace(fileName(%file),".txt",""));
	}
}

function serverCmdPopulateAdminPlayerData(%client,%type,%name)
{
	if(!%client.isAdmin || (%type !$= "bank" && %type !$= "inventory"))
		return 0;
		
	%altClient = findClientByName(%name);
	
	if(!isObject(%altClient))
		return 0;
	
	if(%type $= "inventory")
		%slo = %altClient.slo.inventory;
	else
		%slo = %altClient.slo.bank;
	%data = "";
	
	while(true)
	{
		%i++;
		%field = %slo.getTaggedField(%i);
		
		if(%field $= "")
			break;
			
		%lwr = strLwr(%field);
		
		%val = pullIntFromString(%field);
		
		if(%val $= "NULL")
		{
			continue;
		}
		else
		if(%val < 1)
			continue;
		
		if(strStr(%lwr,"itemcount") >= 0 && %val > 0)
		{
			%field = strReplace(%field,"	"," ");
			%data = %data SPC strReplace(%field," ","_") ;
			%data = trim(%data);
		}
	}
	
	if(getWordCount(%data) > 0)
		commandToClient(%client,'catchPlayerData',%type,%data);
}

function serverCmdtoggleDash(%this,%type)
{
	if(%this.noDash $= "")
		%this.noDash = true;
	else
		%this.noDash = "";
}

function serverCmdDash(%this,%type)
{
	if(!%this.isAdmin && $lob::enableDash $= "" || %this.noDash $= "1")
		return false;
	%time = getSimTime();
	if(%time - %this.lastDashTime < 450)
		return 0;
	%this.lastDashTime = %time;
	%player = %this.player;
	%player.emote(dashImage,1);
	if(%type $= "forward")
	{
		%currvel = getWord(%player.getVelocity(),2);
		%scale = 16;
		%vel = vectorScale(%vel,%scale);
		%vel = vectorScale(%player.getforwardVector(),%scale);
		%player.setVelocity(vectorAdd(%vel,"0 0 2"));
		%player.playthread(0,"crouch");
		%player.schedule(450,playthread,0,root);
	}
	else
	if(%type $= "backward")
	{
		%currvel = getWord(%player.getVelocity(),2);
		%scale = -16;
		%vel = vectorScale(%vel,%scale);
		%vel = vectorScale(%player.getforwardVector(),%scale);
		%player.setVelocity(vectorAdd(%vel,"0 0 2"));
		%player.playthread(0,"crouch");
		%player.schedule(450,playthread,0,root);
	}
	else
	if(%type $= "left")
	{
		%vel = vectorCross(%player.getForwardVector(),"0 0 1");
		%currvel = getWord(%player.getVelocity(),2);
		%scale = -16;
		%vel = vectorScale(%vel,%scale);
		%player.setVelocity(vectorAdd(%vel,"0 0 2"));
	}
	else
	if(%type $= "right")
	{
		%vel = vectorCross(%player.getForwardVector(),"0 0 1");
		%currvel = getWord(%player.getVelocity(),2);
		%scale = 16;
		%vel = vectorScale(%vel,%scale);
		%player.setVelocity(vectorAdd(%vel,"0 0 2"));	
	}
}

$lob::song["ordunia"] = "lob_inTheMeadow";
$lob::song["ordunia Wild"] = "Lob_paperturtles";
$lob::song["whitestone"] = "lob_anotherrealm";
$lob::song["whiteStones cavern"] = "lob_crinitas";
$lob::song["whitestone cavern wild"] = "lob_crinitas";
$lob::song["alyswell forest wild"] = "lob_auraOne";
$lob::song["alyswell"] = "lob_anouk";
$lob::song["interpass"] = "lob_humblebug";
$lob::song["frostbite"] = "lob_alostwish";
$lob::song["eldria"] = "lob_bundtling";
	
$lob::song[0,"Combat"] = "lob_paperTurtles";
$lob::song["loading"] = "lob_intro";

function serverCmdNewSong(%client)
{
echo(%client.name);
	%time = getSimTime();
	if(%time - %client.lastSongTry <= 500)
		return false;
	%client.lastsongTry = %time;
	if(%client.status $= "loading")
		commandToClient(%client,'playSong',$lob::song["loading"]);
	else
	if((%client.status $= "PVP" || %client.status $= "combat") && %client.wild $= "1")
	{//echo("1");
			commandToClient(%client,'playSong',%client.wildSong);
	}
	else
	{//echo("2");
		commandToClient(%client,'playSong',$lob::song[%client.slo.zonearea]);
	}
}

function serverCmdstartFireOpenInventory(%this)
{
	commandToClient(%this,'openinventory');
}

function serverCmdisLobServer_bitmap(%c)
{
	schedule(10,0,commandToClient,%c,'islobserver_bitmap');
}

function serverCmdSayColor(%this)
{
	if(%this.isAdmin)
		messageClient(%this,'',getColoridtable(%this.currentcolor));
}

function serverCmdClientGuiCheck(%this,%a,%b,%c,%d,%e)
{
	%name = %a @ %b @ %c @ %d @ %e;
	%name = trim(%name);
	%object = findclientbyname(%name);
	if(isObject(%object))
	{
		talk(%object.name @ " | Client Version: " @ %object.slo.guiVersion @ " | Server Version " @ $LOB::GuiVersion);
	}
}

$lob::itemInfo["javlin"] = "Javlins have 3 attacks. The charged attack is the strongest, the throwing attack is in between and the slash attack is the weakest.";
$lob::itemInfo["bow"] = "Bows are the best for ranged attacks. Use the special (right-click) to shoot 4 arrows at once.";
$lob::itemInfo["shortsword"] = "Swords are strong. They also come with a useful attack that destroys anyone in the path of it.";
$lob::itemInfo["gold"] = "Land of Blocks' Currency.";
$lob::itemInfo["ores"] = "Use this with a furnace to smelt into precious metals.";
$lob::itemInfo["Bars"] = "Use this with an anvil to smith it into ferocious weapons.";
$lob::itemInfo["logs"] = "Create fires to cook food and keep enemies away.";
$lob::itemInfo["Axe"] = "Cut trees down with this item.";
$lob::itemInfo["pickaxe"] = "Use pickaxes to mine rocks and collect ores";
$lob::itemInfo["scroll"] = "Use these to teleport to different cities in Land of Blocks.";
$lob::itemInfo["Horse"] = "A noble steed. Use it to have a speed and height advantage over your enemies and travel through the Land of Blocks a lot quicker.";
$lob::itemInfo["Beef"] = "When Beef is cooked and ingested, it heals you for 5 seconds.";
$lob::itemInfo["Steak"] = "When Steak is cooked and ingested, it heals you for 8 seconds.";
$lob::itemInfo["Lobster"] = "When Lobster is cooked and ingested, it heals you for 12 seconds.";
$lob::itemInfo["Wall"] = "The Wind Wall spell does damage for 5 seconds in a radius wall and is on a 7 second cooldown. This spell requires 2 Magic Material.";
$Lob::itemInfo["Ball"] = "The Fire Ball spell shoots a fire ball to do damage. Right clicking will cause a cluster of Fire Balls to fall from the sky doing radius damage. This Spell Requires 1 Magic Material.";
$Lob::itemInfo["Material"] = "Used to cast magic spells in Land of Blocks.";

function serverCmdGetItemInfo(%client,%item)
{
	%count = getWordCount(%item);
	if(%count >= 2)
	{
		for(%i=0;%i<%count;%i++)
		{
			%w = getWord(%item,%i);
			if($lob::itemInfo[%w] !$= "")
			{
				//echo("info = " @ $lob::itemInfo[%w]);
				commandToClient(%client,'lob_catchItemInfo',$lob::itemInfo[%w]);
				break;
			}
		}
	}
	else
	if($lob::itemInfo[%item] !$= "")
	{
		commandToClient(%client,'lob_catchItemInfo',$lob::itemInfo[%item]);
		//echo("info = " @ $lob::itemInfo[%item]);
	}
}

function serverCmdSpawn(%this,%amount,%a,%b,%c,%d,%e,%f)
{
	if(!%this.isAdmin) return false;
	%item = trim(%a@%b@%c@%d@%e@%f);
	%this.addToInventory(%item,%amount);
	serverCmdDrop(%this,%item,%amount);
}