package NPC
{
	function paintProjectile::onCollision(%a,%b,%c,%d,%e)
	{
		if(%b.getClassName() $= "fxDtsBrick" || %c.getClassName() $= "fxDtsBrick")
			parent::onCollision(%a,%b,%c,%d,%e);
	}
	
	function armor::onCollision(%a,%b,%c,%d,%e)
	{
		if(%c.getClassName() $= "Item")
		{
			%b.pickup(%c);
		}
		else
			parent::onCollision(%a,%b,%c,%d,%e);
			
		if(%b.getClassname() $= "aiPlayer" && %c.getclassname() $= "aiPlayer")
		{
			if(%b.dataBlock $= "DogPet" && %c.getState() $= "move")
			{%time = getsimtime();
				if(%time - %b.lastRamTime >= 800 && %b.ram)
				{
					%b.ram = "";
					shortSwordOnHit(%b.client,"bronze",%c);
					%amt = vectorAdd(%d,%b.getVelocity());
					%amt = vectorScale(vectorNormalize(%amt),20);
					%c.addVelocity(getWords(%amt,0,1) SPC 2);
					%b.setVelocity("0 0 0");
					%b.lastRamTime = %time;
				}
			}
		}
		
		if(isObject(%b) && isObject(%c) && %b.getClassName() $= "Player" && %c.getClassName() $= "Player")
		{
			//disable pushing
			return false;
			%client1 = %b.client;
			%client2 = %c.client;
			
			if(%client2.noVelocity || %client2.buildmode)
			{
				echo("ignoring "@%client2.name);
				%client2.noVelocity = "";
				return 0;
			}
			else
			{
				%client1.noVelocity = true;
				%amt = vectorAdd(%d,%client2.player.getVelocity());
				%amt = vectorScale(vectorNormalize(%amt),10);
				%client2.player.addVelocity(getWords(%amt,0,1) SPC 1);
			}
		}
	}

	function player::activateStuff(%this)
	{
		parent::activateStuff(%this);
		
		%client = %this.client;
		%EyeVector = %this.getEyeVector();
		%EyePoint = %this.getEyePoint();
		%Range = 4;
		%RangeScale = VectorScale(%EyeVector, %Range);
		%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
		%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::ItemObjectType , %this);
		%o = getWord(%raycast,0);

		if(isObject(%o))
		{
			%client.lastClicked = %o;
			if(%o.getClassName() $= "item")
			{
				if(%o.isFire)
				{
					commandToClient(%client,'setdlg',"Interactive",%o.user.name @ "\'s fire. You can cook food on this.","#string Cook Food #command beginCookFood");
				}
			}
			else
			if(%o.getClassName() $= "AiPlayer")
			{
				if((%o.hasShop || isObject(%o.shop)) && %o.name !$= "Dungeoneer")
				{
					%client.shopKeep = %o;
					commandToClient(%client,'openShopWindow');
				}
				else
				if($OnClickActionSet[%o.name] !$= "")
				{
					%name = %o.Name;
					eval(%name@".onClick(%o,%this);");
				}
				else
				{
					if($lob::npcTalk[%o.name])
						doNpcTalk(%o,%client);
					else
					commandToClient(%client,'messageboxok',"Dialogue with "@%o.name,%o.name@" is not interested in talking right now.");
				}
			}
			else
			if(%o.getClassName() $= "fxDtsBrick")
			{
				if(!%o.isRaycasting())
				{
					if(strStr(strLwr(%o.getDataBlock().getName()),"water") >= 0)
					{
						commandToClient(%client,'setdlg',"Interaction","Water, what would you like to do?","#string Fish #command fish","#string Gather #command gather");
						return true;
					}
					
					return false;
				}
				
				if($lob::ClimbingExpGain[%o.name] !$= "")
				{
					climbing_onTreeClicked(%client,%o);
				}
				
				%bn = trim(strReplace(%o.getName(),"_"," "));
				%type = getWord(%bn,0);
				%name = getWord(%bn,1);
				
				if(%type $= "robin")
				{
					//robin quest 2
					if(%name $= "clue")
					{
						if(%client.slo.robin_q2_started)
						{
							if(!%client.slo.robin_q2_completed)
							{
								commandToClient(%client,'setDlg',"Robin's Plight - Quest","I see a lot of Orgre foot prints around here, they look fresh.","#string Find the General Ogre #command");
								%client.slo.robin_q2_foundClue1 = true;
							}
						}
					}
				}
				else
				if(%type $= "vitel")
				{
					if(%name $= "sword")
					{
						if(%client.slo.vitel_q1_p3 $= "1" && %client.slo.vitel_q1_hasSword $= "")
						{
							%ran = getRandom(1,100);
							if(%ran <= 10)
							{
								//success
								%m = "*THE SWORD PULLS FROM THE ICE*\n I should go see Vitel.. ";
								%client.slo.vitel_q1_hasSword = 1;
							}
							else
							{
								%m = "It's stuck in ice, I should keep trying to lift it.";
							}
							
							commandToClient(%client,'setDlg',"Supplies for Eldria - Quest",%m);
						}
					}
				}
				else
				if(%type $= "eldin")
				{
					//quest
					if(%name $= "skull")
					{
						if(%client.slo.eldin_q1_hasMixture $= "")
						{
							%client.slo.eldin_q1_hasMixture = true;
							commandToClient(%client,'centerprint',"<color:fff333><font:impact:30>You move the skull and grab the potion.",5);
							%m = setKeyWords("\c6I should report back to Eldin, in Alyswell","Eldin Alyswell","\c6");
							messageClient(%client,'',%m);
						}
						else
						{
							commandToClient(%client,'centerprint',"<color:fff333><font:impact:30>You move the skull and see nothing.",5);
						}
						%o.setRendering(0);
						%o.schedule(1000,setRendering,1);
					}
				}
				else
				if(%type $= "furnace")
				{
					commandToClient(%client,'openSmeltingGui');
					%client.furnaceBrick = %o;
				}
				else
				if(%type $= "anvil")
				{
					//smithing
					commandToClient(%client,'openSmithingGui');
					%client.anvilBrick = %o;
				}
				else
				if(%type $= "tree")
				{
					//if(strStr(%client.examined,%name) == -1)
					//{
					//	%client.examined = %client.examined SPC %name;
					//	%client.examined = trim(%client.examined);
					//}
					
					//%info = $LOB::treeInfo[%name];
					//commandToClient(%client,'messageboxok',"Interactive",%info);
				}
				else
				if(%type $= "rock")
				{
					if(strStr(%client.examined,%name) == -1)
					{
						%client.examined = %client.examined SPC %name;
						%client.examined = trim(%client.examined);
					}
					
					%info = $LOB::rockInfo[%name];
					commandToClient(%client,'messageboxok',"Interactive",%info);
				}
				else
				if(%type $= "bank")
				{
					commandToClient(%client,'openBank');
					%client.bankTeller = %o;
				}
				else
				if(getWord(%rayCast,3) <= getWord(%this.position,2))
				{
					%client.potentialFirePosition = getWords(%raycast,1,3);
					commandToClient(%client,'setDlg',"Interactive","What would you like to do?","#string Make a fire. #command startFireOpenInventory");
				}
			}
			else
			if(%o.getClassName() $= "Player")
			{
				%client.potentialPlayerInteraction = %o.client;
				commandToClient(%client,'setDlg',"Interacting with " @ %o.client.name,"What would you like to do?","#string View Stats #command stats","#string Send Trade Request #command sendTradeRequest");
			}
		}
	}
	
	function fxDtsBrick::onRemove(%this)
	{
		if(isObject(%this.npc))
		{
			removeFromList("$Lob::EnemySpawn[\"all\"]",%this);
			removeFromList("$Lob::NpcSpawn[\"All\"]",%this);
			%this.npc.delete();
			
			echo("Deleted NPC from brick.");
		}
		
		parent::onRemove(%this);
	}
	
	function serverCmdSetWrenchData(%client,%data)
	{
		parent::serverCmdSetWrenchData(%client,%data);
		
		%bn = strReplace(getField(%data,0),"N ","");
		%bn = trim(strReplace(%bn,"_"," "));
		%brick = %client.wrenchBrick;
		
		if(strStr(strLwr(%bn),"npcspawn") >= 0)
		{
			%area = getWord(%bn,0);
			
			%name = getWord(%bn,getWordCount(%bn) - 1);
			%name = strUpr(getSubStr(%name,0,1)) @ getSubStr(%name,1,strLen(%name));
			
			if(%brick.NPC $= "")
			{
				%datablock = $LOB::NPC[%name,"Datablock"];
				
				%o = new aiPlayer()
				{
					onClickAction = $LOB::OnClickAction[%area,%name];
					datablock = playerStandardArmor;
					position = vectorAdd(%brick.position,"0 0 0.2");
					name = %name;
				};
				
				%o.brick = %brick;
				%brick.NPC = %o;
				
				if($nodeColor[%name,"chest"] $= "")
					dressplayer(%o,"citizen");
				else
					dressplayer(%o,%name);
				
				//echo("EQUIP NAME = " @ $equip[%name]);
				
				if($equip[%name] !$= "")
				{
					equipPlayer(%o,$equip[%name]);
				}
				
				if($task[%name] $= "")
				{
					%o.schedule(200,roam);
				}
				else
					eval("%o.schedule(200,"@$task[%name]@");");
					
				addToList("$LOB::Npcspawn[\"All\"]",%brick);
				
				if($lob::isShopNpc[%name])
					%o.lob_newShop();
				
				if(isFunction(%o.name,"onObjectSpawned"))
				{
					eval("" @ %o.name @ ".onObjectSpawned(" @ %o @ ");");
				}
					
				echo("Ai Player registered to brick complete.");
			}
		}
		else
		if(strStr(strLwr(%bn),"enemyspawn") >= 0)
		{
			%area = getWord(%bn,0);
			
			%name = getWord(%bn,getWordCount(%bn) - 1);
			
			if(%brick.NPC $= "")
			{
				%datablock = $LOB::Enemy[%name,"Datablock"];
				%aggressive = $LOB::Enemy[%name,"Aggressive"];
				%health = $LOB::Enemy[%name,"Health"];
				%o = new aiPlayer()
				{
					onClickAction = $LOB::OnClickAction[%area,%name];
					datablock = %datablock;
					position = vectorAdd(%brick.position,"0 0 0.2");
					name = %name;
					Level = getRandom(getWord($LOB::Enemy[%name,"Level"],0),getWord($LOB::Enemy[%name,"Level"],1));
					Aggressive = %aggressive;
					tempHealth = %health;
				};
				
				%o.brick = %brick;
				%brick.NPC = %o;
				
				newCombatDataFromLevel(%o,%o.level);
				
				if($nodeColor[%name,"chest"] $= "" && $roam[%name] $= "")
					dressplayer(%o,"citizen");
				else
				if($nodeColor[%name,"chest"] !$= "")
					dressplayer(%o,%name);
					
				if($equip[%name] !$= "")
				{
					equipPlayer(%o,$equip[%name]);
				}
				
				if($task[%name] $= "")
				{
					%o.schedule(200,roam);
				}
				else
					eval("%o.schedule(200,"@$task[%name]@");");
				
				addToList("$LOB::EnemySpawn[\"all\"]",%brick);
				
				if(isFunction(%o.name,"onObjectSpawned"))
				{
					eval("" @ %o.name @ ".onObjectSpawned(" @ %o @ ");");
				}
				
				echo("Ai Player registered to brick complete.");
			}
		}
	}
};
activatePackage(NPC);

