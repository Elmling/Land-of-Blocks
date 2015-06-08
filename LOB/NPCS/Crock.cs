$nodeColor["Crock","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["Crock","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Crock","larm"] = "0.392157 0.196078 0 1 1";
$nodeColor["Crock","lhand"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["Crock","lshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Crock","pack"] = "0 0.435323 0.831776 1 1";
$nodeColor["Crock","rarm"] = "0.392157 0.196078 0 1 1";
$nodeColor["Crock","rhand"] = "0.900 0.900 0.900 1.000 1";
$nodeColor["Crock","rshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Crock","chest"] = "0.392157 0.196078 0 1 1";
$smiley["crock"] = "adamSavage";
$decal["crock"] = "linkTunic";
$pack["crock"] = "";
//$pack["Crock"] = none;
$task["Crock"] = "roam";
//$taskInner["Crock"] = "iron";
$OnClickActionSet["Crock"] = "1";
$roam["Crock"] = 0;
//$equip["Crock"] = "goldImage";
$LOB::Crock["Crock","Datablock"] = playerStandardArmor;
$lob::roamMsgCount["Crock"] = -1;
$lob::vision["crock"] = 15;
$lob::roamMsg["Crock",$lob::roamMsgCount["Crock"]++] = "BOWLS BOWLS BOWLS, COME GET THESE BOWLS OF PROSPERITY!";
$lob::roamMsg["Crock",$lob::roamMsgCount["Crock"]++] = "BIG BOWLS, SMALL BOWLS, WIDE BOWLS, TALL BOWLS!";
$lob::roamMsg["Crock",$lob::roamMsgCount["Crock"]++] = "BUY 1 BOWL AND GET 3 BOWLS FOR FREE!";
$lob::roamMsg["Crock",$lob::roamMsgCount["Crock"]++] = "SELLING A RARE BOWL, THE BOWL OF WHITESTONE!";


while(isObject(Crock))Crock.delete();

new scriptObject(Crock);

function Crock::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	%client = %player.client;
	%slo = %client.slo;
	
	%m = "I'm busy.";
	
	%name = %c.name;
	%head = "Dialogue with Crock";
	%m = "Do you enjoy Bowls, " @ %client.name @ "?";
	%um1 = "#string I sure do #command";
	%um2 = "#string No way! #command";
	//%client.Crock = %ai;
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with Crock",%m);
}

