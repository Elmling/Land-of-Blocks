
package Combat
{
	function GameConnection::onDeath(%this,%obj,%killer,%type,%area)
	{
		if(%killer != %this)
		{
			//echo("killer = " @ %killer.name SPC "|" SPC %this.name);
			if(%killer.wild && %this.wild)
			{
				if(%this.slo.pkPoints >= $lob::wantedLevel)
				{
					//nothing
				}
				else
				{
					%killer.slo.pkPoints++;
					messageclient(%killer,'',"\c6You're PK Points: " @ %killer.slo.pkPoints @ ".");
				}
			}
			
			if(%killer.slo.pkPoints !$= "" && %killer.slo.pkPoints == $lob::wantedLevel)
			{
				%m = setKeyWords("\c6" @ %killer.name @ " is now wanted for the deaths of \c3" @ %killer.slo.pkPoints @ "\c6 players.",%killer.name SPC %this.name SPC "wanted","\c6");
				messageAll('',%m);
				commandToClient(%killer,'showTip',"\c0You are wanted, you'll lose everything when you die and are vulnerable to attack even in safe zones!",10000);
			}
		}
		
		if(%this.wild)
		{
			%this.dropRandomItemFromInventory();
		}
		
		if(%this.slo.pkPoints >= $lob::wantedLevel)
			%this.dropFullInventory();
		
		%this.activity.newActivity("died.");
		%pl = %this.player;
		
		for(%i=0;%i<%pl.dataBlock.maxTools;%i++)
		{
			%tool = %pl.tool[%i];
			
			%this.deathTool[%i] = %tool;
		}
		
		%this.lastwield = "";
		%p = parent::onDeath(%this,%obj,%killer,%type,%area);
		
		if(isObject(%this.tradeObject))
			serverCmdInTradeDecline(%this);
			
		if(isObject(%this.dungeon))
		{
			%alive = lob_dungeonGetAlive(%this.dungeon);
			talk("alive = " @ %alive);
			if(%alive <= 0)
			{
				//end dungeon
				%this.dungeon.script.endDungeon();
			}
		}
		
		return %p;
	}
	
	function serverCmdSuicide(%a,%b,%c,%d)
	{
		if(%a.isSmithing || %a.slo.pkPoints >= $lob::wantedLevel)
			return 0;
			
		return parent::serverCmdSuicide(%a,%b,%c,%d);
	}
};
activatePackage(Combat);

function lob_doFoodDrop(%npc)
{//echo("trying to drop food for "@ %npc.name);
	%name = %npc.name;
	if($foodDropChance[%name] !$= "" && getRandom(0,$foodDropChance[%name]) == $foodDropChance[%name] -1)
	{
		//echo("drop for " @ %name @" scussess");
		%npc.dropItem($drop[%name,"food"],1);
		return true;
	}
	
	return false;
}

function lob_doQuestDrop(%npc,%client)
{
	if(%client.slo.eldin_q1_toxicLeafPending)
	{
		if(%npc.name $= "ogre")
			if(getRandom(0,10) <= 2)
			{
				%client.slo.eldin_q1_toxicLeafPending = "";
				%client.slo.eldin_q1_hasToxicLeaf = true;
				messageClient(%client,'',"\c6You've gotten a Toxic Leaf from the Ogre. You should report back to Eldin.");
			}
	}
	
	if(%client.slo.robin_q2_readyToSlay)
	{
		if(%npc.name $= "ogre")
		{
			%client.slo.robin_q2_ogreCount++;
			%count = 5;
			%amount =  5 - %client.slo.robin_q2_ogreCount;
			
			if(%client.slo.robin_q2_ogreCount >= 5)
			{
				%client.slo.robin_q2_ReadyToSlay = 0;
				messageClient(%client,'',"\c6[QUEST] I've slayed 5 Ogres, I should probably talk to The Ogre General.");
				return;
			}
			
			messageClient(%client,'',"\c6[QUEST] " @ %client.slo.robin_q2_ogreCount @ " Ogres down, " @ %amount @ " left to go.");
		}
	}
}

