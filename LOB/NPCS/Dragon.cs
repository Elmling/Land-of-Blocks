$nodeColor["Dragon","headskin"] = "";
$nodeColor["Dragon","chest"] = "";
$nodeColor["Dragon","pants"] = "";
$nodeColor["Dragon","larm"] = "";
$nodeColor["Dragon","rarm"] = "";
$nodeColor["Dragon","lhand"] = "";
$nodeColor["Dragon","rhand"] = "";
$nodeColor["Dragon","lshoe"] = "";
$nodeColor["Dragon","rshoe"] = $nodeColor["Dragon","lshoe"];
//$nodeColor["Dragon","pack"] = $nodeColor["Dragon","pants"];
//$pack["Dragon"] = "pack";
//$smiley["Dragon"] = "smiley-evil2";
//$decal["Dragon"] = "archer";
$LOB::Enemy["Dragon","Health"] = 1000;
$LOB::Enemy["Dragon","Level"] = "80 90";
$LOB::Enemy["Dragon","Datablock"] = landDragonArmor;
$LOB::Enemy["Dragon","Aggressive"] = 1;
$LOB::Enemy["Dragon","RespawnTime"] = 90000;
$roam["Dragon"] = 20;
$task["Dragon"] = "Combat";
$equip["Dragon"] = "";
$drop["Dragon","gold"] = "300 500";
$drop["Dragon","item"] = "Wind Wall";
$drop["Dragon","material"] = "Fire Ball";
$itemDropChance["Dragon"] = 30;
$materialDropChance["Dragon"] = 25;
$drop["Dragon","food"] = "cooked lobster";
$foodDropChance["Dragon"] = 2;
$view["Dragon"] = 70;
$equip["Dragon"] = "dragonbreathImage";


if(isObject(Dragon))Dragon.delete();

new scriptObject(Dragon);

function dragon::onObjectSpawned(%this,%npc)
{
	%npc.setmovespeed(1.3);
	%npc.setScale("1.5 1.5 1.5");
	%npc.jumpForce = 15;
}



package lob_dragon
{

	function dragonBreathProjectile::onCollision(%a,%b,%c,%d,%e,%f)
	{
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f);
		
		if(isObject(%c))
		{
			if(%c.dataBlock.getName() $= "advancedHorseArmor")
			{
					%c.lastDragonBreathTime = %time;
					%dragon = %b.sourceObject;
					%dmg = getDamage(%dragon,%c);
					
					if(%dmg >= 1)
					{
						%c.playPain(1);
						%c.setDamageFlash(%dmg);
					}
					
					%c.Health -= %dmg;
					
					if(%c.Health <= 0)
					{
						%c.owner.killHorse();
					}		
			}
			else
			if(%c.getclassname() $= "Player")
			{	
				%time = getSimTime();
				if(%time - %c.lastDragonBreathTime > 500)
				{
					%c.lastDragonBreathTime = %time;
					%dragon = %b.sourceObject;
					%dmg = getDamage(%dragon,%c);
					
					if(%dmg >= 1)
					{
						%c.playPain(1);
						%c.setDamageFlash(%dmg);
					}
					
					%slo = %c.client.slo;
					%slo.tempHealth -= %dmg;
					
					if(%slo.tempHealth <= 0)
					{
						%slo.tempHealth = %slo.health;
						%c.tempHealth = %c.health;
						%c.client.lobKill(%dragon);
					}
				}
			}
		}
		
		return %p;
	}

	function landDragonArmor::onCollision(%a,%b,%c,%d,%e,%f)
	{
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f);
		
		if(%b.getState() $= "dead")
			return %p;
		
		//if(%c.client.name $= "elm")
		//	messageclient(findclientbyname("elm"),'',"ye");
			
		%cz = getWord(%c.position,2);
		%bz = getWord(%b.position,2);
		
		if(%cz > %bz)
			%c.setVelocity(getRandom(-5,5) SPC getRandom(-5,5) SPC 8);
		else
		{
			%b.playthread(0,activate);
			%amt = vectorAdd(%d,%c.getvelocity());
			%amt = vectorScale(vectorNormalize(%amt),30);
			%c.setVelocity(getWords(%amt,0,1) SPC 1);
		}
		return %p;
	}

};
activatePackage(lob_dragon);

function dragon_Aim(%this)
{
	cancel(%this.dragonaim);
	
	%player = %this.findclosestplayer();
	
	if(isObject(%player))
	{
	
	}
	
	%this.dragonAim = %this.schedule(getRandom(500,1000),dragon_aim,%this);
}

