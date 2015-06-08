$lob::animationtime["rawbeef"] = 3000;
$lob::animationTime["rawsteak"] = 4500;
$lob::animationTime["rawlobster"] = 6000;

$lob::cookingExp["rawbeef"] = 150;
$lob::cookingExp["rawsteak"] = 300;
$lob::cookingExp["rawLobster"] = 500;

$lob::cookingLevelNeeded["RawBeef"] = 1;
$lob::cookingLevelNeeded["rawSteak"] = 10;
$lob::cookingLevelNeeded["rawLobster"] = 25;

function serverCmdCookFood(%this,%food)
{
	if(isEventPending(%this.cookingAnimationLoop))
		return false;
	if(%this.slo.cookingLevel < $lob::cookingLevelNeeded[%food])
	{
		%m = setKeyWords("\c6You need to have a cooking level of " @ $lob::cookingLevelNeeded[%food] @ " to cook that food type.","need cook","\c6");
		messageClient(%this,'',%m);
		return false;
	}
	else
	if(%this.slo.inventory.itemCount[%food] >= 1)
	{
		%EyeVector = %this.player.getEyeVector();
		%EyePoint = %this.player.getEyePoint();
		%Range = 4;
		%RangeScale = VectorScale(%EyeVector, %Range);
		%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
		%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::ItemObjectType , %this.player);
		%o = getWord(%raycast,0);
		
		if(isObject(%o) && %o.isFire)
		{
			commandtoclient(%this,'closeinventory');
			commandToClient(%this,'CloseDialogue');
			%this.doCookingAnimation(%food,%o);
		}
		//subtract food
	}
}

function gameConnection::doCookingAnimation(%this,%food,%brick)
{
	%pos = %this.player.position;
	%this.controlToCamera("Point",%pos);
	%this.player.playThread(0,sit);
	%this.player.playThread(1,armreadyboth);
	%this.cookingAnimationLoop();
	
	//echo("food = " @ %food);
	%tfood = strLwr(%food);
	%tfood = strReplace(%tfood,"raw","");
	%item = "uncooked" @ %tfood;
	
	%this.schedule($lob::animationTime[%food],stopCookingAnimation,%food);
	
}

function gameConnection::cookingAnimationLoop(%client)
{
	cancel(%client.cookingAnimationLoop);
	
	%time = getRandom(500,2000);
	%client.player.playThread(2,activate2);
	%client.setStatus("Cooking",%time + 1000);
	
	
	%client.cookingAnimationLoop = %client.schedule(%time,cookingAnimationLoop);
}

function serverCmdBeginCookFood(%this)
{
	commandToClient(%this,'openInventory');
}

function gameConnection::stopCookingAnimation(%this,%food)
{
	%this.player.playThread(0,root);
	%this.player.playThread(1,root);
	cancel(%this.cookingAnimationLoop);
	
	if(%this.slo.inventory.itemCount[%food] >= 1)
	{
		%remove = strReplace(strLwr(%food),"raw","");
		%this.removeFromInventory("raw " @ %remove,1);
		%this.addToInventory("cooked " @ %remove,1);
		%exp = $lob::cookingExp[%food];
		%m = setKeyWords("\c6You cooked the raw " @ %remove @ " into edible food and gained " @ %exp @ " EXP.","cooked gained EXP" @ %remove,"\c6");
		messageClient(%this,'',%m);
		giveExp(%this,"cooking",%exp);
		cancel(%this.cookitem.delete);
		%this.cookItem.delete();

	}
	
	%this.endorbit();
}