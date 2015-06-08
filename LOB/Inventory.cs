
package Inventory
{
	function shapeBase::pickUp(%player,%item)
	{
		if(%item.isSpecialAttack)
		{
			%client = %player.client;

			if(strStr(strLwr(%item.getDataBlock().getName()),"shortsworditem") >= 0)
			{
				%type = strReplace(strLwr(%item.getDataBlock().getName()),"shortsworditem","");

				if(%item.client !$= %client)
				{
					shortSwordSpecialOnHit(%item.client,%type,%player);
				}
			}
			return true;
		}
		
		if(%player.getClassName() $= "aiPlayer" || %player.getState() $= "dead")
			return false;
		
		if(%item.isLOBItem)
		{
			%client = %player.client;

			if(%item.specifiedClient !$= "")
				if(%client !$= %item.specifiedClient)
					return 0;
					
			if(%item.name $= "GrandFather's Sword")
			{
				%message = setKeyWords("\c6You've found Robin's Grandfather's Sword. Report back to him.","Grandfather's Sword","\c6");
				messageClient(%client,'',%message);	
				%client.slo.robin_q1_hassword = true;
				%item.delete();
				return 0;
			}
			else
			{
				if(%item.name $= "gold")
					serverPlay3d(coinssound,%client.player.position);
				%message = setKeyWords("\c6+" @ %item.amount SPC %item.name,"itemName","\c6");
				messageClient(%client,'',%message);	
				%client.addToInventory(convertToItemName(%item.name),%item.amount);
			}
			
			%item.delete();
		}
		else
			parent::pickup(%player,%item);
	}
};

function gameConnection::addToDefaultInventory(%this,%item)
{
	if(!isObject(%item))
		return 0;
		
	%player = %this.player;
	%count = %player.dataBlock.maxTools;
	%freeSlot = -1;
	
	for(%i=0; %i<%count; %i++)
	{
		if(!%player.tool[%i] && %freeslot $= "-1")
		{
			%freeSlot = %i;
		}
		if(%player.tool[%i] !$= "-1" && %player.tool[%i].uiName $= %item.uiName)
			return;
	}
	if(%freeSlot !$= -1)
	{
		if(%player.tool[%freeslot].uiName $= %item.uiName)
			return 0;
		%player.tool[%freeSlot] = nameToId(%item);
		messageClient(%player.client, 'MsgItemPickup', '' , %freeslot, nameToId(%item));
	}
	else
	{
		//echo("no free slot");
	
	}
}
//Quests
$lob::itemDatablock["Grandfather'sSword"] = bronzeShortSwordItem;
$lob::itemCorrectName["Grandfather'sSword"] = "Grandfather's Sword";

//armor
$lob::itemDatablock["bronzeFullHelmet"] = bronzeFullHelmetItem;
$lob::itemCorrectName["Bronzefullhelmet"] = "Bronze Full Helmet";
$lob::itemDatablock["ironFullHelmet"] = ironFullHelmetItem;
$lob::itemCorrectName["ironfullhelmet"] = "Iron Full Helmet";
$lob::itemDatablock["steelFullHelmet"] = steelFullHelmetItem;
$lob::itemCorrectName["steelfullhelmet"] = "Steel Full Helmet";
$lob::itemDatablock["mithrilFullHelmet"] = mithrilFullHelmetItem;
$lob::itemCorrectName["mithrilfullhelmet"] = "Mithril Full Helmet";
$lob::itemDatablock["adamantiteFullHelmet"] = adamantiteFullHelmetItem;
$lob::itemCorrectName["adamantitefullhelmet"] = "Adamantite Full Helmet";

//weapons
$lob::itemDatablock["lob_fireThrow"] = lob_firethrowItem;
$lob::itemCorrectName["lob_fireThrow"] = "lob_Fire Throw";

$lob::itemDatablock["fireBall"] = fireBallItem;
$lob::itemCorrectName["fireBall"] = "Fire Ball";
$lob::itemDatablock["windWall"] = windWallItem;
$lob::itemCorrectName["windWall"] = "Wind Wall";

