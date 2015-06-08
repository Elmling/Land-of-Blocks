$defaultKeyWordColor = "<color:1040DE>";

function setKeyWords(%string,%words,%currColor,%color)
{
	if(%string $= "" || %words $= "" || %currColor $= "")
		return "0 FIELD MISSING";
	else
	if(%color $= "")
		%color = $defaultKeyWordColor;
		
	%wordCount = getWordCount(%words);
	
	for(%i=0;%i<%wordCount;%i++)
	{
		%word = getWord(%words,%i);
		
		if(strStr(strLwr(%string),strLwr(%word)) >= 0)
			%string = strReplace(%string,%word,%color@%Word@%currColor);
	}
	
	return %string;
}

function smartMessage(%client,%message,%timeOut,%type)
{
	if(%client $= "" || %message $= "" || %timeOut $= "")
	{
		warn("Field missing \nUsage: smartMessage(%client,%message,%timeout)");
		return "0 FIELD MISSING";
	}
	
	%time = getSimTime();
	
	if(%time - $smartMessageLastTimeOut[%client,%message] > $smartMessageTimeOut[%client,%message])
	{
		%setTimeOut = true;
		
		if(%type $= "CenterPrint")
			commandToClient(%client,'centerprint',%message,3);
		else 
		if(%type $= "BottomPrint")
			commandToClient(%client,'bottomprint',%message,3);
		else
			messageClient(%client,'',%message);
	}
	else
	if(!$smartMessage[%client,%message])
	{
		%setTimeOut = true;
		$smartMessage[%client,%message] = 1;
	}

	if(%setTimeOut)
	{
		$smartMessageLastTimeOut[%client,%message] = %time;
		$smartMessageTimeOut[%client,%message] = %timeOut;
	}
	
	return "1 SUCCESS";
}

function linkParser(%string)
{	
	if(strStr(strLwr(%string),"http:") >= 0 || strStr(strLwr(%string),"https:") >= 0)
	{
		%stringCount = getWordCount(%string);
		
		for(%i=0;%i<%stringCount;%i++)
		{
			%word = getWord(%string,%i);

			if(strCmp(%word,"https") == 1)
			{
				%replace = getSubStr(%word,0,strStr(%word,"//") + 2);
				%string = strReplace(%string,%replace,"");
			}
			else
			if(strCmp(%word,"http") == 1)
			{
				%replace = getSubStr(%word,0,strStr(%word,"//") + 2);
				%string = strReplace(%string,%replace,"");
			}
				
			%string = setWord(%string,%i,"<a:" @ %string @ ">LINK</a>");
		}
	}
	return %string;
}

function gameConnection::guiCheck(%this)
{
	%this.lob_guiOutdated = true;
	%this.handShakePending = true;
	commandToClient(%this,'shakeHands');
	%this.setGuiNull = %this.schedule(2000,0,setguinull);
}

function gameConnection::setGuiNull(%this)
{
	%this.slo.hasgui = "";
	%this.slo.guiVersion = "";
}

function serverCmdShakeHands(%client,%version)
{
	if(%client.handShakePending)
	{
		cancel(%client.setGuiNull);
		%client.slo.hasGui = true;
		%client.slo.guiVersion = %version;
		echo(%client.name @ " is running GUI version " @%version @ " | Server GUI Version " @ $lob::guiVersion);
		if(%version < $LOB::GuiVersion || %version > $LOB::GuiVersion)
		{
			echo(%client.name @ " has an old version of the gui.");
			//%client.delete("Sorry " @ %client.name @ ", you need the GUI. " @ linkParser("http://www.elmshouse.weebly.com/land-of-blocks.html") @ "\n Hurry and join the fun.");
			//messageClient(%this,'',"\c6You have an old version of the GUI. Please update.");
		}
		else
		{
			cancel(%client.guiWarn);
			%client.lob_guiOutdated = "";
		}
	}
	
	%client.handShakePending = false;
}