function gameConnection::lobKill(%this,%client2)
{
	if(!isObject(%client2))
		return false;
		
	if(%this.duel)
	{
		%this.duel = "";
		%this.dueling.duel = "";
		%this.duel[%this.dueling.name] = "";
		%this.wild = 0;
		%this.dueling.wild = 0;
		%this.dueling.duel[%this.dueling.dueling.name] = "";
	}
		
	if(%client2.getclassName() $= "aiPlayer")
	{
		%client2.client = %client2;
		%client2.player = %client2;
	}
	else
	if(%this.getclassName() $= "aiPlayer")
	{
		%this.client = %client;
		%this.player = %client;
	}
	
	if(%client2.getClassName() $= "aiPlayer")
		if(isFirstLetterAlpha(%client2.name))
				%anExpression = "an ";
			else
				%anExpression = "a ";
	
	%m = setKeyWords("<font:impact:20>\c6" @ %this.name @ " was slain by " @ %anExpression @ %client2.name @".",%this.name SPC %client2.name SPC "slain","\c6");
	
	messageAll('',%m);
	return %this.player.damage(%client2.player,%this.position,1000000);
}

function aiplayer::lobKill(%this,%client2)
{
	if(!isObject(%client2))
		return false;
		
	if(%client2.getclassName() !$= "aiPlayer")
	{
		//nothing
	}
	else
		return false;
		
	talk("a");
	
	if(isFirstLetterAlpha(%this.name))
		%anExpression = "an";
	else
		%anExpression = "a";
		
	%m = setKeyWords("<font:impact:20>\c6" @ %client2.client.name @ " was slain by " @ %anExpression @ " " @ %this.name,%thist.name SPC %client2.client.name SPC "slain","\c6");
	
	messageAll('',%m);
	return %this.player.damage(%client2.player,%this.position,1000000);
}

function gameConnection::dropFullInventory(%this)
{
	if(%this.duel)	return false;
	%inv = %this.slo.inventory;
	%i =-1;
	
	while(true)
	{
		%i++;
		%item = %inv.getTaggedField(%i);
		if(mFloor(getWord(%item,getWordCount(%item) -1 )) <= 0 && %item !$= "")
			continue;
		%item = getWord(%item,0);
		if(%item !$= "")
		{
			%item = strReplace(strLwr(%item),"itemcount","");
			%item = trim(%item);
			serverCmdDoRealFastDrop(%this,$lob::itemcorrectName[%item],10000000);
		}
		else
			break;

	}
	
	%m = setKeyWords("\c6Every item in your inventory has fallen.","Every inventory fallen","\c6");
	smartMessage(%this,%m,100);
	
}

function gameConnection::dropRandomItemFromInventory(%this)
{
	if(%this.duel)	return false;
	%inv = %this.slo.inventory;
	%i =-1;
	
	while(true)
	{
		%i++;
		%item = %inv.getTaggedField(%i);
		if(mFloor(getWord(%item,getWordCount(%item) -1 )) <= 0 && %item !$= "")
			continue;
		if(%item $= "")
		{
			%itemCount = %i;
			break;
		}

	}
	%time = getRealTime();
	while(true)
	{
		%item = %inv.getTaggedField(getRandom(0,%itemCount));
		//messageClient(fcbn("elm"),'',getWord(%item,getWordCount(%item) - 1));
		%count = getWord(%item,getWordCount(%item) - 1);
		
		if(%count > 0 && getWord(%item,0) !$= "itemCount")
			break;
		
		if(getRealTime() - %time > 50)
			break;
		
		//break;
	}
	
	//messageClient(fcbn("elm"),'',%item);
			
	%itemName = trim(strReplace(strLwr(%item),"itemcount",""));
	%itemName = getWord(%itemName,0);
	%itemName = trim(%itemName);
	%correctName = $lob::itemCorrectName[%itemName];
	//%amount = getWord(%item,getWordCount(%item) - 1);
			//echo(%tf @ " | " @ %itemname @ " | " @ %correctname @ " | " @ %amount);
			
	if(%correctName $= "")
		return false;
	
	%m = setKeyWords("\c6All instances of " @ %correctName @ " has dropped from your inventory.",%correctName SPC "dropped","\c6");
	smartMessage(%this,%m,100);

	serverCmdDrop(%this,%correctName SPC "10000000");
	
}

