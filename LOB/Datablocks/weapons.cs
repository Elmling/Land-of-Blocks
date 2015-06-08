$LOB::WeaponDamage["Bronze"] = 2;
$LOB::WeaponDamage["Iron"] = 4;
$LOB::WeaponDamage["Steel"] = 6;
$LOB::weaponDamage["Mithril"] = 8;
$LOB::weaponDamage["Adamantite"] = 10;

$lob::weaponcolor["bronze"] = "0.803922 0.521569 0.247059 1";
$lob::weaponcolor["iron"] = "0.654902 0.639216 0.627451 1.000000";
$lob::weaponcolor["steel"] = "0.976471 0.976471 0.976471 1.000000";
$lob::weaponcolor["mithril"] = "0.2 0.3 0.6 1.000000";
$lob::weaponcolor["Adamantite"] = "0.1 0.2 0.2 1.000000";

datablock ParticleData(swordSpecialParticle)
{
   dragCoefficient      = 5.0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 1.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 500;
   useInvAlpha          = false;
   textureName          = "./swordspecial";
   colors[0]     = "1.0 1.0 1.0 1";
   colors[1]     = "1.0 1.0 1.0 1";
   colors[2]     = "0.0 0.0 0.0 0";
   sizes[0]      = 0.4;
   sizes[1]      = 1.5;
   sizes[2]      = 2.5;
   times[0]      = 0.1;
   times[1]      = 0.2;
   times[2]      = 0.3;
};

datablock ParticleEmitterData(swordSpecialEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 0;
   ejectionVelocity = 0.5;
   ejectionOffset   = 1.0;
   velocityVariance = 0.49;
   thetaMin         = 0;
   thetaMax         = 120;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "swordSpecialParticle";

   uiName = "Emote - swordSpecial";
};

datablock ShapeBaseImageData(swordspecialImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.01;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.350;
	stateEmitter[1]					= swordspecialEmitter;
	stateEmitterTime[1]				= 0.350;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};

//--

datablock ParticleData(bowSpecialParticle)
{
   dragCoefficient      = 5.0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 1.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 500;
   useInvAlpha          = false;
   textureName          = "./bowspecial";
   colors[0]     = "1.0 1.0 1.0 1";
   colors[1]     = "1.0 1.0 1.0 1";
   colors[2]     = "0.0 0.0 0.0 0";
   sizes[0]      = 0.4;
   sizes[1]      = 1.5;
   sizes[2]      = 2.5;
   times[0]      = 0.1;
   times[1]      = 0.2;
   times[2]      = 0.3;
};

datablock ParticleEmitterData(bowSpecialEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 0;
   ejectionVelocity = 0.5;
   ejectionOffset   = 1.0;
   velocityVariance = 0.49;
   thetaMin         = 0;
   thetaMax         = 120;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "bowSpecialParticle";

   uiName = "Emote - bowSpecial";
};

datablock ShapeBaseImageData(bowspecialImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.01;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.350;
	stateEmitter[1]					= bowspecialEmitter;
	stateEmitterTime[1]				= 0.350;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};

//Short Sword Begin

//bronze

datablock projectileData(bronzeShortSwordProjectile : swordProjectile)
{
    directDamage = 5;
    lifeTime = 100;
    explodeOnDeath = false;
};

datablock itemData(bronzeShortSwordItem : swordItem)
{
    uiName = "Bronze Short Sword";
    image = bronzeShortSwordImage;
    colorShiftColor = "0.39 0.19 0 1";
	rpgType = "bronze";
};

datablock shapeBaseImageData(bronzeShortSwordImage : swordImage)
{
    item = bronzeShortSwordItem;
    projectile = bronzeShortSwordProjectile;
    colorShiftColor = "0.39 0.19 0 1";
	rpgType = "Bronze";
};

function bronzeShortSwordImage::onFire(%this, %obj, %slot)
{
    parent::onFire(%this, %obj, %slot);
    %obj.playThread(2, "armAttack");
}

function bronzeShortSwordimage::onStopFire(%this, %obj, %slot)
{
    %obj.playThread(2, "root");
}

function bronzeShortSwordProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		shortSwordOnHit(%obj.sourceObject.client,"Bronze",%col);
	else
		shortSwordOnHit(%obj.sourceobject,"Bronze",%col);
}

//iron

datablock projectileData(ironShortSwordProjectile : swordProjectile)
{
    directDamage = 5;
    lifeTime = 100;
    explodeOnDeath = false;
};

datablock itemData(ironShortSwordItem : swordItem)
{
    uiName = "iron Short Sword";
    image = ironShortSwordImage;
    colorShiftColor = "0.529412 0.513726 0.494118 1.000000";
	rpgType = "iron";
};

datablock shapeBaseImageData(ironShortSwordImage : swordImage)
{
    item = ironShortSwordItem;
    projectile = ironShortSwordProjectile;
    colorShiftColor = "0.529412 0.513726 0.494118 1.000000";
	rpgType = "iron";
};

function ironShortSwordImage::onFire(%this, %obj, %slot)
{
    parent::onFire(%this, %obj, %slot);
    %obj.playThread(2, "armAttack");
}

function ironShortSwordimage::onStopFire(%this, %obj, %slot)
{
    %obj.playThread(2, "root");
}

function ironShortSwordProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		shortSwordOnHit(%obj.sourceObject.client,"iron",%col);
	else
		shortSwordOnHit(%obj.sourceobject,"iron",%col);
}

//steel

datablock projectileData(steelShortSwordProjectile : swordProjectile)
{
    directDamage = 5;
    lifeTime = 100;
    explodeOnDeath = false;
};

datablock itemData(steelShortSwordItem : swordItem)
{
    uiName = "Steel Short Sword";
    image = steelShortSwordImage;
    colorShiftColor = "0.729412 0.721569 0.701961 1.000000";
	rpgType = "steel";
};

datablock shapeBaseImageData(steelShortSwordImage : swordImage)
{
    item = steelShortSwordItem;
    projectile = steelShortSwordProjectile;
    colorShiftColor = "0.729412 0.721569 0.701961 1.000000";
	rpgType = "steel";
};

function steelShortSwordImage::onFire(%this, %obj, %slot)
{
    parent::onFire(%this, %obj, %slot);
    %obj.playThread(2, "armAttack");
}

function steelShortSwordimage::onStopFire(%this, %obj, %slot)
{
    %obj.playThread(2, "root");
}

function steelShortSwordProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		shortSwordOnHit(%obj.sourceObject.client,"steel",%col);
	else
		shortSwordOnHit(%obj.sourceobject,"steel",%col);
}

//mithril

datablock projectileData(mithrilShortSwordProjectile : swordProjectile)
{
    directDamage = 5;
    lifeTime = 100;
    explodeOnDeath = false;
	rpgType = "mithril";
};

