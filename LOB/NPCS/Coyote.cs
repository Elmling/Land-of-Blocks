//THIS IS YOUR NEW ENEMY. BE SURE TO REPLACE Coyote 
//WITH THE CORRECT NAME OF YOUR ENEMY.
//------------------------------------------------------------


//Avatar Data
//------------------------------------------------------------
$nodeColor["Coyote","headskin"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Coyote","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Coyote","larm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Coyote","lhand"] = "0.392157 0.196078 0 1 1";
$nodeColor["Coyote","lshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Coyote","pack"] = "0.392157 0.196078 0 1 1";
$nodeColor["Coyote","rarm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Coyote","rhand"] = "0.392157 0.196078 0 1 1";
$nodeColor["Coyote","rshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Coyote","chest"] = "0.0784314 0.0784314 0.0784314 1 1";
//$decal["Coyote"] = "LinkTunic";
//$smiley["Coyote"] = "asciiTerror";
//--------------------------------------------------------------

//Enemy's total health
$LOB::Enemy["Coyote","Health"] = 150;
//Enemy's Level range
$LOB::Enemy["Coyote","Level"] = "20 40";
//Enemy's datablock
$LOB::Enemy["Coyote","Datablock"] = dogPet;
//Enemy is aggresive or not?
$LOB::Enemy["Coyote","Aggressive"] = 1;
//Enemy's respawn time when killed
$LOB::Enemy["Coyote","RespawnTime"] = 20000;
//Enemy's roam distance
$roam["Coyote"] = 30;
//Enemy's task, view the NPC.cs file
$task["Coyote"] = "Combat";
//Enemy's weapon they will hold/use
$equip["Coyote"] = "";
//Enemy drops gold in a range of x to y
$drop["Coyote","gold"] = "0 90";
//Enemy drops this item when killed with the drop chance specified
//----------------------------------------------------------------
$drop["Coyote","item"] = "mithrilPickaxe";
$itemDropChance["Coyote"] = 30;
//----------------------------------------------------------------
//Enemy drops this material when killed with the drop chance specified
//----------------------------------------------------------------
$drop["Coyote","material"] = "Alyswell Scroll";
$materialDropChance["Coyote"] = 7;
//----------------------------------------------------------------
//Enemy drops this food type when killed with the drop chance specified
//----------------------------------------------------------------
$drop["Coyote","food"] = "Cooked Beef";
$foodDropChance["Coyote"] = 3;
//----------------------------------------------------------------
//View and vision are the same, but both need to be specified
//----------------------------------------------------------------
$view["Coyote"] = 15;
$vision["Coyote"] = 15;
//----------------------------------------------------------------



if(isObject(Coyote))Coyote.delete();

new scriptObject(Coyote);
//This function gets called when the object is created
function Coyote::onObjectSpawned(%this,%Coyote)
{
	//Example:
	//%Coyote.setScale("2 2 2");
	%Coyote.setmovespeed("0.5");
	%coyote.name = "Coyote";
	//the default dog script causes the name to be set wrong
	//so we have a ghetto fix here
	%coyote.schedule(200,setCoyote);
	%this.coyoteRoam();
}

function aiPlayer::setCoyote(%this)
{
	%this.name = "Coyote";
}

function aiPlayer::coyoteRoam(%this)
{
	cancel(%this.roamloop);
	
	
	
	%this.roamLoop = %this.schedule(1000,coyoteRoam);	
}

function aiPlayer::canWalkTo(%this,%position)
{
	%position = %player.position;
	InitContainerRadiusSearch(%position,0.25,$TypeMasks::all);
	while((%targetObject=containerSearchNext()) !$= 0)
	{
		return false;
	}
	
	%eye = %this.getEyePoint();
	%EyeVector = %this.player.getEyeVector();
	%EyePoint = %this.player.getEyePoint();
	%Range = 100;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%rangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType , %this.player);
	%o = getWord(%raycast,0);
	
	return true;
}

