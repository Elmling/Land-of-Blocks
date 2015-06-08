$lob::GatherSkill["Water"] = 0;
$lob::gatherSkill["Roots"] = 10;


function serverCmdBuySash(%client)
{

	if(%client.slo.sash)
	{
		//already bought a sash
		return false;
	}
	else
	{		
		%sashCost = 120000;
		
		%clientGold = %client.slo.inventory.itemCount["gold"];
		
		if(%clientGold >= %sashCost)
		{
			%client.slo.sash = 1;
			%client.slo.sashSlots = 6;
			%client.slo.inventory.itemCount["gold"] = (%client.slo.inventory.itemCount["gold"] - %sashCost);
		}
		else
		{
			//Not enough Gold
		}
	}
}

function serverCmdGather(%client)
{
	%player = %client.player;
	
	if(isObject(%client.lastclicked))
	{
		if(vectorDist(%player.position,%client.lastclicked.position) <= 6)
		{
			%o = %client.lastclicked;
			echo(%o.getDataBlock().getName());
			%isWater = (strStr(strLwr(%o.getDataBlock().getName()),"water"));
			%isTree = (strStr(strLwr(%o.getDataBlock().getName()),"tree"));
			
			if(%isWater >= 0)
			{
			
			}
			else
			if(%tree >= 0)
			{
				echo("tree");
			}
		}
	}
}

package skill_gathering
{
	function skill_gathering(){}
};

activatePackage(skill_gathering);