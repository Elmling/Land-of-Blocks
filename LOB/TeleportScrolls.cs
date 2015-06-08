
function serverCmdUseScroll(%client,%scroll)
{
	%area = %scroll;
	%scroll = %scroll @ "scroll";
	if($lob::itemDatablock[%scroll] $= "" || %client.slo.area $= "jail" && %client.slo.pkPoints >= $lob::wantedLevel)
		return false;
		
	if(%client.slo.inventory.itemCount[%scroll] $= "" || %client.slo.inventory.itemCount[%scroll] <= 0)
	{
		messageClient(%client,'',"\c6You have " @ %client.slo.inventory.itemCount[%scroll] SPC %area @ " scrolls.");
		return false;
	}
	
	$lob::zonePlayerCount[%client.slo.zoneArea]--;
	$lob::zonePlayerCount[%area]++;
	%client.slo.zoneArea = %area;
	%client.slo.area = %area;
	%client.removeFromInventory(%area SPC "scroll",1);
	%client.scrollUsed = true;
	%client.instantRespawn();
}