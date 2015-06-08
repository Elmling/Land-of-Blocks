$nodeColor["Evelyn","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Evelyn","chest"] = "0.9 0.5 0.6 1";
$nodeColor["Evelyn","pants"] = "0.9 0.5 0.6 1";
$nodeColor["Evelyn","larm"] = "0.9 0.5 0.6 1";
$nodeColor["Evelyn","rarm"] = "0.9 0.5 0.6 1";
$nodeColor["Evelyn","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Evelyn","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Evelyn","lshoe"] = "0.07 0.07 0.07 1";
$nodeColor["Evelyn","rshoe"] = $nodeColor["Evelyn","lshoe"];
//$nodeColor["Evelyn","pack"] = $nodeColor["Evelyn","pants"];
//$pack["Evelyn"] = "";
$smiley["Evelyn"] = "smileyfemale1";
$decal["Evelyn"] = "";
$task["Evelyn"] = "roam";
//$taskInner["Evelyn"] = "iron";
$OnClickActionSet["Evelyn"] = "";
$roam["Evelyn"] = 5;
$equip["Evelyn"] = "";
$LOB::NPC["Evelyn","Datablock"] = playerStandardArmor;
//$lob::roamMsgCount["Evelyn"] = -1;
//$lob::roamMsg["Evelyn",$lob::roamMsgCount["Evelyn"]++] = "Greetings adventurer, I have some useful information for you.";
//$lob::roamMsg["Evelyn",$lob::roamMsgCount["Evelyn"]++] = "Hey you, come over here.";
//$lob::roamMsg["Evelyn",$lob::roamMsgCount["Evelyn"]++] = "I have something I need you to do.";

while(isObject(Evelyn))Evelyn.delete();

new scriptObject(Evelyn);

function Evelyn::onClick(%this,%ai,%player)
{

}