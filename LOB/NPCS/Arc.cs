
//self explanatory
//--
$nodeColor["Arc","headskin"] = "1 0.603922 0.423529 1 1";
$nodeColor["Arc","pants"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Arc","larm"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Arc","lhand"] = "0.392157 0.196078 0 1 1";
$nodeColor["Arc","lshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Arc","pack"] = "0.392157 0.196078 0 1 1";
$nodeColor["Arc","rarm"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Arc","rhand"] = "0.392157 0.196078 0 1 1";
$nodeColor["Arc","rshoe"] = "0.392157 0.196078 0 1 1";
$nodeColor["Arc","chest"] = "0.500 0.500 0.500 1.000 1";
$decal["Arc"] = "worm_engineer";
$smiley["Arc"] = "Jamie";
$pack["arc"] = "none";
$lob::vision["arc"] = 10;
//--

//function the Arc will use
$task["arc"] = "roam";

//for some task, there is an inner task
//$taskInner["arc"] = "iron";

//if we click the Arc, are they ready for dialogue?
$OnClickActionSet["arc"] = "1";

//roam range
$roam["arc"] = 1;

//item to be heald
$equip["arc"] = "adamantitePickAxeImage";

//datablock
$LOB::Arc["arc","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the Arc's head
//--
$lob::roamMsgCount["arc"] = -1;
$lob::roamMsg["arc",$lob::roamMsgCount["arc"]++] = "All we do is mine, mine, mine!";
$lob::roamMsg["arc",$lob::roamMsgCount["arc"]++] = "Mining builds character!.";
$lob::roamMsg["arc",$lob::roamMsgCount["arc"]++] = "Hit that rock with that pick axe!";
$lob::roamMsgCount++;
//--

while(isObject(arc))arc.delete();

//SO
new scriptObject(arc);

function arc::onObjectSpawned(%this,%Arc)
{
	//callback for when they first spawn
	
	//nothing
}

function arc::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	
	%m = "I'm busy.";

	if(!%client.slo.hasGui)
	{
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to ArcS.");
		return false;
	}

	%name = %c.name;
	%head = "Dialogue with arc";
	%m = "My name is arc, do you need a pickaxe?";
	%um1 = "#string Yes? #command arcFreePickAxe";
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with arc",%m);
}

function serverCmdArcFreePickAxe(%client)
{
	%dt = getDateTime();
	%dt = getWord(%dt,0);
	
	%name = %c.name;
	%head = "Dialogue with arc";
	
	if(%client.slo.lastFreePickAxe $= %dt)
	{
		%m = "I've already given you a free pickaxe today, try again tomorrow.";
		%um1 = "#string Ok #command";		
	}
	else
	{
		%client.slo.lastFreePickAxe = %dt;
		%client.slo.inventory.itemCountBronzePickAxe+=1;
		%m = "I'll give you a pickaxe today, but you only get 1 for today, don't lose it!";
		%um1 = "#string I won't! #command";		
	}
	
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
}
