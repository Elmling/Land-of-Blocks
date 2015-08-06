$LOB::saveLoadPath = "base/LOB/Saves/";

package SaveLoad
{
	function gameConnection::autoAdminCheck(%client)
	{
		%p = parent::autoAdminCheck(%client);
		
		lob_sendInfo(%client);
		
		if(!isFile("base/lob/saves/" @ %client.bl_id @ "/donations.txt"))
		{
			lob_convertToNew("base/lob/saves/" @ %client.bl_id @ "/" @ %client.bl_id @ ".txt");
			lob_convertToNew("base/lob/saves/" @ %client.bl_id @ "/bank.txt");
			lob_msgAdmins(%client.name @ "'s inventory and bank has been converted to the new system.");
		}
		
		if(isObject(%client.slo))
			%client.saveProfile();
		
		%client.loadProfile();
		lob_activity.register(%client);
		%client.guiWarn = %client.schedule(1000,guiWarn);
		%client.guiCheck();	
		%client.status = "loading";
		if(%client.slo.guiVersion $= "")%client.slo.guiVersion = 0;
		schedule(1500,lob_SayVersion,%client);
		servercmdnewsong(%client);
		commandToClient(%client,'IsLobServer_bitmap');
		%client.saveprofile();
		if(%client.slo.pkPoints $= "")
			%client.slo.pkPoints = 0;
			
		fixHp(%client.name);
		
		if(%client.slo.currentAvatar !$= "")
			%client.customAvatar = %client.slo.currentAvatar;
		
		lob_registerItemsToClient();
		
		if(%client.slo.zoneArea $= "")
		{
			%client.slo.area = "ordunia";
			%client.slo.zoneArea = "ordunia";
		}
		
		$lob::zonePlayerCount[%client.slo.zoneArea]++;
		return %p;
	}
	
	function gameConnection::onDrop(%client)
	{
		%client.slo.currentAvatar = %client.customAvatar;
		%client.saveProfile();
		
		$lob::zonePlayerCount[%client.slo.zoneArea]--;
		
		return parent::onDrop(%client);
	}
};

activatePackage(saveLoad);

function lob_sendInfo(%client)
{
	%m = "You can show your support for the server by donating! " @ linkParser("http://www.landofblocks.weebly.com");
	
	messageClient(%client,'',"\c2" @ %m);
}

function lob_SayVersion(%client)
{
	lob_msgAdmins(%client.name @ " is running GUI version " @ %client.slo.guiversion);
}

//Dynamically add new items so the client can recognize them on their side.
function lob_RegisterItemsToClient()
{
	%c = clientGroup.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%client = clientGroup.getObject(%i);
		//syntax, arg1: name arg2: correctname
		commandToClient(%client,'lobregisteritemvariable',"Horse","Horse");
		commandToClient(%client,'lobRegisterItemVariable',"RawBeef","Raw Beef");
		commandToClient(%client,'lobRegisterItemVariable',"RawSteak","Raw Steak");
		commandToClient(%client,'lobRegisterItemVariable',"RawLobster","Raw Lobster");
		commandToClient(%client,'lobRegisterItemVariable',"BronzeFullHelmet","Bronze Full Helmet");
		commandToClient(%client,'lobRegisterItemVariable',"ironFullHelmet","Iron Full Helmet");
		commandToClient(%client,'lobRegisterItemVariable',"steelFullHelmet","Steel Full Helmet");
		commandToClient(%client,'lobRegisterItemVariable',"MithrilFullHelmet","Mithril Full Helmet");
		commandToClient(%client,'lobRegisterItemVariable',"adamantiteFullHelmet","Adamantite Full Helmet");
		commandToClient(%client,'lobRegisterItemVariable',"FireBall","Fire Ball");
		commandToClient(%client,'lobRegisterItemVariable',"WindWall","Wind Wall");
		commandToClient(%client,'lobRegisterItemVariable',"MagicMaterial","Magic Material");
		commandToClient(%client,'lobRegisterItemVariable',"InterpassScroll","Interpass scroll");
		commandToClient(%client,'lobRegisterItemVariable',"AlyswellScroll","Alyswell Scroll");
		commandToClient(%client,'lobRegisterItemVariable',"EldriaScroll","Eldria Scroll");
		commandToClient(%client,'lobRegisterItemVariable',"DungeonCoin","Dungeon Coin");
	}
}