function lob_dmgArgs(%client,%type,%col)
{
	if(%col.getClassName() $= "Player")
	{
		if(%client.dueling $= %col.client)return true;
		if(%col.client.slo.combatLevel <= 10)
		{
			%level = %col.client.slo.combatlevel;
			%tillPVP = (11 - %level);
			%m = setKeyWords("\c6" @ %col.client.name @ " needs " @ %tillPVP @ " more Combat Levels until you're able to attack him.",%col.client.name SPC "attack","\c6");
			smartMessage(%client,%m,10000);
			
			%m = setKeyWords("\c6You are protected from PVP for " @ %tillpvp @ " more Combat Levels.","protected PVP Combat Levels","\c6");
			smartMessage(%col.client,%m,10000);
			return false;
		}
		else
		if(%client.slo.combatLevel <= 10 && %col.client.slo.pkpoints <= 2)
		{
			%level = %client.slo.combatLevel;
			%tillPVP = (11 - %level);
			%m = setKeyWords("\c6You need level 11 or greater to PK other players, " @ %tillPVP @ " more levels to go.","need greater pk players","\c6");
			smartMessage(%client,%m,10000);
			return false;
		}
	}
	
	return true;
}

function shortSwordOnHit(%client,%type,%col)
{
	%class = %col.getClassName();

	if(%client.getClassName() $= "GameConnection")
	{
		//attacking a player
		if(%class $= "Player")
		{
			if(lob_dmgArgs(%client,%type,%col) $= "0")
				return false;
				
			if(%client.buildMode $= "1" || %col.client.buildMode $= "1")
				return false;
				
			if(%client.slo.area $= "jail" || %col.client.slo.area $= "jail")
				return false;
				
			if(%client.wild && %col.client.wild || %col.client.slo.pkPoints >= $lob::wantedLevel)
			{
				%client.activity.newActivity("is fighting " @ %col.client.name@".");
				%col.client.activity.newActivity("is being attacked by " @ %client.name@".");
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%col.client.status = "Combat";
				cancel(%col.client.clearStatus);
				%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
				
				%player = %client.player;
				%dmg = getDamage(%player,%col);
				
				if(%dmg > 0)
				{
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%slo = %col.client.slo;
				%slo.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				giveExp(%client,"combat",%exp);
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.client.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(%slo.tempHealth <= 0)
				{
					%m = setKeyWords("\c6You've killed " @ %col.client.name @ " level " @ %col.client.slo.combatLevel,%col.client.name,"\c6");
					%slo.tempHealth = %slo.health;
					%col.client.lobkill(%client);
				}
				else
				{
					%m = setKeyWords("\c6" @ %col.client.name @ "\'s Health = " @ %col.client.slo.tempHealth,%col.client.slo.tempHealth,"\c6");
				}
				
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
		else
		if(%class $= "AiPlayer")
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%bn = strReplace(%col.brick.getname(),"_"," ");
			%bn = trim(%bn);
			if(getWord(%bn,0) $= "EnemySpawn")
			{
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%player = %client.player;
				
				%dmg = getDamage(%player,%col);
				
				if(%dmg > 0)
				{
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%col.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(!$lob::Enemy[%col.name,"Aggressive"])
				{
					%col.setTempAggression(5000);
				}
				
				giveExp(%client,"combat",%exp);
				
				if(%col.tempHealth <= 0)
				{
					//drop
					
					if(%col.name $= "bandit")
						if(%client.slo.robin_q1_started && !%client.slo.robin_q1_hasSword)
						{
							%ran = getRandom(1,10);
							
							if(%ran < 3)
							{
								//drop sword
								%amount = 1;
								%col.dropItem("GrandFather's Sword",%amount,%client);
							}
						}
						
					lob_doFoodDrop(%col);
					lob_doQuestDrop(%col,%client);
						
					if($itemDropChance[%col.name] !$= "" && getRandom(0,$itemDropChance[%col.name]) == $itemDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"item"],1);
					}
					
					if($materialDropChance[%col.name] !$= "" && getRandom(0,$materialDropChance[%col.name]) == $materialDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"material"],1);
					}
					
					%amt = getRandom(getWord($drop[%col.name,"gold"],0),getWord($drop[%col.name,"gold"],1));
					
					if(%amt > 0)
						%col.dropItem("Gold",%amt);
						
					%m = setKeyWords("\c6You've killed a level " @ %col.level SPC %col.name,%col.name,"\c6");
					%col.tempHealth = $LOB::Enemy[%col.name,"Health"];
					%col.fakeYourDeath($LOB::Enemy[%col.name,"RespawnTime"]);
				}
				else
					%m = setKeyWords("\c6" @ %col.name @ "\'s Health = " @ %col.tempHealth,%col.name,"\c6");
					
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
	}
	else
	if(%client.getClassName() $= "aiplayer")
	{
		if(%class $= "Player")
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%col.client.rejuvenateHealth == 1)
			{
				%col.client.rejuvenateHealth = 0;
				cancel(%col.client.rejuvenateHealthLoopStart);
			}
			
			%col.client.status = "Combat";
			cancel(%col.client.clearStatus);
			%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
			
			%player = %client;
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
	
			%slo = %col.client.slo;
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.client.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = %slo.health;
				%col.client.lobkill(%client);
			}
		}
		else
		if(%class $= "AiPlayer")
		{
			%player = %client;
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%slo = %col.client.slo;
			
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = $LOB::Enemy[%col.name,"Health"];
				//%col.kill();
			}	
		}
	}

	if(%col.isHorse)
	{
		if(%col.Wild)
		{
			if(%client.getclassname() $= "aiplayer")
				%player = %client;
			else
				%player = %client.player;
			
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%col.Health -= %dmg;
			
			%exp = mRound(msqrt(%dmg));
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%col.health <= 0)
				%col.owner.killHorse();
		}
	}
}