$lob::itemDatablock["bronzeshortsword"] = bronzeShortSwordItem;
$lob::itemCorrectName["bronzeShortSword"] = "Bronze Shortsword";
$lob::itemDatablock["ironShortSword"] = ironShortSwordItem;
$lob::itemCorrectName["IronShortSword"] = "Iron Shortsword";
$lob::itemDatablock["steelShortSword"] = steelShortSwordItem;
$lob::itemCorrectName["steelShortSword"] = "Steel Shortsword";
$lob::itemDatablock["mithrilShortSword"] = mithrilShortSwordItem;
$lob::itemCorrectName["mithrilShortSword"] = "Mithril Shortsword";
$lob::itemDatablock["adamantiteShortSword"] = adamantiteShortSwordItem;
$lob::itemCorrectName["adamantiteShortSword"] = "Adamantite Shortsword";

$lob::itemDatablock["bronzeBow"] = bronzeBowItem;
$lob::itemCorrectName["bronzeBow"] = "Bronze Bow";
$lob::itemDatablock["ironBow"] = ironBowItem;
$lob::itemCorrectName["IronBow"] = "Iron Bow";
$lob::itemDatablock["steelBow"] = steelBowItem;
$lob::itemCorrectName["steelBow"] = "Steel Bow";
$lob::itemDatablock["mithrilBow"] = mithrilBowItem;
$lob::itemCorrectName["mithrilBow"] = "Mithril Bow";
$lob::itemDatablock["adamantiteBow"] = adamantiteBowItem;
$lob::itemCorrectName["adamantiteBow"] = "Adamantite Bow";

$lob::itemDatablock["bronzeJavlin"] = bronzeJavlinItem;
$lob::itemCorrectName["bronzeJavlin"] = "Bronze Javlin";
$lob::itemDatablock["ironJavlin"] = ironJavlinItem;
$lob::itemCorrectName["IronJavlin"] = "Iron Javlin";
$lob::itemDatablock["steelJavlin"] = steelJavlinItem;
$lob::itemCorrectName["steelJavlin"] = "Steel Javlin";
$lob::itemDatablock["mithrilJavlin"] = mithrilJavlinItem;
$lob::itemCorrectName["mithrilJavlin"] = "Mithril Javlin";
$lob::itemDatablock["adamantiteJavlin"] = adamantiteJavlinItem;
$lob::itemCorrectName["adamantiteJavlin"] = "Adamantite Javlin";

//tools
$lob::itemDatablock["bronzePickAxe"] = bronzePickAxeItem;
$lob::itemCorrectName["BronzePickAxe"] = "Bronze Pickaxe";
$lob::itemDatablock["ironPickAxe"] = ironPickAxeItem;
$lob::itemCorrectName["ironPickAxe"] = "Iron Pickaxe";
$lob::itemDatablock["steelPickaxe"] = steelPickaxeItem;
$lob::itemCorrectName["steelPickaxe"] = "Steel Pickaxe";
$lob::itemdatablock["MithrilPickaxe"] = mithrilPickaxeItem;
$lob::itemCorrectName["mithrilPickaxe"] = "Mithril Pickaxe";
$lob::itemCorrectName["adamantitePickaxe"] = "Adamantite Pickaxe";
$lob::itemdatablock["adamantitePickaxe"] = adamantitePickaxeItem;

$lob::itemDatablock["bronzeAxe"] = bronzeAxeItem;
$lob::itemCorrectName["BronzeAxe"] = "Bronze Axe";
$lob::itemDatablock["ironAxe"] = ironAxeItem;
$lob::itemCorrectName["IronAxe"] = "Iron Axe";
$lob::itemDatablock["steelAxe"] = steelAxeItem;
$lob::itemCorrectName["SteelAxe"] = "Steel Axe";
$lob::itemDatablock["mithrilAxe"] = mithrilAxeItem;
$lob::itemCorrectName["mithrilAxe"] = "Mithril Axe";
$lob::itemDatablock["adamantiteAxe"] = adamantiteAxeItem;
$lob::itemCorrectName["adamantiteAxe"] = "Adamantite Axe";

