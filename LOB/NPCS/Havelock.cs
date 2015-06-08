$nodeColor["havelock","headskin"] = "1 0.8 0.6 1";
$nodeColor["havelock","chest"] = "0.078 0.078 0.078 1";
$nodeColor["havelock","pants"] = "0.9 0.9 0.9 1";
$nodeColor["havelock","larm"] = "0.9 0.9 0.9 1";
$nodeColor["havelock","rarm"] = "0.9 0.9 0.9 1";
$nodeColor["havelock","lhand"] = "1 0.8 0.6 1";
$nodeColor["havelock","rhand"] = "1 0.8 0.6 1";
$nodeColor["havelock","lshoe"] = "0.9 0.9 0.9 1";
$nodeColor["havelock","rshoe"] = $nodeColor["havelock","lshoe"];
//$nodeColor["havelock","pack"] = $nodeColor["havelock","pants"];
//$pack["havelock"] = "pack";
$smiley["havelock"] = "smileyevil1";
$decal["havelock"] = "archer";
$task["havelock"] = "roam";
$OnClickActionSet["haveLock"] = true;
$roam["Havelock"] = 5;
$LOB::NPC["Havelock","Datablock"] = playerStandardArmor;


while(isObject(havelock))havelock.delete();

new scriptObject(havelock);

function havelock::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	%client = %player.client;
	
	commandToClient(%client,'messageBoxOk',"Dialouge with Havelock",%m);
}