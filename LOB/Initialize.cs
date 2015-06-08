$LOB::GuiVersion = 13;
$LOB::ServerVersion = 6;

$LOB::correctName["PineSpaceValley"] = "Pine Valley";

$LOB::hasDatablock["gold"] = goldItem;

$nodeColor["citizen","headskin"] = "1 0.8 0.6 1";
$nodeColor["citizen","chest"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["citizen","pants"] = "0.898039 0.898039 0.000000 1.000000";
$nodeColor["citizen","larm"] = "0.898039 0.898039 0.000000 1.000000";
$nodeColor["citizen","rarm"] = "0.898039 0.898039 0.000000 1.000000";
$nodeColor["citizen","lhand"] = "1 0.8 0.6 1";
$nodeColor["citizen","rhand"] = "1 0.8 0.6 1";
$nodeColor["citizen","lshoe"] = "0.392157 0.192157 0.000000 1.000000";
$nodeColor["citizen","rshoe"] = $nodeColor["citizen","lshoe"];
$roam["citizen"] = 5;
//$nodeColor["citizen","pack"] = $nodeColor["citizen","pants"];
//$pack["citizen"] = "pack";
$smiley["citizen"] = "";
$decal["citizen"] = "";
$task["citizen"] = "roam";

$LOB::Commands = "#whisper \n#players \n#commands\n #stats playername";

//-

//ALL THIS IS DEPRECATED, IT IS NOT IN USE

$lob::itemConvert["itemorestin"] = "Tin Ores";
$lob::itemConvert["itemorescopper"] = "Copper Ores";
$lob::itemConvert["itemoresiron"] = "Iron Ores";

$lob::itemConvert["itemlogspine"] = "Pine Logs";
$lob::itemConvert["itemlogsoak"] = "Oak Logs";
$lob::itemConvert["itemlogswillow"] = "Willow Logs";
$lob::itemConvert["itemlogsmaple"] = "Maple Logs";

//bronze
$lob::itemConvert["itembronzeshortsword"] = "Bronze Short Sword";
$lob::itemConvert["itembronzeaxe"] = "Bronze Axe";
$lob::itemConvert["itemBronzepickaxe"] = "Bronze Pickaxe";
$lob::itemConvert["itemBronzeBow"] = "Bronze Bow";

//iron
$lob::itemConvert["itemironshortsword"] = "Iron Short Sword";
$lob::itemConvert["itemironaxe"] = "Iron Axe";
$lob::itemConvert["itemironpickaxe"] = "Iron Pickaxe";
$lob::itemConvert["itemIronBow"] = "Iron Bow";

//steel
$lob::itemConvert["itemSteelshortsword"] = "Steel Short Sword";
$lob::itemConvert["itemSteelaxe"] = "Steel Axe";
$lob::itemConvert["itemSteelpickaxe"] = "Steel Pickaxe";
$lob::itemConvert["itemSteelBow"] = "Steel Bow";

//mithril
$lob::itemConvert["itemMithrilBow"] = "Mithril Bow";
$lob::itemConvert["itemMithrilshortsword"] = "Mithril Short Sword";
$lob::itemConvert["itemMithrilaxe"] = "Mithril Axe";
$lob::itemConvert["itemMithrilpickaxe"] = "Mithril Pickaxe";
$lob::itemConvert["itemMithrilBow"] = "Mithril Bow";

$lob::itemConvert["itemBarsBronze"] = "Bronze Bars";
$lob::itemConvert["itemBarsIron"] = "Iron Bars";
$lob::itemConvert["itembarssteel"] = "Steel Bars";
$lob::itemConvert["itembarsmithril"] = "Mithril Bars";

$lob::itemRevert["Bronze Bars"] = "barsBronze";
$lob::itemRevert["Iron Bars"] = "barsIron";
$lob::itemRevert["steel Bars"] = "barsSteel";
$lob::itemRevert["mithril Bars"] = "barsMithril";

$lob::itemRevert["Pine Logs"] = "logsPine";
$lob::itemRevert["Oak Logs"] = "logsOak";
$lob::itemRevert["Willow Logs"] = "logsWillow";

$lob::itemRevert["PineLogs"] = "logsPine";
$lob::itemRevert["OakLogs"] = "logsOak";
$lob::itemRevert["willowLogs"] = "logsWillow";

$lob::itemRevert["Tin Ores"] = "oresTin";
$lob::itemRevert["Copper Ores"] = "oresCopper";
$lob::itemRevert["Iron Ores"] = "oresIron";
$lob::itemRevert["Steel Ores"] = "oresSteel";
$lob::itemRevert["Mithril Ores"] = "oresMithril";

$lob::itemRevert["TinOres"] = "oresTin";
$lob::itemRevert["CopperOres"] = "oresCopper";
$lob::itemRevert["ironOres"] = "oresIron";
$lob::itemRevert["SteelOres"] = "oresSteel";
$lob::itemRevert["MithrilOres"] = "oresMithril";

$lob::itemConvert["itemGold"] = "Gold";
$lob::itemRevert["gold"] = "Gold";

//ALL DEPRECATED, END

$lob::tc["random"] = -1;
$lob::tip["Random",$lob::tc["random"]++] = "Did you know you can examine your surroundings to train different skills. Next time you're near a body of water, try clicking it to see what happens.";
$lob::tip["Random",$lob::tc["random"]++] = "Did you know that the text you send in the world chat, can be seen above your head by other players around you?";
$lob::tip["Random",$lob::tc["random"]++] = "There are a list of commands you're able to use. To see this list at anytime, type #commands into the chat.";
$lob::tip["Random",$lob::tc["random"]++] = "You can view player's stats by either clicking on them, or using the chat command.";
$lob::tip["Random",$lob::tc["random"]++] = "You can whisper your friends in-game. Use the #whisper username function to do so.";
$lob::tip["Random",$lob::tc["random"]++] = "Some enemies will attack you without warning while others need to be provoked into attacking you.";
$lob::tip["Random",$lob::tc["random"]++] = "Equipping an item from the inventory GUI places an item in your sub inventory. Just drop the weapon if you need space.";
$lob::tip["Random",$lob::tc["random"]++] = "Upon dying you will lose EVERYTHING in your inventory. The only item you'll keep is the item you have equipped in your hand. So make sure to bank frequently.";

$lob::tc["mining"] = -1;
$lob::tip["mining",$lob::tc["mining"]++] = "You can mine ores and smelt them in a furnace to create metals. Metals then can be used to make weapons.";
$lob::tip["mining",$lob::tc["mining"]++] = "The rock models used in Land of Blocks, look similiar to a popular web-browser MMORPG.";

$lob::tc["woodCutting"] = -1;
$lob::tip["woodCutting",$lob::tc["woodcutting"]++] = "You can cut down trees to build fires on which you can cook raw meat which will heal you.";
$lob::tip["woodCutting",$lob::tc["woodcutting"]++] = "When woodcutting, the axe and Wood Cutting level you have will determine the amount of damage you will do to the tree.";
$lob::tip["woodCutting",$lob::tc["woodcuttin"]++] = "Every tree's health is different. The higher the Wood Cutting level it requires, the better quality wood it produces.";


$stacked["Gold"] = true;
$questItem["grandFather's Sword"] = true;

//GHETTO FIXES

golditem.friction=1;
uncookedBeefItem.friction=1;
cookedBeefItem.friction=1;
uncookedLobsterItem.friction=1;
cookedLobsterItem.friction=1;
uncookedSteakItem.friction=1;
cookedSteakItem.friction=1;
scrollItem.friction=1;
//--

if($Server::Dedicated)
{
	if(!isObject($lob::fakeClient))
	{
		$Lob::fakeClient = new aiConnection()
		{
			Name = "Fake Client";
			Bl_ID = "1337";
			BrickGroup = new SimGroup("BrickGroup_1337") 
			{
				client = 0;
				BL_ID = "1337";
				Name = "Fake Client";
			};
		};
		
		if(isObject(MainBrickGroup))
		{
			MainBrickGroup.add($LOB::fakeClient.brickGroup);
		}
	}
}

if(!$LOB::loadExp)
{
	$LOB::loadExp = true;
	$lob::levelcap = 99;
	
	for(%i=0;%i<100;%i++)
	{
		$LOB::ExpNeeded["Mining",%i] = %i * 250;
		$LOB::ExpNeeded["WoodCutting",%i] = %i * 250;
		$LOB::ExpNeeded["Combat",%i] = %i * 250;
		$LOB::ExpNeeded["Attack",%i] = %i * 250;
		$LOB::ExpNeeded["Defense",%i] = %i * 250;
		$LOB::ExpNeeded["Strength",%i] = %i * 250;
		$LOB::ExpNeeded["Firemaking",%i] = %i * 150;
		$LOB::ExpNeeded["Smithing",%i] = %i * 250;
		$LOB::ExpNeeded["Smelting",%i] = %i * 250;
		$LOB::ExpNeeded["Cooking",%i] = %i * 250;
		$LOB::ExpNeeded["Magic",%i] = %i * 250;
		$lob::expneeded["Climbing",%i] = %i * 250;
	}
	
	for(%i=0;%i<1000;%i++)
		$LOB::lowerBountyCost[%i] = %i * 300;
}

function initLobServer()
{
	%groupCount = MainBrickGroup.getCount();
	
	for(%i=0;%i<%groupCount;%i++)
	{
		%group = MainBrickGroup.getObject(%i);
		%count = %group.getCount();
		
		for(%j=0;%j<%count;%j++)
		{
			%brick = %group.getObject(%j);
			
			if($LOB::brickloaded[%brick] && (getWord(%bn,0) !$= "NPCSpawn" || getWord(%bn,0) !$= "enemyspawn"))
				continue;
			
			
			%bn = getSubStr(%brick.getName(),1,strLen(%brick.getName()));
			%bn = trim(strReplace(%bn,"_"," "));
				
			if(getWord(%bn,0) $= "rock")
			{

				$LOB::brickLoaded[%brick] = 1;
				$LOB::brickLoadedCount++;
				%rockType = getWord(%bn,1);
				
				addToList("$LOB::rock["@%rockType@"]",%brick);
				addToList("$LOB::rock[\"ALL\"]",%brick);
			}
			else
			if(getWord(%bn,0) $= "Tree")
			{
				
				$LOB::brickLoaded[%brick] = 1;
				$LOB::brickLoadedCount++;
				%treeType = getWord(%bn,1);
				
				addToList("$LOB::tree["@%treeType@"]",%brick);
				addToList("$LOB::tree[\"ALL\"]",%brick);
			}
			else	
			if(getWord(%bn,1) $= "spawn")
			{
				$LOB::brickLoaded[%brick] = 1;
				$LOB::brickLoadedCount++;
				%area = getWord(%bn,0);
				
				addToList("$LOB::spawn["@%area@"]",%brick);
				
			}
			else
			if(getWord(%bn,0) $= "NPCSpawn")
			{
				$LOB::brickLoaded[%brick] = 1;
				$LOB::brickLoadedCount++;
				
				%area = getWord(%bn,0);
				%name = getWord(%bn,2);
				%datablock = $LOB::NPC[%name,"Datablock"];
				
				addToList("$LOB::NPCSpawn["@%area@"]",%brick);
				addToList("$LOB::NPCSpawn[\"ALL\"]",%brick);
					
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
					
				if($equip[%name] !$= "")
				{
					equipPlayer(%o,$equip[%name]);
				}
				
				if($task[%name] $= "")
					%o.schedule(200,roam);
				else
					eval("%o.schedule(200,"@$task[%name]@");");
					
				if($lob::isShopNpc[%name])
					%o.lob_newShop();

				if(isFunction(%o.name,"onObjectSpawned"))
					eval("" @ %o.name @ ".onObjectSpawned(" @ %o @ ");");
			}
			else
			if(getWord(%bn,0) $= "EnemySpawn")
			{
				$LOB::brickLoaded[%brick] = 1;
				$LOB::brickLoadedCount++;
				
				%area = getWord(%bn,0);
				%name = getWord(%bn,2);
				%datablock = $LOB::Enemy[%name,"Datablock"];
				%aggressive = $LOB::Enemy[%name,"Aggressive"];
				%health = $LOB::Enemy[%name,"Health"];
				
				addToList("$LOB::EnemySpawn["@%area@"]",%brick);
				addToList("$LOB::EnemySpawn[\"ALL\"]",%brick);
					
				%o = new aiPlayer()
				{
					onClickAction = $LOB::OnClickAction[%area,%name];
					datablock = %datablock;
					position = vectorAdd(%brick.position,"0 0 0.2");
					name = %name;
					Level = getRandom(getWord($LOB::Enemy[%name,"Level"],0),getWord($LOB::Enemy[%name,"Level"],0));
					Aggressive = %aggressive;
					TempHealth = %health;
				};
				
				%o.brick = %brick;
				%brick.NPC = %o;
				
				newCombatDataFromLevel(%o,%o.level);
				
				if($nodeColor[%name,"chest"] $= "")
					dressplayer(%o,"citizen");
				else
					dressplayer(%o,%name);
					
				if($equip[%name] !$= "")
				{
					equipPlayer(%o,$equip[%name]);
				}
				
				if($task[%name] $= "")
					%o.schedule(200,roam);
				else
					eval("%o.schedule(200,"@$task[%name]@");");
					
				if(isFunction(%o.name,"onObjectSpawned"))
					eval("" @ %o.name @ ".onObjectSpawned(" @ %o @ ");");
			}
		}
	}
	
	schedule(1000,0,deleteNpcs);
}