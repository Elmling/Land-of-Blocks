package gameConnection
{
	function serverCmdCreateMinigame()
	{
		//nothing
	}
	function armor::onTrigger(%datablock,%player,%slot,%io)
	{
		parent::onTrigger(%datablock,%player,%slot,%io); //call the original functions function

		// 0 Fire
		// 1 Jump
		// 2 idk?
		// 3 Crouch
		// 4 Jet
		%client = %player.client;
		
		if(%slot == 4 && %io == 1)
		{
			%name = %player.getmountedimage(0);
			if(isObject(%name))
				%name = %name.getName();
				
			%time = getSimTime();
			%amt = %time - %client.lastSpecialTime;
			
			if(%client.bowSling && %amt <= 3000)
			{
				%m = setKeyWords("\c6You have to wait " @ mfloor(((3000 - %amt) / 1000)) @ " seconds to use your special again.",mfloor(((3000 - %amt) / 1000)) SPC "special","\c6");
				smartMessage(%client,%m,5000);
				return 0;
			}
			else
			if(%client.bowSling)
			{
				%client.specialReady = true;
			}
			else
			if(strStr(strLwr(%name),"fire") >= 0)
			{
				%time = getSimTime();
				if(%time - %player.client.lastSpecialTime >= 3000)
				{
					%player.client.doFireBallSpecial();
					//serverPlay3d(swordSpecialSound,%client.player.position);
					//%client.player.emote(swordSpecialImage,1);
					%client.lastSpecialTime = %time;
				}
				else
				{
					%m = setKeyWords("\c6You have to wait " @ mfloor(((3000 - %amt) / 1000)) @ " seconds to use your special again.",mfloor(((3000 - %amt) / 1000)) SPC "special","\c6");
					smartMessage(%client,%m,5000);
				}			
			}
			else
			if(strStr(strLwr(%name),"shortsword") >= 0)
			{
				%time = getSimTime();
				if(%time - %player.client.lastSpecialTime >= 3000)
				{
					lob_sword_doSpecial(%name,%client.player);
					serverPlay3d(swordSpecialSound,%client.player.position);
					%client.player.emote(swordSpecialImage,1);
					%client.lastSpecialTime = %time;
				}
				else	
				{
					%m = setKeyWords("\c6You have to wait " @ mfloor(((3000 - %amt) / 1000)) @ " seconds to use your special again.",mfloor(((3000 - %amt) / 1000)) SPC "special","\c6");
					smartMessage(%client,%m,5000);
				}
			}
		}

		if(%slot == 0 && %io == 1)
		{
			if(isObject(%player.getmountedimage(0)))
			{
				%name = %player.getmountedimage(0).getName();

				if(strStr(strLwr(%name),"bow") >= 0)
				{
					%client.bowSling = true;
				}
			}
		}
		else
		if(%slot == 0 && %io == 0)
		{
			if(isObject(%player.getmountedimage(0)))
			{
				%name = %player.getmountedimage(0).getName();
				
				if(strStr(strLwr(%name),"bow") >= 0)
				{
					%client.bowSling = false;
				}
			}
		}
	}
};

activatePackage(gameConnection);

function gameConnection::beginTips(%this)
{
	cancel(%this.tipLoop);
	
	%status = %this.status;
	
	if(%status $= "idle" || %status $= "exploring")
		%status = "Random";
	
	%status = strReplace(%status," ","");
	%tip = $lob::tip[%status,getRandom(0,$lob::tc[%status])];
	
	commandToClient(%this,'showTip',%tip,20000);
	
	%this.tipLoop = %this.schedule(60000,beginTips);
}

function gameConnection::setStatus(%client,%status,%time)
{
	%client.activity.newActivity("started " @ %status);
	%client.status = %status;
	cancel(%client.clearStatus);
	if(%time $= "")
		%time = 500;
		
	if(%time > 5000)
		%time = 5000;
	%client.clearStatus = %client.schedule(%time,clearStatus);
}

function gameConnection::clearStatus(%this)
{
	%this.status = "";
}

function gameConnection::initClientGUI(%client)
{
	commandToClient(%client,'lob_assemble');
	commandToClient(%client,'lob_registerButton',"Inventory","canvas.pushDialog(lob_inventory);");
	commandToClient(%client,'lob_registerButton',"Skills","canvas.pushDialog(lob_skills);");
	commandToClient(%client,'lob_registerButton',"Avatar","canvas.pushDialog(lob_Avatar);");
	commandToClient(%client,'lob_registerButton',"Updates","goToWebPage(\"landofblocks.weebly.com\");");
	
	if(%client.isAdmin)
		commandToClient(%client,'lob_registerButton',"Admin","canvas.pushDialog(lob_admin);");
	//commandToClient(%client,'lob_registerButton',"Options","canvas.pushDialog(lob_options);");
	
	commandToClient(%client,'lob_registerSkill',"Combat","commandToServer('combatData');");
	commandToClient(%client,'lob_registerSkill',"Mining","commandToServer('miningData');");
	commandToClient(%client,'lob_registerSkill',"Wood Cutting","commandToServer('woodCuttingData');");
	commandToClient(%client,'lob_registerSkill',"Fire Making","commandToServer('fireMakingData');");
	commandToClient(%client,'lob_registerSkill',"Smithing","commandToServer('smithingData');");
	commandToClient(%client,'lob_registerSkill',"Smelting","commandToServer('smeltingData');");
	commandToClient(%client,'lob_registerSkill',"Cooking","commandToServer('cookingData');");
	commandToClient(%client,'lob_registerSkill',"Climbing","commandToServer('climbingData');");
	commandToClient(%client,'lob_registerSkill',"Magic","commandToServer('magicData');");
	
	commandToClient(%client,'visionInit');
}