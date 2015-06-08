$nodeColor["Ogre","headskin"] = "0.541176 0.698039 0.552941 1 1";
$nodeColor["Ogre","pants"] = "0.133333 0.270588 0.270588 1 1";
$nodeColor["Ogre","larm"] = "0.541176 0.698039 0.552941 1 1";
$nodeColor["Ogre","lhand"] = "0.541176 0.698039 0.552941 1 1";
$nodeColor["Ogre","lshoe"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Ogre","pack"] = "0.392157 0.196078 0 1 1";
$nodeColor["Ogre","rarm"] = "0.541176 0.698039 0.552941 1 1";
$nodeColor["Ogre","rhand"] = "0.541176 0.698039 0.552941 1 1";
$nodeColor["Ogre","rshoe"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Ogre","chest"] = "0.133333 0.270588 0.270588 1 1";
$decal["Ogre"] = "Medieval-Tunic";
$smiley["Ogre"] = "asciiTerror";

$LOB::Enemy["ogre","Health"] = 250;
$LOB::Enemy["ogre","Level"] = "20 40";
$LOB::Enemy["ogre","Datablock"] = playerStandardArmor;
$LOB::Enemy["ogre","Aggressive"] = 1;
$LOB::Enemy["ogre","RespawnTime"] = 20000;
$roam["ogre"] = 30;
$task["ogre"] = "Combat";
$equip["ogre"] = "mithrilShortSwordImage";
$drop["ogre","gold"] = "0 80";
$drop["ogre","item"] = "mithrilBow";
$drop["ogre","material"] = "Ordunia Scroll";
$itemDropChance["ogre"] = 70;
$materialDropChance["ogre"] = 7;
$view["ogre"] = 30;
$lob::vision["ogre"] = 30;
$drop["ogre","food"] = "Raw Steak";
$foodDropChance["ogre"] = 3;

if(isObject(ogre))ogre.delete();

new scriptObject(ogre);

function ogre::onObjectSpawned(%this,%Ogre)
{
	%Ogre.setScale("1 1 0.8");
	%Ogre.setmovespeed("1");
}