function deleteNPCS(%this)
{
	%count = getWordCount($LOB::NPCSpawn["all"]);
	
	for(%i=0;%i<%count+1;%i++)
	{
		%o = getWord($LOB::NPCSpawn["All"],%i);
		
		if(isObject(%o.npc))
		{
			%o.npc.delete();
			%o.npc = "";
		}
			
			$lob::brickLoaded[%o] = "0";
		
	}

	%count = $LOB::EnemySpawn["all"];
	
	for(%i=0;%i<%count+1;%i++)
	{
		%o = getWord($LOB::EnemySpawn["All"],%i);
		
		if(isObject(%o.npc))
			%o.npc.delete();
			
		$lob::brickLoaded[%o] = "0";
	}
}

function npc_stabelizer(%ai,%player)
{
	return 200;
	%vd = vectorDist(%ai.position,%player.position);
	
	%time = (1000 * %vd);
	%time = (%time / 1.5);

	%time = (%time / 2.5);
	%time = mfloor(%time);

	//if(%ai.name $= "goblin")
	//messageClient(findclientbyname("elm"),'',"= >" @ %ai.name @ " = " @ %time);
	
	if(%time >= 5000)
		%time = 5000;
	return %time;
}

//Roaming Begin

function aiPlayer::roam(%this)
{
	cancel(%this.roamLoop);
	
	if(!isObject(%this)) return "ERROR";
		
	%this.setImageTrigger(1,0);
	%this.setimagetrigger(0,0);
		
	%tp = %this.position;
	%cp = %this.findClosestPlayer();
	%cpp = %cp.position;
	%home = %this.brick.position;
	%time = getSimTime();
	
	if($lob::vision[%this.name] $= "")
	{
		//echo("no vision for " @ %this.name);
		%vision = 10;
	}
	else
		%vision = $lob::vision[%this.name];
	
	if(isObject(%cp) && vectorDist(%cpp,%tp) <= %vision)
	{
		%this.aimAt(%cp,1);
		%time = getSimTime();
		
		if($lob::roamMsgCount[%this.name] !$= "" && %this.lastClientPlayerTalkedTo != %cp && %time - %this.lastTalkTime >= 6000)
		{
			%this.lastTalkTime = %time;
			%this.lastClientTalkedTo = %cp.client;
			%m = $lob::roamMsg[%this.name,getRandom(0,$lob::roamMsgCount[%this.name])];
			%pos = %tp;
			%pos = getWords(%tp,0,1) SPC getWord(%this.getEyePoint(),2);
			%timer = 5000;
			%this.talk(%m,%pos,%timer);
			%this.killWalk = true;
		}
		
		%looptime = 1;
	}
	else
	{
		%looptime = npc_stabelizer(%this,%cp);
			
		if(isObject(%this.getAimObject())) %this.clearAim();

		if(%time - %this.lastRoamTime >= %this.lastSelectedTime)
		{
			if($roam[%this.name] $= "")
				%roam = $roam["citizen"];
			else
				%roam = $roam[%this.name];
			
			%vd = vectorDist(%this.position,%this.lookingAt);
			
			if(%this.roamPos $= "" || %vd <= 2.5)
			{
				%roampos = vectorAdd(%home,getRandom(-1*%roam,%roam) SPC getRandom(-1*%roam,%roam) SPC mAbs(getWord(%home,2) - getWord(%this.getEyePoint(),2)));		
				%EyeVector = %this.getEyeVector();
				%EyePoint = %this.getEyePoint();
				%Range = 1000;
				%RangeScale = VectorScale(%EyeVector, %Range);
				%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
				%raycast = containerRayCast(%eyePoint,%rangeend,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType , %this);
				%o = getWord(%raycast,0);
				
				//if(%this  $= "2176098")
				//	echo("object = " @ vectorDist(%this.position,%o.position));
				
				//if(!isObject(%o))
				//{
					%this.roamPos = %roamPos;
				//}
				//else
				//%this.roamPos = %home;
			
			}
			
			if($roam[%this.name] $= "0")
			{
				%this.roamLoop = %this.schedule(1000,roam);
				return true;
			}
			
			%speed = %this.getSpeed();
			
			if(%speed <= 1.5 && %time - %this.lastRoamTime >= 4000)
			{
				%this.roamPos = "";
				%this.lastRoamTime = %time;
				%this.lastSelectedTime = getRandom(1000,4000);
			}
			else
			{
				%this.aimAt(%this.roamPos);
				%this.walkTo(%this.roamPos);
			}
			
			if(vectorDist(%this.roamPos,%this.position) <= 3)
			{
				%this.walkTo(%this.position);
				%this.roamPos = "";
				%this.lastRoamTime = %time;
				%this.lastSelectedTime = getRandom(1000,10000);
			}
		}
	}

	%this.roamLoop = %this.schedule(%looptime,roam);
}

