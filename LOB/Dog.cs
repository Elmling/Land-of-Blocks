
function test(){
				bob.delete();
				%o = new aiplayer(bob)
				{
					position = findlocalclient().player.position;
					datablock = dogpet;
					scale = "1 1 1";
					
				};}


function dog_new(%client)
{
	if(isObject(%client.dog))
	{
		%client.dog.setTransform(vectorAdd(%client.player.getHackPosition(),%client.player.getEyeVector()));
		return %client.dog;
	}
	
	%client.dog = new aiplayer()
	{
		datablock = dogPet;
		scale = "1.3 1.3 1.3";
		position = vectorAdd(%client.player.getHackPosition(),%client.player.getEyeVector());
		client = %client;
	};
	
	%client.dog.ailoop();
	
	return %client.dog;
}

function aiPlayer::aiLoop(%this)
{//echo("main");
	if(!isObject(%this.client))
	{
		%this.delete();
		return;
	}
	cancel(%this.ai);

	%enemyCheck = %this.enemyCheck();
	
	if(isObject(%enemyCheck))
	{
		//echo("isobject enemycheck");
		%this.killEnemy(%enemyCheck);
		return false;
	}
	
	%this.setAimObject(%this.client.player);
	
	if(getRandom(1,20) == 1)
	{
		%this.playthread(0,root);
		//%this.playthread(1,root);
		%this.playthread(2,root);
		%this.playThread(0,activate);
		serverplay3d(dogbarksound,%this.position);
	}
	
	%vd = vectorDist(%this.client.player.position,%this.position);
	
	%dogz = getWord(%this.position,2);
	%playerz = getWord(%this.client.player.position,2);
	
	if(%playerz - %dogz > 2)
	{
		%this.addVelocity("0 0 4");
		//%this.playthread(1,jump);
	}
	
	if(%vd >= 20 && %vd <= 25 && %this.client.slo.status !$= "combat")
	{
		//echo("in");
		%pos = %this.client.player.position;
		if(!isEventPending(%this.reposition))
			%this.reposition = %this.schedule(1000,setTransform,%pos);
	}
	else
	if(%vd > 5)
	{
		if(%this.sit == true)
		{
			%this.sit = false;
			%this.playthread(1,root);
		}
		%this.setMoveDestination(%this.client.player.position);
	}
	else
	if(%vd <= 4)
	{
		%this.clearMoveDestination();
		if(%this.sit != true)
		{
			%this.schedule(100,playthread,1,sit);
			%this.sit = true;
		}
	}
	
	
	%this.ai = %this.schedule(100,ailoop);
}


function aiPlayer::enemyCheck(%this)
{
	%time = getSimTime();
	if(%time - %this.enemyCheckTime <= 500)
		return false;
	%zoneArea = %this.client.slo.zoneArea;
	%this.enemyCheckTime = %time;
	%enemy = $lob::enemySpawn[%zoneArea];
	%enemyCount = getWordCount(%enemy);
	//echo("enemy = " @ %enemy @ " | " @%zonearea);
	%d1 = %this.client.player.position;
	//echo("doing for loop");
	for(%i=0;%i<%enemyCount; %i++)
	{
		%en = getWord(%enemy,%i);
		
		if(isObject(%en.npc) && %en.npc.getState() $= "move")
		{
			%en = %en.npc;
			%d2 = %en.position;
			if(vectorDist(%d1,%d2) <=18)
			{
				//echo("enemy found in enemyvheck");
				//%this.KillEnemy(%en);
				break;
			}
		}
	}
	
	return %en;
}

function aiPlayer::killEnemy(%this,%enemy)
{
	cancel(%this.killEnemyLoop);
	if(!isObject(%enemy) || %enemy.getState() !$= "move")
	{
		//%this.enemyCheck();
		%this.aiLoop();
		return false;
	}

	//echo("killenemy");
	%vdMaster = vectorDist(%this.client.player.position,%this.position);
	%vdNow = vectorDist(%this.position,%enemy.position);
	
	if(%vdMaster >= 20)
	{//echo("master");
		%this.aiLoop();
		return false;
	}
	
	if(%vdNow >= 15)
	{//echo("vdnow");
		%this.aiLoop();
		return false;
	}
	
	if(%vdNow <= 10)
	{
		if(%this.sit)
		{
			%this.sit=false;
			%this.playthread(1,root);
		}
		%this.setAimObject(%enemy);	
		%this.ram(%enemy);
		return false;
	}
	
	%this.setAimObject(%enemy);
	%this.setMoveDestination(%enemy.position);
	
	%this.killEnemyLoop = %this.schedule(800,killEnemy,%enemy);
}

function aiPlayer::ram(%this,%enemy)
{//echo("ram");
	%this.ram = 1;
	%this.setMoveDestination(%this.client.player.position);
	%player = %this;
	%eyeVector = %player.getEyeVector();%eyePoint = %player.getEyePoint();
	%range = 3;%rangeScale = vectorScale(%eyeVector,%range);
	%rangeEnd = vectorAdd(%eyePoint, %rangeScale);
	//%raycast = containerRayCast(%eyePoint,%rangeEnd,$TypeMasks::PlayerObjectType, %player);
	//%altPlayer = getWord(%rayCast,0);
	//%client.player.playThread(0,activate2);
	%force = 30;
	%vel = vectorAdd("0 0 0",vectorScale(%player.getEyeVector(),%force));
	%vel = getWords(%vel,0,1) SPC 5;
	%this.addVelocity(%vel);
	//%this.playthread(0,jump);
	%this.schedule(1500,killEnemy,%enemy);
}


function serverCmdSpawnDog(%this)
{
	if(!%this.slo.inventory.itemCountDog $= "")
	{
		messageClient(%this,'',"\c6You don't have a dog.");
		return false;
	}
	
	if(isObject(%client.dog))
	{
		messageClient(%this,'',"\c6You already spawned a dog.");
		return false;		
	}
	
	%dog = client_dog(%this);
}

function serverCmdDespawnDog(%this)
{
	if(!isObject(%this.dog))
	{
	
	}
	else
	{
		%this.dog.delete();
	}
}