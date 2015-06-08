//registerOutputEvent("CLASS NAME", "FUNCTION", "argumenttype BOUNDS OR SETTINGS	argumenttype BOUNDS OR SETTINGS	...", APPENDCLIENT)

RegisterOutPutEvent("fxDtsBrick", "lob_event_Set_Dialogue","String 100 250",1);

function fxdtsBrick::lob_Event_set_dialogue(%this,%string,%client)
{
	commandToClient(%client,'setDlg',%client.name,%string);
}

RegisterOutPutEvent("fxDtsBrick", "lob_event_RunRouteForward","String 100 250",1);

function fxDtsBrick::lob_event_RunRouteForward(%this,%string,%client)
{
	
	if(isObject(%this.npc))
	{
		echo("running");
		%this.npc.runBrickRouteForward(%string);
	}
}

RegisterOutPutEvent("fxDtsBrick", "lob_event_SlowlyRunRouteForward","String 100 250",1);

function fxDtsBrick::lob_event_slowlyRunRouteForward(%this,%string,%client)
{
	
	if(isObject(%this.npc))
	{
		echo("running");
		%this.npc.SlowlyrunBrickRouteForward(%string);
	}
}