function aiPlayer::talk(%this,%message,%pos,%timer)
{
	%this.lastTalkTime = getSimTime();
	%so = %this.buildWorldText(%message,%pos,%timer);
	%so.animate();
}

function aiPlayer::findClosestPlayer(%this)
{
	%tp = %this.position;
	%temp = 0;
	%brick = %this.brick;
	if(!isObject(%brick))%this.delete();
	%type = strReplace(strLwr(%brick.getname()),"_"," ");
	%type = trim(%type);
	%inWildZone = strpos(strLwr(%type),"wild");
	%type = getWord(%type,0);
	
	for(%i=0;%i<clientGroup.getCount();%i++)
	{
		%c = clientGroup.getObject(%i);
		%cp = %c.player;
				
		if(!isObject(%cp) || %c.buildmode $= "1" || (!%c.wild && %type $= "enemyspawn" && %inWildZone >= 0)) continue;

		%ps = %cp.position;
		%vd = vectorDist(%tp,%ps);
		%tvd = vectorDist(%temp.position,%tp);
		
		if(%temp $= "0")
			%temp = %cp;
		else
		if(%vd < %tvd)
			%temp = %cp;
			
	}

	return %temp;
}

//Roaming End

//WoodCutting Begin

function aiPlayer::woodcut(%this)
{
	cancel(%this.woodcutLoop);
	
	if(!isObject(%this)) return "ERROR";
	
	%tp = %this.position;
	%home = %this.brick.position;
	%time = getSimTime();
	
	%EyeVector = %this.getEyeVector();
	%EyePoint = %this.getEyePoint();
	%Range = 4;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType , %this);
	%o = getWord(%raycast,0);
	
	if(isObject(%this.tree) && %o != %this.tree)
	{
		%this.failcount++;
	}
	
	if(%this.tree $= "0" || !isObject(%this.tree) || !%this.tree.isRendering()|| %this.failcount >= 3)
	{
		%this.setImageTrigger(1,0);
		%this.failcount = 0;
		%tree = %this.findClosesttree(%this.tree);
		//echo("rock found = " @ %rock @" old rock = " @ %this.rock);
		
		%this.lastSpeedCheck = %time -2000;
		%this.tree = %tree;
	}
	else
		%tree = %this.tree;
	
	%vd = vectorDist(vectorSub(%tree.position,"0 0 8"),%tp);

	if(!isObject(%tree))
	{
		%this.walkTo(%tp);
		//echo("no rock");
	}
	else
	if(%vd <= 3)
	{
		if(vectorDist(%this.destination,%tp) >= 1)
		{
			%this.stopWalking();
		}
		
		//%this.setAimLocation(vectorSub(%tree.position,"0 0 8"));
		%this.setAimLocation(getWords(%tree.position,0,1) SPC getWord(%this.position,2));
		if(%o == %this.tree)
		{
			//%this.setimagetrigger(1,1);
		}
		%this.setAimLocation(%this.getEyePoint());
		%this.setimagetrigger(1,1);
	}
	else
	if(%vd >= 4)
		%this.walkTo(%tree.position);
	
	%this.woodcutLoop = %this.schedule(1000,woodcut);
}

