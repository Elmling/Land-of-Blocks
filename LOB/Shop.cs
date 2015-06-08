function aiPlayer::lob_newShop(%this)
{
	if(%this.hasShop)
	{
		
		return false;
	}
	
	%this.shop = new scriptObject()
	{
		class = lobShop;
		npc = %this;
	};
	
	%this.shop.initialize();
}

function aiPlayer::lob_deleteShop(%this)
{
	if(%this.hasShop)
	{
		%this.hasShop = "";
		%this.shop.delete();
		%this.shop = "";
	}
}

$lob::npcBuyPrice["Wind Wall"] = 50000;
$lob::npcBuyPrice["Fire Ball"] = 35000;

$Lob::npcBuyPrice["pine logs"] = 2;
$Lob::npcBuyPrice["oak logs"] = 15;
$Lob::npcBuyPrice["willow logs"] = 40;
$Lob::npcBuyPrice["maple logs"] = 100;

$Lob::npcBuyPrice["tin ores"] = 2;
$Lob::npcBuyPrice["copper ores"] = 2;
$Lob::npcBuyPrice["iron ores"] = 15;
$Lob::npcBuyPrice["coal ores"] = 20;
$Lob::npcBuyPrice["mithril ores"] = 75;
$Lob::npcBuyPrice["adamantite ores"] = 175;

$Lob::npcBuyPrice["bronze bars"] = 30;
$Lob::npcBuyPrice["iron bars"] = 150;
$Lob::npcBuyPrice["steel bars"] = 350;
$Lob::npcBuyPrice["mithril bars"] = 1500;
$Lob::npcBuyPrice["adamantite bars"] = 7500;

$Lob::npcBuyPrice["bronze pickaxe"] = 10;
$Lob::npcBuyPrice["iron pickaxe"] = 650;
$Lob::npcBuyPrice["steel pickaxe"] = 1100;
$Lob::npcBuyPrice["mithril pickaxe"] = 4150;
$Lob::npcBuyPrice["adamantite pickaxe"] = 17900;

$Lob::npcBuyPrice["bronze axe"] = 10;
$Lob::npcBuyPrice["iron axe"] = 650;
$Lob::npcBuyPrice["steel axe"] = 1100;
$Lob::npcBuyPrice["mithril axe"] = 4150;
$Lob::npcBuyPrice["adamantite axe"] = 17900;

$Lob::npcBuyPrice["bronze shortsword"] = 15;
$Lob::npcBuyPrice["iron shortsword"] = 1300;
$Lob::npcBuyPrice["steel shortsword"] = 2200;
$Lob::npcBuyPrice["mithril shortsword"] = 8300;
$Lob::npcBuyPrice["adamantite shortsword"] = 35800;

$Lob::npcBuyPrice["bronze bow"] = 275;
$Lob::npcBuyPrice["iron bow"] = 1625;
$Lob::npcBuyPrice["steel bow"] = 2750;
$Lob::npcBuyPrice["mithril bow"] = 10375;
$Lob::npcBuyPrice["adamantite bow"] = 44750;

$Lob::npcBuyPrice["bronze javlin"] = 495;
$Lob::npcBuyPrice["iron javlin"] = 2925;
$Lob::npcBuyPrice["steel javlin"] = 4950;
$Lob::npcBuyPrice["mithril javlin"] = 18675;
$Lob::npcBuyPrice["adamantite javlin"] = 80550;

$Lob::npcBuyPrice["Raw Beef"] = 5;
$Lob::npcBuyPrice["Raw Steak"] = 8;
$Lob::npcBuyPrice["Raw Lobster"] = 10;
$Lob::npcBuyPrice["Cooked Beef"] = 10;
$Lob::npcBuyPrice["Cooked Steak"] = 16;
$Lob::npcBuyPrice["Cooked Lobster"] = 20;

//Selling
$lob::npcSellPrice["Wind Wall"] = 150000;
$lob::npcSellPrice["Fire Ball"] = 100000;

