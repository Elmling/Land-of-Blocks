//Avatar Data
//------------------------------------------------------------
$nodeColor["Onyx","headskin"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Onyx","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Onyx","larm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Onyx","lhand"] = "0.392157 0.196078 0 1 1";
$nodeColor["Onyx","lshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Onyx","pack"] = "0.392157 0.196078 0 1 1";
$nodeColor["Onyx","rarm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Onyx","rhand"] = "0.392157 0.196078 0 1 1";
$nodeColor["Onyx","rshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Onyx","chest"] = "0.0784314 0.0784314 0.0784314 1 1";
$decal["Onyx"] = "LinkTunic";
$smiley["Onyx"] = "asciiTerror";
//--------------------------------------------------------------

//Enemy's total health
$LOB::Enemy["Onyx","Health"] = 150;
//Enemy's Level range
$LOB::Enemy["Onyx","Level"] = "20 40";
//Enemy's datablock
$LOB::Enemy["Onyx","Datablock"] = playerStandardArmor;
//Enemy is aggresive or not?
$LOB::Enemy["Onyx","Aggressive"] = 1;
//Enemy's respawn time when killed
$LOB::Enemy["Onyx","RespawnTime"] = 20000;
//Enemy's roam distance
$roam["Onyx"] = 30;
//Enemy's task, view the NPC.cs file
$task["Onyx"] = "Combat";
//Enemy's weapon they will hold/use
$equip["Onyx"] = "mithrilShortSwordImage";
//Enemy drops gold in a range of x to y
$drop["Onyx","gold"] = "0 80";
//Enemy drops this item when killed with the drop chance specified
//----------------------------------------------------------------
$drop["Onyx","item"] = "adamantiteShortsword";
$itemDropChance["Onyx"] = 30;
//----------------------------------------------------------------
//Enemy drops this material when killed with the drop chance specified
//----------------------------------------------------------------
$drop["Onyx","material"] = "Whitestone Scroll";
$materialDropChance["Onyx"] = 7;
//----------------------------------------------------------------
//Enemy drops this food type when killed with the drop chance specified
//----------------------------------------------------------------
$drop["onyx","food"] = "Raw Lobster";
$foodDropChance["onyx"] = 3;
//----------------------------------------------------------------
//View and vision are the same, but both need to be specified
//----------------------------------------------------------------
$view["Onyx"] = 15;
$vision["Onyx"] = 15;
//----------------------------------------------------------------



if(isObject(onyx))onyx.delete();

new scriptObject(onyx);
//This function gets called when the object is created
function onyx::onObjectSpawned(%this,%Onyx)
{
	%Onyx.setScale("2 2 2");
	%Onyx.setmovespeed("0.5");
}