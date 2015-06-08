package Avatar
{
	function GameConnection::applyBodyParts(%client,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l)
	{
		if(%client.overRideParts)
		{
			%client.overRideParts = "";
			parent::applyBodyParts(%client,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l);
		}
	}
	
	function GameConnection::applyBodyColors(%client,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l)
	{
		if(%client.overRideColors)
		{
			%client.overRideColors = "";
			parent::applyBodyParts(%client,%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l);
		}	
	}
};
activatePackage(avatar);

function equipPlayer(%player,%this)
{
	if(!isObject(%this))
	{
		if(!isObject(%this@"image"))
		{
			echo("Could not equip "@%this@".");
			return 0;
		}
		else
		%this = %this@"image";
	}
	
	%type = %player.getclassname();
	
	if(%type $= "aiPlayer")
	{
		%player.mountImage(%this,1);
		%player.setArmThread("armReadyRight");
	}
}

function dressPlayer(%player,%this)
{
	hideallnodes(%player);
	%a=-1;
	%node[%a++] = "headskin";
	%node[%a++] = "chest";
	%node[%a++] = "larm";
	%node[%a++] = "rarm";
	%node[%a++] = "lhand";
	%node[%a++] = "rhand";
	%node[%a++] = "pants";
	%node[%a++] = "lshoe";
	%node[%a++] = "rshoe";
	%node[%a++] = $pack[%this];
	%node[%a++] = $hat[%this];
	%node[%a++] = $chest[%this];
	//%node[%a++] = "cape";
	
	if(%this !$= "citizen")
	{
		%color["headskin"] = $nodeColor[%this,"headskin"];
		%color["chest"] = $nodeColor[%this,"chest"];
		%color["pants"] = $nodeColor[%this,"pants"];
		%color["larm"] = $nodeColor[%this,"larm"];
		%color["rarm"] = $nodeColor[%this,"rarm"];
		%color["lhand"] = $nodeColor[%this,"lhand"];
		%color["rhand"] = $nodeColor[%this,"rhand"];
		%color["lshoe"] = $nodeColor[%this,"lshoe"];
		%color["rshoe"] = $nodeColor[%this,"rshoe"];
		%color["pack"] = $nodeColor[%this,"pack"];
		%color["quiver"] = $nodeColor[%this,"pack"];
		%color["cape"] = $nodeColor[%this,"pack"];
		%color["helmet"] = $nodeColor[%this,"pack"];
		%color[$hat[%this]] = $nodeColor[%this,"pack"];
		%color[$chest[%this]] = $nodecolor[%this,"pack"];
	}
	else
	{
		%color["headskin"] = $nodeColor["citizen","headskin"];
		%color["chest"] = lob_getRandomAvatarColor() SPC lob_getRandomAvatarColor() SPC lob_getRandomAvatarColor() SPC "1";
		%color["pants"] = lob_getRandomAvatarColor() SPC lob_getRandomAvatarColor() SPC lob_getRandomAvatarColor() SPC "1";
		%color["larm"] = %color["chest"];
		%color["rArm"] = %color["chest"];
		%color["lhand"] = %color["headskin"];
		%color["rhand"] = %color["headskin"];
		%color["lshoe"] = lob_getRandomAvatarColor() SPC lob_getRandomAvatarColor() SPC lob_getRandomAvatarColor() SPC "1";
		%color["rshoe"] = %color["lshoe"];
	}
	
	//%pack = $pack[%this];
	%decal = $decal[%this];
	%smiley = $smiley[%this];
	
	
	for(%i=0;%i<%a+1;%i++)
	{
		%n = %node[%i];
		
		if(%color[%n] $= "")continue;
		
		%player.unhidenode(%n);
		%player.setnodecolor(%n,%color[%n]);
	}
	
	%player.setFaceName(%smiley);
	%player.setDecalName(%decal);
}

function lob_getRandomAvatarColor()
{
	%color = getRandom(2,4);
	
	if(%color $= "10")
		%color = "1.0";
	else
		%color = "0." @ %color;
		
	return %color;
}

function serverCmdPopulateAvatar(%client)
{
	%donation = %client.slo.donation;
	
	%count = %donation.avatarCount;
	
	for(%i=0;%i<%count+1;%i++)
	{
		%av = %donation.avatar[%i];
		commandToClient(%client,'catchAvatarData',%i,%av);
	}
}

function serverCmdgetAvatarInfo(%client,%text)
{
	%m = $lob::avatarDescription[%text];
	commandToClient(%client,'catchAvatarDescription',%m);
}

function serverCmdAvatar(%this,%id)
{
	%time = getSimTime();
	if(%time - %this.lastAvatarChangeTime <= 5000)
	{
		smartMessage(%this,"\c6There is a \c05\c6 second timeout between switching avatars.",5000);
		return false;
	}
	
	if(%this.slo.donation.avatar[%id] !$= "")
	{
		messageClient(%this,'',"\c6You are now using avatar: " @ %this.slo.donation.avatar[%id]);
		%this.customAvatar = %id;
		dressPlayer(%this.player,%this.slo.donation.avatar[%id]);
		%this.lastAvatarChangeTime = %time;
	}
}

function lob_giveAvatar(%player,%name)
{
	%name = capFirstLetter(%name);

	if(!isObject(%player))
		return false;
		
	%time = getRealTime();
	%i=-1;
	while(true)
	{
		%i++;
		%tag = %player.slo.donation.avatar[%i];
		if(%tag $= %name)
			return false;
			
		if(%tag $= "")
			break;
			
		if(getRealTime() - %time >= 50)
			return false;
	}
	
	messageClient(%player,'',"\c6" @ %player.name @ ", you've recieved the \c2" @ capFirstLetter(%name) @ "\c6 avatar!");
	%player.slo.donation.avatar[%i] = %name;
	%player.slo.donation.avatarCount++;
}

function serverCmdGiveAvatar(%this,%player,%name)
{
	%name = capFirstLetter(%name);
	if(%this.isAdmin)
	{
		%player = findclientbyname(%player);
		if(!isObject(%player))
			return false;
			
		%time = getRealTime();
		%i=-1;
		while(true)
		{
			%i++;
			%tag = %player.slo.donation.avatar[%i];
			if(%tag $= %name)
				return false;
				
			if(%tag $= "")
				break;
				
			if(getRealTime() - %time >= 50)
				return false;
		}
		
		messageClient(%player,'',"\c6" @ %player.name @ ", you've recieved the \c2" @ capFirstLetter(%name) @ "\c6 avatar!");
		messageClient(%this,'',"\c6Avatar has been given");
		%player.slo.donation.avatar[%i] = %name;
		%player.slo.donation.avatarCount++;
	}
}