$Lob::npcSellPrice["pine logs"] = 5;
$Lob::npcSellPrice["oak logs"] = 25;
$Lob::npcSellPrice["willow logs"] = 65;
$Lob::npcSellPrice["maple logs"] = 150;

$Lob::npcSellPrice["tin ores"] = 5;
$Lob::npcSellPrice["copper ores"] = 5;
$Lob::npcSellPrice["iron ores"] = 30;
$Lob::npcSellPrice["coal ores"] = 40;
$Lob::npcSellPrice["mithril ores"] = 150;
$Lob::npcSellPrice["adamantite ores"] = 400;

$Lob::npcSellPrice["bronze bars"] = 60;
$Lob::npcSellPrice["iron bars"] = 350;
$Lob::npcSellPrice["steel bars"] = 600;
$Lob::npcSellPrice["mithril bars"] = 2250;
$Lob::npcSellPrice["adamantite bars"] = 9500;

$Lob::npcSellPrice["bronze pickaxe"] = 15;
$Lob::npcSellPrice["iron pickaxe"] = 800;
$Lob::npcSellPrice["steel pickaxe"] = 1400;
$Lob::npcSellPrice["mithril pickaxe"] = 5200;
$Lob::npcSellPrice["adamantite pickaxe"] = 21200;

$Lob::npcSellPrice["bronze axe"] = 15;
$Lob::npcSellPrice["iron axe"] = 800;
$Lob::npcSellPrice["steel axe"] = 1400;
$Lob::npcSellPrice["mithril axe"] = 5200;
$Lob::npcSellPrice["adamantite axe"] = 21200;

$Lob::npcSellPrice["bronze shortsword"] = 20;
$Lob::npcSellPrice["iron shortsword"] = 1600;
$Lob::npcSellPrice["steel shortsword"] = 2800;
$Lob::npcSellPrice["mithril shortsword"] = 10400;
$Lob::npcSellPrice["adamantite shortsword"] = 42400;

$Lob::npcSellPrice["bronze bow"] = 350;
$Lob::npcSellPrice["iron bow"] = 2000;
$Lob::npcSellPrice["steel bow"] = 3500;
$Lob::npcSellPrice["mithril bow"] = 13000;
$Lob::npcSellPrice["adamantite bow"] = 53000;

$Lob::npcSellPrice["bronze javlin"] = 630;
$Lob::npcSellPrice["iron javlin"] = 3600;
$Lob::npcSellPrice["steel javlin"] = 6300;
$Lob::npcSellPrice["mithril javlin"] = 23400;
$Lob::npcSellPrice["adamantite javlin"] = 95400;

$Lob::npcSellPrice["Raw Beef"] = 8;
$Lob::npcSellPrice["Raw Steak"] = 11;
$Lob::npcSellPrice["Raw Lobster"] = 15;
$Lob::npcSellPrice["Cooked Beef"] = 15;
$Lob::npcSellPrice["Cooked Steak"] = 22;
$Lob::npcSellPrice["Cooked Lobster"] = 30;


function serverCmdGetPriceInfo(%client,%item,%type)
{
	if(%type $= "buy")
	{
		if($lob::npcSellPrice[%item] !$= "")
		{
			%msg = "You can buy a(n) " @ %item @ " for " @ $lob::npcSellPrice[%item] @ " gold.";
			%msg = setKeyWords(%msg, %item, "<color:ffffff>");
			commandToClient(%client,'catchPriceInfo',%msg);
		}
			
	}
	else
	if(%type $= "sell")
	{
		if($lob::npcBuyPrice[%item] !$= "")
		{
			%msg = "You can sell a(n) " @ %item @ " for " @ $lob::npcBuyPrice[%item] @ " gold.";
			%msg = setKeyWords(%msg, %item, "<color:ffffff>");
			commandToClient(%client,'catchPriceInfo',%msg);
		}
		else
		{
			%msg = "You cannot sell a(n) " @ %item @ ".";
			%msg = setKeyWords(%msg, %item, "<color:ffffff>");
			commandToClient(%client,'catchPriceInfo',%msg);			
		}
	}
}