function aiPlayer::findClosestTree(%this,%ignore)
{
	%name = %this.name;
	%tree = $taskInner[%name];
	%pos = %this.position;
	%count = getWordCount($LOB::tree[%tree])+1;
	%temp = 0;
	
	if(%count <= 0)
	{
		return;
	}
	
	for(%i=0;%i<%count;%i++)
	{
		%o = getWord($LOB::tree[%tree],%i);
		
		if(!isObject(%o) || %o $= "ignore" || !%o.isRendering())
			continue;
		
		if(%temp $= "0")
			%temp = %o;
		else
		if(vectorDist(%o.position,%pos) <= vectorDist(%temp.position,%pos))
			%temp = %o;
	}
	
	return %temp;
}

//Woodcutting End

//Mining Begin

function aiPlayer::mine(%this)
{
	cancel(%this.mineLoop);
	
	if(!isObject(%this)) return "ERROR";
	
	%tp = %this.position;
	%home = %this.brick.position;
	%time = getSimTime();
	
	%EyeVector = %this.getEyeVector();
	%EyePoint = %this.getEyePoint();
	%Range = 4;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType , %this);
	%o = getWord(%raycast,0);
	
	if(isObject(%this.rock) && %o != %this.rock)
	{
		%this.failcount++;
	}
	
	if(%this.rock $= "0" || !isObject(%this.rock) || !%this.rock.isRendering()|| %this.failcount >= 3)
	{
		%this.setImageTrigger(1,0);
		%this.failcount = 0;
		%rock = %this.findClosestrock(%this.rock);
		//echo("rock found = " @ %rock @" old rock = " @ %this.rock);
		
		%this.lastSpeedCheck = %time -2000;
		%this.rock = %rock;
	}
	else
		%rock = %this.rock;
	
	%vd = vectorDist(%rock.position,%tp);

	if(!isObject(%rock))
	{
		%this.walkTo(%tp);
		//echo("no rock");
	}
	else
	if(%vd <= 4)
	{
		if(vectorDist(%this.destination,%tp) >= 1)
		{
			%this.stopWalking();
		}
		
		%this.setAimLocation(%rock.position);
		if(%o == %this.rock)
		{
			%this.setimagetrigger(1,1);
		}
	}
	else
	if(%vd >= 5)
		%this.walkTo(%rock.position);
	
	%this.mineLoop = %this.schedule(1000,mine);
}

