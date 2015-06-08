$lob::ClimbinglevelRequired["Pine"] = 0;
$lob::ClimbingExpGain["Pine"] = 20;
$lob::climbingSuccessRate["pine"] = 10;

$lob::ClimbingLevelRequired["Oak"] = 20;
$lob::climbingExpGain["Oak"] = 60;
$lob::climbingSuccessRate["Oak"] = 25;

$lob::ClimbingLevelRequired["Willow"] = 30;
$lob::ClimbingExpGain["Willow"] = 90;
$lob::climbingSuccessRate["Willow"] = 40;

$lob::ClimbingLevelRequired["Maple"] = 45;
$lob::climbingExpGain["Maple"] = 340;
$lob::climbingSuccessRate["Maple"] = 130;

function climbing_onTreeclicked(%client,%object)
{
		
	if(%client.slo.climbingLevel $= "")
	{
		commandToClient(%client,'messageBoxOk',"About the climbing skill","They key to climbing a tree is to jump and then attempt to climb the tree, to gain momentum. Climbing is a useful skill in Combat.");
		%client.slo.climbingLevel = 1;
		%client.slo.climbingExp = 0;
		return false;
	}
	
	%cz = getWord(%client.player.position,2);
	%bz = getWord(%object.position,2);
	
	if(%cz > %bz)
	{
		commandtoClient(%client,'centerprint',"\c6You're too high to climb that tree.",1);
		return false;
	}
	
	%treeType = %object.name;
	
	if($lob::climbingExpGain[%treeType] !$= "")
	{
		if(%client.slo.climbingLevel < $lob::climbingLevelRequired[%treeType])
		{
			smartMessage(%client,"\c6You need a climbing level of atleast \c2" @ $lob::climbingLevelRequired[%treeType] @ " \c6to climb a(n) " @ %treeType @ " Tree.",3000);
			return false;
		}

		%time = getSimTime();
		if(%time - %client.lastClimbAttemptTime >= 2500)
		{
		
			%client.lastClimbAttemptTime = %time;
			%lvl = %client.slo.climbingLevel;
			%success = climbing_climbTreeAttempt(%client,%object);
			
			if(getRandom(0,50) <= 2)
			{
				%client.beginRandomEvent("climbing");
				return true;
			}
			
			%client.setStatus("Climbing",2000);
			
			if(%success)
			{
				%exp = $lob::climbingExpGain[%treeType];
				%expBonus = getRandom(5,10);
				%exp = %exp + %expBonus;
				giveExp(%client,"climbing",%exp);
				%bonusVelocity = mclamp(msqrt(%client.slo.climbingLevel),0,5);
				%client.player.addVelocity("0 0 " @ (15 + %bonusVelocity));
				messageClient(%client,'centerprint',"\c6You've climbed the tree and earned " @ %exp @ " Climbing Exp!",1);
			}
			else
			{
				%exp = $lob::climbingExpGain[%treeType];
				giveExp(%client,"climbing",%exp);
				%client.player.addVelocity("0 0 5");
				messageClient(%client,'centerprint',"\c6You failed to climb the tree but earned " @ %exp @ " Climbing Exp.",1);
			}
		}
		else
		{
			commandToClient(%client,'centerprint',"\c6You're too tired to climb right now.",0.5);
		}
	}
}

function climbing_ClimbTreeAttempt(%client,%tree)
{
	%successRate = $lob::ClimbingSuccessRate[%tree.name] + %client.slo.climbingLevel;
	%math = %client.slo.climbingLevel + mfloor(msqrt(%client.slo.ClimbingLevel));
	%math = %math + getRandom(1,mfloor(%successRate / 3));
	
	echo(%math @ " > " @ mCeil(%successRate / 1.3) @ " ?");
	
	if(%math >= mCeil(%successRate / 1.3))
	{
		echo("success");
		return true;
	}
	else
		return false;
}