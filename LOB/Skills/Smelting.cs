$lob::oreNeeded["bronzeBar"] = "Tin Copper";
$lob::oreAmountNeeded["bronzeBar","Tin"] = 5;
$lob::oreAmountNeeded["bronzeBar","Copper"] = 5;
$lob::smeltingLevelNeeded["BronzeBar"] = 0;
$lob::smeltingAnimationTime["BronzeBar"] = 3000;
$lob::chatMessageName["bronzeBar"] = "Bronze Bar";
$lob::smeltingExp["bronzeBar"] = "100";

$lob::oreNeeded["IronBar"] = "Iron";
$lob::oreAmountNeeded["ironBar","iron"] = 10;
$lob::smeltingLevelNeeded["ironBar"] = 10;
$lob::smeltingAnimationTime["ironBar"] = 5000;
$lob::chatMessageName["ironBar"] = "Iron Bar";
$lob::smeltingExp["ironBar"] = "200";

$lob::oreNeeded["SteelBar"] = "Iron Coal";
$lob::oreAmountNeeded["steelBar","coal"] = 5;
$lob::oreAmountNeeded["steelBar","iron"] = 10;
$lob::smeltingLevelNeeded["steelBar"] = 15;
$lob::smeltingAnimationTime["steelBar"] = 8000;
$lob::chatMessageName["steelBar"] = "Steel Bar";
$lob::smeltingExp["steelBar"] = "300";

$lob::oreNeeded["MithrilBar"] = "Mithril Coal";
$lob::oreAmountNeeded["MithrilBar","coal"] = 10;
$lob::oreAmountNeeded["MithrilBar","Mithril"] = 10;
$lob::smeltingLevelNeeded["MithrilBar"] = 35;
$lob::smeltingAnimationTime["MithrilBar"] = 16000;
$lob::chatMessageName["MithrilBar"] = "Mithril Bar";
$lob::smeltingExp["MithrilBar"] = "450";

$lob::oreNeeded["AdamantiteBar"] = "Adamantite Coal";
$lob::oreAmountNeeded["AdamantiteBar","coal"] = 10;
$lob::oreAmountNeeded["AdamantiteBar","Adamantite"] = 20;
$lob::smeltingLevelNeeded["AdamantiteBar"] = 50;
$lob::smeltingAnimationTime["MithrilBar"] = 24000;
$lob::chatMessageName["AdamantiteBar"] = "Adamantite Bar";
$lob::smeltingExp["AdamantiteBar"] = "600";

function serverCmdSmeltBar(%client,%bar)
{
	%slo = %client.slo;
	%metalType = getWord(%bar,0);
	%smash = convertToitemName(%bar);
	
	//check vector
	if(!isObject(%client.furnaceBrick))
		return 0;
	
	if(vectorDist(%client.furnaceBrick.position,%client.player.position) >= 7)
	{
		return 0;
	}
	
	if(%slo.smeltingLevel < $lob::smeltingLevelNeeded[%smash])
	{
		%m = setKeyWords("\c6You need a smelting level of " @ $lob::smeltingLevelNeeded[%smash] @ " to smelt an " @ %bar @".",$lob::smeltingLevelNeeded[%smash] SPC %bar,"\c6");
		smartMessage(%client,%m,2000);
		return 0;
	}
	
	if(%client.isSmelting)
		return 0;
		
	if(getWordCount($lob::oreNeeded[%smash]) == 2)
	{
		%ore1 = getWord($lob::oreNeeded[%smash],0);
		%ore2 = getWord($lob::oreNeeded[%smash],1);
		
		%amt1 = $lob::oreAmountNeeded[%smash,%ore1];
		%amt2 = $lob::oreAmountNeeded[%smash,%ore2];
		//echo(%slo.inventory.itemcount[%ore1 @ "ores"] @ " | " @ %slo.inventory.itemCount[%ore2 @ "ores"]);
		if(%slo.inventory.itemCount[%ore1 @ "ores"] < %amt1)
		{
			%m = setKeyWords("\c6You need atleast " @ %amt1 SPC %ore1 @ " to smelt an " @ %bar @".",%amt1 SPC %ore1 SPC %bar,"\c6");
			smartMessage(%client,%m,2000);
			return 0;
		}
		else
		if(%slo.inventory.itemCount[%ore2 @ "ores"] < %amt2)
		{
			%m = setKeyWords("\c6You need atleast " @ %amt2 SPC %ore2 @ " to smelt an " @ %bar @".",%amt2 SPC %ore2 SPC %bar,"\c6");
			smartMessage(%client,%m,2000);
			return 0;
		}	
		
		%client.isSmelting = true;
		%client.doSmeltingAnimation(%ore1 SPC %ore2,%bar,1);
	}
	else
	{
		%ore = $lob::oreNeeded[%smash];
		%amt = $lob::oreAmountNeeded[%smash,%ore];
		
		if(%slo.inventory.itemCount[%ore @ "ores"] < %amt)
		{
			%m = setKeyWords("\c6You need atleast " @ %amt SPC %ore @ " to smelt an " @ %bar @".",%amt SPC %ore SPC %bar,"\c6");
			smartMessage(%client,%m,2000);
			return 0;
		}
		
		%client.isSmelting = true;
		%client.doSmeltingAnimation(%ore1 SPC %ore2,%bar);
	}
}

