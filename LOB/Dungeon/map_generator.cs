if(!isobject(map_generator))
{
	new scriptObject(map_generator)
	{
		Survival = true;
	};
}

function map_generator::getForwardStep(%this,%position)
{
	return vectorAdd(%position,"0 16 0");
}

function map_generator::getBackwardStep(%this,%position)
{
	return vectorAdd(%position,"0 -16 0");
}

function map_generator::getRightStep(%this,%position)
{
	return vectorAdd(%position,"16 0 0");
}

function map_generator::getLeftStep(%this,%position)
{
	return vectorAdd(%position,"-16 0 0");
}

function map_generator::chooseStep(%this)
{
	%a[0] = "forward";
	%a[1] = "backward";
	%a[2] = "left";
	%a[3] = "right";
	
	return %a[getRandom(0,3)];
}

function map_generator::spawnAi(%this,%position)
{
	return false;
	%w[0] = "submachinegunimage";
	%w[1] = "pumpshotgunimage";
	%w[2] = "pistolimage";
	%w[3] = "TAssaultRifleimage";
	%w[4] = "battlerifleimage";
	%w[5] = "combatshotgunimage";
	%w[6] = "magnumimage";
	%c = 6;
	%ai = new aiplayer()
	{
		position = %position;
		dataBlock = playerStandardArmor;
		scale = "1 1 " @ getRandom(1,2);
		isSurvivalAi = true;
	};
	
	%ai.mountImage(%w[getRandom(0,%c)],1);
	%ai.playthread(1,"armReadyRight");
	%ai.doroam();
	
	missionCleanup.add(%ai);
}

function map_generator::createWallBrick(%this,%position)
{
	%b = new fxdtsBrick()
	{
		//Default
		angleID = "0";
		//FIX THISSSSSSSS
		client = findlocalClient();
		colorFxId = "0";
		//truenos colorset id
		colorId = "37";
		dataBlock = "brick32xCubeData";
		isBasePlate = true;
		isPlanted = "1";
		position = vectorAdd(%position,"0 0 8");
		printId = "0";
		rotation = "1 1 1";
		shapeFxId = "0";
		stackBL_ID = "0";
		//Survival
		isWallBrick = true;
		map_generator = map_generator;
		map_generator_step = map_generator.chooseStep();
	};

	%b.plant();
	%b.isPlanted=1;
	%b.setTrusted(1);
	brickGroup_35295.add(%b);
	%b.setname("temp");
	
	%c = new fxdtsbrick(create : temp)
	{
		position = vectorAdd(%position,"0 0 24");
	};
	
	%c.plant();
	%c.isPlanted=1;
	%c.setTrusted(1);
	brickGroup_35295.add(%c);
	%c.setname("");
	%b.setname("");
	
	missionCleanup.add(%b);
	missionCleanup.add(%c);
}

function map_generator::createFloorBrick(%this,%position)
{
	%b = new fxdtsBrick()
	{
		//Default
		angleID = "0";
		//FIX THISSSSSSSS
		client = findlocalClient();
		colorFxId = "0";
		colorId = "32";
		dataBlock = "brick32x32fData";
		isBasePlate = true;
		isPlanted = "1";
		position = %position;
		printId = "0";
		rotation = "1 1 1";
		shapeFxId = "0";
		stackBL_ID = "0";
		//Survival
		isFloorBrick = true;
		map_generator = map_generator;
		map_generator_step = map_generator.chooseStep();
	};
	
	$map_generator::plantArea::pos[%position] = true;
	
	%b.plant();
	%b.isPlanted=1;
	%b.setTrusted(1);
	brickGroup_35295.add(%b);
	
	missionCleanup.add(%b);
}

function map_generator::createTreeBrick(%this,%position)
{
	%b = new fxdtsBrick()
	{
		//Default
		angleID = "0";
		//FIX THISSSSSSSS
		client = findlocalClient();
		colorFxId = "0";
		colorId = "32";
		dataBlock = "brickLargePineTreeData";
		isBasePlate = true;
		isPlanted = "1";
		position = %position;
		printId = "0";
		rotation = "1 1 1";
		shapeFxId = "0";
		stackBL_ID = "0";
		//Survival
		isTreeBrick = true;
		map_generator = map_generator;
		map_generator_step = map_generator.chooseStep();
	};
	
	%b.plant();
	%b.isPlanted=1;
	%b.setTrusted(1);
	brickGroup_35295.add(%b);
	
	missionCleanup.add(%b);
}

