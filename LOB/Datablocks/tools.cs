$toolDamage["bronze"] = "2";
$toolDamage["iron"] = "4";
$toolDamage["steel"] = "6";
$toolDamage["mithril"] = "8";
$toolDamage["Adamantite"] = "10";

$lob::toolcolor["bronze"] = "0.803922 0.521569 0.247059 1";
$lob::toolcolor["iron"] = "0.654902 0.639216 0.627451 1.000000";
$lob::toolcolor["steel"] = "0.976471 0.976471 0.976471 1.000000";
$lob::toolcolor["mithril"] = "0.2 0.3 0.6 1.000000";
$lob::toolcolor["Adamantite"] = "0.1 0.2 0.2 1.000000";


//axes Start

//bronze

datablock AudioProfile(pickAxeHitSound : swordHitSound)
{
   filename    = "base/lob/datablocks/sounds/pickaxehitsound.wav";
   description = AudioClosest3d;
   preload = true;
};

datablock ExplosionData(bronzeAxeExplosion : rpgAxeExplosion)
{
   lifeTimeMS = 300;

   soundProfile = swordHitSound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 8;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "12.0 14.0 12.0";
   camShakeAmp = "0.7 0.7 0.7";
   camShakeDuration = 0.35;
   camShakeRadius = 7.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
   
   image = bronzeAxeImage;
};

datablock ItemData(bronzeAxeItem : rpgAxeItem)
{
	uiName = "Bronze Axe";
	doColorShift = true;
	colorShiftColor = "0.803922 0.521569 0.247059 1";
	image = BronzeAxeImage;
	canDrop = true;
};

AddDamageType("rpgAxe",   '<bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',    '%2 <bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',0.75,1);

datablock ProjectileData(bronzeAxeProjectile : rpgAxeProjectile)
{
   directDamage      = 20;
   directDamageType  = $DamageType::rpgAxe;
   radiusDamageType  = $DamageType::rpgAxe;
   explosion         = bronzeAxeExplosion;

   muzzleVelocity      = 65;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Bronze Axe Hit";
};

datablock ShapeBaseImageData(bronzeAxeImage : rpgAxeImage)
{
   //shapeFile = "./Lil_Axe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = bronzeAxeItem;
   ammo = " ";
   projectile = bronzeAxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
   colorShiftColor = "0.803922 0.521569 0.247059 1";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function bronzeAxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function bronzeAxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function bronzeAxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	axeOnHit(%obj.client,bronzeAxeImage,%col);
}

//iron

datablock ExplosionData(ironAxeExplosion : rpgAxeExplosion)
{
   lifeTimeMS = 300;

   soundProfile = swordHitSound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 8;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "12.0 14.0 12.0";
   camShakeAmp = "0.7 0.7 0.7";
   camShakeDuration = 0.35;
   camShakeRadius = 7.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
   
   image = ironAxeImage;
};

datablock ItemData(ironAxeItem : rpgAxeItem)
{
	uiName = "Iron Axe";
	doColorShift = true;
	colorShiftColor = "0.654902 0.639216 0.627451 1.000000";

	image = ironAxeImage;
	canDrop = true;
};

AddDamageType("rpgAxe",   '<bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',    '%2 <bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',0.75,1);

datablock ProjectileData(ironAxeProjectile : rpgAxeProjectile)
{
   directDamage      = 20;
   directDamageType  = $DamageType::rpgAxe;
   radiusDamageType  = $DamageType::rpgAxe;
   explosion         = ironAxeExplosion;

   muzzleVelocity      = 65;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Iron Axe Hit";
};

datablock ShapeBaseImageData(ironAxeImage : rpgAxeImage)
{
   //shapeFile = "./Lil_Axe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = ironAxeItem;
   ammo = " ";
   projectile = ironAxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
   colorShiftColor = "0.654902 0.639216 0.627451 1.000000";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function ironAxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function ironAxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function ironAxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	axeOnHit(%obj.client,ironAxeImage,%col);
}

//steel

