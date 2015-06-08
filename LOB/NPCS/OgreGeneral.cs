$nodeColor["OgreGeneral","headskin"] = "0.000 0.500 0.250 1.000 1";
$nodeColor["OgreGeneral","pants"] = "0.200 0.200 0.200 1.000 1";
$nodeColor["OgreGeneral","larm"] = "0.000 0.500 0.250 1.000 1";
$nodeColor["OgreGeneral","lhand"] = "0.000 0.500 0.250 1.000 1";
$nodeColor["OgreGeneral","lshoe"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["OgreGeneral","pack"] = "0 0.435323 0.831776 1 1";
$nodeColor["OgreGeneral","rarm"] = "0.000 0.500 0.250 1.000 1";
$nodeColor["OgreGeneral","rhand"] = "0.000 0.500 0.250 1.000 1";
$nodeColor["OgreGeneral","rshoe"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["OgreGeneral","chest"] = "0.0784314 0.0784314 0.0784314 1 1";
$smiley["OgreGeneral"] = "orc";
$decal["OgreGeneral"] = "linkTunic";
$pack["OgreGeneral"] = "";
//$pack["OgreGeneral"] = none;
$task["OgreGeneral"] = "roam";
//$taskInner["OgreGeneral"] = "iron";
$OnClickActionSet["OgreGeneral"] = "1";
$roam["OgreGeneral"] = 0;
//$equip["OgreGeneral"] = "goldImage";
$LOB::OgreGeneral["OgreGeneral","Datablock"] = playerStandardArmor;
$equip["ogreGeneral"] = "bronzeSpearImage";


while(isObject(OgreGeneral))OgreGeneral.delete();

new scriptObject(OgreGeneral);

function ogreGeneral::onObjectSpawned(%this,%OgreGeneral)
{
	%OgreGeneral.setScale("2 2 2");
	%OgreGeneral.setmovespeed("0.5");
}

function OgreGeneral::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	%client = %player.client;
	%slo = %client.slo;
	
	%m = "I'm busy.";
	
	if(%client.slo.robin_q1_completed && !%client.slo.robin_q2_completed)
	{
		if(%client.slo.robin_q2_foundClue1)
		{
			%name = %c.name;
			%head = "Dialogue with OgreGeneral";
			%m = "What do you want " @ %client.name @ "?";
			%um1 = "#string Tell your ogres to quit messing around with Ordunia's Well! #command robinQuitMessingWithWell";
		}
		else
		{
			%name = %c.name;
			%head = "Dialogue with OgreGeneral";
			%m = "Leave me be humann!";
			%um1 = "#string I was just leaving.. #command";
			//%client.OgreGeneral = %ai;		
		}
	}
	else
	{
		%name = %c.name;
		%head = "Dialogue with OgreGeneral";
		%m = "Leave me be human!";
		%um1 = "#string I was just leaving.. #command";
		//%client.OgreGeneral = %ai;
	}
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with OgreGeneral",%m);
}
