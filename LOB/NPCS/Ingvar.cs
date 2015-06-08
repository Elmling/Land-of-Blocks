$nodeColor["Ingvar","headskin"] = "1 0.8 0.6 1";
$nodeColor["Ingvar","chest"] = "0.654902 0.639216 0.627451 1.000000";
$nodeColor["Ingvar","pants"] = "0.654902 0.639216 0.627451 1.000000";
$nodeColor["Ingvar","larm"] = "0.654902 0.639216 0.627451 1.000000";
$nodeColor["Ingvar","rarm"] = "0.654902 0.639216 0.627451 1.000000";
$nodeColor["Ingvar","lhand"] = "1 0.8 0.6 1";
$nodeColor["Ingvar","rhand"] = "1 0.8 0.6 1";
$nodeColor["Ingvar","lshoe"] = "0.654902 0.639216 0.627451 1.000000";
$nodeColor["Ingvar","rshoe"] = $nodeColor["Ingvar","lshoe"];
//$nodeColor["Ingvar","pack"] = $nodeColor["Ingvar","pants"];
//$pack["Ingvar"] = "pack";
$smiley["Ingvar"] = "smileyold";
$decal["Ingvar"] = "archer";
$task["Ingvar"] = "roam";
$OnClickActionSet["Ingvar"] = true;
$roam["Ingvar"] = 5;
$LOB::NPC["Ingvar","Datablock"] = playerStandardArmor;

$lob::roamMsgCount["Ingvar"] = -1;
$lob::roamMsg["Ingvar",$lob::roamMsgCount["Ingvar"]++] = "Darn, I need some help..";
$lob::roamMsg["Ingvar",$lob::roamMsgCount["Ingvar"]++] = "Hey, hey you!";
$lob::roamMsg["Ingvar",$lob::roamMsgCount["Ingvar"]++] = "Weak guy, over there, help me out!";

while(isObject(Ingvar))Ingvar.delete();

new scriptObject(Ingvar);

function Ingvar::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	
	if(!%slo.ingvar_Q1_Complete)
	{
		if(%slo.ingvar_Q1_p1 $= "")
		{
			%slo.ingvar_Q1_p1 = true;
			%m = "So you're this new guy named " @ %client.name @ " I've been hearing about lately? It seems it is true what they say about you. I can see in your eyes that";
			%m = %m @ " you will become a legend around these parts.\n\n*Press Ok and talk to " @ %ai.name @ " again to continue dialouge*";
		}
		else
		if(%slo.ingvar_Q1_P2 $= "")
		{
			%slo.ingvar_Q1_p2 = true;
			%m = "Anyways " @ %client.name @ ", I need to ask you a favor. Do you think you can collect 20 logs from these Pine Trees? I will pay you 50 gold. I know";
			%m = %m @ " it's not much but I'm sure you could use the experience. Thanks a lot " @ %client.name@ ", come back to me when you have my materials.";
		}
		else
		{
			if(%slo.ItemLogs["Pine"] >= 20)
			{
				Ingvar_Complete_Q1(%client);
				%slo.ingvar_Q1_Complete = true;
				%slo.itemLogs["Pine"] -= 20;
				%client.addToInventory("dataItem","Gold",50);
				%m = "Ahh yes, now I can finish putting together my horse carriage that broke down. I thank you very much " @ %client.name @", here is your gold.";
			}
			else
			{
				%m = "Back already? Well it looks like you are " @ 20 - %slo.ItemLogs["Pine"] @ " logs short of the 20 I need. Keep cutting those trees for me " @ %client.name @ "!";
			}
		}
	}
	else
	{
		%m = "How's life treating you " @ %client.name @ "?";
	}
	
	commandToClient(%client,'messageBoxOk',"Dialouge with Ingvar",%m);
}