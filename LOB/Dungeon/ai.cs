function aiplayer::doAi(%this)
{
	%this.toolAmmo[%this.currtool] = 999;
	if(%this.getState() !$= "move")
	{
		if(%this.getState() $= "dead")
		{
			if(getRandom(0,1) <= 3)
			{
				%this.dropRandomPowerup();
			}
			
		}
		
		return;
	}
	cancel(%this.doAiLoop);
	if(isObject(%this.getAimObject()))
	{
		%time = getRandom(50,200);
		%eye = vectorScale(%this.getEyeVector(), 300);
		%pos = %this.getEyePoint();
		%mask = $TypeMasks::All;
		%hit = firstWord(containerRaycast(%pos, vectorAdd(%pos, %eye), %mask, %this));

		if(!isObject(%hit))
		{
			%this.doAiLoop = %this.schedule(%time,doai);
			return false;
		}
			
		%hitobj = getWord(%hit,0);
			
		if(%hitobj.getClassName() $= "fxdtsbrick")
		{
			if(%hitobj.isWallBrick)
			{
				this.doAiLoop = %this.schedule(%time,doai);
				return false;
			}
		}
		else
		if(%hitObj.getClassname() $= "player")
		{
			%this.setAimObject(%hitObj);
			%this.setImageTrigger(1,1);
			%this.schedule(%time-30,setImageTrigger,1,0);
		}
	}
	else
		return false;
		
	%this.doAiLoop = %this.schedule(%time,doai);
}

function aiplayer::getClosestPlayer(%this)
{
	%c = clientgroup.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%o = clientgroup.getObject(%i);
		if(isObject(%o.player))
		{
			if(%temp $= "")
				%temp = %o.player;
			else
			if(vectorDist(%o.player.position,%this.position) <= vectorDist(%temp.position,%this.position))
				%temp = %o.player;
		}
		
	}
	
	return %temp;
}

function aiplayer::doRoam(%this)
{
	cancel(%this.doroamloop);
	%potentialEnemy = %this.getClosestPlayer();

	if(isObject(%potentialEnemy) && vectorDist(%this.position,%potentialEnemy.position) <= 50)
	{
		if(!isObject(%this.getAimObject()))
		{
			%this.setAimObject(%potentialEnemy);
			%this.doAi();
		}
	}
	if(!%this.home)%this.home = %this.position;
	
	%p = vectorAdd(%this.home,getRandom(-10,10) SPC getRandom(-10,10) SPC 0);
	%this.setMoveDestination(%p);
	%this.doRoamloop = %this.schedule(getRandom(1000,3000),doroam);
}

function aiplayer::dropRandompowerup(%this)
{
	//return deprecated
	return false;
	%p[0] = "regenItem";
	%p[1] = warlockItem;
	%p[2] = strengthitem;
	%p[3] = agilityitem;
	%p[4] = haste;
	
	%c = %p[getRandom(0,4)];
	%item = %this.dropItem(%c);
	%item.isalive=true;
}

function aiPlayer::dropItem(%this,%name)
{
	if(!isObject(%name))
	{
		echo("no datablock for item " @ %name);
		return false;
	}
	

	%item = new item()
	{
		datablock = %name;
		canPickup = true;
		isSurvivalItem = true;
		scale = "1 1 1";
		specifiedClient = %specifiedClient;
	};
	
	%item.setCollisionTimeout(%this);
	%item.position = %this.getEyePoint();
	%item.setVelocity(vectorScale(%this.getEyeVector(),getRandom(2,8)));
	//%item.setShapeName(%name SPC "(" @ %amount @ ")");
	
	%item.schedule(60000,doDelete);
	
	return %item;
}