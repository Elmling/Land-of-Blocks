package lob_trade
{
	function gameConnection::onDrop(%client)
	{
		if(isObject(%client.tradeObject))
		{
			%trading = %client.tradeObject.trading;
			serverCmdInTradeDecline(%client,%trading);
		}
	
		%d = parent::onDrop(%client);
		
		return %d;
	}
};
activatePackage(lob_trade);

function serverCmdSendTradeRequest(%this,%client)
{
	if(%client $= "")
		%client = %this.potentialPlayerInteraction;
		
	if(%client.wild || %this.wild)
	{
		%m = setKeyWords("\c6You cannot trade in the wilderness.","trade wilderness","\c6");
		smartMessage(%this,%m,1000);
		return 0;
	}
	else
	if(isObject(%this.tradeObject) || !isObject(%client) || (%client.wild || %this.wild) || vectorDist(%this.player.position,%client.player.position) >= 10)
	{
		return 0;
	}
	else
	if(isObject(%client.tradeObject))
	{
		%m = setKeyWords("\c6" @ %client.name @ " is already in a trade.", %client.name, "\c6");
		smartMessage(%this,%m,1000);
		return 0;
	}
	
	if(%this.tradeRequestPending[%client.name])
	{
		serverCmdAcceptTradeRequest(%this,%client);
		%client.tradeRequestPending[%this.name] = "";
		%this.tradeRequestPending[%client.name] = "";
		return 1;
	}
	
	%client.tradeRequestPending[%this.name] = true;
	%client.tradeRequestSchedule = %client.schedule(10000,killPendingTradeRequest,%this);
	%m = setKeyWords("\c6" @ %this.name @ " would like to trade with you.",%this.name SPC "trade","\c6");
	smartMessage(%client,%m,3000);
	%m = setKeyWords("\c6Trade request sent successfully.","Trade","\c6");
	smartMessage(%this,%m,3000);
}

function serverCmdAcceptTradeRequest(%this,%client)
{
	if(%this.wild || %client.wild)
	{
		%m = setKeyWords("\c6You cannot trade in the wilderness.","trade wilderness","\c6");
		smartMessage(%this,%m,1000);
		return 0;
	}
	
	commandToClient(%client,'setTradeStatus',"Waiting on both users to accept.");
	commandToClient(%this,'setTradeStatus',"Waiting on both users to accept.");
	
	%this.buildTradeObject();
	%this.tradeObject.trading = %client;
	%client.buildTradeObject();
	%client.tradeObject.trading = %this;
	%this.controlToCamera("point",%this.player.position);
	%client.controlToCamera("point",%client.player.position);
	commandToClient(%this,'openTradeGui');
	commandToClient(%client,'openTradeGui');
}

function serverCmdInTradeDecline(%client)
{
	if(isObject(%client.tradeObject.trading))
	{
		%trading = %client.tradeObject.trading;
		%trading.endOrbit();
		%client.endOrbit();
		%client.tradeObject.delete();
		%trading.tradeObject.delete();
		commandToClient(%trading,'closeTradeGui');
		commandToClient(%client,'closeTradeGui');
		%m = setKeyWords("\c6" @ %client.name @ " has declined the trade.",%client.name SPC "declined","\c6");
		smartMessage(%trading,%m,200);
		%m = setKeyWords("\c6You've declined the trade with " @ %trading.name @ ".","declined" SPC %trading.name,"\c6");
		smartMessage(%client,%m,200);
		commandToClient(%trading,'closeTradeGui');
		commandToClient(%client,'closeTradeGui');
	}
}

function serverCmdInTradeAccept(%client)
{
	if(isObject(%client.tradeObject))
	{
		if(%client.tradeObject.tradeAccepted)
			return 0;
	
		%client.tradeObject.tradeAccepted = true;
		%trading = %client.tradeObject.trading;
		
		if(%trading.tradeObject.tradeAccepted)
		{
			//finish trade, transfer items
			
			%trading = %client.tradeObject.trading;
			%trading.endOrbit();
			%client.endOrbit();
			//swap objects from the trade object
			%tcc = %trading.tradeObject.getCount();
			for(%i=0;%i<%tcc;%i++)
			{
				%o = %trading.tradeObject.getObject(%i);
				%client.addToInventory(%o.name,%o.amount);
				%trading.removeFromInventory(%o.name,%o.amount);
			}
			%ctc = %client.tradeObject.getCount();
			for(%i=0;%i<%ctc;%i++)
			{
				%o = %client.tradeObject.getObject(%i);
				%trading.addToInventory(%o.name,%o.amount);
				%client.removeFromInventory(%o.name,%o.amount);
			}
			//----------------------------------
			%client.tradeObject.delete();
			%trading.tradeObject.delete();
			%m = setKeyWords("\c6Trade session with " @ %client.name @ " is complete.",%client.name SPC "complete","\c6");
			smartMessage(%trading,%m,200);
			%m = setKeyWords("\c6Trade session with " @ %trading.name @ " is complete.",%trading.name SPC "complete","\c6");
			smartMessage(%client,%m,200);
			commandToClient(%trading,'closeTradeGui');
			commandToClient(%client,'closeTradeGui');
		}
		else
		{
			commandToClient(%client,'setTradeStatus',"You have accepted. Waiting for " @ %client.tradeObject.trading.name @ " to accept.");
			commandToClient(%client.tradeObject.trading,'setTradeStatus',%client.name @ " has accepted, waiting for you to accept.");
		}
	}
}