function arrowOnHit(%client,%type,%col)
{
	%class = %col.getClassName();
	%arrowBonus = 5;
	
	if(%client.getClassName() $= "GameConnection")
	{
		if(%class $= "Player")
		{
		
			if(lob_dmgArgs(%client,%type,%col) $= "0")
				return false;
				
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%client.slo.area $= "jail" || %col.client.slo.area $= "jail")
				return false;
				
			if(%client.wild && %col.client.wild || %col.client.slo.pkPoints >= $lob::wantedLevel)
			{
				%client.activity.newActivity("is fighting " @ %col.client.name@".");
				%col.client.activity.newActivity("is being attacked by " @ %client.name@".");
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%col.client.status = "Combat";
				cancel(%col.client.clearStatus);
				%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
				
				%player = %client.player;
				%dmg = getDamage(%player,%col);
				
				if(%dmg > 0)
				{
					%dmg = %dmg * 2;
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%slo = %col.client.slo;
				%slo.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				giveExp(%client,"combat",%exp);
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.client.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(%slo.tempHealth <= 0)
				{
					%m = setKeyWords("\c6You've killed " @ %col.client.name @ " level " @ %col.client.slo.combatLevel,%col.client.name,"\c6");
					%slo.tempHealth = %slo.health;
					%col.client.lobkill(%client);
				}
				else
				{
					%m = setKeyWords("\c6" @ %col.client.name @ "\'s Health = " @ %col.client.slo.tempHealth,%col.client.slo.tempHealth,"\c6");
				}
				
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
		else
		if(%class $= "AiPlayer" && !%col.isHorse)
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%bn = strReplace(%col.brick.getname(),"_"," ");
			%bn = trim(%bn);
			if(getWord(%bn,0) $= "EnemySpawn")
			{
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%player = %client.player;
				
				%dmg = getDamage(%player,%col);
				
				if(%dmg > 0)
				{
					%dmg = %dmg * 2;
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%col.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(!$lob::Enemy[%col.name,"Aggressive"])
				{
					%col.setTempAggression(5000);
				}
				
				giveExp(%client,"combat",%exp);
				
				if(%col.tempHealth <= 0)
				{
					//drop
					
					if(%col.name $= "bandit")
						if(%client.slo.robin_q1_started && !%client.slo.robin_q1_hasSword)
						{
							%ran = getRandom(1,10);
							
							if(%ran < 3)
							{
								//drop sword
								%amount = 1;
								%col.dropItem("GrandFather's Sword",%amount,%client);
							}
						}
						
					lob_doFoodDrop(%col);
					lob_doQuestDrop(%col,%client);
					
					if($itemDropChance[%col.name] !$= "" && getRandom(0,$itemDropChance[%col.name]) == $itemDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"item"],1);
					}
					
					if($materialDropChance[%col.name] !$= "" && getRandom(0,$materialDropChance[%col.name]) == $materialDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"material"],1);
					}
					
					%amt = getRandom(getWord($drop[%col.name,"gold"],0),getWord($drop[%col.name,"gold"],1));
					
					if(%amt > 0)
						%col.dropItem("Gold",%amt);
						
					%m = setKeyWords("\c6You've killed a level " @ %col.level SPC %col.name,%col.name,"\c6");
					%col.tempHealth = $LOB::Enemy[%col.name,"Health"];
					%col.fakeYourDeath($LOB::Enemy[%col.name,"RespawnTime"]);
				}
				else
					%m = setKeyWords("\c6" @ %col.name @ "\'s Health = " @ %col.tempHealth,%col.name,"\c6");
					
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
	}
	else
	if(%client.getClassName() $= "aiplayer")
	{
		if(%class $= "Player")
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%col.client.rejuvenateHealth == 1)
			{
				%col.client.rejuvenateHealth = 0;
				cancel(%col.client.rejuvenateHealthLoopStart);
			}
			
			%col.client.status = "Combat";
			cancel(%col.client.clearStatus);
			%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
			
			%player = %client;
			%dmg = getDamage(%player,%col);
				
			if(%dmg > 0)
			{
				%dmg = %dmg * 2;
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
	
			%slo = %col.client.slo;
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.client.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = %slo.health;
				%col.client.lobkill(%client);
			}
		}
		else
		if(%class $= "AiPlayer" && !%col.isHorse)
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%player = %client;
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%dmg = %dmg * 2;
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%slo = %col.client.slo;
			
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = $LOB::Enemy[%col.name,"Health"];
				//%col.kill();
			}	
		}
	}
	if(%col.isHorse)
	{
		if(%col.Wild)
		{
			if(%client.getclassname() $= "aiplayer")
				%player = %client;
			else
				%player = %client.player;
			
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%col.Health -= %dmg;
			
			%exp = mRound(msqrt(%dmg));
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%col.health <= 0)
				%col.owner.killHorse();
		}
	}
}