function gameConnection::doSmeltingAnimation(%client,%ore,%bar)
{
	%smash = convertToItemName(%bar);
	commandToClient(%client,'closeSmeltingGUI');
	%pos = %client.player.position;
	%client.controlToCamera("Point",%pos);
	
	//%client.player.updateArm(hammerimage);
	//%client.player.mountImage(hammerimage,0);
	
	%animationTime = $lob::smeltingAnimationTime[%smash];
	
	%client.setStatus("Smelting",%animationTime + 1000);
	
	%client.smeltingAnimationLoop();
	%client.player.schedule(%animationTime,playThread,0,"root");
	%client.schedule(%animationTime,stopSmeltingAnimation,%ore,%bar);
}

function gameConnection::smeltingAnimationLoop(%client)
{
	cancel(%client.smeltingAnimationLoop);
	
	%time = getRandom(500,2000);
	%client.player.playThread(0,activate2);
	
	%client.smeltingAnimationLoop = %client.schedule(%time,smeltingAnimationLoop);
}

function gameConnection::stopSmeltingAnimation(%client,%ore,%bar)
{
	cancel(%client.smeltingAnimationLoop);
	%client.endOrbit();
	%slo = %client.slo;
	%metalType = getword(%bar,0);
	%metalType = strReplace(%metalType,"bar","");
	%smash = convertToItemName(%bar);
	//serverCmdUnuseTool(%client);
	//%client.player.unmountImage(0);
	
	if(getWordCount(%ore) >= 2)
	{
		%ore1 = getWord($lob::oreNeeded[%smash],0);
		%ore2 = getWord($lob::oreNeeded[%smash],1);
		
		%amt1 = $lob::oreAmountNeeded[%smash,%ore1];
		%amt2 = $lob::oreAmountNeeded[%smash,%ore2];
		
		if(%slo.inventory.itemCount[%ore1 @ "ores"] < %amt1)
		{
			return 0;
		}
		else
		if(%slo.inventory.itemCount[%ore2 @ "ores"] < %amt2)
		{
			return 0;
		}
		//echo("metaltype = " @ %metaltype);
		commandToClient(%client,'openSmeltingGUI');
		%client.removeFromInventory(%ore1 SPC "ores",$lob::oreAmountNeeded[%smash,%ore1]);
		%client.removeFromInventory(%ore2 SPC "ores",$lob::oreAmountNeeded[%smash,%ore2]);
		%client.addToInventory(%metalType @ " bars", 1);
		%m = setKeyWords("\c6Congratulations, you've smelted an " @ $lob::chatMessageName[%smash] @".",$lob::chatMessageName[%smash],"\c6");
		smartMessage(%client,%m,2000);
		giveExp(%client,"Smelting",$lob::smeltingExp[%smash]);
	}
	else
	{
		%ore = $lob::oreNeeded[%smash];
		%amt = $lob::oreAmountNeeded[%smash,%ore];
		
		if(%slo.inventory.itemCount[%ore @ "ores"] < %amt)
		{
			return 0;
		}	
		
		commandToClient(%client,'openSmeltingGUI');
		%client.removeFromInventory(%ore SPC "ores",$lob::oreAmountNeeded[%smash,%ore]);
		%client.addToInventory(%metalType @ " bars", 1);
		%m = setKeyWords("\c6Congratulations, you've smelted an " @ $lob::chatMessageName[%smash] @".",$lob::chatMessageName[%smash],"\c6");
		smartMessage(%client,%m,2000);
		giveExp(%client,"Smelting",$lob::smeltingExp[%smash]);
	}

	%client.isSmelting = false;
}

function serverCmdPopulateSmeltingList(%client)
{
	%lci=-1;
	%l["items",%lci++] = "Bronze";
	%l["items",%lci++] = "Iron";
	%l["items",%lci++] = "Steel";
	%l["items",%lci++] = "Mithril";
	%l["items",%lci++] = "Adamantite";
	
	for(%i=0;%i<%lci+1;%i++)
		%l1 = trim(%l1 SPC %l["items",%i]);
		
	commandToClient(%client,'populateBarsList',%l1);
	
	%i=-1;while(true)
	{	
		%field = %client.slo.inventory.getTaggedField(%i++);
		%fieldL = strLwr(%field);
		if(%field $= "")
			break;
		if(strStr(%fieldl,"ores") >= 0)
		{
			%count = getField(%field,1);
			if(%count <= 0)
				continue;
			%name = strReplace(%fieldl,"itemCount","");
			%name = getField(%name,0);
			%name = strReplace(strLwr(%name),"ores","");
			%name = strUpr(getSubStr(%name,0,1)) @ getSubStr(%name,1,strLen(%name)-1);
			%name = strReplace(strLwr(%name),"itemcount","");
			%name = trim(%name);
			%finalname = %name @ "_Ores_(" @ %count @ ")";
			%b = trim(%b SPC %finalname);
		}
	}
	
	if(%b $= "")
		%b = -1;
	
	commandToClient(%client,'populateYourOresList',%b);
}

function serverCmdBarRequirements(%client,%bar)
{
	%ores = $lob::oreNeeded[%bar@"bar"];
	%c = getWordCount(%ores);

	if(%c >= 2)
	{
		%ore1 = getWord(%ores,0);
		%ore2 = getWord(%ores,1);
		
		%amt1 = $lob::oreAmountNeeded[%bar@"bar",%ore1];
		%amt2 = $lob::oreAmountNeeded[%bar@"bar",%ore2];
		
		%string = %amt1 @ "_" @ %ore1 SPC %amt2 @ "_" @ %ore2;
	}
	else
	{
		%amt = $lob::oreAmountNeeded[%bar@"bar",%ores];
		%string = %amt @ "_" @ %ores;
	}
	
	//echo(%string);
	
	commandToClient(%client,'barRequirements',%string);
}