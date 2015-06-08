$fly::height["ordunia"] = 25;
$fly::height["whitestone"] = 30;
$fly::height["alyswell"] = 38;
$fly::height["eldria"] = 30;
$fly::height["interpass"] = 30;

package skill_flying
{
	function armor::onTrigger(%a,%b,%c,%d,%e)
	{
		%p = parent::onTrigger(%a,%b,%c,%d,%e);
		
		//echo(%b SPC %c SPC %d);
		
		if(isEventPending(%b.flyloop))
			if(%c == 2 && %d == 0)
			{
				if(%b.flySlow)
				{
					%b.fly();
					%b.playthread(0,crouch);
				}
				else
				{
					%b.fly(1);
					%b.playthread(0,root);
				}
				
			}
	}
};
activatePackage(skill_flying);

function player::MagicMaterialCost(%this)
{

}

function player::fly(%this,%slow)
{
	cancel(%this.flyloop);
	%this.client.playerEmitter();
	%p = %this.position;
	%this.flySlow = %slow;

	if(getWord(%p,2) >= $fly::height[%this.client.slo.area])
	{
		cancel(%this.flyloop);
		%this.setVelocity("0 0 0");
		%this.playthread(0,root);
		commandToClient(%this.client,'centerprint',"\c6 You can't fly that high...",5);
		//schedule(1000,0,serverCmdFly,%this.client);
		return false;
	}
	else
	if($fly::height[%this.client.slo.area] - getWord(%p,2) <=10)
	{
		commandToClient(%this.client,'centerprint',"<just:right>\c6 Max Fly Height In: " @ mfloor($fly::height[%this.client.slo.area] - getWord(%p,2)),1);
	}
	
	%vec = %this.geteyevector();
	%vec = vectorNormalize(%vec);

	if(%slow)
	{
		%vec = vectorScale(%vec,0);
		%this.setVelocity(vectorAdd(%vec,"0 0 0.76"));
	}
	else
	{
		%vec = vectorScale(%vec,20);
		%this.setVelocity(%vec);
	}
	
	%this.flyloop = %this.schedule(1,fly,%slow);
}

function servercmdfly(%this)
{
	if(%this.isAdmin == false)
		return false;
	if(isEventpending(%this.player.flyloop))
	{
		cancel(%this.player.flyloop);
		%this.player.playthread(0,root);
		if(isObject(%this.emitter))
		{
			cancel(%this.playerEmitterLoop);
			%this.emitter.delete();
		}
	}
	else
	{
		commandToClient(%this,'centerprint',"<font:impact:30>\c6Press spacebar to hover.",3,1);
		%this.player.setVelocity("0 0 15");
		%this.player.schedule(500,fly);
		%this.player.playthread(0,crouch);
	}
}