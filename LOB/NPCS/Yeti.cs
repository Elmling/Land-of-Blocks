$nodeColor["Yeti","headskin"] = "";
$nodeColor["Yeti","chest"] = "";
$nodeColor["Yeti","pants"] = "";
$nodeColor["Yeti","larm"] = "";
$nodeColor["Yeti","rarm"] = "";
$nodeColor["Yeti","lhand"] = "";
$nodeColor["Yeti","rhand"] = "";
$nodeColor["Yeti","lshoe"] = "";
$nodeColor["Yeti","rshoe"] = $nodeColor["Yeti","lshoe"];
//$nodeColor["Yeti","pack"] = $nodeColor["Yeti","pants"];
//$pack["Yeti"] = "pack";
//$smiley["Yeti"] = "smiley-evil2";
//$decal["Yeti"] = "archer";
$LOB::Enemy["Yeti","Health"] = 300;
$LOB::Enemy["Yeti","Level"] = "30 40";
$LOB::Enemy["Yeti","Datablock"] = yetiArmor;
$LOB::Enemy["Yeti","Aggressive"] = 1;
$LOB::Enemy["Yeti","RespawnTime"] = 20000;
$roam["Yeti"] = 20;
$task["Yeti"] = "Combat";
$equip["Yeti"] = "";
$drop["Yeti","gold"] = "76 125";
$drop["Yeti","item"] = "mithril javlin";
$drop["Yeti","material"] = "oak logs";
$itemDropChance["Yeti"] = 13;
$materialDropChance["Yeti"] = 7;
$drop["Yeti","food"] = "Raw lobster";
$foodDropChance["Yeti"] = 2;
$lob::vision["Yeti"] = 20;

if(isObject(Yeti))Yeti.delete();

new scriptObject(Yeti)
{
		class="Yeti";
};

function Yeti::onObjectSpawned(%this,%Yeti)
{
	%Yeti.setScale("2 2 1.5");
	%Yeti.setmovespeed("0.2");
	%Yeti.removeTreeCollisionLoop();
}