function map_generator::createRockBrick(%this,%position)
{

	%size[0] = "brickData_lobBrick_rockSmall";
	%size[1] = "brickData_lobBrick_rockMedium";
	%size[2] = "brickData_lobBrick_rockLarge";
	
	%vectoraddz[0] = 0;
	%vectoraddz[1] = "0.2";
	%vectoraddz[2] = "0.9";
	
	%ran = getRandom(0,2);
	
	%size = %size[%ran];
	%vecAdd = %vectorAddz[%ran];
	
	//truenos colorset
	%color = getRandom(47,51);
	
	%b = new fxdtsBrick()
	{
		//Default
		angleID = getRandom(0,1);
		//FIX THISSSSSSSS
		client = findlocalClient();
		colorFxId = "0";
		colorId = %color;
		dataBlock = %size;
		isBasePlate = true;
		isPlanted = "1";
		position = vectorAdd(%position,"0 0 " @ %vecAdd);
		printId = "0";
		rotation = "1 1 1";
		shapeFxId = "0";
		stackBL_ID = "0";
		//Survival
		isRockBrick = true;
		map_generator = map_generator;
		map_generator_step = map_generator.chooseStep();
	};
	
	%b.plant();
	%b.isPlanted=1;
	%b.setTrusted(1);
	brickGroup_35295.add(%b);
	
	missionCleanup.add(%b);
}

function map_generator::setDynamicGeneratorTimer(%this)
{
	%this.timer = 2000 + (clientgroup.getCount() * 600);
}

map_generator.timer = 2000;
function map_generator::generate(%this,%position,%step)
{
	%this.setDynamicGeneratorTimer();
	if(%position $= "")
		return false;
	else
	if(getRandom(0,100000) <= 1)
	{
		%this.wrap();
		%this.applyTrees();
		%this.applyRocks();
		%this.applyai();
		%this.defineStartAndEndPoints();
		deleteVariables("$map_generator::plantArea::*");
		return false;
	}
	%time = getSimTime();
	
	cancel(%this.generatorLoop);
	
	if(%step $= "")
	{
		commandtoAll('centerPrint',"\c6Generation timer: \c2" @ map_generator.timer,7);
		map_generator.timestart = %time;
		%step = %this.chooseStep();
	}
	
	if(%time - map_generator.timeStart >= map_generator.timer)
	{
		%this.wrap();
		%this.applyTrees();
		%this.defineStartAndEndPoints();
		%this.applyRocks();
		%this.applyai();
		deleteVariables("$map_generator::plantArea::*");
		return false;
	}
	
	if(getRandom(1,20) <= 15)
	{
		%step = %this.chooseStep();
		echo("new step: " @ %step);
		//eval("%position = map_generator.get" @ %step @ "step(%position);");
	}
	
	if($map_generator::plantArea::pos[%position] || %this.detectBrick(%position))
	{
		eval("%position = map_generator.get" @ %step @ "step(%position);");
		%this.generatorLoop = %this.schedule(1,generate,%position,%step);
		return false;
	}
	%b = %this.createFloorBrick(%position);
	%this.stepCount[%b.map_generator_step]++;
	
	eval("%position = map_generator.get" @ %step @ "step(%position);");
	
	%this.generatorLoop = %this.schedule(10,generate,%position,%step);
}
function map_generator::clearBricks(%this)
{
	%c=missionCleanup.getCount();
	for(%i=%c;%i>=0;%i--)
		if(isObject(%o=missioncleanup.getObject(%i)))
		{
			if(%o.isfloorbrick || %o.isWallBrick || %o.istreebrick || %o.isRockBrick
			|| %o.isSurvivalAi)
			{
				%o.delete();
			}
		}
}