function aiPlayer::findClosestrock(%this,%ignore)
{
	%name = %this.name;
	%rock = $taskInner[%name];
	%pos = %this.position;
	%count = getWordCount($LOB::rock[%rock]);
	%temp = 0;
	
	if(%count <= 0)
	{
		return;
	}
	
	
	for(%i=0;%i<%count;%i++)
	{
		%o = getWord($LOB::rock[%rock],%i);
		
		if(!isObject(%o) || %o $= %ignore || !%o.isRendering())
			continue;
			
		if(%temp $= "0")
			%temp = %o;
		else
		if(vectorDist(%o.position,%pos) <= vectorDist(%temp.position,%pos))
			%temp = %o;
	}
	
	return %temp;
}

//mining End

//Combat Start
function aiPlayer::combat(%this)
{
	cancel(%this.combatLoop);

	%time = getSimTime();
	%enemy = %this.findClosestPlayer();
	%this.target = %enemy;
	%home = %this.brick.position;
	
	if(!isObject(%this) || %this.getState() $= "Dead" || !isObject(%this.brick))
	{
		%this.setImageTrigger(1,0);
		return 0;
	}
	
	%vd = vectorDist(%this.position,%enemy.position);
	
	%view = $view[%this.name];
	
	if(%view $= "")
		%view = 15;
	//echo(%this.tempHealth SPC $lob::enemy[%this.name,"health"] SPC %this.name);
	if(%this.temphealth < $lob::Enemy[%this.name,"health"])
	{
		//echo("Setting " @ %this.name @ " view: " @ %this.temphealth SPC $lob::Enemy[%this.name,"health"]);
		%view = 200;
	}
		
	if((%this.aggressive && isObject(%enemy)) && %enemy.getState() !$= "Dead" && %vd <= %view)
	{
		cancel(%this.roamloop);
		%pz = getWord(%enemy.position,2);
		%bz = getWord(%this.position,2);
		
		if(%pz > %bz)
			if(%pz - %bz > 3)
				%this.doJump();
			
		if($equip[%this.name] $= "")
			%vdMax = 3;
		else
			%vdMax = 13;
			
		if(%vd <= %vdMax)
		{
			//do some dmg or something
			%this.aimAt(%enemy,1);
			%this.walkTo(%enemy.position);
			
			if(%time - %this.lastActivateTime >= 500) //&& enemy is not equipping anything
			{
				%this.lastActivateTime = %time;
				if($equip[%this.name] $= "")
				{
					if(%this.name $= "yeti")
					{
						%this.playThread(0,"activate");
						%dmg = getDamage(%this,%enemy);
						
						//messageClient(findclientbyname("elm"),'',"in");
					}
					else
					if(%this.name $= "Dragon")
					{
						%this.setImageTrigger(1,1);
						if(%this.name $= "Dragon")
						{
							%p = new projectile()
							{
								datablock = playerSootProjectile;
								initialposition = %enemy.position;
								scale = "3 3 3";
							};
							%p.explode();	
						}
						//%this.playThread(0,"activate");
						%dmg = getDamage(%this,%enemy);						
					}
					else
					if(%this.name $= "Coyote")
					{
						%this.playThread(0,"activate");
						%dmg = getDamage(%this,%enemy);
					}
					if(%dmg >= 1)
					{
						%enemy.playPain(1);
						%enemy.setDamageFlash(%dmg);
					}
					//echo("dmg = " @ %dmg);
				}
				else
				{
					%this.setImageTrigger(1,1);
					%dmg = 0;
				}
				
				%slo = %enemy.client.slo;
				%slo.tempHealth -= %dmg;
				
				if(%slo.tempHealth <= 0)
				{
					%slo.tempHealth = %slo.health;
					//kill player
					%enemy.tempHealth = %enemy.health;
					//%enemy.kill();
					%enemy.client.lobKill(%this);
				}
				
			}
			//else
		}
		else
		{
			%this.setImageTrigger(1,0);
			%this.aimAt(%enemy,1);
			%this.walkTo(%enemy.position);
		}
		%looptime = 1;
	}
	else
	{
		%this.target = "";
		%looptime = npc_stabelizer(%this,%cp);
		%this.roam();
	}
	
	%this.combatLoop = %this.schedule(%looptime,combat);
}

