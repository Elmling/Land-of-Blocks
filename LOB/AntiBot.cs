package lob_antiBot
{
	function fxDtsBrick::onPlayerTouch(%a,%b,%c)
	{
		%bn = %a.getName();
		if(%bn $= "_respawn")
		{
			%spawn = "_antibot_spawn";
			%pos = %spawn.position;
			%b.setVelocity("0 0 0");
			%b.setTransform(%pos);
		}
		else
		if(%bn $= "_endAntiBot")
		{
			%time = getSimTime();
			if(%time - %b.client.lastrandomevent >= 2000 && %b.client.randomEvent)
			{
				%b.client.randomEvent = "";
				%b.setTransform(%b.client.preRandomEventPos);
				%b.client.lastRandomEvent = %time;
				eval("%lvl = %b.client.slo." @ %b.client.randomEventReward @ "level;");
				%exp = 100 * %lvl;
				giveExp(%b.client,%b.client.randomEventReward,%exp);
				messageClient(%b.client,'',"\c6You've recieved " @ %exp @ " exp in " @ capfirstLetter(%b.client.randomEventReward) @ ".");
				%b.client.randomEvent = false;
			}
		}
		
		parent::onPlayerTouch(%a,%b,%c);
	}
};
activatePackage(lob_antiBot);

function gameconnection::beginRandomEvent(%client,%skill)
{
	if(isObject(%client.horse))
		%client.pickupHorse();
		
	%time = getSimTime();
	
	if(%time - %client.lastRandomEvent <= 120000)
		return false;
		
	%client.lastRandomEvent = %time;
	messageClient(%client,'',"\c6Complete this event and be rewarded with exp in " @ capfirstLetter(%skill) @ ".");
	%client.randomEventReward = %skill;
	%client.randomEvent = true;
	%client.preRandomEventPos = %client.player.position;
	%spawn = "_antibot_spawn";
	%pos = %spawn.position;
	%client.player.setTransform(%pos);
}
