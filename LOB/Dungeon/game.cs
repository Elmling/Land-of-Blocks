ak47projectile.velinheritfactor=0;

function survival_game_start()
{
	%m = map_generator;

}

function survival_game_end()
{
	for(%i=0;%i<clientgroup.getCount();%i++)
	{
		%o = clientGroup.getObject(%o);
		if(%o.forestMinigame)
			%o.forestMinigame = false;
	}
	map_generator.clearBricks();
}

function survival_game_newRound()
{
	cancel($s_g_onendbricktouch);
	%m = map_generator;
	$survival_game_waitingformap = true;
	%m.clearBricks();
	%m.generate("1 1 0.2");
}

function survival_game_beginRound()
{
	survival_game_onEndBrickTouch();
	%c=clientGroup.getCount();
	%time = getSimTime();
	for(%i=0;%i<%c;%i++)
	{
		%o = clientgroup.getObject(%i);
		%o.roundStartTime = %time;
		%o.instantRespawn();
	}
}

function survival_game_GetRandomPlayer(%ignore)
{
	%c = clientGroup.getCount();
	%pc=-1;
	for(%i=0;%i<%c;%i++)
	{
		%o = clientgroup.getObject(%i);
		if(!%o.forestMinigame)
		{
			error(%o.name @ " is not in a forest minigame");
			continue;
		}
		if(isObject(%o.player) && %ignore != %o)
		{
			%p[%pc++] = %o;
		}
	}
	
	if(%pc == -1)
		return false;
	else
		return %p[getRandom(0,%pc)];
}

function survival_game_onEndBrickTouch()
{
	cancel($s_g_onendbricktouch);
	
	%c = clientgroup.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%o = clientgroup.getObject(%i);
		if(!%o.forestMinigame)
		{
			error(%o.name @ " is not in a forest minigame");
			continue;
		}
		if(isObject(%o.player))
		{
			if(vectorDist(%o.player.position,map_generator.endBrick.position) <= 8)
			{
				serverplay2d(onroundendsound);
				survival_game_end();
				//schedule(300,0,survival_game_newRound);
				return false;
			}
		}
	}
	
	$s_g_onendbricktouch = schedule(1000,0,survival_game_onEndBrickTouch);
}

package survival_game
{
	function armor::onTrigger(%datablock,%player,%slot,%io)
	{
		parent::onTrigger(%datablock,%player,%slot,%io); //call the original functions function

		// 0 Fire
		// 1 Jump
		// 2 idk?
		// 3 Crouch
		// 4 Jet
		%client = %player.client;
		
	}
	function map_generator::wrap(%this)
	{
		parent::wrap(%this);
		//done loading new map
		if($survival_game_waitingformap)
		{
			schedule(1000,0,survival_game_beginRound);
		}
	}
	function gameConnection::spawnPlayer(%this)
	{
		%p = parent::spawnPlayer(%this);
		
		if(%this.forestMinigame)
		{
			//spawn playe at red brick
			%this.player.setTransform(vectorAdd(map_Generator.startBrick.position,"0 0 2"));
			//set music if not playing already
			
		}
		
		return %p;
	}
	
	function GameConnection::onDeath(%this,%obj,%killer,%type,%area)
	{
	
		%p = parent::onDeath(%this,%obj,%killer,%type,%area);
		
		
		
		return %p;
	}
};

activatePackage(survival_game);

function servercmdsSpy(%this,%a)
{
	%a=findclientbyname(%a);
	if(isobject(%a))
	{
		%trans = %a.player.getTransform();
		%this.camera.setOrbitMode(%a.player,%trans,10,49,38,false);
		%this.setControlObject(%this.camera);
	}
	else
		%this.setControlObject(%this.camera);
}

$fly::height["a"] = 100;

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

function player::fly(%this,%slow)
{
	cancel(%this.flyloop);
	
	%p = %this.position;
	%this.flySlow = %slow;

	if(getWord(%p,2) >= $fly::height["a"])
	{
		cancel(%this.flyloop);
		%this.setVelocity("0 0 0");
		%this.playthread(0,root);
		commandToClient(%this.client,'centerprint',"\c6 You can't fly that high...",5);
		//schedule(1000,0,serverCmdFly,%this.client);
		return false;
	}
	else
	if($fly::height["a"] - getWord(%p,2) <=10)
	{
		commandToClient(%this.client,'centerprint',"<just:right>\c6 Max Fly Height In: " @ mfloor($fly::height["a"] - getWord(%p,2)),1);
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
	if(%this.canfly==0)return;
	if(isEventpending(%this.player.flyloop))
	{
		cancel(%this.player.flyloop);
		%this.player.playthread(0,root);
	}
	else
	{
		commandToClient(%this,'centerprint',"<font:impact:30>\c6Press spacebar to hover.",3,1);
		%this.player.setVelocity("0 0 15");
		%this.player.schedule(500,fly);
		%this.player.playthread(0,crouch);
	}
}

datablock AudioProfile(onroundendsound)
{
	filename = "./onroundendsound.wav";
	description = AudioClosest3d;
	preload = false;
};

function servercmdgroup(%this)
{
	%this.player.setTransform(vectorAdd(survival_game_GetRandomPlayer(%this).player.getEyePoint(),"4 4 0"));
	//%this.player.setTransform(survival_game_GetRandomPlayer());
}