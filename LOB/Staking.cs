function serverCmdStakeGuiSendData(%client, %data)
{
	%client.acceptedStake = false;
	%client.duelOpponent.acceptedStake = false;
	commandToClient(%client, 'updateStakeStatus', "");
	commandToClient(%client.duelOpponent, 'updateStakeStatus', "");
	
	talk(%data);
	%val = getField(%data, 2);
	%newData = getField(%data, 0) TAB getField(%data, 1) TAB !%val;
	
	commandToClient(%client, 'updateStakeGui', %newData);
	commandToClient(%client.duelOpponent, 'updateStakeGui', %newData);
}

function beginStake(%client, %client2)
{	
	// reset vars and set up client gui
	%client.acceptedStake = false;
	%client.duelOpponent.acceptedStake = false;
	commandToClient(%client, 'updateStakeStatus', "");
	commandToClient(%client2, 'updateStakeStatus', "");
	
	// set all rules active
	%rules = "Melee Range Magic Dash Food";
	for(%i = 0; %i < 5; %i++)
	{
		%data = "rules" TAB getWord(%rules, %i) @ "Button" TAB "1";
		echo(%data);
		commandToClient(%client, 'updateStakeGui', %data);
		commandToClient(%client2, 'updateStakeGui', %data);
	}
	
	//set the title of the stake gui
	commandToClient(%client, 'openStakeGui', "Stake with " @ %client2.name);
	commandToClient(%client2, 'openStakeGui', "Stake with " @ %client.name);
	
	//link the opponents
	%client.duelOpponent = %client2;
	%client2.duelOpponent = %client;
}

function serverCmdSendAcceptStake(%client)
{
	%client2 = %client.duelOpponent;
	
	if(!%client2.acceptedStake)
	{
		commandToClient(%client2, 'updateStakeStatus', %client.name @ " has accepted.");
		commandToClient(%client, 'updateStakeStatus', "Waiting for " @ %client2.name @ " to accept.");
		%client.acceptedStake = true;
	}
	else
	{
		talk("beginning duel");
		%client.acceptedStake = 0;
		%client2.acceptedStake = 0;
		commandToClient(%client, 'closeStakeGui');
		commandToClient(%client2, 'closeStakeGui');
	}
}

function serverCmdSendDeclineStake(%client)
{
	%client2 = %client.duelOpponent;
	commandToClient(%client, 'closeStakeGui');
	commandToClient(%client2, 'closeStakeGui');
	
	%client2.duelOpponent = "";
	%client.duelOpponent = "";
	
	// message clients about the decline
	talk(%client.name @ " declined the stake with " @ %client2.name @ ".");
}

function serverCmdSendStakeRequest(%client, %name)
{
	%opponent = findClientByName(%name);
	
	if(isObject(%opponent))
	{
		messageClient(%opponent, '', %client.name @ " wants to challenge you to a duel. /accept to accept his duel or /decline to decline.");
		%opponent.pendingDuelRequest = %client;
		%opponent.cprsched = schedule(10000, 0, cancelPendingRequest, %opponent);
	}
}

function cancelPendingRequest(%client)
{
	%client.pendingDuelRequest = "";
}

function serverCmdAccept(%client)
{
	cancel(%client.cprsched);
	
	if(%client.pendingDuelRequest)
		beginStake(%client, %client.pendingDuelRequest);
	else
		messageClient(%client, '', "Unable to accept duel request.");
	
	%client.pendingDuelRequest = "";
}

function serverCmdDecline(%client)
{
	cancel(%client.cprsched);
	
	if(%client.pendingDuelRequest)
	{
		messageclient(%client.pendingDuelRequest, '', %client.name @ " has declined your request.");
		%client.pendingDuelRequest = "";
	}
}