datablock ExplosionData(steelAxeExplosion : rpgAxeExplosion)
{
   lifeTimeMS = 300;

   soundProfile = swordHitSound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 8;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "12.0 14.0 12.0";
   camShakeAmp = "0.7 0.7 0.7";
   camShakeDuration = 0.35;
   camShakeRadius = 7.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
   
   image = steelAxeImage;
};

datablock ItemData(steelAxeItem : rpgAxeItem)
{
	uiName = "steel Axe";
	doColorShift = true;
	colorShiftColor = "0.976471 0.976471 0.976471 1.000000";

	image = steelAxeImage;
	canDrop = true;
};

AddDamageType("rpgAxe",   '<bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',    '%2 <bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',0.75,1);

datablock ProjectileData(steelAxeProjectile : rpgAxeProjectile)
{
   directDamage      = 20;
   directDamageType  = $DamageType::rpgAxe;
   radiusDamageType  = $DamageType::rpgAxe;
   explosion         = steelAxeExplosion;

   muzzleVelocity      = 65;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "steel Axe Hit";
};

datablock ShapeBaseImageData(steelAxeImage : rpgAxeImage)
{
   //shapeFile = "./Lil_Axe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = steelAxeItem;
   ammo = " ";
   projectile = steelAxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
   colorShiftColor = "0.976471 0.976471 0.976471 1.000000";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function steelAxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function steelAxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function steelAxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	axeOnHit(%obj.client,steelAxeImage,%col);
}

//mithril

datablock ExplosionData(mithrilAxeExplosion : rpgAxeExplosion)
{
   lifeTimeMS = 300;

   soundProfile = swordHitSound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 8;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "12.0 14.0 12.0";
   camShakeAmp = "0.7 0.7 0.7";
   camShakeDuration = 0.35;
   camShakeRadius = 7.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
   
   image = mithrilAxeImage;
};

datablock ItemData(mithrilAxeItem : rpgAxeItem)
{
	uiName = "mithril Axe";
	doColorShift = true;
	colorShiftColor = $lob::toolColor["mithril"];

	image = mithrilAxeImage;
	canDrop = true;
};

AddDamageType("rpgAxe",   '<bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',    '%2 <bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',0.75,1);

datablock ProjectileData(mithrilAxeProjectile : rpgAxeProjectile)
{
   directDamage      = 20;
   directDamageType  = $DamageType::rpgAxe;
   radiusDamageType  = $DamageType::rpgAxe;
   explosion         = mithrilAxeExplosion;

   muzzleVelocity      = 65;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "mithril Axe Hit";
};

datablock ShapeBaseImageData(mithrilAxeImage : rpgAxeImage)
{
   //shapeFile = "./Lil_Axe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = mithrilAxeItem;
   ammo = " ";
   projectile = mithrilAxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
   colorShiftColor = $lob::toolColor["mithril"];

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function mithrilAxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function mithrilAxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function mithrilAxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	axeOnHit(%obj.client,mithrilAxeImage,%col);
}

//adamantite

datablock ExplosionData(adamantiteAxeExplosion : rpgAxeExplosion)
{
   lifeTimeMS = 300;

   soundProfile = swordHitSound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 8;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "12.0 14.0 12.0";
   camShakeAmp = "0.7 0.7 0.7";
   camShakeDuration = 0.35;
   camShakeRadius = 7.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
   
   image = adamantiteAxeImage;
};

datablock ItemData(adamantiteAxeItem : rpgAxeItem)
{
	uiName = "adamantite Axe";
	doColorShift = true;
	colorShiftColor = $lob::toolColor["adamantite"];

	image = adamantiteAxeImage;
	canDrop = true;
};

AddDamageType("rpgAxe",   '<bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',    '%2 <bitmap:add-ons/Tool_RPG/CI_rpgAxe> %1',0.75,1);

datablock ProjectileData(adamantiteAxeProjectile : rpgAxeProjectile)
{
   directDamage      = 20;
   directDamageType  = $DamageType::rpgAxe;
   radiusDamageType  = $DamageType::rpgAxe;
   explosion         = adamantiteAxeExplosion;

   muzzleVelocity      = 65;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "adamantite Axe Hit";
};