datablock itemData(mithrilShortSwordItem : swordItem)
{
    uiName = "mithril Short Sword";
    image = mithrilShortSwordImage;
    colorShiftColor = $lob::weaponcolor["Mithril"];
	rpgType = "mithril";
};

datablock shapeBaseImageData(mithrilShortSwordImage : swordImage)
{
    item = mithrilShortSwordItem;
    projectile = mithrilShortSwordProjectile;
    colorShiftColor = $lob::weaponcolor["Mithril"];
};

function mithrilShortSwordImage::onFire(%this, %obj, %slot)
{
    parent::onFire(%this, %obj, %slot);
    %obj.playThread(2, "armAttack");
}

function mithrilShortSwordimage::onStopFire(%this, %obj, %slot)
{
    %obj.playThread(2, "root");
}

function mithrilShortSwordProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		shortSwordOnHit(%obj.sourceObject.client,"mithril",%col);
	else
		shortSwordOnHit(%obj.sourceobject,"mithril",%col);
}

//Adamantite

datablock projectileData(adamantiteShortSwordProjectile : swordProjectile)
{
    directDamage = 5;
    lifeTime = 100;
    explodeOnDeath = false;
};

datablock itemData(adamantiteShortSwordItem : swordItem)
{
    uiName = "adamantite Short Sword";
    image = adamantiteShortSwordImage;
    colorShiftColor = $lob::weaponcolor["adamantite"];
	rpgType = "adamantite";
};

datablock shapeBaseImageData(adamantiteShortSwordImage : swordImage)
{
    item = adamantiteShortSwordItem;
    projectile = adamantiteShortSwordProjectile;
    colorShiftColor = $lob::weaponcolor["adamantite"];
};

function adamantiteShortSwordImage::onFire(%this, %obj, %slot)
{
    parent::onFire(%this, %obj, %slot);
    %obj.playThread(2, "armAttack");
}

function adamantiteShortSwordimage::onStopFire(%this, %obj, %slot)
{
    %obj.playThread(2, "root");
}

function adamantiteShortSwordProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		shortSwordOnHit(%obj.sourceObject.client,"adamantite",%col);
	else
		shortSwordOnHit(%obj.sourceobject,"adamantite",%col);
}

function lob_sword_doSpecial(%imageData,%obj)
{
	if(!isObject(%obj.client))
		return 0;
		
	%obj.client.swordSpecial++;
	%pos = vectorAdd(%obj.getEyePoint(),vectorScale(%obj.getEyeVector(),%obj.client.swordspecial * 2));
	
	if(%obj.client.swordSpecial <= 4)
	{
		%item = new item()
		{
			datablock = %imageData.item;
			position = %pos;
			scale = "1 1 1";
			client = %obj.client;
			isSpecialAttack = true;
			canPickup = true;
		};
		
		%item.canpickup = true;
		%item.setVelocity("0 0 -10");
		missionCleanup.add(%item);
		%item.schedule(500,delete);
		schedule(50,0,lob_sword_doSpecial,%imageData,%obj);
	}
	else
		%obj.client.swordSpecial = 0;
}

//Short Sword End

//bow begin

//bronze

datablock projectileData(bronzeBowProjectile : recurveBowProjectile)
{
    directDamage = 5;
    lifeTime = 2000;
    explodeOnDeath = false;

	isBallistic         = true;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 10;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01; 
};

datablock ItemData(bronzeBowItem : RecurveBowItem)
{
    uiName = "Bronze Bow";
    image = bronzeBowImage;
    colorShiftColor = "0.39 0.19 0 1";
};

datablock shapeBaseImageData(bronzeBowImage : RecurveBowImage)
{
    item = bronzeBowItem;
    projectile = bronzeBowProjectile;
    colorShiftColor = "0.39 0.19 0 1";
};

function bronzeBowImage::onMount(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}

function bronzeBowImage::onUnMount(%this, %obj, %slot)
{
	  	%obj.unhideNode(lhand);
		%obj.unhideNode(rhand);
}

function bronzeBowImage::onReady(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}


function bronzeBowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		arrowOnHit(%obj.sourceObject.client,"bronze",%col);
	else
		arrowOnHit(%obj.sourceobject,"bronze",%col);
}

//iron

datablock projectileData(ironBowProjectile : recurveBowProjectile)
{
    directDamage = 5;
    lifeTime = 2000;
    explodeOnDeath = false;

	isBallistic         = true;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 10;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01; 
};

datablock ItemData(ironBowItem : RecurveBowItem)
{
    uiName = "iron Bow";
    image = ironBowImage;
    colorShiftColor = "0.529412 0.513726 0.494118 1.000000";
};

datablock shapeBaseImageData(ironBowImage : RecurveBowImage)
{
    item = ironBowItem;
    projectile = ironBowProjectile;
    colorShiftColor = "0.529412 0.513726 0.494118 1.000000";
};

function ironBowImage::onMount(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}

function ironBowImage::onUnMount(%this, %obj, %slot)
{
	  	%obj.unhideNode(lhand);
		%obj.unhideNode(rhand);
}

function ironBowImage::onReady(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}


function ironBowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		arrowOnHit(%obj.sourceObject.client,"iron",%col);
	else
		arrowOnHit(%obj.sourceobject,"iron",%col);
}

//steel

datablock projectileData(steelBowProjectile : recurveBowProjectile)
{
    directDamage = 5;
    lifeTime = 2000;
    explodeOnDeath = false;

	isBallistic         = true;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 10;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01; 
};

datablock ItemData(steelBowItem : RecurveBowItem)
{
    uiName = "Steel Bow";
    image = steelBowImage;
    colorShiftColor = $lob::weaponColor["steel"];
};

datablock shapeBaseImageData(steelBowImage : RecurveBowImage)
{
    item = steelBowItem;
    projectile = steelBowProjectile;
    colorShiftColor = "0.529412 0.513726 0.494118 1.000000";
};

function steelBowImage::onMount(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}

function steelBowImage::onUnMount(%this, %obj, %slot)
{
	  	%obj.unhideNode(lhand);
		%obj.unhideNode(rhand);
}

function steelBowImage::onReady(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}


function steelBowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		arrowOnHit(%obj.sourceObject.client,"steel",%col);
	else
		arrowOnHit(%obj.sourceobject,"steel",%col);
}

//mithril

datablock projectileData(mithrilBowProjectile : recurveBowProjectile)
{
    directDamage = 5;
    lifeTime = 2000;
    explodeOnDeath = false;

	isBallistic         = true;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 10;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01; 
};

datablock ItemData(mithrilBowItem : RecurveBowItem)
{
    uiName = "mithril Bow";
    image = mithrilBowImage;
    colorShiftColor = $lob::weaponColor["mithril"];
};

datablock shapeBaseImageData(mithrilBowImage : RecurveBowImage)
{
    item = mithrilBowItem;
    projectile = mithrilBowProjectile;
    colorShiftColor = $lob::weaponColor["mithril"];
};

