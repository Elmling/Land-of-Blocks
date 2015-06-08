$LOB::TreeHealth["pine"] = 100;
$LOB::TreeDrop["pine"] = "1 3";
$LOB::TreeInfo["pine"] = "A pine tree.";
$LOB::TreeDeath["Pine"] = 30000;
$LOB::TreeRequiredLevel["pine"] = 0;

$LOB::TreeHealth["Oak"] = 200;
$LOB::TreeDrop["Oak"] = "1 2";
$LOB::TreeInfo["Oak"] = "An oak tree.";
$LOB::TreeDeath["Oak"] = 120000;
$LOB::TreeRequiredLevel["Oak"] = 15;

$LOB::TreeHealth["Willow"] = 350;
$LOB::TreeDrop["willow"] = "1 2";
$LOB::TreeInfo["willow"] = "A willow tree.";
$LOB::TreeDeath["willow"] = 200000;
$LOB::TreeRequiredLevel["willow"] = 25;

$LOB::TreeHealth["maple"] = 500;
$LOB::TreeDrop["maple"] = "1 3";
$LOB::TreeInfo["maple"] = "A maple tree.";
$LOB::TreeDeath["maple"] = 30000;
$LOB::TreeRequiredLevel["maple"] = 40;

package woodCutting
{
	function serverCmdSetWrenchData(%client,%data)
	{
		parent::serverCmdSetWrenchData(%client,%data);
		
		%bn = strReplace(getField(%data,0),"N ","");
		%bn = trim(strReplace(%bn,"_"," "));
	
		%brick = %client.wrenchBrick;
		
		if(getWord(%bn,0) $= "Tree")
		{
			%treeType = getWord(%bn,1);
			
			if(strStr($LOB::tree["ALL"],%brick) >= 0)
				return false;
				
			addTolist("$LOB::Tree["@%treeType@"]",%this);
			addToList("$LOB::Tree[\"All\"]",%this);
				
			//echo("Registered tree to "@strUpr(%treeType)@" and to ALL categories.");
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
			
			if(%cat $= "Tree")
			{
				addTolist("$LOB::Tree["@%Type@"]",%this);
				addToList("$LOB::Tree[\"All\"]",%this);
			}
		}
	}
	
	function fxdtsBrick::onLoadPlant(%this)
	{
		parent::onloadPlant(%this);
		
		%name = %this.getName();
		%name = strReplace(%name,"_"," ");
		%name = trim(%name);
		
		if(%name !$= "")
		{
			%cat = getWord(%name,0);
			%type = getWord(%name,1);
			
			if(%cat $= "Tree")
			{
				addTolist("$LOB::Tree["@%Type@"]",%this);
				addToList("$LOB::Tree[\"All\"]",%this);
			}
		}
	}
	
	function FxDtsBrick::onRemove(%this)
	{
		%bn = %this.getName();
		%bn = trim(strReplace(%bn,"_"," "));
		
		if(getWord(%bn,0) $= "Tree")
		{
			%treeType = getWord(%bn,1);
			
			removeFromList("$LOB::tree["@%treeType@"]",%this);
			removeFromList("$LoB::tree[\"ALL\"]",%this);
			
			//echo("Brick tree group cleanup successfull.");
		}
		
		parent::onRemove(%this);
	}
};
activatePackage(woodCutting);

function axeOnHit(%client,%axe,%col)
{
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%name = %col.getName();
		%name = strReplace(%name,"_"," ");
		%name = trim(%name);
		
		%category = getWord(%name,0);
		%type = getWord(%name,1);
		
		if(%category $= "tree")
		{
		
			if(%client.slo.woodcuttingLevel < $LOB::treeRequiredLevel[%type])
			{
				%m = setKeyWords("\c6You need a woodcutting level of " @ $Lob::treeRequiredLevel[%type] @ " to cut a(n) " @ %type @ ".","woodcutting" SPC %type,"\c6");
				commandToClient(%client,'centerprint',%m,3);
				return 0;	
			}
			
			if(!isObject(%col.treeOwner))
				%col.treeOwner = "";
			else
			if(isObject(%col.treeOwner))
			{
				if(vectorDist(%col.treeOwner.player.position,%col.position) >= 10)
					%col.treeOwner = "";
			}
			
			if(%col.treeOwner !$= "" && %col.treeOwner != %client)
			{
				%m = setKeyWords("\c6Someone else is cutting this tree this.","Someone else","\c6");
				commandToClient(%client,'centerprint',%m,3);
				return 0;				
			}

			%col.treeOwner = %client;
			%client.tree = %col;
			
			if(%client !$= "")
			{
				%client.setStatus("Wood Cutting",500);
				cancel(%client.loseTreeOwnerShipLoop);
				%client.loseTreeOwnerShipLoop = %client.schedule(1000,loseTreeOwnerShip);
			}
			
			%m = setKeyWords("\c6Chopping a(n) " @ %type @ " tree.",%type,"\c6");
			commandToClient(%client,'centerPrint',%m,3);
			
			if(%col.tempHealth $= "")
			{
				%col.tempHealth = $LOB::treeHealth[%type];
			}
			
			if(%client $= "")
			{
				%col.tempHealth--;
			}
			else
			{
				%dmg = getRandom(1,$toolDamage[%weaponClass] + mFloor(%client.slo.woodCuttingLevel));
				
				if(%dmg > $LOB::treeHealth[%type])%dmg = $LOB::treeHealth[%type];
				
				%col.tempHealth -= %dmg;
				
				%exp = mRound(msqrt(%dmg));
				
				giveExp(%client,"woodCutting",%exp);
				
				if(getRandom(0,800) <= 2)
				{
					%client.beginRandomEvent("woodcutting");
				}
			}
			
			//echo(%col.temphealth);
			
			if(%col.tempHealth <= 0)
			{
				if(isObject(%client))
				{
					%logsDropped = getRandom(getWord($LOB::treeDrop[%type],0),getWord($LOB::treeDrop[%type],1));
					//%client.inventory.mat[%type@"Logs"] += %logsDropped;
					%client.addToInventory(%type SPC "logs",%logsDropped);
					%m = setKeyWords("\c6+ " @ %logsDropped SPC %type @" logs (" @ %client.slo.inventory.itemCount[%type @ "logs"] @ " total)",%type SPC "total","\c6");
					messageClient(%client,'',%m);
				}
				%col.treeOwner = "";
				%col.tempHealth = $LOB::treeHealth[%type];
				%col.fakeYourDeath($LOB::TreeDeath[%type]);
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

function gameConnection::loseTreeOwnerShip(%this)
{	
	if(isObject(%this.tree))
	{
		%this.tree.treeOwner = "";
		%this.tree = "";
	}
}