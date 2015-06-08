
//self explanatory
//--

$nodeColor["Zil","headskin"] = "1 0.603922 0.423529 1 1";
$nodeColor["Zil","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Zil","larm"] = "1 0.603922 0.423529 1 1";
$nodeColor["Zil","lhand"] = "1 0.603922 0.423529 1 1";
$nodeColor["Zil","lshoe"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Zil","pack"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Zil","rarm"] = "1 0.603922 0.423529 1 1";
$nodeColor["Zil","rhand"] = "1 0.603922 0.423529 1 1";
$nodeColor["Zil","rshoe"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Zil","chest"] = "0.000 0.500 0.250 1.000 1";
$decal["Zil"] = "Medieval-Tunic";
$smiley["Zil"] = "ChefSmiley";
$pack["Zil"] = "pack";
$lob::vision["Zil"] = 10;
//--

//function the Zil will use
$task["Zil"] = "roam";

//for some task, there is an inner task
//$taskInner["Zil"] = "iron";

//if we click the Zil, are they ready for dialogue?
$OnClickActionSet["Zil"] = "1";

//roam range
$roam["Zil"] = 0;

//item to be heald
//$equip["Zil"] = "fishingpoleImage";

//datablock
$LOB::Zil["Zil","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the Zil's head
//--
$lob::roamMsgCount["Zil"] = -1;
$lob::roamMsg["Zil",$lob::roamMsgCount["Zil"]++] = "It's a lovely day out here.";
$lob::roamMsg["Zil",$lob::roamMsgCount["Zil"]++] = "Teach a man to fish.. Pay me and I will! Har Har";
$lob::roamMsg["Zil",$lob::roamMsgCount["Zil"]++] = "Fishing relaxes the mind.";
$lob::roamMsgCount++;
//--

while(isObject(Zil))Zil.delete();

//SO
new scriptObject(Zil);

function Zil::onObjectSpawned(%this,%Zil)
{
	//callback for when they first spawn
	
	//nothing
}

function Zil::onClick(%this,%ai,%player)
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
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to Zil.");
		return false;
	}		

	%name = %c.name;
	%head = "Dialogue with Zil";
	%m = "My name is Zil and I like to fish";
	%um1 = "#string How do I fish? #command ZilHowDoIFish";
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with Zil",%m);
}

function serverCmdZilHowDoIFish(%this)
{
	%name = %c.name;
	%head = "Dialogue with Zil";
	%m = "Fishing will be available soon, just wait.";
	%um1 = "#string Okay! #command";
		
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);	
}