function mithrilBowImage::onMount(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}

function mithrilBowImage::onUnMount(%this, %obj, %slot)
{
	  	%obj.unhideNode(lhand);
		%obj.unhideNode(rhand);
}

function mithrilBowImage::onReady(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}


function mithrilBowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		arrowOnHit(%obj.sourceObject.client,"mithril",%col);
	else
		arrowOnHit(%obj.sourceobject,"mithril",%col);
}

//adamantite

datablock projectileData(adamantiteBowProjectile : recurveBowProjectile)
{
    directDamage = 5;
    lifeTime = 2000;
    explodeOnDeath = false;

	isBallistic         = true;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 10;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01; 
};

datablock ItemData(adamantiteBowItem : RecurveBowItem)
{
    uiName = "adamantite Bow";
    image = adamantiteBowImage;
    colorShiftColor = $lob::weaponColor["adamantite"];
};

datablock shapeBaseImageData(adamantiteBowImage : RecurveBowImage)
{
    item = adamantiteBowItem;
    projectile = adamantiteBowProjectile;
    colorShiftColor = $lob::weaponColor["adamantite"];
};

function adamantiteBowImage::onMount(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}

function adamantiteBowImage::onUnMount(%this, %obj, %slot)
{
	  	%obj.unhideNode(lhand);
		%obj.unhideNode(rhand);
}

function adamantiteBowImage::onReady(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}


function adamantiteBowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	if(%obj.sourceObject.getClassName() $= "Player")
		arrowOnHit(%obj.sourceObject.client,"adamantite",%col);
	else
		arrowOnHit(%obj.sourceobject,"adamantite",%col);
}

function lob_bow_doSpecial(%imageData,%obj,%slot)
{
	if(!isObject(%obj.client))
		return 0;
	%obj.client.bowSpecial++;
	%this = %imageData;
	
	%projectile = %imageData.projectile;
	%spread = 0.00001;
	%shellcount = 3;

	%vector = %obj.getMuzzleVector(%slot);
	%objectVelocity = %obj.getVelocity();
	%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
	%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
	%velocity = VectorAdd(%vector1,%vector2);
	%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
	%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
	%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
	%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
	%velocity = MatrixMulVector(%mat, %velocity);

	%p = new (%this.projectileType)()
	{
		dataBlock = %projectile;
		initialVelocity = %velocity;
		initialPosition = %obj.getMuzzlePoint(%slot);
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	MissionCleanup.add(%p);
	
	if(%obj.client.bowSpecial <= 3)
	{
		schedule(200,0,lob_bow_doSpecial,%imageData,%obj,%slot);
	}
	else
		%obj.client.bowSpecial = 0;
		
	return %p;
}

$Lob::specialAttack["bronzeShortSwordimage"] = "Falling Swords";
$Lob::specialAttack["ironshortswordimage"] = "Falling Swords";
$Lob::specialAttack["steelShortSwordimage"] = "Falling Swords";
$Lob::specialAttack["mithrilShortSwordImage"] = "Falling Swords";
$Lob::specialAttack["AdamantiteShortSwordImage"] = "Falling Swords";

$Lob::specialAttack["bronzeBowimage"] = "Aerial Shot";
$Lob::specialAttack["ironBowimage"] = "Aerial Shot";
$Lob::specialAttack["steelBowimage"] = "Aerial Shot";
$Lob::specialAttack["mithrilBowImage"] = "Aerial Shot";
$Lob::specialAttack["AdamantiteBowImage"] = "Aerial Shot";

package bowSpecials
{
	function serverCmdUseTool(%this,%a,%b)
	{
		parent::serverCmdUseTool(%this,%a);
		
		%tool = %this.player.tool[%a];
		%image = %tool.image;
		if($Lob::specialAttack[%image.getname()] !$= "")
			smartMessage(%this,"<just:right><color:000000><font:arial bold italic:35>Special: \n<font:impact:30><color:a3f398>" @ $Lob::specialAttack[%image.getname()],1000,"centerprint");
			
	}
	function steelBowImage::onFire(%this,%obj,%slot)
	{
		parent::onFire(%this,%obj,%slot);
		if(%obj.client.specialReady)
		{
			%obj.client.specialReady = 0;
			lob_bow_doSpecial(%this,%obj,%slot);
			%obj.client.lastSpecialTime = getSimTime();
			%obj.playThread(0,rotccw);
			serverplay3d(bowspecialSound,%obj.position);
			%obj.emote(bowspecialimage,1);
		}
	}
	function ironBowImage::onFire(%this,%obj,%slot)
	{
		parent::onFire(%this,%obj,%slot);
		if(%obj.client.specialReady)
		{
			%obj.client.specialReady = 0;
			lob_bow_doSpecial(%this,%obj,%slot);
			%obj.client.lastSpecialTime = getSimTime();
			%obj.playThread(0,rotccw);
			serverplay3d(bowspecialSound,%obj.position);
			%obj.emote(bowspecialimage,1);
		}
	}
	function bronzeBowImage::onFire(%this,%obj,%slot)
	{
		parent::onFire(%this,%obj,%slot);
		if(%obj.client.specialReady)
		{
			%obj.client.specialReady = 0;
			lob_bow_doSpecial(%this,%obj,%slot);
			%obj.client.lastSpecialTime = getSimTime();
			%obj.playThread(0,rotccw);
			serverplay3d(bowspecialSound,%obj.position);
			%obj.emote(bowspecialimage,1);
		}
	}
	function mithrilBowImage::onFire(%this,%obj,%slot)
	{
		parent::onFire(%this,%obj,%slot);
		if(%obj.client.specialReady)
		{
			%obj.client.specialReady = 0;
			lob_bow_doSpecial(%this,%obj,%slot);
			%obj.client.lastSpecialTime = getSimTime();
			%obj.playThread(0,rotccw);
			serverplay3d(bowspecialSound,%obj.position);
			%obj.emote(bowspecialimage,1);
		}
	}
	function adamantiteBowImage::onFire(%this,%obj,%slot)
	{
		parent::onFire(%this,%obj,%slot);
		if(%obj.client.specialReady)
		{
			%obj.client.specialReady = 0;
			lob_bow_doSpecial(%this,%obj,%slot);
			%obj.client.lastSpecialTime = getSimTime();
			%obj.playThread(0,rotccw);
			serverplay3d(bowspecialSound,%obj.position);
			%obj.emote(bowspecialimage,1);
		}
	}
};
activatePackage(bowSpecials);

//bow end

//javlin begin

//bronze start
datablock ProjectileData(bronzeJavlinProjectile : JavlinProjectile)
{
	isLob = true;
};

datablock ProjectileData(bronzeJavlinStabProjectile : JavlinStabProjectile)
{
	isLob = true;

};

datablock ProjectileData(bronzeJavlinCutProjectile : JavlinCutProjectile)
{
	isLob = true;
};

datablock ItemData(bronzeJavlinItem : JavlinItem)
{
	colorShiftColor = $lob::weaponColor["bronze"];
	image = bronzeJavlinImage;
	uiName = "Bronze Javlin";
};

datablock shapeBaseimageData(bronzeJavlinImage : Javlinimage)
{
	item = BronzeJavlinItem;
	projectile = BronzeJavlinProjectile;
	colorShiftColor = $lob::weaponColor["bronze"];
};	

function BronzeJavlinimage::onMount(%this,%obj,%slot)	
{
	Parent::onMount(%this,%obj,%slot);
	%obj.playThread(0, armreadyboth);
	//%obj.unMountImage(1);
	%obj.hideNode("RHand");
	%obj.hideNode("RHook");
	%obj.hideNode("LHand");
	%obj.hideNode("LHook");
}

function BronzeJavlinimage::onUnMount(%this,%obj,%slot)
{
	parent::onUnMount(%this,%obj,%slot);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

datablock ShapeBaseImageData(BronzeJavlinThrowImage : JavlinThrowImage)
{
   item = BronzeJavlinItem;
   projectile = BronzeJavlinProjectile;
   colorShiftColor = $lob::weaponColor["bronze"];
};

function bronzeJavlinThrowImage::onUnEquip(%this,%obj,%slot)
{
	%obj.mountImage(bronzeJavlinimage, 0, 0, 0);
}

function bronzeJavlinThrowImage::onMount(%this, %obj, %slot)
{		
	Parent::onMount(%this,%obj,%slot);
	//%obj.playThread(1, armreadyboth);
  	if(%obj.getMountedImage(1))
	{
		%obj.unMountImage(1);
		%obj.hideNode("RHand");
		%obj.hideNode("RHook");
		%obj.hideNode("LHand");
		%obj.hideNode("LHook");
	}
}

function bronzeJavlinThrowImage::onUnMount(%this, %obj, %slot)
{	
	Parent::onUnMount(%this,%obj,%slot);
	//%obj.playThread(0, root);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

function bronzeJavlinThrowimage::onFire(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
	%transform = %obj.getTransform();  
	$throwRot = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
	
	if(%obj.isImageMounted(bronzeJavlinThrowImage))
	{
		%projectile = bronzeJavlinProjectile;
		%spread = 0.000;
		%shellcount = 1;
		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
	
			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
		return %p;
	}

}
function bronzeJavlinimage::onFireStab(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
							$TypeMasks::PlayerObjectType |	//AI/Players
							$TypeMasks::StaticObjectType |	//Static Shapes
							$TypeMasks::TerrainObjectType |	//Terrain
							$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = bronzeJavlinStabProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 15;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = bronzeJavlinStabprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}
function bronzeJavlinimage::onFireCut(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   					$TypeMasks::PlayerObjectType |	//AI/Players
   					$TypeMasks::StaticObjectType |	//Static Shapes
   					$TypeMasks::TerrainObjectType |	//Terrain
   					$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = bronzeJavlinCutProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 10;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = bronzeJavlinCutprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function bronzeJavlinCutProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"bronze",%col,"quick");
	else
		javlinOnHit(%obj.sourceobject,"bronze",%col);
}

function bronzeJavlinStabProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"bronze",%col,"charged");
	else
		javlinOnHit(%obj.sourceobject,"bronze",%col);
}

function bronzeJavlinProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"bronze",%col,"throw");
	else
		javlinOnHit(%obj.sourceobject,"bronze",%col);
}

//bronze end

//iron start

datablock ProjectileData(ironJavlinProjectile : JavlinProjectile)
{
	isLob = true;
};

datablock ProjectileData(ironJavlinStabProjectile : JavlinStabProjectile)
{
	isLob = true;

};

datablock ProjectileData(ironJavlinCutProjectile : JavlinCutProjectile)
{
	isLob = true;
};

datablock ItemData(ironJavlinItem : JavlinItem)
{
	colorShiftColor = $lob::weaponColor["iron"];
	image = ironJavlinImage;
	uiName = "iron Javlin";
};

datablock shapeBaseimageData(ironJavlinImage : Javlinimage)
{
	item = ironJavlinItem;
	projectile = ironJavlinProjectile;
	colorShiftColor = $lob::weaponColor["iron"];
};	

function ironJavlinimage::onMount(%this,%obj,%slot)	
{
	Parent::onMount(%this,%obj,%slot);
	%obj.playThread(0, armreadyboth);
	//%obj.unMountImage(1);
	%obj.hideNode("RHand");
	%obj.hideNode("RHook");
	%obj.hideNode("LHand");
	%obj.hideNode("LHook");
}

function ironJavlinimage::onUnMount(%this,%obj,%slot)
{
	parent::onUnMount(%this,%obj,%slot);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

datablock ShapeBaseImageData(ironJavlinThrowImage : JavlinThrowImage)
{
   item = ironJavlinItem;
   projectile = ironJavlinProjectile;
   colorShiftColor = $lob::weaponColor["iron"];
};

function ironJavlinThrowImage::onUnEquip(%this,%obj,%slot)
{
	%obj.mountImage(ironJavlinimage, 0, 0, 0);
}

function ironJavlinThrowImage::onMount(%this, %obj, %slot)
{		
	Parent::onMount(%this,%obj,%slot);
	//%obj.playThread(1, armreadyboth);
  	if(%obj.getMountedImage(1))
	{
		%obj.unMountImage(1);
		%obj.hideNode("RHand");
		%obj.hideNode("RHook");
		%obj.hideNode("LHand");
		%obj.hideNode("LHook");
	}
}

function ironJavlinThrowImage::onUnMount(%this, %obj, %slot)
{	
	Parent::onUnMount(%this,%obj,%slot);
	//%obj.playThread(0, root);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

function ironJavlinThrowimage::onFire(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
	%transform = %obj.getTransform();  
	$throwRot = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
	
	if(%obj.isImageMounted(ironJavlinThrowImage))
	{
		%projectile = ironJavlinProjectile;
		%spread = 0.000;
		%shellcount = 1;
		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
	
			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
		return %p;
	}

}
function ironJavlinimage::onFireStab(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
							$TypeMasks::PlayerObjectType |	//AI/Players
							$TypeMasks::StaticObjectType |	//Static Shapes
							$TypeMasks::TerrainObjectType |	//Terrain
							$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = ironJavlinStabProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 15;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = ironJavlinStabprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}
function ironJavlinimage::onFireCut(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   					$TypeMasks::PlayerObjectType |	//AI/Players
   					$TypeMasks::StaticObjectType |	//Static Shapes
   					$TypeMasks::TerrainObjectType |	//Terrain
   					$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = ironJavlinCutProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 10;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = ironJavlinCutprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function ironJavlinCutProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"iron",%col,"quick");
	else
		javlinOnHit(%obj.sourceobject,"iron",%col);
}

function ironJavlinStabProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"iron",%col,"charged");
	else
		javlinOnHit(%obj.sourceobject,"iron",%col);
}

function ironJavlinProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"iron",%col,"throw");
	else
		javlinOnHit(%obj.sourceobject,"iron",%col);
}

//iron end

//steel start

datablock ProjectileData(steelJavlinProjectile : JavlinProjectile)
{
	isLob = true;
};

datablock ProjectileData(steelJavlinStabProjectile : JavlinStabProjectile)
{
	isLob = true;

};

datablock ProjectileData(steelJavlinCutProjectile : JavlinCutProjectile)
{
	isLob = true;
};

datablock ItemData(steelJavlinItem : JavlinItem)
{
	colorShiftColor = $lob::weaponColor["steel"];
	image = steelJavlinImage;
	uiName = "steel Javlin";
};

datablock shapeBaseimageData(steelJavlinImage : Javlinimage)
{
	item = steelJavlinItem;
	projectile = steelJavlinProjectile;
	colorShiftColor = $lob::weaponColor["steel"];
};	

function steelJavlinimage::onMount(%this,%obj,%slot)	
{
	Parent::onMount(%this,%obj,%slot);
	%obj.playThread(0, armreadyboth);
	//%obj.unMountImage(1);
	%obj.hideNode("RHand");
	%obj.hideNode("RHook");
	%obj.hideNode("LHand");
	%obj.hideNode("LHook");
}

function steelJavlinimage::onUnMount(%this,%obj,%slot)
{
	parent::onUnMount(%this,%obj,%slot);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

datablock ShapeBaseImageData(steelJavlinThrowImage : JavlinThrowImage)
{
   item = steelJavlinItem;
   projectile = steelJavlinProjectile;
   colorShiftColor = $lob::weaponColor["steel"];
};

function steelJavlinThrowImage::onUnEquip(%this,%obj,%slot)
{
	%obj.mountImage(steelJavlinimage, 0, 0, 0);
}

function steelJavlinThrowImage::onMount(%this, %obj, %slot)
{		
	Parent::onMount(%this,%obj,%slot);
	//%obj.playThread(1, armreadyboth);
  	if(%obj.getMountedImage(1))
	{
		%obj.unMountImage(1);
		%obj.hideNode("RHand");
		%obj.hideNode("RHook");
		%obj.hideNode("LHand");
		%obj.hideNode("LHook");
	}
}

function steelJavlinThrowImage::onUnMount(%this, %obj, %slot)
{	
	Parent::onUnMount(%this,%obj,%slot);
	//%obj.playThread(0, root);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

function steelJavlinThrowimage::onFire(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
	%transform = %obj.getTransform();  
	$throwRot = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
	
	if(%obj.isImageMounted(steelJavlinThrowImage))
	{
		%projectile = steelJavlinProjectile;
		%spread = 0.000;
		%shellcount = 1;
		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
	
			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
		return %p;
	}

}
function steelJavlinimage::onFireStab(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
							$TypeMasks::PlayerObjectType |	//AI/Players
							$TypeMasks::StaticObjectType |	//Static Shapes
							$TypeMasks::TerrainObjectType |	//Terrain
							$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = steelJavlinStabProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 15;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = steelJavlinStabprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}
function steelJavlinimage::onFireCut(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   					$TypeMasks::PlayerObjectType |	//AI/Players
   					$TypeMasks::StaticObjectType |	//Static Shapes
   					$TypeMasks::TerrainObjectType |	//Terrain
   					$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = steelJavlinCutProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 10;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = steelJavlinCutprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function steelJavlinCutProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"steel",%col,"quick");
	else
		javlinOnHit(%obj.sourceobject,"steel",%col);
}

function steelJavlinStabProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"steel",%col,"charged");
	else
		javlinOnHit(%obj.sourceobject,"steel",%col);
}

function steelJavlinProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"steel",%col,"throw");
	else
		javlinOnHit(%obj.sourceobject,"steel",%col);
}

//steel end

//mithril start

datablock ProjectileData(mithrilJavlinProjectile : JavlinProjectile)
{
	isLob = true;
};

datablock ProjectileData(mithrilJavlinStabProjectile : JavlinStabProjectile)
{
	isLob = true;

};

datablock ProjectileData(mithrilJavlinCutProjectile : JavlinCutProjectile)
{
	isLob = true;
};

datablock ItemData(mithrilJavlinItem : JavlinItem)
{
	colorShiftColor = $lob::weaponColor["mithril"];
	image = mithrilJavlinImage;
	uiName = "mithril Javlin";
};

datablock shapeBaseimageData(mithrilJavlinImage : Javlinimage)
{
	item = mithrilJavlinItem;
	projectile = mithrilJavlinProjectile;
	colorShiftColor = $lob::weaponColor["mithril"];
};	

function mithrilJavlinimage::onMount(%this,%obj,%slot)	
{
	Parent::onMount(%this,%obj,%slot);
	%obj.playThread(0, armreadyboth);
	//%obj.unMountImage(1);
	%obj.hideNode("RHand");
	%obj.hideNode("RHook");
	%obj.hideNode("LHand");
	%obj.hideNode("LHook");
}

function mithrilJavlinimage::onUnMount(%this,%obj,%slot)
{
	parent::onUnMount(%this,%obj,%slot);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

datablock ShapeBaseImageData(mithrilJavlinThrowImage : JavlinThrowImage)
{
   item = mithrilJavlinItem;
   projectile = mithrilJavlinProjectile;
   colorShiftColor = $lob::weaponColor["mithril"];
};

function mithrilJavlinThrowImage::onUnEquip(%this,%obj,%slot)
{
	%obj.mountImage(mithrilJavlinimage, 0, 0, 0);
}

function mithrilJavlinThrowImage::onMount(%this, %obj, %slot)
{		
	Parent::onMount(%this,%obj,%slot);
	//%obj.playThread(1, armreadyboth);
  	if(%obj.getMountedImage(1))
	{
		%obj.unMountImage(1);
		%obj.hideNode("RHand");
		%obj.hideNode("RHook");
		%obj.hideNode("LHand");
		%obj.hideNode("LHook");
	}
}

function mithrilJavlinThrowImage::onUnMount(%this, %obj, %slot)
{	
	Parent::onUnMount(%this,%obj,%slot);
	//%obj.playThread(0, root);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

function mithrilJavlinThrowimage::onFire(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
	%transform = %obj.getTransform();  
	$throwRot = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
	
	if(%obj.isImageMounted(mithrilJavlinThrowImage))
	{
		%projectile = mithrilJavlinProjectile;
		%spread = 0.000;
		%shellcount = 1;
		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
	
			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
		return %p;
	}

}
function mithrilJavlinimage::onFireStab(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
							$TypeMasks::PlayerObjectType |	//AI/Players
							$TypeMasks::StaticObjectType |	//Static Shapes
							$TypeMasks::TerrainObjectType |	//Terrain
							$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = mithrilJavlinStabProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 15;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = mithrilJavlinStabprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}
function mithrilJavlinimage::onFireCut(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   					$TypeMasks::PlayerObjectType |	//AI/Players
   					$TypeMasks::StaticObjectType |	//Static Shapes
   					$TypeMasks::TerrainObjectType |	//Terrain
   					$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = mithrilJavlinCutProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 10;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = mithrilJavlinCutprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function mithrilJavlinCutProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"mithril",%col,"quick");
	else
		javlinOnHit(%obj.sourceobject,"mithril",%col);
}

function mithrilJavlinStabProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"mithril",%col,"charged");
	else
		javlinOnHit(%obj.sourceobject,"mithril",%col);
}

function mithrilJavlinProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"mithril",%col,"throw");
	else
		javlinOnHit(%obj.sourceobject,"mithril",%col);
}

//mithril end

//adamantite start

datablock ProjectileData(adamantiteJavlinProjectile : JavlinProjectile)
{
	isLob = true;
};

datablock ProjectileData(adamantiteJavlinStabProjectile : JavlinStabProjectile)
{
	isLob = true;

};

datablock ProjectileData(adamantiteJavlinCutProjectile : JavlinCutProjectile)
{
	isLob = true;
};

datablock ItemData(adamantiteJavlinItem : JavlinItem)
{
	colorShiftColor = $lob::weaponColor["adamantite"];
	image = adamantiteJavlinImage;
	uiName = "adamantite Javlin";
};

datablock shapeBaseimageData(adamantiteJavlinImage : Javlinimage)
{
	item = adamantiteJavlinItem;
	projectile = adamantiteJavlinProjectile;
	colorShiftColor = $lob::weaponColor["adamantite"];
};	

function adamantiteJavlinimage::onMount(%this,%obj,%slot)	
{
	Parent::onMount(%this,%obj,%slot);
	%obj.playThread(0, armreadyboth);
	//%obj.unMountImage(1);
	%obj.hideNode("RHand");
	%obj.hideNode("RHook");
	%obj.hideNode("LHand");
	%obj.hideNode("LHook");
}

function adamantiteJavlinimage::onUnMount(%this,%obj,%slot)
{
	parent::onUnMount(%this,%obj,%slot);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

datablock ShapeBaseImageData(adamantiteJavlinThrowImage : JavlinThrowImage)
{
   item = adamantiteJavlinItem;
   projectile = adamantiteJavlinProjectile;
   colorShiftColor = $lob::weaponColor["adamantite"];
};

function adamantiteJavlinThrowImage::onUnEquip(%this,%obj,%slot)
{
	%obj.mountImage(adamantiteJavlinimage, 0, 0, 0);
}

function adamantiteJavlinThrowImage::onMount(%this, %obj, %slot)
{		
	Parent::onMount(%this,%obj,%slot);
	//%obj.playThread(1, armreadyboth);
  	if(%obj.getMountedImage(1))
	{
		%obj.unMountImage(1);
		%obj.hideNode("RHand");
		%obj.hideNode("RHook");
		%obj.hideNode("LHand");
		%obj.hideNode("LHook");
	}
}

function adamantiteJavlinThrowImage::onUnMount(%this, %obj, %slot)
{	
	Parent::onUnMount(%this,%obj,%slot);
	//%obj.playThread(0, root);
	%obj.playThread(0, root);
  	%obj.unMountImage(1);
  	//%obj.mountImage(FishingPoleBackImage,1);
	//%obj.unHideNode("ALL");
	%obj.unHideNode("Rhand");
	%obj.unHideNode("LHand");
}

function adamantiteJavlinThrowimage::onFire(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
	%transform = %obj.getTransform();  
	$throwRot = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
	
	if(%obj.isImageMounted(adamantiteJavlinThrowImage))
	{
		%projectile = adamantiteJavlinProjectile;
		%spread = 0.000;
		%shellcount = 1;
		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
	
			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
		return %p;
	}

}
function adamantiteJavlinimage::onFireStab(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
							$TypeMasks::PlayerObjectType |	//AI/Players
							$TypeMasks::StaticObjectType |	//Static Shapes
							$TypeMasks::TerrainObjectType |	//Terrain
							$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = adamantiteJavlinStabProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 15;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = adamantiteJavlinStabprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}
function adamantiteJavlinimage::onFireCut(%this,%obj,%slot,%spread,%shellcount,%recoil)
{
   	%raycastWeaponRange = 15;
   	%raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   					$TypeMasks::PlayerObjectType |	//AI/Players
   					$TypeMasks::StaticObjectType |	//Static Shapes
   					$TypeMasks::TerrainObjectType |	//Terrain
   					$TypeMasks::VehicleObjectType;	//Vehicles
   	%raycastWeaponPierceTargets = "";
   	%raycastExplosionProjectile = adamantiteJavlinCutProjectile;				//Gun cannot pierce
   	%raycastExplosionPlayerSound = PHitSound;
   	%raycastExplosionBrickSound = MetalHit1Sound;
   	%raycastDirectDamage = 10;
   	%raycastDirectDamageType = $DamageType::Javlin;
	%raycastFromMuzzle	= false;

	%projectile = adamantiteJavlinCutprojectile;
	%spread = 0.0000;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function adamantiteJavlinCutProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"adamantite",%col,"quick");
	else
		javlinOnHit(%obj.sourceobject,"adamantite",%col);
}

function adamantiteJavlinStabProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"adamantite",%col,"charged");
	else
		javlinOnHit(%obj.sourceobject,"adamantite",%col);
}

function adamantiteJavlinProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);

	if(%obj.sourceObject.getClassName() $= "Player")
		javlinOnHit(%obj.sourceObject.client,"adamantite",%col,"throw");
	else
		javlinOnHit(%obj.sourceobject,"adamantite",%col);
}

//adamanat end

package lobJavlinThrow
{
	function armor::onTrigger(%this, %obj, %triggerNum, %val)
	{
		%client = %obj.client;
		//talk(%client.name SPC %triggernum);
		if(%triggerNum == 4)
		{
			if(isObject(%obj.getMountedImage(0)) && %val)
			{
				%name = %obj.getMountedImage(0).getName();
				if(strStr(strLwr(%name),"javlin") >= 0)
				{
					%type = getSubStr(%name,0,strStr(strLwr(%name),"javlin"));
					%weap = %type @ "JavlinthrowImage";
					%obj.mountImage(%weap, 0, 0, 0);
				}
			}
		}
		else
		if(%triggerNum == 0)
		{
			if(isObject(%obj.getMountedImage(0)) && %val)
			{
				%name = %obj.getMountedImage(0).getName();
				
				if(strStr(strLwr(%name),"javlin") >= 0)
				{
					//%client.javlinSupport();
				}
			}
			
			if(%val == 0 && %client.javlinSupport)
				cancel(%client.javlinSupportLoop);
		}
		Parent::onTrigger(%this, %obj, %triggerNum, %val);
	}
};
activatePackage(lobJavlinThrow);