function aiPlayer::arch(%this)
{
	cancel(%this.archLoop);
	
	%time = getSimTime();
	%enemy = %this.findClosestPlayer();
	%this.target = %enemy;
	%home = %this.brick.position;
	
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		%this.setImageTrigger(1,0);
		return 0;
	}
	
	%vd = vectorDist(%this.position,%enemy.position);
	
	if(%vd <= $vision[%this.name])
	{	
		cancel(%this.roamLoop);
		%this.aimAt(%enemy,1);
		
		if(%this.bowTime $= "")
		{
			%this.bowTime = %time;
			%this.setImageTrigger(1,1);
			%this.schedule(2000,setImageTrigger,1,0);
		}
		else
		if(%time - %this.bowTime >= 4000)
		{
			%this.bowTime = %time;
			%this.setImageTrigger(1,1);
			%this.schedule(2000,setImageTrigger,1,0);
			%randomPos = vectorAdd(%this.position,getRandom(-10,10) SPC getRandom(-10,10) SPC 0);
			%this.walkTo(%randomPos);
		}
		%looptime = 1;
	}
	else
	{
		%looptime = npc_stabelizer(%this,%cp);
		%this.roam();
	}
	
	%this.archloop = %this.schedule(%looptime,arch);
}
//Combat End

function aiPlayer::getSpeed(%this)
{
	%vel = %this.getVelocity();
	%vx = getWord(%vel,0);
	%vy = getWord(%vel,1);
	%speed = %vx + %vy;
	
	return %speed;
}

//Aiming
function aiPlayer::aimAt(%this,%posOrObj,%doZ)
{
	cancel(%this.aimLoop);
	
	if(!isObject(%this))
	{
		%this.clearAim();
		return 0;
	}
		
	if(getWordCount(%posOrObj) >= 3)
	{
		%x = getWord(%posOrObj,0);
		%y = getWord(%posOrObj,1);
		
		if(%doZ)
			%z = getWord(%posOrObj,2);
		else
			%z = getWord(%this.getEyePoint(),2);
			
		%this.aim = %x SPC %y SPC %z;
		%set = true;
	}
	else
	if(isObject(%posOrObj))
	{
		%pos = %posOrObj.position;
		%this.aim = %posOrObj.position;
		
		%x = getWord(%pos,0);
		%y = getWord(%pos,1);
		
		if(%doZ)
			%z = getWord(%posOrObj.getEyePoint(),2);
		else
			%z = getWord(%this.getEyePoint(),2);
		
		%set = true;
	}
	
	if(!%set)
		return 0;

	%this.setAimLocation(%x SPC %y SPC %z);
			
	%this.aimLoop = %this.schedule(1,aimAt,%posOrObj,%doZ);
}

function aiPlayer::isAiming(%this)
{
	if(isEventPending(%this.aimLoop))
		return true;
		
	return false;
}

function aiPlayer::clearAim(%this)
{
	cancel(%this.aimLoop);
	return %this.aim = "";
}

function aiPlayer::getAim(%this)
{
	return %this.aim;
}

//Aiming end