function serverCmdDoAction(%client,%item,%amount,%type)
{
	%shopkeep = %client.shopKeep;
	
	if(%amount !$= "" && %amount <= 0)
		return false;
	
	if(mFloor(vectorDist(%shopkeep.position,%client.player.position)) > 10)
		return false;

	if(%type $= "buy")
	{
		%shopkeep.shop.clientBuy(%client,%item,%amount);
	}
	else
	if(%type $= "sell")
	{
		%shopKeep.shop.clientSell(%client,%item,%amount);
	}
}


function lobShop::clientBuy(%this,%client,%item,%itemAmount)
{
	if(%itemamount !$= "" && %itemamount <= 0)
		return false;
		
	%npc = %this.npc;
	%player = %client.player;
	
	if(vectorDist(%player.position,%npc.position) <= 10)
	{
		if($lob::npcSellPrice[%item] $= "")
		{
			//echo(%npc.name @ " does not sell " @ %item);
			return false;
		}
		
		%in = convertToItemName(%item);
		
		if(mFloor(%this.itemCount[%in]) <= 0)
			return false;
			
		if(%itemAmount > %this.itemCount[%in])
		{
			%itemAmount = %this.itemCount[%in];
		}
		
		%goldNeeded = $lob::npcSellPrice[%item] * %itemAmount;
		
		if(%client.slo.inventory.itemCount["Gold"] < %goldNeeded)
		{
			//echo(%client.name @ " does not have enough gold to purchase an item.");
		}
		else
		{
			
			%client.removeFromInventory("Gold", %goldNeeded);
			%client.addToInventory(%item, %itemAmount);
			
			%this.itemCount[%in]-= %itemAmount;
			commandToClient(%client,'closeShopWindow');
			commandToClient(%client,'openshopwindow');
		}
	}
}

function lobShop::clientSell(%this,%client,%item,%amount)
{
	if(%amount !$= "" && %amount <= 0)
		return false;
		
	%npc = %this.npc;
	%player = %client.player;
	
	%smash = convertToItemName(%item);
	
	if(vectorDist(%player.position,%npc.position) <= 10)
	{
		if($lob::npcBuyPrice[%item] $= "")
		{
			//echo(%npc.name @ " does not accept " @ %smash);
			return false;
		}
		
		if(%amount > %client.slo.inventory.itemCount[%smash])
			%amount = %client.slo.inventory.itemCount[%smash];
		
		%goldGiving = $lob::npcBuyPrice[%item] * %amount;
		
		%this.itemCount[%smash] += %amount;

		%client.addToInventory("Gold", %goldGiving);
		%client.removeFromInventory(%item, %amount);
		commandToClient(%client,'closeShopWindow');
		commandToClient(%client,'openshopwindow');
	}	
}

function lobShop::initialize(%this)
{
	%this.itemCount["bronzeBow"] = getRandom(1,4);
	%this.itemCount["bronzeShortSword"] = getRandom(5,20);
	%this.itemCount["bronzeAxe"] = getRandom(10,40);
	%this.itemCount["bronzePickAxe"] = getRandom(10,40);
	%this.itemCount["bronzeJavlin"] = getRandom(1,4);
}

function serverCmdPopulateShop(%this)
{
	%shopkeep = %this.shopKeep;
	
	if(mFloor(vectorDist(%shopkeep.position,%this.player.position)) > 10)
	{
		return false;
	}
	
	serverCmdPopulateInventory(%this);
	%i=-1;
	while(%i !$= "")
	{
		%i++;
		%item = %shopKeep.shop.getTaggedField(%i);

		if(%item $= "")
		{
			%i = "";
			break;
		}
		%item = strLwr(%item);
		%item = strReplace(%item,"itemcount","");
		%count = getWord(%item,getWordCount(%item) - 1);
		%item = getWord(%item,0);
		//echo("itemm = " @ %item);
		%item = $Lob::itemCorrectName[convertToItemName(%item)];
		//echo("item = " @ %item);
		if(%item $= "")continue;
		if(mfloor(%count) == 0)
			continue;
		
		commandToClient(%this,'catchShopData',%i,%item,%count);
	}
}
