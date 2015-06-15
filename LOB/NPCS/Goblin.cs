$nodeColor["Goblin","headskin"] = "0.000000 0.501961 0.231373 1.000000";
$nodeColor["Goblin","chest"] = "0.000000 0.501961 0.231373 1.000000";
$nodeColor["Goblin","pants"] = "0.537255 0.694118 0.549020 1.000000";
$nodeColor["Goblin","larm"] = "0.000000 0.501961 0.231373 1.000000";
$nodeColor["Goblin","rarm"] = "0.000000 0.501961 0.231373 1.000000";
$nodeColor["Goblin","lhand"] = "0.000000 0.501961 0.231373 1.000000";
$nodeColor["Goblin","rhand"] = "0.000000 0.501961 0.231373 1.000000";
$nodeColor["Goblin","lshoe"] = "1.000000 0.000000 0.000000 1.000000";
$nodeColor["Goblin","rshoe"] = $nodeColor["Goblin","lshoe"];
//$nodeColor["Goblin","pack"] = $nodeColor["Goblin","pants"];
//$pack["Goblin"] = "pack";
$smiley["Goblin"] = "orc";
$decal["Goblin"] = "knight";
$LOB::Enemy["Goblin","Health"] = 60;
$LOB::Enemy["Goblin","Level"] = "12 15";
$LOB::Enemy["Goblin","Datablock"] = playerStandardArmor;
$LOB::Enemy["Goblin","Aggressive"] = 1;
$LOB::Enemy["Goblin","RespawnTime"] = 20000;
$roam["Goblin"] = 50;
$task["Goblin"] = "arch";
$equip["Goblin"] = "bronzeBowImage";//"ironShortSwordImage";
$drop["Goblin","gold"] = "15 30";
$drop["Goblin","item"] = "bronzeBowImage";
$drop["Goblin","material"] = "logsOak";
$dropName["Goblin","material"] = "Oak Logs";
$itemDropChance["Goblin"] = 7;
$materialDropChance["Goblin"] = 3;
$vision["Goblin"] = 50;

$drop["goblin","food"] = "Raw Steak";
$foodDropChance["Goblin"] = 3;

if(isObject(Goblin))Goblin.delete();

new scriptObject(Goblin);

function Goblin::onObjectSpawned(%this,%npc)
{
	%npc.setmovespeed(1.3);
	%npc.setScale("1.5 1.5 1.5");
	%npc.jumpForce = 16;
	%npc.removeTreeCollisionLoop();
}


