package zoneManager
{
	function null(){}
	
	function gameConnection::onDrop(%this)
	{
	
		%area = %this.slo.area;
		%wild = %this.wild;
		
		if(%wild $= "1")
		{
			lob_zoneManager.areaCount[%area,"wild"]--;
		}
		return parent::onDrop(%this);
	}
};
activatePackage(zoneManager);

if(isObject(lob_zoneManager))
	lob_zoneManager.delete();
	
new scriptGroup(lob_zoneManager)
{
	zoneManager = true;
};

function lob_zoneManager::newArea(%this,%name)
{
	%o = new simSet()
	{
		name = %name;
	};
	
	%this.add(%o);
}

function lob_zoneManager::areaByName(%this,%name)
{
	%count = %this.getCount();
	
	for(%i=0;%i<%count;%i++)
	{
		%o = %this.getObject(%i);
		
		if(%o.name $= %name)
			return %o;
	}
	
	return false;
}

function gameConnection::enterZone(%this,%brickname)
{
	if(%client.lastZone !$= "")
	{
		%areaObj = lob_zoneManager.areaByName(%client.lastZone);
		%areaObj.remove(%this);
		if(%areaObj.getCount() <= 0)
		{
			//remove npcs
			talk("removing npcs");
		}
	}
	
	if(%this.wild)
	{
		%zoneName = $lob::wildBrick[%brickName,"outname"];
		%zoneObject = lob_zoneManager.areaByName(%zoneName);
		if(isObject(%zoneObject))
		{
			%client.lastZone = %zoneName;
			%zoneObject.add(%this);
			if(%zoneObject.getCount() == 1)
			{
				//add npcs
				for(%i=0;%i<getWordCount($lob::NpcSpawn["all"]);%i++)
				{
					%w = getWord($lob::npcSpawn["all"],%i);
					%name = convertToItemName(%zoneObject.name);
					
					if(strStr(%w,%name) >= 0)
						
				}
			}
		}
	}
	else
	if(!%this.wild)
	{
		%zoneName = $lob::wildBrick[%brickName,"inname"];
		%zoneObject = lob_zoneManager.areaByName(%zoneName);
		if(isObject(%zoneObject))
		{
			%client.lastZone = %zoneName;
			%zoneObject.add(%this);
			if(%zoneObject.getCount() == 1)
			{
				//add npcs
			}
		}
	}
		
}

lob_zoneManager.newArea("Ordunia");
lob_zoneManager.newArea("Whitestone");
lob_zoneManager.newArea("alyswell");

lob_zoneManager.newArea("Bandit Land Wild");
lob_zoneManager.newArea("Ordunia wild");
lob_zoneManager.newArea("alyswell forest wild");
lob_zoneManager.newArea("whitestone's cavern wild");