//items
$lob::itemDatablock["MagicMaterial"] = "scrollItem";
$lob::itemCorrectName["MagicMaterial"] = "Magic Material";
$lob::itemDatablock["pineLogs"] = wood;
$lob::itemCorrectName["PineLogs"] = "Pine Logs";
$lob::itemDatablock["oakLogs"] = wood;
$lob::itemCorrectName["oakLogs"] = "Oak Logs";
$lob::itemDatablock["mapleLogs"] = wood;
$lob::itemCorrectName["mapleLogs"] = "Maple Logs";
$lob::itemDatablock["willowLogs"] = wood;
$lob::itemCorrectName["willowLogs"] = "Willow Logs";

$lob::itemDatablock["tinOres"] = tinOreItem;
$lob::itemCorrectName["tinOres"] = "Tin Ores";
$lob::itemDatablock["ironOres"] = ironOreItem;
$lob::itemCorrectName["ironOres"] = "Iron Ores";
$lob::itemDatablock["copperOres"] = copperOreItem;
$lob::itemCorrectName["copperOres"] = "Copper Ores";
$lob::itemDatablock["coalOres"] = coalOreItem;
$lob::itemCorrectName["coalOres"] = "Coal Ores";
$lob::itemDatablock["mithrilOres"] = mithrilOreItem;
$lob::itemCorrectName["mithrilOres"] = "Mithril Ores";
$lob::itemDatablock["adamantiteOres"] = adamantiteOreItem;
$lob::itemCorrectName["adamantiteOres"] = "Adamantite Ores";

$lob::itemDatablock["gold"] = goldItem;
$lob::itemCorrectName["gold"] = "Gold";

$lob::itemDatablock["bronzeBars"] = bronzeBarItem;
$lob::itemCorrectName["bronzeBars"] = "Bronze Bars";
$lob::itemDatablock["ironBars"] = ironBarItem;
$lob::itemCorrectName["ironBars"] = "Iron Bars";
$lob::itemDatablock["steelBars"] = steelBarItem;
$lob::itemCorrectName["steelBars"] = "Steel Bars";
$lob::itemDatablock["mithrilBars"] = mithrilBarItem;
$lob::itemCorrectName["mithrilBars"] = "Mithril Bars";
$lob::itemDatablock["AdamantiteBars"] = adamantiteBarItem;
$lob::itemCorrectName["AdamantiteBars"] = "Adamantite Bars";

$lob::itemDatablock["OrduniaScroll"] = scrollItem;
$lob::itemCorrectName["OrduniaScroll"] = "Ordunia Scroll";

$lob::itemDatablock["WhiteStoneScroll"] = scrollItem;
$lob::itemCorrectName["WhiteStoneScroll"] = "Whitestone Scroll";

$lob::itemDatablock["EldriaScroll"] = scrollItem;
$lob::itemCorrectName["EldriaScroll"] = "Eldria Scroll";

$lob::itemDatablock["AlyswellScroll"] = scrollItem;
$lob::itemCorrectName["AlyswellScroll"] = "Alyswell Scroll";

$lob::itemDatablock["InterpassScroll"] = scrollItem;
$lob::itemCorrectName["InterpassScroll"] = "Interpass Scroll";

//food
$lob::itemDatablock["rawBeef"] = uncookedBeefItem;
$lob::itemCorrectName["Rawbeef"] = "Raw Beef";
$lob::itemDatablock["CookedBeef"] = cookedBeefItem;
$lob::itemCorrectName["CookedBeef"] = "Cooked Beef";

$lob::itemDatablock["rawSteak"] = uncookedSteakItem;
$lob::itemCorrectName["RawSteak"] = "Raw Steak";
$lob::itemDatablock["CookedSteak"] = cookedSteakItem;
$lob::itemCorrectName["CookedSteak"] = "Cooked Steak";

$lob::itemDatablock["rawLobster"] = uncookedLobsterItem;
$lob::itemCorrectName["RawLobster"] = "Raw Lobster";
$lob::itemDatablock["CookedLobster"] = cookedLobsterItem;
$lob::itemCorrectName["CookedLobster"] = "Cooked Lobster";

//misc
$lob::itemDatablock["Horse"] = advancedHorseArmor;
$lob::itemCorrectName["Horse"] = "Horse";


