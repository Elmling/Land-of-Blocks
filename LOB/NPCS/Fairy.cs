if(isObject(fairy))
	fairy.delete();
	
new scriptObject(fairy);
	
function fairy::getVisionMessage(%this,%type,%object)
{
	%message = 0;
	
	if(%type $= "anvil")
	{
		%message = "You can create weapons on anvils.";
		%message = setKeyWords(%message,"anvils.","<color:000000>");
	}
	else
	if(%type $= "rock")
	{
		%message = "You can gather " @ capFirstLetter(%object.name) @" ore from this rock.";
		%message = setKeyWords(%message, capFirstLetter(%object.name),"<color:000000>");
	}
	else
	if(%type $= "tree")
	{
		%message = "You can gather " @ capFirstLetter(%object.name) @ " logs from this tree.";
		%message = setKeyWords(%message, capFirstLetter(%object.name),"<color:000000>");
	}
	else
	if(%type $= "player")
	{
		%message = %object.client.name @ " is currently level " @ %object.client.slo.combatLevel @ ".";
		%message = setKeyWords(%message,%object.client.name,"<color:000000>"); 
	}
	else
	if(%type $= "aiEnemy")
	{
		%mo = %object.getMountedImage(1);
		
		if(isObject(%mo))
		{
			if(strStr(strLwr(%mo.getName()),"bow"))
				%uses = "Range";
			else
				%uses = "Melee";
		}
		else
			%uses = "Melee";

		%message = "This level " @ %object.level SPC %object.name @ " uses " @ %uses @ ".";
		%message = setKeyWords(%message,%object.name SPC %uses,"<color:000000>");
	}
	else
	if(%type $= "aiNpc")
	{
		%message = "You can talk to " @ capFirstLetter(%object.name) @ ".";	
		%message = setKeyWords(%message, capFirstLetter(%object.name),"<color:000000>");
	}
	else
	if(%type $= "other")
	{
		%message = %object.owner.name @ "'s Horse. " @ %object.health @ " HP";
	}
	else
	if(%type $= "furnace")
	{
		%message = "You can smelt ores into bars in the furnace.";
		%message = setKeyWords(%message,%object.name SPC "ores","<color:000000>");
	}
	%message = "Fairy:" @ %message @ "";
	%message = setKeyWords(%message,"Fairy","<color:000000>");
	return "<font:segoe ui light:22>" @ %message;

}