function shortSwordSpecialOnHit(%client,%type,%col)
{
	%class = %col.getClassName();

	if(%client.getClassName() $= "GameConnection")
	{
		if(%class $= "Player")
		{
		
			if(lob_dmgArgs(%client,%type,%col) $= "0")
				return false;
				
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%client.wild && %col.client.wild || %col.client.slo.pkPoints >= $lob::wantedLevel)
			{
				%client.activity.newActivity("is fighting " @ %col.client.name@".");
				%col.client.activity.newActivity("is being attacked by " @ %client.name@".");
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%col.client.status = "Combat";
				cancel(%col.client.clearStatus);
				%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
				
				%player = %client.player;
				%dmg = getDamage(%player,%col) * 2;
				
				if(%dmg > 0)
				{
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%slo = %col.client.slo;
				%slo.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				giveExp(%client,"combat",%exp);
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.client.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(%slo.tempHealth <= 0)
				{
					%m = setKeyWords("\c6You've killed " @ %col.client.name @ " level " @ %col.client.slo.combatLevel,%col.client.name,"\c6");
					%slo.tempHealth = %slo.health;
					%col.client.lobkill(%client);
				}
				else
				{
					%m = setKeyWords("\c6" @ %col.client.name @ "\'s Health = " @ %col.client.slo.tempHealth,%col.client.slo.tempHealth,"\c6");
				}
				
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
		else
		if(%class $= "AiPlayer" && !%col.isHorse)
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%bn = strReplace(%col.brick.getname(),"_"," ");
			%bn = trim(%bn);
			if(getWord(%bn,0) $= "EnemySpawn")
			{
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%player = %client.player;
				
				%dmg = getDamage(%player,%col) * 2;
				
				if(%dmg > 0)
				{
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%col.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(!$lob::Enemy[%col.name,"Aggressive"])
				{
					%col.setTempAggression(5000);
				}
				
				giveExp(%client,"combat",%exp);
				
				if(%col.tempHealth <= 0)
				{
					//drop
					
					if(%col.name $= "bandit")
						if(%client.slo.robin_q1_started && !%client.slo.robin_q1_hasSword)
						{
							%ran = getRandom(1,10);
							
							if(%ran < 3)
							{
								//drop sword
								%amount = 1;
								%col.dropItem("GrandFather's Sword",%amount,%client);
							}
						}
						
					lob_doFoodDrop(%col);
					lob_doQuestDrop(%col,%client);
						
					if($itemDropChance[%col.name] !$= "" && getRandom(0,$itemDropChance[%col.name]) == $itemDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"item"],1);
					}
					
					if($materialDropChance[%col.name] !$= "" && getRandom(0,$materialDropChance[%col.name]) == $materialDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"material"],1);
					}
					
					%amt = getRandom(getWord($drop[%col.name,"gold"],0),getWord($drop[%col.name,"gold"],1));
					
					if(%amt > 0)
						%col.dropItem("Gold",%amt);
						
					%m = setKeyWords("\c6You've killed a level " @ %col.level SPC %col.name,%col.name,"\c6");
					%col.tempHealth = $LOB::Enemy[%col.name,"Health"];
					%col.fakeYourDeath($LOB::Enemy[%col.name,"RespawnTime"]);
				}
				else
					%m = setKeyWords("\c6" @ %col.name @ "\'s Health = " @ %col.tempHealth,%col.name,"\c6");
					
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
	}
	else
	if(%client.getClassName() $= "aiplayer")
	{
		if(%class $= "Player")
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%col.client.rejuvenateHealth == 1)
			{
				%col.client.rejuvenateHealth = 0;
				cancel(%col.client.rejuvenateHealthLoopStart);
			}
			
			%col.client.status = "Combat";
			cancel(%col.client.clearStatus);
			%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
			
			%player = %client;
			%dmg = getDamage(%player,%col) * 2;
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
	
			%slo = %col.client.slo;
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.client.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = %slo.health;
				%col.client.lobkill(%client);
			}
		}
		else
		if(%class $= "AiPlayer" && !%col.isHorse)
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%player = %client;
			%dmg = getDamage(%player,%col) * 2;
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%slo = %col.client.slo;
			
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = $LOB::Enemy[%col.name,"Health"];
				//%col.kill();
			}	
		}
	}
	if(%col.isHorse)
	{
		if(%col.Wild)
		{
			if(%client.getclassname() $= "aiplayer")
				%player = %client;
			else
				%player = %client.player;
			
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%col.Health -= %dmg;
			
			%exp = mRound(msqrt(%dmg));
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%col.health <= 0)
				%col.owner.killHorse();
		}
	}
}