function gameConnection::addToInventory(%this,%item,%amount)
{
	if(%item $= "")
		return false;
		
	%amount = mFloor(%amount);
	
	if(%amount <= 0)
		return false;
		
	%item = convertToItemName(%item);
		
	if(%this.slo.inventory.itemCount[%item] $= "" || mFloor(%this.slo.inventory.itemCount[%item]) <= 0)
	{
		%this.slo.inventory.ItemCount++;
		//%this.slo.itemName[%this.slo.itemCount] = %item;
	}
		
	%this.slo.inventory.itemCount[%item] += %amount;
	
	return true;
}

function gameConnection::removeFromInventory(%this,%item,%amount)
{
	if(%item $= "")
		return false;
		
	%item = convertToItemName(%item);
	
	if(%this.slo.inventory.itemCount[%item] $= "")
	{
		%this.slo.inventory.itemCount = 0;
		return false;
	}
	
	if(%amount >= %this.slo.inventory.itemCount[%item])
	{
		%this.slo.inventory.itemCount[%item] = 0;
		%this.slo.inventory.itemCount--;
	}
	else
		%this.slo.inventory.itemCount[%item] = %this.slo.inventory.itemCount[%item] - %amount;
	
	return true;
	
}

function convertToItemName(%string)
{
	return strReplace(%string," ","");
}

function gameConnection::addToBank(%this,%item,%amount)
{
	if(%item $= "")
		return false;
		
	%amount = mFloor(%amount);
	
	if(%amount <= 0)
		return false;
		
	%item = convertToItemName(%item);
		
	if(%this.slo.bank.itemCount[%item] $= "" || mFloor(%this.slo.bank.itemCount[%item]) <= 0)
	{
		%this.slo.bank.ItemCount++;
		//%this.slo.bank.itemName[%this.slo.itemCount] = %item;
	}
		
	%this.slo.bank.itemCount[%item] += %amount;
	
	return true;
}

function gameConnection::removeFromBank(%this,%item,%amount)
{
	if(%item $= "")
		return false;
		
	%item = convertToItemName(%item);
	
	if(%this.slo.bank.itemCount[%item] $= "")
	{
		%this.slo.bank.itemCount = 0;
		return false;
	}
	
	if(%amount >= %this.slo.bank.itemCount[%item])
	{
		%this.slo.bank.itemCount[%item] = 0;
		//%this.slo.bank.itemCount--;
	}
	else
		%this.slo.bank.itemCount[%item] -= %amount;
	
	return true;
	
}

function serverCmdBankDeposit(%client,%item,%amount)
{
	if(vectorDist(%client.player.position,%client.bankTeller.position) >= 10)
	{
		%m = setKeyWords("\c6You aren't near a banker","banker","\c6");
		smartMessage(%client,%m,100);
		return 0;
	}

	%amount = mfloor(%amount);
	
	if(%amount <= 0)
	{
		return 0;
	}
	
	%inventory = %client.slo.inventory;
	%squish = convertToItemName(%item);
	%name = $lob::itemCorrectName[%squish];
	
	if(%name $= "")
		return false;
		
	//echo(%item @ " | " @ %amount @ " | " @ %inventory.itemCount[convertToItemName(%item)]);
		
	if(%amount > %inventory.itemCount[%squish])
	{
		%amount = %inventory.itemCount[%squish];
	}
	
	%client.removeFromInventory(%item,%amount);
	%client.addToBank(%item,%amount);
	commandToClient(%client,'openBank');
}

function serverCmdBankwithdrawal(%client,%item,%amount)
{
	if(vectorDist(%client.player.position,%client.bankTeller.position) >= 10)
	{
		%m = setKeyWords("\c6You aren't near a banker","banker","\c6");
		smartMessage(%client,%m,100);
		return 0;
	}
		
	%amount = mFloor(%amount);
		
	if(%amount <= 0)
		return 0;
		
	%bank = %client.slo.bank;
	%amount = mfloor(%amount);
	%itemSquish = convertToItemName(%item);
		
	//echo(%client.name @ " has " @ %bank.itemCount[%itemsquish] SPC %item @ " in the bank.");
	
	if(mFloor(%bank.itemCount[%itemSquish]) <= 0)
		return false;
		
	if(%amount > %bank.itemCount[%itemSquish])
		%amount = %bank.itemCount[%itemSquish];

	%client.addToInventory(%item,%amount);
	%client.removeFromBank(%item,%amount);
	
	commandToClient(%client,'openBank');
}

activatePackage(Inventory);

//--