datablock ShapeBaseImageData(adamantiteAxeImage : rpgAxeImage)
{
   //shapeFile = "./Lil_Axe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = adamantiteAxeItem;
   ammo = " ";
   projectile = adamantiteAxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
   colorShiftColor = $lob::toolColor["adamantite"];

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function adamantiteAxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function adamantiteAxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function adamantiteAxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	
	axeOnHit(%obj.client,adamantiteAxeImage,%col);
}

//Axes End

//Pickaxes start

//bronze

datablock ExplosionData(bronzePickaxeExplosion : rpgPickaxeExplosion)
{
   lifeTimeMS = 400;

   soundProfile = pickaxehitsound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.5 0.5 0.5";
   camShakeDuration = 0.25;
   camShakeRadius = 5.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};


datablock ItemData(bronzePickAxeItem : rpgPickaxeItem)
{
	//shapeFile = "./Lil_Pickaxe.dts";
	uiName = "Bronze Pickaxe";
	doColorShift = true;
	colorShiftColor = "0.803922 0.521569 0.247059 1";

	image = bronzePickaxeImage;
	canDrop = true;
	//iconName = "./icon_rpgPickaxe";
};

datablock ProjectileData(bronzePickAxeProjectile : rpgPickaxeProjectile)
{
   directDamage        = 15;
   directDamageType  = $DamageType::rpgPickaxe;
   radiusDamageType  = $DamageType::rpgPickaxe;
   explosion           = bronzePickaxeExplosion;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Bronze Pickaxe Hit";
};



datablock ShapeBaseImageData(bronzePickAxeImage : rpgPickaxeImage)
{
   //shapeFile = "./Lil_Pickaxe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = bronzePickaxeItem;
   ammo = " ";
   projectile = bronzePickaxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
	colorShiftColor = "0.803922 0.521569 0.247059 1";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.2;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function bronzePickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function bronzePickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function bronzePickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	pickAxeOnHit(%obj.client,bronzePickAxeImage,%col);
}

//iron

datablock ExplosionData(ironPickaxeExplosion : rpgPickaxeExplosion)
{
   lifeTimeMS = 400;

   soundProfile = pickaxehitsound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.5 0.5 0.5";
   camShakeDuration = 0.25;
   camShakeRadius = 5.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};


datablock ItemData(ironPickAxeItem : rpgPickaxeItem)
{
	//shapeFile = "./Lil_Pickaxe.dts";
	uiName = "iron Pickaxe";
	doColorShift = true;
	colorShiftColor = "0.654902 0.639216 0.627451 1.000000";

	image = ironPickaxeImage;
	canDrop = true;
	//iconName = "./icon_rpgPickaxe";
};

datablock ProjectileData(ironPickAxeProjectile : rpgPickaxeProjectile)
{
   directDamage        = 15;
   directDamageType  = $DamageType::rpgPickaxe;
   radiusDamageType  = $DamageType::rpgPickaxe;
   explosion           = ironPickaxeExplosion;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "iron Pickaxe Hit";
};



datablock ShapeBaseImageData(ironPickAxeImage : rpgPickaxeImage)
{
   //shapeFile = "./Lil_Pickaxe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = ironPickaxeItem;
   ammo = " ";
   projectile = ironPickaxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
	colorShiftColor = "0.654902 0.639216 0.627451 1.000000";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.2;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function ironPickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function ironPickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function ironPickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	pickAxeOnHit(%obj.client,ironPickAxeImage,%col);
}

//steel

datablock ExplosionData(steelPickaxeExplosion : rpgPickaxeExplosion)
{
   lifeTimeMS = 400;

   soundProfile = pickaxehitsound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.5 0.5 0.5";
   camShakeDuration = 0.25;
   camShakeRadius = 5.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};


datablock ItemData(steelPickAxeItem : rpgPickaxeItem)
{
	//shapeFile = "./Lil_Pickaxe.dts";
	uiName = "steel Pickaxe";
	doColorShift = true;
	colorShiftColor = "0.976471 0.976471 0.976471 1.000000";

	image = steelPickaxeImage;
	canDrop = true;
	//iconName = "./icon_rpgPickaxe";
};

