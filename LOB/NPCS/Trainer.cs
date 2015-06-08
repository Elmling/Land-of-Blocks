$nodeColor["Trainer","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["Trainer","pants"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["Trainer","larm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Trainer","lhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Trainer","lshoe"] = "0.0784314 0.0784314 0.0784314 1 1";
//$nodeColor["Trainer","pack"] = "0 0.435323 0.831776 1 1";
$nodeColor["Trainer","rarm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Trainer","rhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["Trainer","rshoe"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Trainer","chest"] = "0.500 0.500 0.500 1.000 1";
$pack["Trainer"] = "";
$smiley["Trainer"] = "smileyevil1";
$decal["Trainer"] = "knight";
$task["Trainer"] = "roam";
//$taskInner["Trainer"] = "iron";
$OnClickActionSet["Trainer"] = "1";
$roam["Trainer"] = 0;
$equip["Trainer"] = "";
$LOB::Trainer["Trainer","Datablock"] = playerStandardArmor;
$lob::roamMsgCount["Trainer"] = -1;
$lob::roamMsg["Trainer",$lob::roamMsgCount["Trainer"]++] = "Click on me, to talk me!";
$lob::roamMsgCount["trainer"]++;
$OnClickActionSet["Trainer"] = "1";

if(isObject(Trainer))
	Trainer.delete();
	
new scriptObject(Trainer)
{
	//quest names
	quest_0 = "info";
	questPath_0 = "base/lob/quests/trainer/info.txt";
	questInfo = 0;
	
	quest_1 = "Hi";
	questPath_1 = "base/lob/quests/trainer/hi.txt";
	questHi = 1;
	
	quest_2 = "test";
	questPath_2 = "base/lob/quests/trainer/test.txt";
	questTest = 2;
};

function Trainer::onClick(%this,%ai,%player)
{	
	%client = %player.client;
	%client.LastNpcTalkedTo = %this;
	serverCmdNpcEvaluateDialogue(%client);
	return;
	%name = %c.name;
	%head = "Dialogue with Trainer";
	%m = "My name is trainer, how may I be of service to you?";
	
	%um1 = "#string Regarding the quest #command npcEvaluateDialogue";
	%um2 = "#string Nevermind. #command";
	
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2);
}