function gameConnection::guiWarn(%this)
{
	//overwride
	//return false;
	//cancel(%this.guiWarnLoop);
	
	if(%this.lob_guiOutdated)
	{
		commandToClient(%this,'MessageBoxOk',"Welcome To Land of Blocks","Welcome " @ %this.name @". You can play Land of Blocks, but will be limited on what you can do until you download the client: " @ linkParser("http://forum.blockland.us/index.php?topic=228085.0") @ ".");
	}
	
	//%this.guiWarnLoop = %this.schedule(15000,guiWarn);
	//%this.delete("Please download the client. Sorry for the inconvenience.\n" @ linkParser("http://www.elmshouse.weebly.com/land-of-blocks.html"));
}

function lob_playerNeedsGui(%client)
{
	%h = "Hey " @ %client.name @ "!";
	%b = "You need the client to talk to NPCS. Download it here: " @ linkParser("http://www.landofblocks.weebly.com/downloads.html") @ ".";
	
	commandtoclient(%client,'messageBoxOk',%h,%b);	
}

function aiPlayer::fakeYourDeath(%this,%ammount)
{
	%brick = %this.brick;
	%name = %this.name;
	%datablock = %this.getDatablock();
	%health = $LOB::Enemy[%name,"Health"];
	%level = %this.level;
	%action = %this.onClickAction;
	%aggressive = $LOB::Enemy[%name,"Aggressive"];
	
	if(%aggressive $= "")
		%aggressive = 0;
		
	if(%this.name $= "onyx")
		serverPlay3d(onyxDeathSound,%this.position);
	
	if(%brick $= "" || %this.isDungeon)
	{
		if(%this.isDungeon)
		{
			%this.brick.delete();
			%this.dungeon.npcsLeft--;
			%this.dungeon.checkNpcs();
		}
		%this.kill();
		return 0;
	}
	
	%this.kill();
	
	if(!isEventPending(%brick.respawnEvent) || isObject(%brick.npc))
		%brick.respawnEvent = schedule(%ammount,0,spawnFromDeath,%brick,%name,%datablock,%health,%level,%action,%aggressive);
}

