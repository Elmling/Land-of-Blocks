function giveExp(%client,%type,%amount)
{
	if(%type $= "Climbing")
	{
		%en = $lob::expNeeded[%type,%client.slo.ClimbingLevel];
		if(%en $= "")
			return false;

		%client.slo.ClimbingExp += %amount;
		
		if(%client.slo.ClimbingExp >= %en)
		{
			%leftOver = %client.slo.ClimbingExp - %en;
			%client.slo.ClimbingLevel++;
			%client.slo.ClimbingExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.ClimbingLevel@".",%type SPC %client.slo.ClimbingLevel@".","\c6");
			messageClient(%client,'',%m);		
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "Magic")
	{
		%en = $lob::expNeeded[%type,%client.slo.magicLevel];
		if(%en $= "")
			return false;
		//make leveling a bit easier
		%amount = %amount * 4;
		%client.slo.magicExp += %amount;
		
		if(%client.slo.magicExp >= %en)
		{
			%leftOver = %client.slo.magicExp - %en;
			%client.slo.magicLevel++;
			%client.slo.magicExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.magicLevel@".",%type SPC %client.slo.magicLevel@".","\c6");
			messageClient(%client,'',%m);		
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "cooking")
	{
		%en = $lob::expNeeded[%type,%client.slo.cookingLevel];
		if(%en $= "")
			return false;
		%client.slo.cookingExp += %amount;
		
		if(%client.slo.cookingExp >= %en)
		{
			%leftOver = %client.slo.cookingExp - %en;
			%client.slo.cookingLevel++;
			%client.slo.cookingExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.cookingLevel@".",%type SPC %client.slo.cookingLevel@".","\c6");
			messageClient(%client,'',%m);		
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "Smelting")
	{
		%en = $lob::expNeeded[%type,%client.slo.smeltingLevel];
		if(%en $= "")
			return false;
		%client.slo.smeltingExp += %amount;
		
		if(%client.slo.smeltingExp >= %en)
		{
			%leftOver = %client.slo.smeltingExp - %en;
			%client.slo.smeltingLevel++;
			%client.slo.smeltingExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.smeltingLevel@".",%type SPC %client.slo.smeltingLevel@".","\c6");
			messageClient(%client,'',%m);		
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "Smithing")
	{
		%en = $lob::expNeeded[%type,%client.slo.smithingLevel];
		if(%en $= "")
			return false;
		%client.slo.smithingExp += %amount;
		
		if(%client.slo.smithingExp >= %en)
		{
			%leftOver = %client.slo.smithingExp - %en;
			%client.slo.smithingLevel++;
			%client.slo.smithingExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.smithingLevel@".",%type SPC %client.slo.smithingLevel@".","\c6");
			messageClient(%client,'',%m);		
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "firemaking")
	{
		%en = $lob::expNeeded[%type,%client.slo.firemakingLevel];
		if(%en $= "")
			return false;
		%client.slo.firemakingExp += %amount;
		
		if(%client.slo.fireMakingExp >= %en)
		{
			%leftOver = %client.slo.fireMakingExp - %en;
			%client.slo.fireMakingLevel++;
			%client.slo.fireMakingExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.fireMakingLevel@".",%type SPC %client.slo.fireMakingLevel@".","\c6");
			messageClient(%client,'',%m);
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "Mining")
	{
		%en = $LOB::expNeeded[%type,%client.slo.miningLevel];
		if(%en $= "")
			return false;
		%client.slo.miningExp += %amount;
		
		if(%client.slo.miningExp >= %en)
		{
			%leftOver = %client.slo.miningExp - %en;
			
			%client.slo.miningLevel++;
			%client.slo.miningExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.miningLevel@".",%type SPC %client.slo.miningLevel@".","\c6");
			messageClient(%client,'',%m);
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "Woodcutting")
	{
		%en = $LOB::expNeeded[%type,%client.slo.woodCuttingLevel];
		if(%en $= "")
			return false;
		%client.slo.woodCuttingExp += %amount;
		
		if(%client.slo.woodCuttingExp >= %en)
		{
			%leftOver = %client.slo.woodCuttingExp - %en;
			
			%client.slo.woodCuttingLevel++;
			%client.slo.woodCuttingExp = %leftOver;
			
			%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.woodCuttingLevel@".",%type SPC %client.slo.woodCuttingLevel@".","\c6");
			messageClient(%client,'',%m);
			serverplay3d(levelupsound,%client.player.position);
		}
	}
	else
	if(%type $= "Combat")
	{
		%en = $LOB::ExpNeeded[%type,%client.slo.combatLevel];
		if(%en $= "")
			return false;
		%client.slo.combatExp += %amount;
		
		if(%client.slo.combatExp >= %en)
		{
			if(%client.slo.combatLevel $= "10")
			{
				%m = setKeyWords("\c6Congrats on level 11. You will now take and give damage when in a PVP area!","congrats 10. take give damage pvp","\c6");
				messageClient(%client,'',%m);
				%nomsg = true;
			}
			
			%leftOver = %client.slo.combatExp - %en;
			
			%client.slo.combatLevel++;
			%client.slo.combatExp = %leftOver;
			
			if(%nomsg $= "")
				%m = setKeyWords("\c6Congratulations, you've leveled your " @ %type @ " to " @ %client.slo.combatLevel@".",%type SPC %client.slo.combatLevel@".","\c6");
			messageClient(%client,'',%m);
			
			%client.slo.health+=3;
			%client.slo.tempHealth = %client.slo.health;
			serverplay3d(levelupsound,%client.player.position);
		}
	}
}