function map_generator::wrap(%this)
{
	%c=missionCleanup.getCount();
	for(%i=0;%i<=%c;%i++)
		if(isObject(%o=missioncleanup.getObject(%i)))
		{
			if(%o.isfloorbrick)
			{
				%p = %o.position;
				
				%stepl = %this.detectBrick(%this.getLeftStep(%p));
				%stepr = %this.detectBrick(%this.getRightStep(%p));
				%stepf = %this.detectBrick(%this.getForwardStep(%p));
				%stepb = %this.detectBrick(%this.getBackwardStep(%p));
				
				if(!%stepl)
				{
					%bpos = %this.getLeftStep(%p);
					%this.createWallBrick(%bpos);
				}

				if(!%stepr)
				{
					%bpos = %this.getRightStep(%p);
					%this.createWallBrick(%bpos);
				}

				if(!%stepb)
				{
					%bpos = %this.getBackwardStep(%p);
					%this.createWallBrick(%bpos);
				}
	
				if(!%stepf)
				{
					%bpos = %this.getForwardStep(%p);
					%this.createWallBrick(%bpos);
				}
			}			
		}
	return true;
}

function map_generator::applyTrees(%this)
{
	%c=missionCleanup.getCount();
	for(%i=0;%i<=%c;%i++)
		if(isObject(%o=missioncleanup.getObject(%i)))
		{
			if(%o.isfloorbrick)
			{
				%p = %o.position;
				
				%amount = getRandom(0,2);
				for(%j=0;%j<%amount;%j++)
				{
					%placement = vectorAdd(%p,getRandom(10,-10) SPC getRandom(10,-10) SPC 8.5);
					if(!%this.detectBrick(%placement))
						%this.createTreeBrick(%placement);
				}
			}			
		}
	return true;	
}

function map_generator::applyRocks(%this)
{
	%c=missionCleanup.getCount();
	for(%i=0;%i<=%c;%i++)
		if(isObject(%o=missioncleanup.getObject(%i)))
		{
			if(%o.isfloorbrick)
			{
				%p = %o.position;
				
				%amount = getRandom(0,2);
				for(%j=0;%j<%amount;%j++)
				{
					%placement = vectorAdd(%p,getRandom(10,-10) SPC getRandom(10,-10) SPC 0.7);
					//if(!%this.detectBrick(%placement))
						%this.createRockBrick(%placement);
				}
			}			
		}
	return true;	
}

//deprecated
function map_generator::applyAi(%this)
{
	%c=missionCleanup.getCount();
	for(%i=0;%i<=%c;%i++)
		if(isObject(%o=missioncleanup.getObject(%i)))
		{
			if(%o.isfloorbrick)
			{
				%p = %o.position;
				
				if(getRandom(0,500) <= 40)
				{
					%amount = getRandom(0,4);
					for(%j=0;%j<%amount;%j++)
					{
						%placement = vectorAdd(%p,getRandom(10,-10) SPC getRandom(10,-10) SPC 1.5);
						if(!%this.detectBrick(%placement))
							%this.spawnAi(%placement);
					}
				}
			}			
		}
	return true;	
}

function map_generator::detectBrick(%this,%position)
{
	%typemasks = $TypeMasks::FxBrickAlwaysObjectType;
	%target = containerRaycast(vectorAdd(%position,"-1 -1 -1"), vectorAdd(%position,"1 1 1"), %typemasks);

	if(isObject(%target))
	{
		//echo("brick found");
		//findlocalclient().player.settransform(%target.position);
		return true;
	}
	
	return false;
}

function map_generator::defineStartAndEndPoints(%this)
{
	%c=missionCleanup.getCount();
	%this.startBrick="";
	for(%i=0;%i<%c;%i++)
		if(isObject(%o=missioncleanup.getObject(%i)))
		{
			if(%o.isfloorbrick)
			{
				if(!isObject(%this.startBrick))
					%this.startBrick = %o;
					
				if(%last $= "")
				{
					%last = %o;
				}
				else
				if(vectorDist(%last.position,%this.startbrick.position) < vectorDist(%o.position,map_generator.startbrick.position))
				{
					%last = %o;
				}
			}
		}
		
	if(isObject(%last))
	{

		%this.endBrick = %last;
		//talk(vectorDist(%last.position,%this.startBrick.position));
		%this.startBrick.setColor(0);
		%this.startBrick.setColorFx(3);
		%this.endBrick.setColor(4);
		%this.endBrick.setColorFx(3);
	}
		
	return true;
}























