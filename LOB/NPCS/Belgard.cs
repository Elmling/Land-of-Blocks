//self explanatory
//--
$nodeColor["Belgard","headskin"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Belgard","chest"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Belgard","pants"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["Belgard","larm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Belgard","rarm"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Belgard","lhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Belgard","rhand"] = "1 0.870588 0.678431 1.000000";
$nodeColor["Belgard","lshoe"] = "0.501961 0.000000 0.000000 1.000000";
$nodeColor["Belgard","rshoe"] = $nodeColor["Belgard","lshoe"];
$nodeColor["Belgard","pack"] = $nodeColor["Belgard","pants"];
$pack["Belgard"] = "pack";
$smiley["Belgard"] = "smileyevil1";
$decal["Belgard"] = "knight";
//--

//function the npc will use
$task["Belgard"] = "roam";

//for some task, there is an inner task
//$taskInner["Belgard"] = "iron";

//if we click the npc, are they ready for dialogue?
$OnClickActionSet["Belgard"] = "1";

//roam range
$roam["Belgard"] = 0;

//item to be heald
$equip["Belgard"] = PushBroomImage;

//datablock
$LOB::NPC["Belgard","Datablock"] = playerStandardArmor;

$lob::vision["belgard"] = 10;

//Stuff they say that users can see above the NPC's head
//--
$lob::roamMsgCount["Belgard"] = -1;
$lob::roamMsg["Belgard",$lob::roamMsgCount["Belgard"]++] = "Horses for sale!.";
$lob::roamMsg["Belgard",$lob::roamMsgCount["Belgard"]++] = "Need transportation? I have horses!";
$lob::roamMsg["Belgard",$lob::roamMsgCount["Belgard"]++] = "Gallop away on your own horse!";
//--

if(isObject(Belgard))Belgard.delete();

new scriptObject(Belgard);

function Belgard::onObjectSpawned(%this,%npc)
{
	%npc.setScale("1.1 1.1 1.1");
	%npc.setmovespeed("0.5");
	%npc.playthread(0,sit);
}

function Belgard::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	
	%name = %c.name;
	%head = "Dialogue with Belgard";
	%m = "Horses are amazing creatures, don't you agree?";
	%um1 = "#string Yeah I agree, do you have any for sale? #command belgardForSale";
	%um2 = "#string I don't like them. #command belgardDontLikeThem";
	
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdBelgardForSale(%this)
{
	%head = "Dialogue with Belgard";
	%m = "I sure do " @ %this.name @ ", but they aren't cheap. I can sell you a horse for 30,000 gold.";
	%um1 = "#string No problem, I'd like to buy one. #command belgardBuyHorse";
	%um2 = "#string Yikes, nevermind #command";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2);
}

function serverCmdBelgardDontLikeThem(%this)
{
	%head = "Dialogue with Belgard";
	%m = "That's such a tragedy.";
	%um1 = "#string Yep. #command";
	commandToClient(%this,'setdlg',%head,%m,%um1);
}

function serverCmdBelgardBuyHorse(%this)
{
	if(%this.slo.inventory.itemCount["gold"] >= 30000)
	{
		%this.removeFromInventory("gold",30000);
		%this.addToInventory("horse",1);
		
		%head = "Dialogue with Belgard";
		%m = "Thank you " @ %this.name @ ", your horse has 500 HP.";
		%um1 = "#string Thanks Belgard. #command";
		commandToClient(%this,'setdlg',%head,%m,%um1);
	}
	else
	{
		%head = "Dialogue with Belgard";
		%m = "It looks like you are lacking the amount of money needed. Come back when you're ready.";
		%um1 = "#string Okay. #command";
		commandToClient(%this,'setdlg',%head,%m,%um1);	
	}
}