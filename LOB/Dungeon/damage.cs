package survival_damage
{
	function projectileData::onCollision(%a,%b,%c,%d,%e)
	{
		%p = parent::onCollision(%a,%b,%c,%d,%e);
		
		//echo(%b.getclassname() SPC %c.getclassname());
		%sourceobject = %b.sourceObject;
		%client = %sourceObject.client;
		if(!isObject(%c))return false;
		if(%c.getClassname() $= %sourceObject.getclassname())
		{
		
		}
		else
		if(%c.getClassName() $= "player" && %sourceObject.getclassname() $= "aiplayer"
		||	%c.getClassName() $= "aiplayer" && %sourceObject.getclassname() $= "player")
		{
			
			if(%c.getClassname() $= "aiplayer")
			{
				if(%b.getDatablock().getName() $= "huntrifle1projectile")
				{
					%dmg = 10000;
				}
				else
					%dmg = 90;
				armor::damage(%c,%c,%sourceObject,%sourceObject.position,%dmg,"");
				%c.setAimObject(%sourceObject);
				%c.doAi();
			}
			else
				armor::damage(%c,%c,%sourceObject,%sourceObject.position,10,"");
		}
		
		if(%c.getClassname() $= "aiplayer")
		{
			if(%c.getDamagePercent() >= 0.7)
			{
				%c.setname("temp");
				%newc = new aiplayer(a : temp)
				{
					isFakePlayer = true;
				};
				
				%c.delete();
				%newc.kill();
					//talk("aaa");
				if(isObject(%client.player))
				{
					%client.tdm_killStreak++;
					//%killerClient.tdm_gold += 20;
					
					if(%client.tdm_killstreak == 3)
					{
						messageAll('',"\c3" @ %client.name @ "'s " @ %client.tdm_killstreak @ " kill-streak has earned him an extra grenade!");
						%client.tdm_extraNade = true;
						serverplay2d("beep_siren_sound");
						%client.addtodefaultinventory("hegrenadeitem");
					}
					else
					if(%Client.tdm_killstreak == 6)
					{
						messageAll('',"\c3" @ %client.name @ "'s " @ %client.tdm_killstreak @ " kill-streak has earned him HASTE!");
						//%client.tdm_UAV = true;
						elm_pu_onitempickup(%client,"override haste");
						serverplay2d("beep_siren_sound");	
						%client.tdm_uav();
					}
					
					if(%client.tdm_killStreak > 1)
						%grammar = "s";
					else
						%grammar = "";
				}
			}
		}
		
		return %p;
	}
	
	function elm_pu_onItemPickup(%client,%item)
	{
		%override = getWord(%item,0);
		if(%override $= "override")
		{
			%item = getWord(%item,1);
			if(%item $= "haste")
			{
				%client.player.changeDatablock(playerhaste);
				commandtoclient(%client,'centerprint',$elm::pu::hasteMessage,7);
			}
		}
		else
		return elm_pu_onItemPickup(%client,%item);
	}
	
	function minigamecandamage(%a,%b)
	{
		if(%b.getclassname() $= "aiplayer")
			return true;
		return parent::minigamecandamage(%a,%b);
		//echo("mini " @ %a.getclassname() SPC %b.getclassname());
	}
};

activatePackage(survival_Damage);
