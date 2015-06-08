$nodeColor["Banker","headskin"] = "0.803922 0.666667 0.486275 1.000000";
$nodeColor["Banker","chest"] = "0.729412 0.721569 0.701961 1.000000";
$nodeColor["Banker","pants"] = "0.729412 0.721569 0.701961 1.000000";
$nodeColor["Banker","larm"] = "0.729412 0.721569 0.701961 1.000000";
$nodeColor["Banker","rarm"] = "0.729412 0.721569 0.701961 1.000000";
$nodeColor["Banker","lhand"] = "0 0 0 1.000000";
$nodeColor["Banker","rhand"] = "0 0 0 1";
$nodeColor["Banker","lshoe"] = "0 0 0 1.000000";
$nodeColor["Banker","rshoe"] = $nodeColor["Banker","lshoe"];
//$nodeColor["Banker","pack"] = $nodeColor["Banker","pants"];
//$pack["Banker"] = "pack";
$smiley["Banker"] = "male07smiley";
$decal["Banker"] = "";
$task["Banker"] = "roam";
$equip["Banker"] = "";
$OnClickActionSet["Banker"] = true;
$roam["Banker"] = 0;
$LOB::NPC["Banker","Datablock"] = playerStandardArmor;
$lob::vision["banker"] = 15;

$lob::roamMsgCount["Banker"] = -1;
$lob::roamMsg["Banker",$lob::roamMsgCount["Banker"]++] = "Bank your items here!";
$lob::roamMsg["Banker",$lob::roamMsgCount["Banker"]++] = "Hey you, you should bank your items over here.";
$lob::roamMsg["Banker",$lob::roamMsgCount["Banker"]++] = "Click on me to bank your items!";

while(isObject(Banker))Banker.delete();

new scriptObject(Banker);

function Banker::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	%ain = %ai.name;
	
	%client.bankTeller = %ai;
	commandToClient(%client,'openBank');
}