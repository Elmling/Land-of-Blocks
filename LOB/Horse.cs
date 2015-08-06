package lob_horseSupport
{
	function gameConnection::onDrop(%client)
	{
		if(isObject(%client.horse))
			%client.pickupHorse();
			
		return parent::onDrop(%client);
	}
	
	function armor::onMount(%this,%obj,%col,%a,%b)
	{
		
		if(isObject(%col))
			if(%col.getDataBlock().getName() $= "AdvancedHorseArmor")
			{
				if(%obj.getClassName() $= "aiPlayer")
				{
					%obj.schedule(10,dismount,1);
					return parent::onMount(%this,%obj,%col,%a,%b);
				}
					
				if(%col.owner $= "")
				{
					//this isn't a LOB affiliated horse
					%obj.schedule(10,dismount,1);
					smartMessage(%obj.client,"\c6The horse knocks you off.",1000,"centerprint");
					%obj.playPain(1);
					return parent::onMount(%this,%obj,%col,%a,%b);
				}
				else if(isObject(%obj) && %obj.getClassname() $= "player")
				{
					if(isObject(%col.owner))
					{
						if(%col.owner $= %obj.client)
						{
							%col.lastMount = %obj.client;
						}
						else
						if(%col.lastMount !$= %obj.client)
						{
							commandtoclient(%obj.client,'messageboxok',"This isn't your horse.","To steal a horse, click the horse. If you choose to steal it, you'll become instantly wanted!");
							%obj.schedule(10,dismount,1);
							return parent::onMount(%this,%obj,%col,%a,%b);
							%obj.client.slo.pkPoints += 10;
							%m = setKeyWords("\c6"@ %obj.client.name @ " has stolen " @ %col.owner.name @ "\'s Horse.",%obj.client.name SPC %col.owner.name, "\c6");
							messageAll('',%m);
							%col.lastMount = %obj.client;
						}
					}
				}
			}
			
		return parent::onMount(%this,%obj,%col,%a,%b);
	}
	
	function Armor::onUnMount(%this,%player,%object,%a,%b,%c)
	{
		if(!isObject(%object))
			return false;
		if(%object.getDatablock().getName() $= "AdvancedHorseArmor")
			if(getWord(%object.getVelocity(),2) $= "0")
			{
				%position = %player.position;
				InitContainerRadiusSearch(%position,1,$TypeMasks::FxBrickAlwaysObjectType);
				while((%targetObject=containerSearchNext()) !$= 0)
				{
					%m = setKeyWords("\c6You can't dismount near this brick.","noble steed diet combat","\c6");
					smartMessage(%player.client,%m,5000);
					%object.schedule(10,mountObject,%player,2);
					%player.schedule(10,setControlObject,%object);
					return false;
				}
				parent::onUnMount(%this,%player,%object,%a,%b,%c);
			}
			else
			{
				%object.schedule(10,mountObject,%player,2);
				%player.schedule(10,setControlObject,%object);
				//%player.schedule(10,mountObject,%object);
				//talk("remount");
			}
		//if(isObject(%object))
		//	if(%object.getDatablock().getName() $= "AdvancedHorseArmor")
		//		%player.schedule(10,setVelocity,"0 0 -30");
	}
};
activatePackage(lob_horseSupport);

function gameConnection::newHorse(%this)
{
	//deprecated, using the item class..
}

function gameConnection::deleteHorse(%this)
{
	//deprecated, using the item class..
}

function gameConnection::spawnHorse(%this)
{
	if(isObject(%this.horse) || %this.slo.inventory.itemCountHorse <= 0)
		return false;
		
	if(%this.slo.horseHealth $= "" || %this.slo.horseHealth <= 0)
		%this.slo.horseHealth = 500;
		
	%this.horse = new aiPlayer()
	{
		datablock = advancedHorseArmor;
		scale = "1 1 1";
		owner = %this;
		health = %this.slo.horseHealth;
		position = %this.player.position;
		isHorse = true;
		level = 1;
		name = "Horse";
	};
}

function gameConnection::pickupHorse(%this)
{
	%horse = %this.horse;
	%this.slo.horsehealth = %this.horse.health;
	%horse.delete();
}

function gameConnection::killHorse(%this)
{
	%this.horse.owner = "";
	%horse = %this.horse;
	%this.horse = "";
	%this.slo.horseHealth = "";
	
	%horse.kill();
	
	%m = setKeyWords("\c6Your noble steed has died in combat.","noble steed diet combat","\c6");
	messageClient(%this,'',%m);
	%this.removeFromInventory("Horse",1);
}