function spawnFromDeath(%brick,%name,%datablock,%health,%level,%action,%aggressive)
{
	
	if(isObject(%brick.npc))
		return false;
		
	if(!isObject(%brick))
		return false;
		
	%spawnName = strLwr(%brick.getname());
	%area = strReplace(%spawnName,"enemyspawn","");
	%area = getSubStr(%area,1,strLen(%area));
	%area = strReplace(%area,"_"," ");
	%area = getWords(%area,0,getWordCount(%area)-2);
	%area = trim(%area);
	
	if($lob::zonePlayerCount[%area] <= 0)
	{
		error("scriptTools.cs spawnFromDeath: will not respawn ai player, nobody is in the zone (" @ %area @ ") ");
		return false;
	}
		
	%o = new aiPlayer()
	{
		brick = %brick;
		name = %name;
		datablock = %datablock;
		aggressive = %aggressive;
		tempHealth = %health;
		level = %level;
		onClickAction = %action;
		position = vectorAdd(%brick.position,"0 0 0.2");
	};
	
	%brick.npc = %o;
	
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

function fxDtsBrick::doFakeDeath(%this,%amount)
{
	%this.rockOwner = "";
	%this.treeOwner = "";
	%this.tempHealth = $Lob::treeHealth[%this.name];
	%this.setRendering(0);
	%this.setColliding(0);
	%this.setRaycasting(0);
	cancel(%this.revive);
	%this.revive = %this.schedule(%amount,reviveFromDeath);
}

function fxDtsBrick::reviveFromDeath(%this)
{
	%this.setRendering(1);
	%this.setColliding(1);
	%this.setRaycasting(1);
}

function fxdtsBrick::fakeYourDeath(%this,%ammount)
{	
	%this.doFakeDeath(%ammount);
	return;
	
	//FUNCTION DEPRICATED USING ABOVE FOR NOW
	//--------------------------------------
	%name = %this.getname();
	%this.setname("temp2");
	
	%brick = new fxDtsBrick(temp : temp2){position = "0 0 -100";};
	%brick.setTrusted(1);
	%brick.plant();
	%brick.setName(%name);
	
	%this.setName(%name);
	%pos = %this.position;
	%this.delete();

	%brick.schedule(%ammount,plantFromDeath,%pos,%name);
}

function fxdtsBrick::plantFromDeath(%this,%position)
{
	if($server::dedicated)
	{
		%client = $lob::fakeClient;
	}
	else
		%client = %brick.client;
	
	%name = %this.getName();
	%this.setName("temp2");
	
	%brick = new fxDtsBrick(temp : temp2)
	{
		position = %position;
		client = %client;
		rockOwner = "";
		TreeOwner = "";
	};
	
	%brick.setTrusted(1);
	%brick.setName(%name);
	%brick.plant();
	
	%client.brickGroup.add(%brick);
	
	%cat = getWord(%this.getName(),0);
	%type = getWord(%this.getName(),1);
	
	if(%cat $= "Tree")
	{
		addToList("$LOB::tree["@%type@"]",%brick);
		addToList("$LOB::tree[\"ALL\"]",%brick);
	}
	else
	if(%cat $= "Rock")
	{
		addToList("$LOB::Rock["@%type@"]",%brick);
		addToList("$LOB::Rock[\"ALL\"]",%brick);
	}
	
	%this.delete();
}

function addToList(%this,%data,%seperator)
{
	eval("%d = "@%this@";");

	%newData = strReplace(%data,%seperator," ");
	
	%lwr1 = strLwr(%d);
	%lwr2 = strLwr(%data);
	
	if(%d $= "")
	{
		eval(%this@" = %newData;");
	}
	else
	{
		if(strStr(%lwr1,%lwr2) == -1)
		{
			eval(%this@" = %d SPC %newData;");
		}
	}
}

function removeFromList(%this,%data,%seperator)
{
	eval("%d = "@%this@";");
	%data = strReplace(%data,%seperator," ");
	eval(%this@" = strReplace(%d,%data,\"\");");
	eval(%this@" = trim(strReplace("@%this@",\"  \",\" \"));");
}

function mRound(%num)
{ 
	if(%num - mFloor(%num) < 0.5)  
		return mFloor(%num);  
	else  
		return mCeil(%num);  
}

function test()
{
	new aiplayer(bob){
		datablock = caterpillar;
		position = findlocalclient().player.position;
	};
}

function getDamage(%this,%enemy)
{
	%cn = %this.getClassName();
	%ecn = %enemy.getClassName();
	
	if(%cn $= "aiplayer")
	{
		%weapBonus = %this.getMountedImage(1);
		%attack = %this.attackLevel;
		%strength = %this.strengthLevel;
		%level = %this.Level;
	}
	else
	{
		%weapBonus = %this.getMountedImage(0);
		%attack = %this.client.slo.attackLevel;
		%strength = %this.client.slo.strengthLevel;
		%level = %this.client.slo.combatLevel;
	}
	
	if(%ecn $= "aiPlayer")
	{
		%edefense = %this.defenseLevel;
		%elevel = %this.combatLevel;	
	}
	else
	{
		%edefense = %this.client.slo.defenseLevel;
		%elevel = %this.client.slo.combatLevel;	
	}
	
	%weapName = %weapBonus;
	
	if(%weapBonus > 0)
	{
		%weapBonus = $LOB::WeaponDamage[getWord(%weapBonus.item.uiName,0)];
	}
	
	if(%weapBonus $= "")%weapBonus = 1;
	
	//echo(%this.client.name @ " bonus = " @%weapName.item.uiname);
	
	if($lob::isMagicItem[%weapName.item.uiName])
	{
		%weapBonus = $lob::weaponDamage[%weapName.item.uiName];
		%maxHit = mFloor((%weapBonus) + %this.client.slo.magiclevel);
		//echo("max = " @ %maxhit SPC  mfloor((msqrt(%elevel)) * 0.7));
		%clash = %maxhit - mfloor((msqrt(%elevel)) * 0.7);
		if(%clash < 0)
			%clash = %clash * -1;
			
		%clash = %clash + mfloor(msqrt(%this.client.slo.combatLevel));
	}
	else
	{
		%maxHit = mFloor((%weapBonus) + %level / 2);
		%clash = %maxhit - mfloor((msqrt(%elevel)) * 0.7);
	}
	
	%helmet = %enemy.helmet;
	
	if(%enemy.getClassName() $= "player")
	{
		if(isObject(%enemy.client.helmet))
		{
			%damageReduction["Bronze"] = "0.1";
			%damageReduction["iron"] = "0.16";
			%damageReduction["steel"] = "0.2";
			%damageReduction["mithril"] = "0.26";
			%damageReduction["adamantite"] = "0.3";
			%sub = mfloor(%clash * %damageReduction[%enemy.client.helmet.type]);
			
			%clash = %clash - %sub;
			echo(%clash @ " | " @ %sub);
		}
	}
	
	%ran = getRandom(0,20);
	
	if(%ran <= 7)
	{
		%clash = getRandom(0,%clash);
	}
	else
	{
		%clash = getRandom(mfloor(msqrt(%clash)),%clash);
	}
	
	//echo(%clash);
	
	return %clash;
}

function lob_getMaxHit(%client)
{
	%weapBonus = %client.player.getmountedImage(0);
	%weapBonus = $LOB::WeaponDamage[getWord(%weapBonus.item.uiName,0)];
	%level = %client.slo.combatlevel;
	%elevel = 1;
	
	if(%weapBonus $= "")%weapBonus = 1;
	
	%maxHit = mFloor((%weapBonus) + %level / 2);
	%clash = %maxhit - mfloor((msqrt(%elevel)) * 0.7);
	
	return %clash;
}

function newCombatDataFromLevel(%object,%level)
{
	//based off of: every 3 levels u level up in a combat level
	%skills = %level * 3;
	
	for(%i=0;%i<%skills;%i++)
	{
		%ran = getRandom(1,3);
		
		if(%ran == 1)
		{
			%object.AttackLevel ++;
		}
		else
		if(%ran == 2)
		{
			%object.DefenseLevel ++;
		}
		else
		if(%ran == 3)
		{
			%object.StrengthLevel ++;
		}
	}
}

function gameConnection::controlToCamera(%this,%mode,%modeArg)
{
	if(%mode $= "object")
	{
	
	}
	else
	if(%mode $= "Point")
	{
		%c = %this.camera;
		%c.setOrbitPointmode(%modeArg,10);
		%this.setControlObject(%c);
	}
}

function gameConnection::endOrbit(%this)
{
	%this.setControlObject(%this.player);
}

function findBotByName(%name)
{
	%list = $LOB::NPCSpawn["all"];
	
	for(%i=0;%i<getWordCount(%list);%i++)
	{
		%b = getWord(%list,%i);
		%npc = %b.npc;
		%npcn = %npc.name;
		
		if(%npcn !$= "" && strStr(strLwr(%npcn),strLwr(%name)) >= 0)
			return %npc;
	}
	
	return 0;
}

function fixHp(%name)
{
	%c = findclientbyname(%name);
	
	if(isObject(%c))
	{
		%lvl = %c.slo.combatLevel - 1;
		%hp = %lvl * 3;
		%hp += 25;
		%c.slo.health = %hp;
	}
}

//bricklargepinetreedata
//bricklargeoaktreedata
//namebundleofbricks(bricklargepinetreedata,"_tree_pine");
//namebundleofbricks(bricklargeoaktreedata,"_tree_oak");
//namebundleofbricks(bricklargewillowtreedata,"_tree_willow");
//namebundleofbricks(bricklargemapletreedata,"_tree_maple");

function nameBundleOfBricks(%dbName,%objname)
{
	%groupCount = MainBrickGroup.getCount();
	
	for(%i=0;%i<%groupCount;%i++)
	{
		%group = MainBrickGroup.getObject(%i);
		%count = %group.getCount();
		
		for(%j=0;%j<%count;%j++)
		{
			%brick = %group.getObject(%j);
			
			if(%brick.getDataBlock().getName() $= %dbName)
				%brick.setName(%objName);
				
		}
	}
}

function pullIntFromString(%this)
{
	%len = strLen(%this);
	%build = "NULL";
	
	for(%i=0;%i<%len;%i++)
	{
		%letter = getSubStr(%this,%i,1);
		
		if(%letter !$= "-" && (%letter * 1 == 0 && %letter !$= "0"))
			continue;
			
		if(%build !$= "-" && %build !$= "" && (%letter * 1 == 0 && %letter !$= "0"))
			break;
		
		if(%build $= "NULL")
			%build = "";
			
		%build = %build @ %letter;
	}
	
	return %build;
}

function capFirstLetter(%string)
{
	if(%string !$= "")
		return strUpr(getSubStr(%string,0,1)) @ getSubStr(%string,1,strLen(%string));
}	

function echoChildClassData(%object)
{
	if(isObject(%object))
	{
		%count = %object.getCount();
		
		if(%count > 0)
			for(%i=0;%i<%count;%i++)
			{
				%child = %object.getObject(%i);
				
				if(isObject(%child))
					if(%child.getClassName() !$= "")
						echo("ID: " @ %child @ " Name: " @ strReplace(%child.getName(),"",0) @ " CN: " @ %child.getClassName());
				
			}
	}
}

function lob_msgAdmins(%string)
{
	for(%i=0;%i<clientgroup.getcount();%i++)
	{
		%c = clientgroup.getobject(%i);
		
		if(%c.isAdmin)
			messageClient(%c,'',"\c2ADMIN:\c6 " @ %string);
	}
}

function lob_deleteProfileValue(%val)
{
	%count = getFileCount("base/lob/saves/*.txt");
	for(%i=0;%i<%count;%i++)
	{
		%file = findNextFile("base/lob/saves/*.txt");
		
		%f = new fileObject();
		%f.openForRead(%file);
		%lc=-1;
		while(!%f.isEof())
		{
			%line[%lc++] = %f.readline();
			%line = %line[%lc];
			
			if(strStr(%line," = ") >= 0)
			{
				%valDupe = getSubStr(%line,0,strStr(%line," ="));
				%valDupe = trim(%valDupe);
				
				if(%val $= %valDupe)
				{
					%line[%lc] = "";
					%found = true;
				}
				//echo("val dupe = " @ %valdupe);
			}
			else
				continue;
		}
		
		%f.close();
		%f.delete();
		
		if(%found)
		{
			%f = new fileObject();
			%f.openForWrite(%file);
			
			for(%j=0;%j<%lc;%j++)
			{
				if(%line[%j] !$= "")
					%f.writeLine(%line[%j]);
				%line[%j] = "";
			}
			
			%f.close();
			%f.delete();
			
			%found = false;
		}
	}
}

function serverCmdSayColor(%client)
{
	talk(getColorIdTable(%client.currentColor));
}

$lob::wantedLevel = 3;

function lob_getWanted()
{
	%count = clientGroup.getCount();
	for(%i=0;%i<%count;%i++)
	{
		%client = clientGroup.getObject(%i);
		
		if(%client.slo.pkPoints !$= "")
			if(%client.slo.pkPoints >= $lob::wantedLevel)
				%list = %list SPC %client;
	}
	
	%list = trim(%list);
	
	if(%list $= "")
		return false;
	
	return %list;
}

function isFirstLetterAlpha(%string)
{
	%alpha = "aeiou";
	if(%string $= "")
		return true;
		
	%isAlph = strStr(%alpha,getSubStr(%string,0,1));
	
	if(%isAlph >= 0)
		return true;
	else
		return false;
}

function fixModularterrainPrints()
{
	%groupCount = MainBrickGroup.getCount();
	for(%i = 0; %i < %groupCount; %i++)
	{
		%group = MainBrickGroup.getObject(%i);
		%count = %group.getCount();
		for(%j = 0; %j < %count; %j++)
		{
			%brick = %group.getObject(%j);
			
			if(isObject(%brick))
				if(%brick.getPrintId() $= "0")
					%brick.setPrint(35);
		}
	}
}