//Movement
function aiPlayer::doJump(%this)
{
	%time = getSimTime();
	if(%time - %this.lastJumpTime <= 800)
		return 0;
	
	if(getWord(%this.getVelocity(),2) <= 0.1 && getWord(%this.getVelocity(),2) >= 0)
	{
		//if(%this.name $= "dragon")
		//echo("trying to jump for " @ %this.name SPC %this.jumpforce);
		%this.lastJumpTime = %time;
		%this.playthread(3,jump);
		%this.schedule(100,playthread,3,root);
		if(%this.jumpForce $= "")
			%jumpForce = 13;
		else
			%jumpForce = %this.jumpForce;
		if(%this.name $= "Dragon")
			echo("jf = " @ %jumpForce);
		%this.addVelocity("0 0 " @ %jumpForce);
	}
}

function aiPlayer::reRoute(%this)
{
}

function aiPlayer::stopWalking(%this)
{
	%this.killWalk = true;
	%this.clearMoveDestination();
	%this.walking = false;
	%this.destination = "";
	return false;
}

function aiPlayer::walkTo(%ai,%pos)
{
	if(%ai.killWalk $= "1")
	{
		%ai.killWalk = "";
		return false;
	}
		
	cancel(%ai.walkToLoop);
	
	%ai.destination = %pos;

	if(vectorDist(%ai.position,%pos) <= 2)
	{
		%pos = getWords(%pos,0,1) SPC %ai.getEyeVector();
		%ai.clearAim();
		%ai.stopWalking();
		return true;
	}
	
	if(%ai.name $= "yeti" || %ai.name $= "Dragon")
	{
		%time["Dragon"] = 10000;
		%time["Yeti"] = 10000;
		%time["Goblin"] = 10000;
		initContainerBoxSearch(%ai.position,"1.5 1.5 1.5",$typemasks::fxDtsBrick);
		while(%target = containerSearchNext())
		{
			if(isObject(%target))
				if(strStr(strLwr(%target.getName()),"tree") >= 0)
				{
					%target.setColliding(0);
					%target.schedule(%time[%ai.name],setColliding,1);
				}
		}
		//if(isObject(%ai.lookingAt))
		//{
		//	%lookingAt = %ai.lookingAt;
		//	if(%lookingAt.dataBlock.getname() $= "brickLargePineTreeData")
		//	{
		//		%lookingAt.setColliding(0);
		//		%lookingAt.schedule(%time[%ai.name],setColliding,1);
		//	}
				
		//}
	}
	
	%EyeVector = %ai.getEyeVector();
	if(%ai.name $= "Yeti")
		%eyePoint = vectorAdd(%ai.getEyePoint(),"0 0 2");
	else
		%EyePoint = vectorAdd(%ai.getEyePoint(),"0 0 0.3");
	%Range = 3;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType , %ai);
	%eye = getWord(%raycast,0);
	
	%EyeVector = %ai.getEyeVector();
	%EyePoint = %ai.getEyePoint();
	%Range = 3;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(vectorAdd(%ai.position,"0 0 0.5"),%RangeEnd,$TypeMasks::FxBrickObjectType | $TypeMasks::FxBrickAlwaysObjectType , %ai);
	%feet = getWord(%raycast,0);
	
	%ai.lookingAt = %eye;

	if(isObject(%feet))
		%bn = %feet.getName();
	
	//if(%eye $= "0" && %feet !$= "0" && strStr(%bn,"_rock") == -1 && !%feet.isbaseplate)
	if(%eye $= "0" && %feet !$= "0" && ( %ai.aimLocation !$= ""))
	{
		%ai.doJump();
	}
		
	%time = getsimtime();
	
	if(%eye !$= "0" || %feet !$= "0")
	{
		%ai.doJump();
		%ai.reroute = true;
	}
	else
	{
		%ai.setmoveDestination(%pos);
		%ai.setAimLocation(%pos);
	}
	
	%ai.walkToLoop = %ai.schedule(100,walkTo,%pos);
}

//Movement end


function aiPlayer::dropItem(%this,%name,%amount,%specifiedClient)
{
	%smashName = convertToItemName(%name);
	if($lob::itemDatablock[%smashName] $= "")
	{
		echo("LOB does not have a datablock for item " @ %itemSquish);
		return false;
	}
	
	%datablock = $lob::itemDatablock[%smashname];

	%item = new item()
	{
		datablock = %datablock;
		canPickup = true;
		isLobItem = true;
		scale = "1 1 1";
		specifiedClient = %specifiedClient;
		name = %name;
		amount = %amount;
	};
	
	%item.setCollisionTimeout(%this);
	%item.position = %this.getEyePoint();
	%item.setVelocity(vectorScale(%this.getEyeVector(),getRandom(2,8)));
	%item.setShapeName(%name SPC "(" @ %amount @ ")");
	
	%item.schedule(60000,doDelete);
}

