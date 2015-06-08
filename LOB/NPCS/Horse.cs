
//self explanatory
//--
$nodeColor["Horse","headskin"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","chest"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","pants"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","larm"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","rarm"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","lhand"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","rhand"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","lshoe"] = "0.5 0.5 0.5 1";
$nodeColor["Horse","rshoe"] = $nodeColor["Horse","lshoe"];
$nodeColor["Horse","pack"] = $nodeColor["Horse","pants"];
$lob::vision["horse"] = 10;
//$pack["Horse"] = "pack";
//$smiley["Horse"] = "smileyevil1";
//$decal["Horse"] = "knight";
//--

//function the npc will use
$task["Horse"] = "roam";

//for some task, there is an inner task
//$taskInner["Horse"] = "iron";

//if we click the npc, are they ready for dialogue?
$OnClickActionSet["Horse"] = "0";

//roam range
$roam["Horse"] = 0;

//item to be heald
//$equip["Horse"] = "bronzeAxeImage";

//datablock
$LOB::NPC["Horse","Datablock"] = advancedhorsearmor;

//Stuff they say that users can see above the NPC's head
//--
$lob::roamMsgCount["Horse"] = -1;
$lob::roamMsg["Horse",$lob::roamMsgCount["Horse"]++] = "neyyyyyyy.";
$lob::roamMsg["Horse",$lob::roamMsgCount["Horse"]++] = "wrrfffffffbbbbbbb.";
$lob::roamMsg["Horse",$lob::roamMsgCount["Horse"]++] = "ffft ftttttt";
//--

while(isObject(Horse))Horse.delete();

//SO
new scriptObject(Horse);

function Horse::onObjectSpawned(%this,%npc)
{
	%npc.setDatablock(advancedHorseArmor);
	%npc.setscale("0.8 0.8 0.8");
	//callback for when they first spawn
	
	//nothing
}

function Horse::onClick(%this,%ai,%player)
{
	%client = %player.client;
	%slo = %client.slo;
	
}