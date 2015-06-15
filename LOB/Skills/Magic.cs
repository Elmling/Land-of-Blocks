$lob::weaponDamage["lob_fire throw"] = 5;
$lob::isMagicItem["lob_fire throw"] = 1;

$lob::weaponDamage["Fire Ball"] = 5;
$lob::isMagicItem["Fire Ball"] = 1;
$lob::magicMaterialRequired["Fire ball"] = 1;


$lob::weaponDamage["wind wall"] = 10;
$lob::isMagicItem["wind wall"] = 1;
$lob::magicMaterialRequired["Wind Wall"] = 2;

function buildEmitter(%this)
{
	if(!isObject(%this))
		return;
		
	if(isObject(%this.emitter))%this.emitter.delete();
		
	%emitter = new particleEmitterNode()
	{
		dataBlock = "GenericEmitterNode";
		emitter = "hammerSparkEmitter";
		position = %this.player.getEyeTransform();
		spherePlacement = 0;
		velocity = 0;
		//scale = "0.02 0.02 0.02";
		scale = "3 3 3";
		user = %this;
	};
	
	%this.emitter = %emitter;
	
	return %emitter;
}

function gameConnection::playerEmitter(%this)
{
	cancel(%this.playerEmitterLoop);
	%player = %this.player;
	
	if(!isObject(%player))
		return false;
		
	%emitter = buildEmitter(%this);
	
	%emitter.schedule(20,delete);
	
	%this.playerEmitterLoop = %this.schedule(10,playerEmitter);
	
}

//FIREBALL SKILL

function gameConnection::doFireBallSpecial(%this)
{
	if(%this.slo.inventory.itemCountMagicMaterial < $lob::magicMaterialRequired["Fire Ball"])
	{
		messageClient(%this,'',"\c6I need more Magic Material.");
		return false;
	}
	else
		%this.slo.inventory.itemCountMagicMaterial-=$lob::magicMaterialRequired["Fire Ball"];
		
	%player = %this.player;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	%Range = 1000;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%rangeEnd,$TypeMasks::all, %player);
	
	if(%raycast !$= "0")
	{
		%pos = getWords(%raycast,1,3);
		
		for(%i=0;%i<20;%i++)
		{
			%this.schedule(getRandom(40,500),dropFireBallto,%pos);
		}
	}
}

function gameConnection::dropFireBallto(%this,%pos)
{
	%player = %this.player;
	%x = getRandom(-5,5);
	%y = getRandom(-5,5);
	%z = 60;
	
	%z = %z + getRandom(0,15);
	%add = vectorAdd(%pos, %x SPC %y SPC %z);

	%p = new projectile()
	{
		dataBlock = fireBallProjectile;
		//dataBlock = lob_firethrowprojectile;
		initialVelocity = "0 0 -80";
		initialPosition = %add;
		sourceObject = %player;
		client = %this;
		scale = "1 1 1";
		isSpecial = true;
	};

}