function gameConnection::loadProfile(%client)
{
	if(isObject(%client.slo))
		%client.slo.delete();
		
	%path = "base/lob/saves/" @ %client.bl_id @ "/" @ %client.bl_id @ ".txt";
	
	if(!isFile(%path))
	{
		//echo("new profile");
		%profile = new scriptObject();
	}
	else
	{
		%f = new fileObject();
		%f.openForRead(%path);
		
		while(!%f.isEOF())
		{
			%line = %f.readLine();
			
			if(strStr(%line,"//") >= 0 || strStr(strLwr(%line),"r's") >= 0)
				continue;
				
			%profile = %profile @ %line;
		}
		
		%f.close();
		%f.delete();

		eval("%profile = " @ %profile);
	}
	
	%f = new fileObject();
	%f.openForRead("base/lob/saves/defaults.txt");
	
	while(!%f.isEOF())
	{
		%line = %f.readLine();
		%variable = getSubStr(%line,0,strStr(%line,"="));
		eval("%val = %profile."@%variable@";");
		
		if(%val $= "")
		{
			eval("%profile."@%line@";");
		}
	}
	
	%f.close();
	%f.delete();
	
	%client.slo = %profile;
	%client.loadBank(%path);
	%client.loadInventory();
	%client.loadDonation();
	
	return %profile;
}

function gameConnection::loadBank(%client,%path)
{
	if(isObject(%client.slo.bank))
		%client.slo.bank.delete();
		
	%path = "base/lob/saves/" @ %client.bl_id @ "/bank.txt";
	
	if(!isFile(%path))
	{
		echo("new Bank");
		%bank = new scriptObject();
	}
	else
	{
		%f = new fileObject();
		%f.openForRead(%path);
		
		while(!%f.isEOF())
		{
			%line = %f.readLine();
			
			if(strStr(%line,"//") >= 0 || strStr(strLwr(%line),"r's") >= 0)
				continue;
				
			%bank = %bank @ %line;
		}
		
		%f.close();
		%f.delete();

		eval("%bank = " @ %bank);
	}
	
	%client.slo.bank = %bank;
	
	return %bank;
}

function gameConnection::loadInventory(%client)
{
	if(isObject(%client.slo.inventory))
		%client.slo.inventory.delete();
		
	%path = "base/lob/saves/" @ %client.bl_id @ "/inventory.txt";
	
	if(!isFile(%path))
	{
		%inventory = new scriptObject()
		{
			itemCount = -1;
		};
	}
	else
	{
		%f = new fileObject();
		%f.openForRead(%path);
		
		while(!%f.isEOF())
		{
			%line = %f.readLine();
			
			if(strStr(%line,"//") >= 0 || strStr(strLwr(%line),"r's") >= 0)
				continue;
				
			%inventory = %inventory @ %line;
		}
		
		%f.close();
		%f.delete();

		eval("%inventory = " @ %inventory);
	}
	
	%client.slo.inventory = %inventory;
	
	return %inventory;
}