function JavlinOnHit(%client,%type,%col,%hitType)
{
	%class = %col.getClassName();
	if(%hitType $= "throw")
		%bonus = 2;
	else
	if(%hitType $= "charged")
		%bonus = 5;
	else
	if(%hitType $= "quick")
		%Bonus = 1;
	
	if(%client.getClassName() $= "GameConnection")
	{
		if(%class $= "Player")
		{
			if(lob_dmgArgs(%client,%type,%col) $= "0")
				return false;
				
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%client.slo.area $= "jail" || %col.client.slo.area $= "jail")
				return false;
				
			if(%client.wild && %col.client.wild || %col.client.slo.pkPoints >= $lob::wantedLevel)
			{
				%client.activity.newActivity("is fighting " @ %col.client.name@".");
				%col.client.activity.newActivity("is being attacked by " @ %client.name@".");
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%col.client.status = "Combat";
				cancel(%col.client.clearStatus);
				%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
				
				%player = %client.player;
				%dmg = getDamage(%player,%col);
				
				if(%dmg > 0)
				{
					%dmg = %dmg * %bonus;
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%slo = %col.client.slo;
				%slo.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				giveExp(%client,"combat",%exp);
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.client.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(%slo.tempHealth <= 0)
				{
					%m = setKeyWords("\c6You've killed " @ %col.client.name @ " level " @ %col.client.slo.combatLevel,%col.client.name,"\c6");
					%slo.tempHealth = %slo.health;
					%col.client.lobkill(%client);
				}
				else
				{
					%m = setKeyWords("\c6" @ %col.client.name @ "\'s Health = " @ %col.client.slo.tempHealth,%col.client.slo.tempHealth,"\c6");
				}
				
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
		else
		if(%class $= "AiPlayer" && !%col.isHorse)
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%bn = strReplace(%col.brick.getname(),"_"," ");
			%bn = trim(%bn);
			if(getWord(%bn,0) $= "EnemySpawn")
			{
				%client.status = "Combat";
				cancel(%client.clearStatus);
				%client.clearStatus = %client.schedule(3500,clearStatus);
				
				%player = %client.player;
				
				%dmg = getDamage(%player,%col);
				
				if(%dmg > 0)
				{
					%dmg = %dmg * %bonus;
					%col.setDamageFlash(0.5);
					%col.playPain();
				}
				
				%col.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				%pos = %col.position;
				%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
				%timer = 2000;
				%so = %col.buildWorldText(%dmg,%pos,%timer);
				%so.animate();
				
				if(!$lob::Enemy[%col.name,"Aggressive"])
				{
					%col.setTempAggression(5000);
				}
				
				giveExp(%client,"combat",%exp);
				
				if(%col.tempHealth <= 0)
				{
					//drop
					
					if(%col.name $= "bandit")
						if(%client.slo.robin_q1_started && !%client.slo.robin_q1_hasSword)
						{
							%ran = getRandom(1,10);
							
							if(%ran < 3)
							{
								//drop sword
								%amount = 1;
								%col.dropItem("GrandFather's Sword",%amount,%client);
							}
						}
						
					lob_doFoodDrop(%col);	
					lob_doQuestDrop(%col,%client);
					
					if($itemDropChance[%col.name] !$= "" && getRandom(0,$itemDropChance[%col.name]) == $itemDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"item"],1);
					}
					
					if($materialDropChance[%col.name] !$= "" && getRandom(0,$materialDropChance[%col.name]) == $materialDropChance[%col.name] -1)
					{
						%col.dropItem($drop[%col.name,"material"],1);
					}
					
					%amt = getRandom(getWord($drop[%col.name,"gold"],0),getWord($drop[%col.name,"gold"],1));
					
					if(%amt > 0)
						%col.dropItem("Gold",%amt);
						
					%m = setKeyWords("\c6You've killed a level " @ %col.level SPC %col.name,%col.name,"\c6");
					%col.tempHealth = $LOB::Enemy[%col.name,"Health"];
					%col.fakeYourDeath($LOB::Enemy[%col.name,"RespawnTime"]);
				}
				else
					%m = setKeyWords("\c6" @ %col.name @ "\'s Health = " @ %col.tempHealth,%col.name,"\c6");
					
				commandToClient(%client,'centerPrint',%m,4);
			}
		}
	}
	else
	if(%client.getClassName() $= "aiplayer")
	{
		if(%class $= "Player")
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			if(%col.client.rejuvenateHealth == 1)
			{
				%col.client.rejuvenateHealth = 0;
				cancel(%col.client.rejuvenateHealthLoopStart);
			}
			
			%col.client.status = "Combat";
			cancel(%col.client.clearStatus);
			%col.client.clearStatus = %col.client.schedule(3500,clearStatus);
			
			%player = %client;
			%dmg = getDamage(%player,%col);
				
			if(%dmg > 0)
			{
				%dmg = %dmg * %bonus;
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
	
			%slo = %col.client.slo;
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.client.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = %slo.health;
				%col.client.lobkill(%client);
			}
		}
		else
		if(%class $= "AiPlayer" && !%col.isHorse)
		{
			if(%client.buildMode || %col.client.buildMode)
				return false;
				
			%player = %client;
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%dmg = %dmg * %bonus;
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%slo = %col.client.slo;
			
			%slo.tempHealth -= %dmg;
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%slo.tempHealth <= 0)
			{
				%slo.tempHealth = $LOB::Enemy[%col.name,"Health"];
				//%col.kill();
			}	
		}
	}
	if(%col.isHorse)
	{
		if(%col.Wild)
		{
			if(%client.getclassname() $= "aiplayer")
				%player = %client;
			else
				%player = %client.player;
			
			%dmg = getDamage(%player,%col);
			
			if(%dmg > 0)
			{
				%col.setDamageFlash(0.5);
				%col.playPain();
			}
			
			%col.Health -= %dmg;
			
			%exp = mRound(msqrt(%dmg));
			
			%pos = %col.position;
			%pos = getWords(%pos,0,1) SPC getWord(%col.getEyePoint(),2);
			%timer = 2000;
			%so = %col.buildWorldText(%dmg,%pos,%timer);
			%so.animate();
			
			if(%col.health <= 0)
				%col.owner.killHorse();
		}
	}
}