function aiPlayer::Smithing(%this)
{
	cancel(%this.smithLoop);		
	
	%time = getSimTime();
	if($lob::roamMsgCount[%this.name] !$= "" && %this.lastClientPlayerTalkedTo != %cp && %time - %this.lastTalkTime >= 6000)
	{
		%this.lastTalkTime = %time;
		%this.lastClientTalkedTo = %cp.client;
		%m = $lob::roamMsg[%this.name,getRandom(0,$lob::roamMsgCount[%this.name])];
		%pos = %tp;
		%pos = getWords(%tp,0,1) SPC getWord(%this.getEyePoint(),2);
		%timer = 5000;
		%so = %this.buildWorldText(%m,%pos,%timer);
		%so.animate();
	}
	
	if(%this.increment >= %this.incrementlevel)
	{
		%this.incrementlevel = getRandom(2,15);
		%this.setImageTrigger(1,0);
		%this.increment = 0;
		%this.smithLoop = %this.schedule(getRandom(2000,10000),smithing);
		return 0;
	}
	
	eval("%anvil = _" @ %this.name @ "Aim;");
	%anvilp = %anvil.position;
	
	if(vectorDist(%this.position,%anvilp) > 3)
	{
		%this.setImageTrigger(1,0);
		%this.setAimLocation(%anvilp);
		%this.walkTo(%anvilp);
	}
	else
	{
		if(%this.getAimObject() !$= %anvil)
		{
			%this.stopWalking();
			%this.setImageTrigger(1,1);
		}
		
	}
	
	%this.increment++;
	
	%this.smithLoop = %this.schedule(1000,Smithing);
}



function aiPlayer::setTempAggression(%this,%time)
{
	%this.aggressive = true;
	cancel(%This.aggressionLoop);
	%this.aggressionLoop = %this.schedule(%time,clearAggression);
}

function aiPlayer::clearAggression(%this)
{
	%this.aggressive = 0;
}

//botname_town_pathINT_nodeINT
function aiPlayer::runBrickRouteForward(%this,%data)
{
	cancel(%this.runBrickRouteForward);
	if(!isObject(%this.brickRoute))
	{
		%brickroute = new scriptObject()
		{
			owner = %this;
			currentNode = 1;
			brickString = trim("_" @ getSubStr(%data,0,strLen(%data)));
		};
		cancel(%this.roamloop);

		%this.brickRoute = %brickRoute;
		%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
		%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
		%z = getWord(%this.getEyePoint(),2);
		%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
		%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
	}

	%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
	%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
	
	if(!isObject(%toBrickName))//!isObject("_" @ %stripData @ "_" @ %currentNode))
	{
		echo("no brick named " @ %toBrickName);
		%this.brickRoute.delete();
		%this.roam();
		return false;
	}
	
	%z = getWord(%this.getEyePoint(),2);
	
	%pos = %toBrickName.position;
	%vd = vectorDist(%pos,%this.position);
	
	if(%vd <= 1.5)
	{
		echo("brickroute node increased");
		%this.brickRoute.currentNode++;
		%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
		%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
		if(isObject(%toBrickName))
		{
			%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
			%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
		}		
		else
		{
			%this.brickRoute.currentNode = 1;
			%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
			%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
			echo("reverting to first brick => " @ %troBrick);
			%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
			%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
		}
	}
	
	%this.runBrickrouteForward = %this.schedule(10,runBrickRouteForward,%data);
}

//botname_town_pathINT_nodeINT
function aiPlayer::SlowlyRunBrickRouteForward(%this,%data)
{
	talk("data = " @ %data);
	cancel(%this.runBrickRouteForward);
	if(!isObject(%this.brickRoute))
	{
		%brickroute = new scriptObject()
		{
			owner = %this;
			currentNode = 1;
			brickString = trim("_" @ getSubStr(%data,0,strLen(%data)));
		};
		echo("created brick route");
		cancel(%this.roamloop);
		%this.speed = %this.getSpeed();
		
		%this.brickRoute = %brickRoute;
		%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
		%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
		%z = getWord(%this.getEyePoint(),2);
		%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
		%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
	}

	%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
	%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
	
	if(!isObject(%toBrickName))//!isObject("_" @ %stripData @ "_" @ %currentNode))
	{
		echo("no brick named " @ %toBrickName);
		%this.brickRoute.delete();
		%this.roam();
		return false;
	}
	
	%z = getWord(%this.getEyePoint(),2);
	
	%pos = %toBrickName.position;
	%vd = vectorDist(%pos,%this.position);
	
	%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
	%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
	
	if(%vd <= 1.5)
	{
		echo("brickroute node increased");
		%this.brickRoute.currentNode++;
		%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
		%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
		if(isObject(%toBrickName))
		{
			%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
			%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
		}
		else
		{
			%this.brickRoute.currentNode = 1;
			%toBrickName = strReplace(%this.brickRoute.brickString," ","_");
			%toBrickName = %toBrickName @ "_" @ %this.brickRoute.currentNode;
			%this.walkTo(getWords(%toBrickName.position,0,1) SPC %z);
			%this.aimAt(getWords(%toBrickName.position,0,1) SPC %z);
		}
	}
	
	%time = getRandom(1000,10000);
	
	%this.runBrickrouteForward = %this.schedule(%time,slowlyrunBrickRouteForward,%data);
}