function gameConnection::loadDonation(%client)
{
	if(isObject(%client.slo.donation))
		%client.slo.donation.delete();
		
	%path = "base/lob/saves/" @ %client.bl_id @ "/donations.txt";
	
	if(!isFile(%path))
	{
		echo("new donation");
		%donation = new scriptObject()
		{
			avatar[0] = "Default";
			avatar[1] = "Adventurer";
			avatarCount = 1;
		};
	}
	else
	{
		%f = new fileObject();
		%f.openForRead(%path);
		
		while(!%f.isEOF())
		{
			%line = %f.readLine();
			
			if(strStr(%line,"//") >= 0 || strStr(strLwr(%line),"r's") >= 0)
				continue;
				
			%donation = %donation @ %line;
		}
		
		%f.close();
		%f.delete();

		eval("%donation = " @ %donation);
	}
	
	%client.slo.donation = %donation;
	
	return %donation;
}

function gameConnection::saveProfile(%client)
{
	%path = "base/lob/saves/" @ %client.bl_id @ "/" @ %client.bl_id @ ".txt";
	%client.slo.save(%path);
	%path2 = "base/lob/saves/" @ %client.bl_id @ "/Bank.txt";
	%client.slo.bank.save(%path2);
	%path3 = "base/lob/saves/" @ %client.bl_id @ "/Inventory.txt";
	%client.slo.inventory.save(%path3);
	%path4 = "base/lob/saves/" @ %client.bl_id @ "/Donations.txt";
	%client.slo.donation.save(%path4);
	//username
	%path5 = "base/lob/saves/" @ %client.bl_id @ "/username.txt";
	%f = new fileObject();
	%f.openForWrite(%path5);
	%f.writeLine(%client.name);
	%f.close();
	%f.delete();
}

function lob_saveAll()
{
	cancel($lob::saveAll);
	
	%c = clientGroup.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%cl = clientGroup.getObject(%i);
		%cl.saveProfile();
	}
	
	$lob::saveAll = schedule(60000,0,lob_saveAll);
}

function lob_convertToNew(%path)
{
	if(isFile(%path))
	{
		//echo("converting " @ %path @ " to new object system.");
		
		%switch["itembarsbronze"] = "bronze bars";
		%switch["itembarsiron"] = "iron bars";
		%switch["itemlogsPine"] = "Pine logs";
		%switch["itemlogsOak"] = "oak logs";
		%switch["itemlogsWilow"] = "willow logs";
		%switch["itembronzeBow"] = "Bronze Bow";
		%switch["itemIronBow"] = "Iron Bow";
		%switch["itemSteelBow"] = "Steel Bow";
		%switch["itemBronzeShortSword"] = "Bronze Shortsword";
		%switch["itemIronShortSword"] = "Iron Shortsword";
		%switch["itemSteelShortSword"] = "Steel Shortsword";
		%switch["itemOresTin"] = "Tin Ores";
		%switch["itemOresCopper"] = "Copper Ores";
		%switch["itemOresIron"] = "Iron Ores";
		%switch["itemGold"] = "Gold";
		
		%f = new fileObject();
		%f.openForRead(%path);
		%lc=-1;
		
		while(!%f.isEof())
		{
			%line = %f.readLine();
			%line = trim(%line);
			%w = getWord(%line,0);
			
			if(%switch[%w] $= "" && strStr(strLwr(%w),"level") < 0 && strStr(strLwr(%w),"exp") < 0 )continue;
			
			if(%switch[%w] !$= "")
			{
				%line[%lc++] = "itemCount" @ convertToItemName(%switch[%w]) @ " = " @ getWord(%line,getWordCount(%line)-1);
			}
			else
				%line[%lc++] = %w @ " = " @ getWord(%line,getWordCount(%line)-1);
			//echo(%w @ " => " @ %amt);
			
		
		}
		
		%f = new fileObject();
		%f.openForWrite(%path);
		
		%f.writeLine("//--- OBJECT WRITE BEGIN ---");
		%f.writeLine("new ScriptObject() {");
		
		for(%i=0;%i<%lc;%i++)
		{
			%f.writeLine(%line[%i]);
		}
		
		%f.writeLine("};");
		%f.writeLine("//--- OBJECT WRITE END ---");
		
		%f.close();
		%f.delete();

	}
}

lob_saveAll();