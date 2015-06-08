$nodeColor["Lilly","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["Lilly","pants"] = "0.92549 0.513726 0.678431 1 1";
$nodeColor["Lilly","larm"] = "0.74902 0.180392 0.482353 1 1";
$nodeColor["Lilly","lhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Lilly","lshoe"] = "0.74902 0.180392 0.482353 1 1";
$nodeColor["Lilly","pack"] = "0.74902 0.180392 0.482353 1 1";
$nodeColor["Lilly","rarm"] = "0.74902 0.180392 0.482353 1 1";
$nodeColor["Lilly","rhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Lilly","rshoe"] = "0.74902 0.180392 0.482353 1 1";
$nodeColor["Lilly","chest"] = "0.74902 0.180392 0.482353 1 1";
$smiley["Lilly"] = "smileyFemale1";
$decal["Lilly"] = "Medieval-Tunic";
$pack["Lilly"] = "";
$hat["lilly"] = "knithat";
$task["Lilly"] = "roam";
//$taskInner["Lilly"] = "iron";
$OnClickActionSet["Lilly"] = "1";
$roam["Lilly"] = 0;
//$equip["Lilly"] = "goldImage";
$LOB::Lilly["Lilly","Datablock"] = playerStandardArmor;
$equip["Lilly"] = "";


while(isObject(Lilly))Lilly.delete();

new scriptObject(Lilly)
{
	//quest name
	quest_0 = "Hey";
	//quest path
	questPath_0 = "base/lob/quests/Lilly/Hey.txt";
	//quest index
	questHey = 0;
};

function Lilly::onObjectSpawned(%this,%Lilly)
{
	%lilly.setScale("0.8 0.8 0.8");
	//%Lilly.setmovespeed("0.5");
}

function Lilly::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}

	%client = %player.client;
	%client.LastNpcTalkedTo = %this;
	serverCmdNpcEvaluateDialogue(%client);
}