//javlin end

//MAGIC
//Firethrow



//Firethrow trail


datablock ParticleData(fireBallAmbient)
{
	dragCoefficient		= 5;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0;
	constantAcceleration	= 10.0;
	lifetimeMS		= 600;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 100.0;
	spinRandomMin		= 50.0;
	spinRandomMax		= 150.0;
	useInvAlpha		= false;
	animateTexture		= false;

	textureName		= "base/data/particles/cloud";

	//Interpolation variables
	colors[0]	= "0.9 0.3 0 1";
	colors[1]	= "1 0.5 0 1";
	colors[2]	= "0.8 0.3 0 1";

	sizes[0]	= 0.2;
	sizes[1]	= 0.3;
	sizes[2]	= 0.00;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(fireBallAmbientEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 1;
   ejectionVelocity = 0.4;
   ejectionOffset   = 0;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 360;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = fireBallAmbient;
   useEmitterColors = false;
   uiName = "FireBall Normal";
};


//effects
datablock ParticleData(FireBallTrailParticle)
{
	 gravityCoefficient   = -0.2;
	dragCoefficient		= 5;
	windCoefficient		= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1000;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 0.0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 0.0;
	useInvAlpha		= false;
	animateTexture		= false;
	textureName		= "base/data/particles/cloud";

	//Interpolation variables
	colors[0]	= "0.9 0.3 0 1";
	colors[1]	= "1 0.5 0 1";
	colors[2]	= "0.8 0.3 0 1";

	sizes[0]	= 0.5;
	sizes[1]	= 0.7;
	sizes[2]	= 0.9;
	times[0]	= 0.4;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(FireBallTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 1;
   ejectionVelocity = 0;
   ejectionOffset   = 1;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 20;
   phiReferenceVel  = 360;
   phiVariance      = 0;
   overrideAdvance = false;
   particles = "fireBallTrailParticle";
   uiName = "fireBallTrail";

};

datablock ParticleData(fireBallExplosionParticle)
{
	dragCoefficient		= 5;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0;
	constantAcceleration	= 10.0;
	lifetimeMS		= 200;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 1.0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 0.0;
	useInvAlpha		= false;
	animateTexture		= false;

	textureName		= "base/data/particles/ring";

	//Interpolation variables
	colors[0]	= "0.9 0.3 0 1";
	colors[1]	= "1 0.5 0 1";
	colors[2]	= "0.8 0.3 0 1";

	sizes[0]	= 0.2;
	sizes[1]	= 0.3;
	sizes[2]	= 0.00;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(fireBallExplosionEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 0.0;
   ejectionOffset   = 2.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "fireBallExplosionParticle";

   useEmitterColors = true;
   uiName = "fireBallExplode";
};

datablock ExplosionData(fireBallExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 650;


   emitter[0] = fireBallExplosionEmitter;
   //particleDensity = 1;
   //particleRadius = 1.0;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "7.0 8.0 7.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.3;
   camShakeRadius = 1.0;

   // Dynamic light
   lightStartRadius = 10;
   lightEndRadius = 0;
   lightStartColor = "1 1 1";
   lightEndColor = "0.9 0.3 0";

   //impulse
   impulseRadius = 1;
   impulseForce = -1000;

   //radius damage
   radiusDamage        = 1;
   damageRadius        = 1;
};


datablock ProjectileData(fireBallProjectile)
{
   projectileShapeName ="base/data/shapes/empty.dts";
   directDamage        = 15;
 //  directDamageType  = $DamageType::/FirethrowDirect;
   impactImpulse	   = 10;
   verticalImpulse	   = 10;
   explosion           = fireBallExplosion;
   particleEmitter     = fireBallTrailEmitter;

   brickExplosionRadius = 0;
   brickExplosionImpact = false; //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;
   brickExplosionMaxVolumeFloating = 0;

   muzzleVelocity      = 100;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 20000;
   fadeDelay           = 19500;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 1;

   hasLight    = false;
   lightRadius = 5.0;
   lightColor  = "0.5 0 0.5";

   uiName = "FireBall Projectile";
};


//////////
// item //
//////////
datablock ItemData(fireBallItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile ="add-ons/item_rusty/scroll.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Fire Ball";
	
	doColorShift = true;
	colorShiftColor = "0.400 0.196 0 1.000";

	 // Dynamic properties defined by the scripts
	image = rightFireBallImage;
	canDrop = true;
};



////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(rightFireballImage)
{
   // Basic Item properties
 shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = fireBallItem;
   ammo = " ";
   projectile = fireBallProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.400 0.196 0 1.000";

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.5;
	stateTransitionOnTimeout[0]     = "Ready";

	stateAllowImageChange[0]		= true;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;
	stateEmitter[1]                = fireBallAmbientEmitter;
	stateEmitterTime[1]            = 300;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Pause1";
	stateTimeoutValue[2]            = 0.05;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = true;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;


	stateName[8]					= "Pause1";
	stateTransitionOnTimeout[8]     = "Fire2";
	stateTimeoutValue[8]            = 0.25;
	stateWaitForTimeout[8]			= true;
	stateSequence[8]                = "Reload";
	stateTransitionOnTriggerUp[8]	= "Reload";
	stateAllowImageChange[8]         = true;
	
	stateName[3]                    = "Fire2";
	stateTransitionOnTimeout[3]     = "Pause2";
	stateTimeoutValue[3]            = 0.05;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = true;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "FireBlast";
	stateWaitForTimeout[3]			= true;


	stateName[9]					= "Pause2";
	stateTransitionOnTimeout[9]     = "Fire3";
	stateTimeoutValue[9]            = 0.15;
	stateWaitForTimeout[9]			= true;
	stateSequence[9]                = "Reload";
	stateTransitionOnTriggerUp[9]	= "Reload";
	stateAllowImageChange[9]         = true;

	stateName[4]                    = "Fire3";
	stateTransitionOnTimeout[4]     = "Reload";
	stateTimeoutValue[4]            = 0.01;
	stateFire[4]                    = true;
	stateAllowImageChange[4]        = true;
	stateSequence[4]                = "Fire";
	stateScript[4]                  = "BlastBow";
	stateWaitForTimeout[4]			= true;


	stateName[5]					= "Reload";
	stateSequence[5]                = "Reload";
	stateAllowImageChange[5]        = true;
	stateTimeoutValue[5]            = 0.25;
	stateWaitForTimeout[5]			= true;
	stateTransitionOnTimeout[5]     = "Check";

	stateName[6]					= "Check";
	stateTransitionOnTriggerUp[6]	= "StopFire";
	stateTransitionOnTriggerDown[6]	= "Fire";
	stateAllowImageChange[6]         = true;
	stateScript[6]					= "FireBlast";

	stateName[7]                    = "StopFire";
	stateTransitionOnTimeout[7]     = "Ready";
	stateTimeoutValue[7]            = 0.25;
	stateAllowImageChange[7]        = true;
	stateWaitForTimeout[7]			= true;
	//stateSequence[7]                = "Reload";
	stateScript[7]                  = "onStopFire";



};

datablock ShapeBaseImageData(LeftFireballImage)
{
   // Basic Item properties
 shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 1;
   offset = "0 0 0";
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = fireBallItem;
   ammo = " ";
   projectile = fireBallProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.5 0.096 0.5 1.000";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.
   // Initial start up state
		stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.5;
	stateTransitionOnTimeout[0]     = "Ready";
	stateAllowImageChange[0]		= true;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;
	stateEmitter[1]                = fireBallAmbientEmitter;
	stateEmitterTime[1]            = 300;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Pause1";
	stateTimeoutValue[2]            = 0.05;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = true;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;


	stateName[8]					= "Pause1";
	stateTransitionOnTimeout[8]     = "Fire2";
	stateTimeoutValue[8]            = 0.25;
	stateWaitForTimeout[8]			= true;
	stateSequence[8]                = "Reload";
	stateTransitionOnTriggerUp[8]	= "Reload";
	stateAllowImageChange[8]         = true;
	
	stateName[3]                    = "Fire2";
	stateTransitionOnTimeout[3]     = "Pause2";
	stateTimeoutValue[3]            = 0.05;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = true;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]			= true;


	stateName[9]					= "Pause2";
	stateTransitionOnTimeout[9]     = "Fire3";
	stateTimeoutValue[9]            = 0.15;
	stateWaitForTimeout[9]			= true;
	stateSequence[9]                = "Reload";
	stateTransitionOnTriggerUp[9]	= "Reload";
	stateAllowImageChange[9]         = true;

	stateName[4]                    = "Fire3";
	stateTransitionOnTimeout[4]     = "Reload";
	stateTimeoutValue[4]            = 0.01;
	stateFire[4]                    = true;
	stateAllowImageChange[4]        = true;
	stateSequence[4]                = "Fire";
	stateScript[4]                  = "onFire";
	stateWaitForTimeout[4]			= true;


	stateName[5]					= "Reload";
	stateSequence[5]                = "Reload";
	stateAllowImageChange[5]        = true;
	stateTimeoutValue[5]            = 0.25;
	stateWaitForTimeout[5]			= true;
	stateTransitionOnTimeout[5]     = "Check";

	stateName[6]					= "Check";
	stateTransitionOnTriggerUp[6]	= "StopFire";
	stateTransitionOnTriggerDown[6]	= "Fire";
	stateAllowImageChange[6]         = true;
	stateScript[6]					= "onCheck";

	stateName[7]                    = "StopFire";
	stateTransitionOnTimeout[7]     = "Ready";
	stateTimeoutValue[7]            = 0.25;
	stateAllowImageChange[7]        = true;
	stateWaitForTimeout[7]			= true;
	//stateSequence[7]                = "Reload";
	stateScript[7]                  = "onStopFire";


};

function fireBallImage::onMount(%this, %obj, %slot)
{
  
   %obj.mountImage(leftFireballImage, 1);
	%obj.playThread(0, ArmreadyBoth, 1);
}
function fireBallImage::onUnMount(%this, %obj, %slot)
{
   %obj.unMountImage(1);
	%obj.playThread(0, ArmreadyBoth, 0);
  %obj.playThread(2, root);
}

function RightFireBall::FireBlast(%this,%obj,%slot)
{
   %obj.setImageTrigger(1,1);
   //ServerPlay3D(cpexpexplodesound,%obj.position);
}

function leftFireBall::FireBlast(%this,%obj,%slot)
{
   %obj.setImageTrigger(1,1);
   //ServerPlay3D(cpexpexplodesound,%obj.position);
}
  

//fire end

//WIND WALL
datablock AudioProfile(WindWallExplosionSound : MagicWandExplosionSound)
{
	WindWall = true;
	
};

datablock AudioProfile(WindWallFireSound : MagicWandFireSound)
{
	WindWall = true;
};

datablock ParticleData(WindWallTrailParticle : MagicWandTrailParticle)
{
	windWall = true;
};

datablock ParticleEmitterData(WindWallTrailEmitter : MagicWandTrailEmitter)
{
	windWall = true;
	particles = WindWallTrailParticle;
};

datablock ItemData(WindWallItem : MagicWandItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "add-ons/weapon_MedPack3/magicwand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Wind Wall";
	iconName = "add-ons/weapon_MedPack3/icon_magicwand";
	doColorShift = true;
   	colorShiftColor = "0.471 0.471 0.471 1.000";

	 // Dynamic properties defined by the scripts
	image = WindWallImage;
	canDrop = true;
};

datablock ShapeBaseImageData(WindWallImage : MagicWandImage)
{
   // Basic Item properties
   shapeFile = "add-ons/weapon_MedPack3/magicwand.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   //eyeOffset = "0 0 0";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = WindWallItem;
   ammo = " ";
   projectile = swordProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.471 0.471 0.471 1.000";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.5;
	stateTransitionOnTimeout[0]    = "Ready";
	stateSound[0]                  = weaponSwitchSound;

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateEmitter[1]                = "";//WindWallTrailEmitter;
	stateEmitterTime[1]            = 300;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateSequence[2]               = "Fire";
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;
	stateSound[2]                  = "";//WindWallFireSound;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function WindWallImage::onFire(%this, %obj, %slot)
{
	if(%obj.client.slo.inventory.itemCountMagicMaterial < $lob::magicMaterialRequired["Wind Wall"])
	{
		messageClient(%obj.client,'',"\c6I need more Magic Material.");
		return false;
	}
	
	%time = getSimTime();
	if(%time - %obj.client.lastWindWallTime >= 7000)
	{
		%obj.client.lastWindWallTime = %time;
		%obj.client.windWall();
		%obj.playthread(2, ShiftUp);
	}
	else
	{
		%time = (7000 - (%time - %obj.client.lastWindWallTime));
		echo(%time);
		%tip = "<color:ffppp3>Wind wall is on cooldown for " @ getSubStr(%time,0,1) @ " more seconds!";
		commandToClient(%obj.client,'showTip',%tip,%time);
	}
	Parent::onFire(%this, %obj, %slot);
}
//WIND WALL END

//MAGIC END