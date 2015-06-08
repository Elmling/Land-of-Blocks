$nodeColor["Olympus","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Olympus","chest"] = "0 0 0 1.000000";
$nodeColor["Olympus","pants"] = "0 0 0 1.000000";
$nodeColor["Olympus","larm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Olympus","rarm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Olympus","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Olympus","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Olympus","lshoe"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Olympus","rshoe"] = $nodeColor["Olympus","lshoe"];
$nodeColor["Olympus","pack"] = $nodeColor["Olympus","pants"];
$pack["Olympus"] = "";
$smiley["Olympus"] = "smileyevil1";
$decal["Olympus"] = "knight";
$task["Olympus"] = "roam";
//$taskInner["Olympus"] = "iron";
$OnClickActionSet["Olympus"] = "1";
$roam["Olympus"] = 0;
$equip["Olympus"] = "";
$LOB::NPC["Olympus","Datablock"] = playerStandardArmor;
$lob::roamMsgCount["Olympus"] = -1;
$lob::roamMsg["Olympus",$lob::roamMsgCount["Olympus"]++] = "Selling and buying goods!";
$lob::roamMsg["Olympus",$lob::roamMsgCount["Olympus"]++] = "Hey! Let's trade!";
$lob::roamMsg["Olympus",$lob::roamMsgCount["Olympus"]++] = "I've got what you need!.";
$lob::isShopNpc["olympus"] = true;
$lob::vision["olympus"] = 10;

if(isObject(olympus))
	olympus.delete();
	
new scriptObject(olympus);

function olympus::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%clientinv = %client.slo.inventory;
	
	if(!%ai.hasShop)
		%ai.lob_newShop();
	
}