function serverCmdInTradeAddItem(%client,%item)
{
	if(isObject(%client.tradeObject))
	{
		%amt = pullIntFromString(%item);
		
		if(%amt $= "NULL")
			return 0;
		
		%item = strReplace(%item,%amt,"");
		%item = strReplace(%item,"()","");
		%item = trim(%item);
		%itemCheck = $lob::itemDatablock[convertToItemName(%item)];
		
		//messageClient(findclientbyname("elm"),'',"\c6item = " @ %item @" | item datablock = " @ %itemcheck);

		if(isObject(%itemCheck))
		{
			%amount = %client.slo.inventory.itemCount[convertToItemName(%item)];
			
			if(%amt > %amount)
				return 0;
			
			%client.tradeObject.addItemToTradeObject(%item,%amt);
			%trading = %client.tradeObject.trading;
			lob_sendNewTradeInfo(%client,%trading);
			%client.tradeObject.tradeAccepted = 0;
			%trading.tradeObject.tradeAccepted = 0;
			commandToClient(%client,'setTradeStatus',"Waiting on both users to accept.");
			commandToClient(%trading,'setTradeStatus',"Waiting on both users to accept.");
		}
	}
}

function serverCmdInTradeRemoveItem(%client,%item)
{
	if(isObject(%client.tradeObject))
	{
		%amt = pullIntFromString(%item);
		
		if(%amt $= "NULL")
			return 0;
		
		%item = strReplace(%item,%amt,"");
		%item = trim(%item);
		%itemCheck = $lob::itemDatablock[convertToItemName(%item)];
		
		if(isObject(%itemCheck))
		{
			eval("%amount = %client.slo.item" @ convertToItemName(%item) @ ";");
			%client.tradeObject.removeItemFromTradeObject(convertToItemName(%item));
			%trading = %client.tradeObject.trading;
			lob_sendNewTradeInfo(%client,%trading);
			%client.tradeObject.tradeAccepted = 0;
			%trading.tradeObject.tradeAccepted = 0;
			commandToClient(%client,'setTradeStatus',"Waiting on both users to accept.");
			commandToClient(%trading,'setTradeStatus',"Waiting on both users to accept.");
		
		}
	}
}

function serverCmdGetTradeData(%client)
{
	if(isObject(%client.tradeObject))
	{
		%trading = %client.tradeObject.trading;
		
		if(isObject(%trading))
			lob_sendNewTradeInfo(%client,%trading);
	}
}

function gameConnection::killPendingTradeRequest(%this,%client)
{
	%this.tradeRequestPending[%client.name] = 0;
}

function gameConnection::buildTradeObject(%client)
{
	if(isObject(%client.tradeObject))
	{
		error(%client.name @ " already has a trade object!");
		return 0;
	}
	
	%client.tradeObject = new scriptGroup()
	{
		class = Lob_TradeObject;
		client = %client;
	};
	
	return %client.tradeObject;
}

function Lob_TradeObject::addItemToTradeObject(%this,%item,%amount)
{
	%client = %this.client;
	%check = %this.getObjectByName(%item);

	if(isObject(%check))
	{
		%check.delete();
	}
	
	%itemName = convertToItemName(%item);
	
	if($lob::itemDatablock[%itemName] $= "")
		return false;
	
	%toc = new scriptObject()
	{
		Parent = %this;
		name = %item;
		Amount = %amount;
	};
	
	%this.add(%toc);
	
	//echo("Adding object " @ %toc.name @ " to " @ %client.name @ "\'s trade object.");
}

function Lob_TradeObject::removeItemFromTradeObject(%this,%item)
{
	%client = %this.client;
	//echo("trying to remove item named " @ %item);
	%item = %this.getObjectByName(%item);
	
	if(isObject(%item))
	{
		//echo("Removing object " @ %item @ " from " @ %client.name @ "\'s trade object.");
		%item.delete();
		return true;
	}
	return false;
}

function Lob_TradeObject::getObjectByName(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getObject(%i);
		if(convertToItemName(%o.name) $= convertToItemName(%name))
			return %o;
	}
	
	return false;
}

function lob_sendNewTradeInfo(%client,%clientTrading)
{

	commandToClient(%client,'catchTradeData',"client","Clear");
	commandToClient(%clientTrading,'catchTradeData',"client","clear");
	
	%c = %clientTrading.tradeObject.getCount();
	
	for(%i=0;%i<%c;%i++)
	{
		%o = %clientTrading.tradeObject.getObject(%i);
		commandToClient(%client,'catchTradeData',"Client",%o.Name,%o.amount);
		commandToClient(%clientTrading,'catchTradeData',"Yours",%o.Name,%o.amount);
	}
	
	%c = %client.tradeObject.getCount();
	
	for(%i=0;%i<%c;%i++)
	{
		%o = %client.tradeObject.getObject(%i);
		commandToClient(%client,'catchTradeData',"Yours",%o.Name,%o.amount);
		commandToClient(%clientTrading,'catchTradeData',"Client",%o.Name,%o.amount);
	}
}