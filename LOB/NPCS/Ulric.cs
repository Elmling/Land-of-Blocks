
//self explanatory
//--
$nodeColor["Ulric","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Ulric","chest"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Ulric","pants"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Ulric","larm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Ulric","rarm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Ulric","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Ulric","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Ulric","lshoe"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Ulric","rshoe"] = $nodeColor["Ulric","lshoe"];
$nodeColor["Ulric","pack"] = $nodeColor["Ulric","pants"];
$pack["Ulric"] = "pack";
$smiley["Ulric"] = "smileyevil1";
$decal["Ulric"] = "knight";
//--

//function the npc will use
$task["Ulric"] = "roam";

//for some task, there is an inner task
//$taskInner["Ulric"] = "iron";

//if we click the npc, are they ready for dialogue?
$OnClickActionSet["Ulric"] = "1";

//roam range
$roam["Ulric"] = 5;

//item to be heald
$equip["Ulric"] = "bronzeAxeImage";

//datablock
$LOB::NPC["Ulric","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the NPC's head
//--
$lob::roamMsgCount["Ulric"] = -1;
$lob::roamMsg["Ulric",$lob::roamMsgCount["Ulric"]++] = "Are you interested in leading your own guild?";
$lob::roamMsg["Ulric",$lob::roamMsgCount["Ulric"]++] = "I manage guilds!";
$lob::roamMsg["Ulric",$lob::roamMsgCount["Ulric"]++] = "Guilds, guilds and more guilds!";
//--

while(isObject(Ulric))Ulric.delete();

//SO
new scriptObject(Ulric);

function Ulric::onObjectSpawned(%this,%npc)
{

}

function Ulric::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	
	%m = "I'm busy.";

	if(!%client.slo.hasGui)
	{
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to NPCS.");
		return false;
	}		

	%name = %c.name;
	%head = "Dialogue with Ulric";
	%m = "My name is Ulric, what would you like to talk about??";
	%um1 = "#View guilds #command UlricViewGuilds";
	%um2 = "#Create a Guild #command UlricCreateAGuild";
	%um3 = "#string Nothing #command";
	
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3);
}

function serverCmdUlricViewGuilds(%client)
{

}

function serverCmdUlricCreateAGuild(%client)
{
	if(%client.slo.guild $= "")
	{

	}
}

function serverCmdCreateGuild(%client,%guild)
{
	if(%client.slo.guild $= "")
	{
		if(%client.slo.inventory.itemCount["gold"] >= 500000)
		{
			if(%client.slo.combatLevel >= 50)
			{
				if(lob_isCleanGuildName(%guild))
				{
					if(pullIntFromString(%guild) $= "NULL")
					{
						if(strLen(%guild) <= 12)
						{
							if(!lob_guildContainsSpaces(%guild))
							{
								%client.removeFromInventory("gold",500000);
								%client.slo.GuildLeader = true;
								%client.slo.guild = %guild;
								%m = setKeyWords("\c6" @ %client.name @ " has successfully created the guild " @ %guild @ ".",%client.name SPC "successfully" SPC %guild,"\c6");
								messageAll('',%m);
								%f = new fileObject();
								%f.openForWrite("base/lob/guilds/" @ %guild @ "/Owner.txt");
								%f.writeLine(%client.bl_id SPC %client.name);
								%f.close();
								%f.delete();
							}
							else
							{
								%m = setKeyWords("\c6Your guild cannot contain spaces.","cannot spaces","\c6");
								messageClient(%client,'',%m);
							}
						}
						else
						{
							%m = setKeyWords("\c6Your guild cannot be more than 12 letters.","cannot 12","\c6");
							messageClient(%client,'',%m);
						}
					}
					else
					{
						%m = setKeyWords("\c6Your guild name cannot contain numbers.","guild name numbers","\c6");
						messageClient(%client,'',%m);
					}
				}
				else
				{
					%m = setKeyWords("\c6You cannot have profanity in your guild name.","profanity","\c6");
					messageClient(%client,'',%m);
				}	
			}
			else
			{
				%m = setKeyWords("\c6You need to be atleast level 50 to create a guild.","50 create guild","\c6");
				messageClient(%client,'',%m);
			}
		}
		else
		{
			%m = setKeyWords("\c6You need atleast 500k to create a guild.","500k create guild","\c6");
			messageClient(%client,'',%m);
		}
	}
}

package lob_npc_ulric
{
	function gameConnection::onDrop(%client)
	{
		if(%client.requestedGuildname !$= "")
		{
			if(isFile("base/lob/guilds/"@ %client.requestedGuildName @ "/Pending Requests/" @ %client.bl_id @ ".txt"))
			{
				fileDelete("base/lob/guilds/"@ %client.requestedGuildName @ "/Pending Requests/" @ %client.bl_id @ ".txt");
			}
		}
		return parent::onDrop(%client);
	}
};
activatePackage(lob_npc_ulric);

function serverCmdJoinGuildRequest(%client,%client2)
{
	if(%client.slo.guild !$= "")
	{
		return false;
	}
	else
	if(%client2.slo.guild $= "")
	{
		return false;
	}
	else
	if(%client.slo.guild $= %client2.slo.guild)
		return false;

	if(%client2.slo.guildLeader || %client2.slo.guildAdmin)
	{
		%client.requestedguildname = %client2.slo.guild;
		%f = new fileObject();
		%f.openForWrite("base/lob/guilds/" @ %client2.slo.guild @ "/Pending Requests/" @ %client.bl_id @ ".txt");
		%f.writeLine(%client.bl_id SPC %client.name);
		%f.close();
		%f.delete();
		%m = setkeywords("\c6Guild Join Request Sent","","\c6");
		messageclient(%client,'',%m);
	}
	else
	{
		return false;
	}
}

function gameConnection::joinGuild(%this,%guild)
{
	%this.slo.guild = %guild;
	if(isFile("base/lob/guilds/"@ %this.requestedGuildName @ "/Pending Requests/" @ %this.bl_id @ ".txt"))
	{
		fileDelete("base/lob/guilds/"@ %this.requestedGuildName @ "/Pending Requests/" @ %this.bl_id @ ".txt");
	}
}	

function lob_guildContainsSpaces(%name)
{
	%len = strLen(%name)-1;
	
	for(%i=0;%i<%len;%i++)
	{
		%l = getSubStr(%name,0,%i);
		if(%l $= " ")
			return true;
	}
	
	return false;
}

function lob_isCleanGuildName(%name)
{
	%words = "fuck shit dick bitch hoe nigga nigger negro pussy whore ! @ # $ % ^ & * ( ) .";
	
	for(%i=0;%i<%words;%i++)
	{
		%w = getWord(%words,%i);
		
		if(strStr(strLwr(%name),%w) >= 0)
			return false;
	}
	
	return true;
}
