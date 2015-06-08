$LOB::rockHealth["Iron"] = 100;
$LOB::rockDrop["Iron"] = "1 3";
$LOB::rockInfo["Iron"] = "It's an Iron rock. I can mine this, smelt it with Coal and create an iron bar to be smithed into weapons.";
$LOB::rockDeath["Iron"] = 120000;
$LOB::rockRequiredLevel["Iron"] = 10;

$LOB::rockHealth["Tin"] = 100;
$LOB::rockDrop["Tin"] = "1 3";
$LOB::rockInfo["Tin"] = "It's a Tin rock. I can mine this, smelt it with a copper ore, and create a bronze bar to be smithed into weapons.";
$LOB::rockDeath["Tin"] = 30000;
$LOB::rockRequiredLevel["tin"] = 0;

$LOB::rockHealth["Copper"] = 100;
$LOB::rockDrop["Copper"] = "1 3";
$LOB::rockInfo["Copper"] = "It's a Copper rock. I can mine this, smelt it with a Tin ore, and create a bronze bar to be smithed into weapons.";
$LOB::rockDeath["Copper"] = 30000;
$LOB::rockRequiredLevel["copper"] = 0;

$LOB::rockHealth["Coal"] = 200;
$LOB::rockDrop["Coal"] = "1 3";
$LOB::rockInfo["Coal"] = "It's a coal rock. I can mine this, smelt it with iron to create steel bars.";
$LOB::rockDeath["Coal"] = 120000;
$LOB::rockRequiredLevel["coal"] = 15;

$LOB::rockHealth["Mithril"] = 1000;
$LOB::rockDrop["Mithril"] = "1 3";
$LOB::rockInfo["Mithril"] = "It's a Mithril rock. I can mine this, smelt it with iron to create Mithril bars.";
$LOB::rockDeath["Mithril"] = 240000;
$LOB::rockRequiredLevel["Mithril"] = 35;

$LOB::rockHealth["Adamantite"] = 2000;
$LOB::rockDrop["Adamantite"] = "1 3";
$LOB::rockInfo["Adamantite"] = "It's an Adamantite rock. I can mine this, smelt it with Coal to create Adamantite bars.";
$LOB::rockDeath["Adamantite"] = 360000;
$LOB::rockRequiredLevel["Adamantite"] = 50;

package Mining
{
	function serverCmdSetWrenchData(%client,%data)
	{
		parent::serverCmdSetWrenchData(%client,%data);
		
		%bn = strReplace(getField(%data,0),"N ","");
		%bn = trim(strReplace(%bn,"_"," "));
	
		%brick = %client.wrenchBrick;
		
		if(getWord(%bn,0) $= "rock")
		{
			%rockType = getWord(%bn,1);
			
			if(strStr($LOB::rock["ALL"],%brick) >= 0)
				return false;
			
			addToList("$LOB::rock["@%rockType@"]",%brick);
			addToList("$LOB::rock[\"ALL\"]",%brick);
				
			//echo("Registered rock to "@strUpr(%rockType)@" and to ALL categories.");
		}
	}
	
	function fxdtsbrick::onPlant(%this)
	{
		parent::onPlant(%this);
		
		%name = %this.getName();
		%name = strReplace(%name,"_"," ");
		%name = trim(%name);
		
		if(%name !$= "")
		{
			%cat = getWord(%name,0);
			%type = getWord(%name,1);
			
			if(%cat $= "Rock")
			{
				addTolist("$LOB::Rock["@%Type@"]",%this);
				addToList("$LOB::Rock[\"All\"]",%this);
			}
		}
	}
	
	function FxDtsBrick::onRemove(%this)
	{
		%bn = %this.getName();
		%bn = trim(strReplace(%bn,"_"," "));
		
		if(getWord(%bn,0) $= "rock")
		{
			%rockType = getWord(%bn,1);
			
			removeFromList("$LOB::rock["@%rockType@"]",%this);
			removeFromList("$LOB::rock[\"ALL\"]",%this);
			
			//echo("Brick rock group cleanup successfull.");
		}
		
		parent::onRemove(%this);
	}
};
activatePackage(Mining);

function pickAxeOnHit(%client,%axe,%col)
{
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%name = %col.getName();
		%name = strReplace(%name,"_"," ");
		%name = trim(%name);
		
		%category = getWord(%name,0);
		%type = getWord(%name,1);
		
		if(%category $= "rock")
		{
			if(%client !$= "")
			{
				if(%client.slo.miningLevel < $LOB::rockRequiredLevel[%type])
				{
					%m = setKeyWords("\c6You need a mining level of " @ $Lob::rockRequiredLevel[%type] @ " to mine " @ %type @ ".","mining" SPC %type,"\c6");
					commandToClient(%client,'centerprint',%m,3);
					return 0;	
				}
				
				if(!isObject(%col.rockOwner))
					%col.rockOwner = "";
				else
				if(isObject(%col.rockOwner))
				{
					if(vectorDist(%col.rockOwner.player.position,%col.position) >= 10)
						%col.rockOwner = "";
				}
					
				if(%col.rockOwner !$= "" && %col.rockOwner != %client)
				{
					%m = setKeyWords("\c6Someone else is mining this.","Someone else","\c6");
					commandToClient(%client,'centerprint',%m,3);
					return 0;				
				}

				%col.rockOwner = %client;
				%client.rock = %col;
				cancel(%client.rockownershiploop);
				%client.rockownershiploop = %client.schedule(1000,loserockownership);

			}
			if(%client !$= "")
			{
				%client.setStatus("Mining",500);
				%m = setKeyWords("\c6Mining a(n) " @ %type @ " rock.", %type,"\c6");
				commandToClient(%client,'centerPrint',%m, 3);
				cancel(%client.loseRockOwnerShipLoop);
				%client.loseRockOwnerShipLoop = %client.schedule(1000,loseRockOwnerShip);
			}
			
			if(%col.tempHealth $= "")
			{
				%col.tempHealth = $LOB::rockHealth[%type];
			}
			
			if(%client $= "")
			{
				%col.tempHealth--;
			}
			else
			{
			
				%dmg = getRandom(1,$toolDamage[%weaponClass] + mFloor(%client.slo.miningLevel));
				
				if(%dmg > $LOB::rockHealth[%type])%dmg = $LOB::rockHealth[%type];
				
				%col.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				giveExp(%client,"mining",%exp);
				
				if(getRandom(0,800) <= 2)
				{
					%client.beginRandomEvent("mining");
				}
			}
			
			//echo(%col.temphealth);
			
			if(%col.tempHealth <= 0)
			{
				if(isObject(%client))
				{
					%oresDropped = getRandom(getWord($LOB::rockDrop[%type],0),getWord($LOB::rockDrop[%type],1));
					//%client.inventory.mat[%type@"Ores"] += %oresDropped;
					%client.addToInventory(%type @ " ores",%oresDropped);
					%m = setKeyWords("\c6+ " @ %oresDropped SPC %type @ " ores (" @ %client.slo.inventory.itemCount[%type @ "ores"] @ " total)",%type SPC "total","\c6");
					messageClient(%client,'',%m);
				}
				%col.rockOwner = "";
				%col.tempHealth = $LOB::rockHealth[%type];
				%col.fakeYourDeath($LOB::rockDeath[%type]);
			}
		}
	}
	else
	if(%col.getClassName() $= "Player")
	{
	
	}
	else
	if(%col.getClassName() $= "AiPlayer")
	{
	
	}
}

function gameConnection::loseRockOwnerShip(%this)
{	
	if(isObject(%this.rock))
	{
		%this.rock.rockOwner = "";
		%this.rock = "";
	}
}