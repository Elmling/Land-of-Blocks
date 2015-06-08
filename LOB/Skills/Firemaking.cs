package fireMaking
{
	function a(){}
};
activatePackage(fireMaking);

$lob::fireTime["Pine"] = 15000;
$lob::fireTime["Oak"] = 30000;
$lob::fireTime["willow"] = 40000;
$lob::fireTime["maple"] = 60000;

$lob::firemakingexp["pine"] = "15 20";
$lob::firemakingexp["oak"] = "30 50";
$lob::firemakingexp["willow"] = "100 150";
$lob::firemakingexp["maple"] = "200 300";

function serverCmdStartFire(%this,%logType)
{	
	%p = %this.player;
	if(%this.slo.inventory.itemCount[%logType@"logs"] $= "" || %this.slo.inventory.itemCount[%logType@"logs"] !$= "" && %this.slo.inventory.itemCount[%logType@"logs"] <= 0)
		return 0;

	%time = getSimTime();
	
	if(%time - %this.lastFireTime <= 1000)
	{
		return 0;
	}

	%player = %this.player;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	%Range = 4;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType , %player);
	%o = getWord(%raycast,0);

	if(isObject(%o) && %o.getClassName() $= "fxDtsBrick")
	{
		%pos = getWords(%raycast,1,3);
		%pz = getWord(%player.position,2);
		%rz = getWord(%pos,2);
		
		if(%rz > %pz)
			return 0;
		else
		{	
			%item = new item()
			{
				position = %pos;
				datablock = Wood;
				logType = %logType;
				isFire = true;
				user = %this;
				static = 1;
			};
			
			%emitter = new particleEmitterNode()
			{
				dataBlock = "GenericEmitterNode";
				emitter = "BurnEmitterA";
				position = %pos;
				spherePlacement = 0;
				velocity = 1;
				user = %this;
			};
			
			%item.canpickup = 0;
			%item.setName("fire_"@%logType);
			%item.schedule($lob::fireTime[%logType],delete);
			%item.setScale("0.8 0.8 0.8");
			%emitter.schedule($lob::fireTime[%logType],delete);
			%emitter.setScale("0.1 0.1 0.1");
			
			%this.player.playthread(0,activate);
			%this.removeFromInventory(%logType SPC "logs",1);
			giveExp(%this,"firemaking",getRandom(getWord($lob::firemakingexp[%logType],0),getWord($lob::firemakingexp[%logType],1)));
			%this.setStatus("Firemaking",5000);
			%this.lastFireTime = %time;
		}
		
		//if(vectorDist(
	}
		
}
