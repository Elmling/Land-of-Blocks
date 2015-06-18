//THIS IS YOUR NEW Gambler. BE SURE TO REPLACE Gambler 
//WITH THE CORRECT NAME OF YOUR Gambler.
//------------------------------------------------------------


//self explanatory
//--
$nodeColor["Gambler","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Gambler","chest"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Gambler","pants"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Gambler","larm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Gambler","rarm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Gambler","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Gambler","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Gambler","lshoe"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Gambler","rshoe"] = $nodeColor["Gambler","lshoe"];
$nodeColor["Gambler","pack"] = $nodeColor["Gambler","pants"];
$pack["Gambler"] = "pack";
$smiley["Gambler"] = "smileyevil1";
$decal["Gambler"] = "knight";
$lob::vision["Gambler"] = 10;
//--

//function the Gambler will use
$task["Gambler"] = "roam";

//for some task, there is an inner task
//$taskInner["Gambler"] = "iron";

//if we click the Gambler, are they ready for dialogue?
$OnClickActionSet["Gambler"] = "1";

//roam range
$roam["Gambler"] = 5;

//item to be heald
$equip["Gambler"] = "";

//datablock
$LOB::Gambler["Gambler","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the Gambler's head
//--
$lob::roamMsgCount["Gambler"] = -1;
$lob::roamMsg["Gambler",$lob::roamMsgCount["Gambler"]++] = "Hey, want to earn some money?";
$lob::roamMsgCount++;
//--

while(isObject(Gambler))Gambler.delete();

//SO
new scriptObject(Gambler);

function Gambler::onObjectSpawned(%this,%Gambler)
{
	//callback for when they first spawn
	
	//nothing
}

function Gambler::onClick(%this,%ai,%player)
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
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to GamblerS.");
		return false;
	}
}	

function serverCmdGamble(%this)
{
	%npc = %this.vision.lookingat;
	talk(%npc.getclassname())
		
}