//THIS IS YOUR NEW DeadBody. BE SURE TO REPLACE DeadBody 
//WITH THE CORRECT NAME OF YOUR DeadBody.
//------------------------------------------------------------


//self explanatory
//--
$nodeColor["DeadBody","headskin"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","pants"] = "0.392157 0.196078 0 1 1";
$nodeColor["DeadBody","larm"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","lhand"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","lshoe"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","pack"] = "0.750 0.750 0.750 1.000 1";
$nodeColor["DeadBody","rarm"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","rhand"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","rshoe"] = "0.956863 0.878431 0.784314 1 1";
$nodeColor["DeadBody","chest"] = "0.956863 0.878431 0.784314 1 1";
$decal["DeadBody"] = "HCZombie";
$smiley["DeadBody"] = "asciiTerror";
$lob::vision["DeadBody"] = 10;
//--

//function the DeadBody will use
$task["DeadBody"] = "roam";

//for some task, there is an inner task
//$taskInner["DeadBody"] = "iron";

//if we click the DeadBody, are they ready for dialogue?
$OnClickActionSet["DeadBody"] = "1";

//roam range
$roam["DeadBody"] = 0;

//item to be heald
$equip["DeadBody"] = "";

//datablock
$LOB::DeadBody["DeadBody","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the DeadBody's head
//--

//--

while(isObject(DeadBody))DeadBody.delete();

//SO
new scriptObject(DeadBody);

function DeadBody::onObjectSpawned(%this,%DeadBody)
{
	//callback for when they first spawn
	%deadBody.setAimLocation(getWords(%deadBody.position,0,1) SPC 1000);
	%deadbody.playthread(0,sit);
	//nothing
}

function DeadBody::onClick(%this,%ai,%player)
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
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to DeadBodyS.");
		return false;	
	}
}	