datablock ProjectileData(steelPickAxeProjectile : rpgPickaxeProjectile)
{
   directDamage        = 15;
   directDamageType  = $DamageType::rpgPickaxe;
   radiusDamageType  = $DamageType::rpgPickaxe;
   explosion           = steelPickaxeExplosion;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "steel Pickaxe Hit";
};



datablock ShapeBaseImageData(steelPickAxeImage : rpgPickaxeImage)
{
   //shapeFile = "./Lil_Pickaxe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = steelPickaxeItem;
   ammo = " ";
   projectile = steelPickaxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
	colorShiftColor = "0.976471 0.976471 0.976471 1.000000";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.2;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

//mithril

datablock ExplosionData(mithrilPickaxeExplosion : rpgPickaxeExplosion)
{
   lifeTimeMS = 400;

   soundProfile = pickaxehitsound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.5 0.5 0.5";
   camShakeDuration = 0.25;
   camShakeRadius = 5.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};


datablock ItemData(mithrilPickAxeItem : rpgPickaxeItem)
{
	//shapeFile = "./Lil_Pickaxe.dts";
	uiName = "mithril Pickaxe";
	doColorShift = true;
	colorShiftColor = $lob::toolColor["mithril"];

	image = mithrilPickaxeImage;
	canDrop = true;
	//iconName = "./icon_rpgPickaxe";
};

datablock ProjectileData(mithrilPickAxeProjectile : rpgPickaxeProjectile)
{
   directDamage        = 15;
   directDamageType  = $DamageType::rpgPickaxe;
   radiusDamageType  = $DamageType::rpgPickaxe;
   explosion           = mithrilPickaxeExplosion;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "mithril Pickaxe Hit";
};



datablock ShapeBaseImageData(mithrilPickAxeImage : rpgPickaxeImage)
{
   //shapeFile = "./Lil_Pickaxe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = mithrilPickaxeItem;
   ammo = " ";
   projectile = mithrilPickaxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
	colorShiftColor = $lob::toolColor["mithril"];

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.2;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function mithrilPickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function mithrilPickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function mithrilPickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	pickAxeOnHit(%obj.client,mithrilPickAxeImage,%col);
}


//Adamantite

datablock ExplosionData(adamantitePickaxeExplosion : rpgPickaxeExplosion)
{
   lifeTimeMS = 400;

   soundProfile = pickaxehitsound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.5 0.5 0.5";
   camShakeDuration = 0.25;
   camShakeRadius = 5.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};


datablock ItemData(adamantitePickAxeItem : rpgPickaxeItem)
{
	//shapeFile = "./Lil_Pickaxe.dts";
	uiName = "adamantite Pickaxe";
	doColorShift = true;
	colorShiftColor = $lob::toolColor["adamantite"];

	image = adamantitePickaxeImage;
	canDrop = true;
	//iconName = "./icon_rpgPickaxe";
};

datablock ProjectileData(adamantitePickAxeProjectile : rpgPickaxeProjectile)
{
   directDamage        = 15;
   directDamageType  = $DamageType::rpgPickaxe;
   radiusDamageType  = $DamageType::rpgPickaxe;
   explosion           = adamantitePickaxeExplosion;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "adamantite Pickaxe Hit";
};



datablock ShapeBaseImageData(adamantitePickAxeImage : rpgPickaxeImage)
{
   //shapeFile = "./Lil_Pickaxe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = adamantitePickaxeItem;
   ammo = " ";
   projectile = adamantitePickaxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = true;
	colorShiftColor = $lob::toolColor["adamantite"];

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.2;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function adamantitePickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function adamantitePickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function adamantitePickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	pickAxeOnHit(%obj.client,adamantitePickAxeImage,%col);
}

function steelPickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function steelPickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function steelPickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	pickAxeOnHit(%obj.client,steelPickAxeImage,%col);
}

//Pickaxes End