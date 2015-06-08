registerInputEvent("fxDTSBrick", "onAiClicked",  "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

function fxDtsBrick::onAiClicked(%obj,%client)
{
	%ai = %obj.npc;
	
	echo("yes");
}