function fireBallOnHit(%client,%type,%col)
{
	%class = %col.getClassName();

	if(%client.getClassName() $= "GameConnection")
	{
		if(%client.slo.magicLevel $= "")
		{
			%client.slo.magicExp=0;
			%client.slo.magicLevel = 1;
		}
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
				
				giveExp(%client,"magic",%exp);
				
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
				
				giveExp(%client,"magic",%exp);
				
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
			
			%col.client.status = "Magic";
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

//FIREBALL SKILL END

//WIND WALL

function gameConnection::windWall(%this)
{
	if(%this.slo.magicLevel $= "")
	{
		%this.slo.magicLevel=1;
		%this.slo.magicExp=1;
	}
	if(%this.slo.inventory.itemCountMagicMaterial < $lob::magicMaterialRequired["Wind Wall"])
	{
		messageClient(%this,'',"\c6I need more Magic Material.");
		return false;
	}
	else
		%this.slo.inventory.itemCountMagicMaterial-=$lob::magicMaterialRequired["Wind Wall"];
	%player = %this.player;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	%Range = 1000;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%rangeEnd,$TypeMasks::all, %player);
	
	%o = getWord(%raycast,0);

	if(isObject(%o))
	{
		%cn = %o.getclassname();
		
		if(%cn $= "fxDtsBrick" || %cn $= "player" || %cn $= "aiPlayer" || %cn $= "fxPlane")
		{
			%position = getWords(%raycast,1,3);
			%position = vectorAdd(%position,"0 0 2");
			
			%windWall = new scriptObject()
			{
				made = true;
			};
			//echo("windwall = " @ %windwall);
			%this.windWall = %windwall;
			
			%emitter = new particleEmitterNode()
			{
				dataBlock = "GenericEmitterNode";
				emitter = "arrowstickexplosionemitter";
				position = %position;
				spherePlacement = 0;
				velocity = 1;
				scale = "5 5 10";
				user = %this;
			};
			
			%emitter2 = new particleEmitterNode()
			{
				dataBlock = "GenericEmitterNode";
				emitter = "llightrayemitter";
				position = %position;
				spherePlacement = 0;
				velocity = 1;
				scale = "5 5 10";
				user = %this;
			};
			
			%emitter3 = new particleEmitterNode()
			{
				dataBlock = "GenericEmitterNode";
				emitter = "hexawhitelaserUpemitter";
				position = %position;
				spherePlacement = 0;
				velocity = 1;
				scale = "5 5 10";
				user = %this;
			};
			
			%windWall.emitter1 = %emitter;
			%windWall.emitter2 = %emitter2;
			%windWall.emitter3 = %emitter3;
			
			//$le = %emitter;
			//$le2 = %emitter2;
			//$le3 = %emitter3;
			
			%emitter.schedule(5000,delete);
			%emitter2.schedule(5000,delete);
			%emitter3.schedule(5000,delete);
			%this.windWall = %windWall;
			%this.windWall.position = %position;
			%this.windWallRadiusDmg();
		}
	}
}

function gameConnection::windWallRadiusDmg(%this)
{
	if(!isObject(%this.windWall.emitter1))
	{
		%this.windWall.delete();
		return;
	}
	cancel(%this.windWallLoop);
	
	initContainerBoxSearch(%this.windWall.position,"7 7 7",$typemasks::all);
	while(%target = containerSearchNext())
	{
		//echo("wind wall " @ %target.getclassname());
		if(%target.getClassname() $= "player" || %target.getclassName() $= "aiPlayer")
		{
			if(%target.getState() !$= "dead" && %this.windWall.emitter1.user !$= %target.client)
				windWallOnHit(%this,"wind",%target);
		}
		
	}
	
	%this.windWallLoop = %this.schedule(500,windWallRadiusDmg);
}

function windWallOnHit(%client,%type,%col)
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
				
				giveExp(%client,"magic",%exp);
				
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
				
				giveExp(%client,"magic",%exp);
				
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
			
			%col.client.status = "Magic";
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

//WIND WALL END

//FROSTBITE
function player::buildFrostBite(%this)
{
	%player = %this;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	%Range = 1000;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%rangeEnd,$TypeMasks::all, %player);
	
	%o = getWord(%raycast,0);
	
	if(isObject(%o))
	{
			%position = getWords(%raycast,1,3);
			%emitter = new particleEmitterNode()
			{
				dataBlock = "GenericEmitterNode";
				emitter = "rainsplashwallemitter";
				position = %position;
				spherePlacement = 0;
				velocity = 1;
				scale = "8 8 1";
				user = %this;
				useEmitterColors=1;
			};
			
			%emitter.setColor("0 0 1 1");
			
			
			%emitter.schedule(3000,delete);
			
			%emitter2 = new particleEmitterNode()
			{
				dataBlock = "GenericEmitterNode";
				emitter = "normcloudbeffectemitter";
				position = %position;
				spherePlacement = 0;
				velocity = 5;
				scale = "8 8 1";
				user = %this;
				useEmitterColors=1;
			};
			
			%emitter2.setColor("0 0.1 1 1");
			%emitter2.schedule(3000,delete);
			
	}
}

//FROSTBITE END

package lob_magic
{
	function fireBallProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal) 
	{
		%p = parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
		
		if(%this.getName() $= "fireBallProjectile")
		{
			%client = %obj.sourceObject.client;
			if(%client.slo.magicLevel $= "")
			{
				%client.slo.magicLevel = 0;
				%client.slo.magicExp = 0;
			}
			//echo(%client.name);
			ServerPlay3D(recurvebowfirefiresound,%pos);
			
			initContainerBoxSearch(%pos,"1.5 1.5 1.5",$typemasks::all);
			while(%target = containerSearchNext())
			{
				//echo(%target.getclassname());
				if(%target.getClassname() $= "player" || %target.getclassName() $= "aiPlayer")
				{
					if(%client.player $= %col || %target.getState() $= "dead")
						return false;
					fireBallOnHit(%client,"fire",%target);
				}
			}
		}
	}

	function weaponImage::onFire(%this,%obj,%a)
	{
		if(%this.getname() $= "rightFireBallImage" && %obj.client.slo.inventory.itemCountMagicMaterial < $lob::magicMaterialRequired["Fire Ball"])
		{
			messageClient(%obj.client,'',"\c6I need more Magic Material.");
			return false;
		}
		else
		if(%this.getname() $= "rightFireBallImage")
		{
			%obj.client.slo.inventory.itemCountMagicMaterial-=$lob::magicMaterialRequired["Fire Ball"];
		}
		%p = parent::onFire(%this,%obj,%a);
		
		if(%this.getname() $= "rightFireBallImage")// || %this.getName() $= "lob_rightFireThrowImage")
		{
			//echo("ojb name " @ %obj.getclassname());
			//%obj.playthread(3,root);
			//%obj.schedule(500,playThread,3,spearready);
			ServerPlay3D(recurvebowfirefiresound,%obj.position);
		}
			
		return %p;
	}
};
activatePackage(lob_magic);