$lob::foodTimer["cookedBeef"] = 5000;
$lob::foodTimer["cookedsteak"] = 8000;
$lob::foodTimer["cookedLobster"] = 12000;

function gameconnection::healFood(%this,%food,%time)
{
	if(%this.foodTimer)
	{
		%m = setKeyWords("\c6You are already healing from " @ %this.currentFood @".",%this.currentFood,"\c6");
		messageClient(%this,'',%m);
		return false;
	}
	%remove = strReplace(strLwr(%food),"cooked","");
	%this.removeFromInventory("cooked " @ %remove,1);
	%this.currentFood = %food;
	%this.foodtimer = 1;
	%this.rejuvenateHealth = true;
	%this.rejuvenateHealth();
	cancel(%this.stopHeal);
	%this.stopHeal = %this.schedule($lob::foodTimer[%food],stopHealFood);
}

function gameConnection::stopHealFood(%this)
{
	%this.foodtimer = "";
	%this.rejuvenateHealth = false;
	cancel(%this.rejuvenateHealth);
	%m = setKeyWords("\c6The " @ %this.currentFood @ " wears off and you stop healing.",%this.currentFood SPC "healing","\c6");
	messageClient(%this,'',%m);
	%this.currentFood = "";
}

function gameConnection::rejuvenateHealth(%this)
{
	cancel(%this.rejuvenateHealthLoop);
	
	if(%this.slo.tempHealth >= %this.slo.health || !%this.rejuvenateHealth || %this.wild && %this.foodTimer $= "")
	{
		%this.rejuvenateHealth = false;
		return true;
	}
	else
	{
		%this.rejuvenateHealth = true;
		%this.slo.tempHealth++;
		%time = getSimTime();
		if(%time - %this.lastEmitterTime > 1500)
		{
			serverplay3d(healsound,%this.player.position);
			%this.player.emote(healImage,1);
			%this.lastEmitterTime = %time;
		}
	}
	%this.rejuvenateHealthLoop = %this.schedule(500,rejuvenateHealth);
}