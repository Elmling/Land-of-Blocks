
//BRONZE START

datablock ItemData(bronzeFullHelmetItem : gc_KnightHelmItem) 
{
	uiName = "Bronze Full Helmet";
	iconName = "add-ons/item_medievalArmor/icon_KnightHelm";
	image = bronzeFullHelmetImage;
	category = "Tools";
	className = "Weapon";
	colorShiftColor = "0.39 0.19 0 1";
	canDrop = true;
	isHelmet = 1;
	type = "Bronze";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	//shapeFile = "./Knight.dts";
};

datablock shapeBaseImageData(bronzeFullHelmetImage : gc_KnightHelmImage)
{
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	item = gc_KnightHelmItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "0.39 0.19 0 1";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
};

datablock ShapeBaseImageData(bronzeFullHelmetMountedImage : gc_KnightHelmMountedImage) 
{
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.015";
	eyeOffset = "0 0 10";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	correctMuzzleVector = true;
	doColorShift = false;
	colorShiftColor = "0.39 0.19 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Idle";

	stateName[1] = "Idle";
	stateAllowImageChange[1] = true;
};

function bronzeFullHelmetImage::onFire(%image, %player, %slot) 
{
	return false;
	if(isObject(%player)) 
	{
		if(%player.getMountedImage(2) $= nametoID(gc_KnightHelmMountedImage))
			%player.unmountImage(2);
		else 
		{
			%player.unmountImage(2);
			%player.mountImage(gc_KnightHelmMountedImage, 2);
		}
	}
}

function bronzeFullHelmetMountedImage::onMount(%image, %player, %slot) 
{
	//Hide all hat and accent nodes
	for(%i=0; $hat[%i] !$= ""; %i++)
		%player.hideNode($hat[%i]);

	for(%i=0; $accent[%i] !$= ""; %i++)
		%player.hideNode($accent[%i]);
		
	%player.client.helmet = %image.item;
}

function bronzeFullHelmetMountedImage::onUnmount(%image, %player, %slot) 
{
	if(isObject(%client = %player.client)) 
	{
		%client.helmet = false;
		%client.schedule(100, applyBodyParts);
		%client.schedule(100, applyBodyColors);
	}
}

//BRONZE END

//IRON START

datablock ItemData(ironFullHelmetItem : gc_KnightHelmItem) 
{
	uiName = "iron Full Helmet";
	iconName = "add-ons/item_medievalArmor/icon_KnightHelm";
	image = ironFullHelmetImage;
	category = "Tools";
	className = "Weapon";
	colorShiftColor = "0.39 0.19 0 1";
	canDrop = true;
	isHelmet = 1;
	type = "iron";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	//shapeFile = "./Knight.dts";
};

datablock shapeBaseImageData(ironFullHelmetImage : gc_KnightHelmImage)
{
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	item = gc_KnightHelmItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "0.39 0.19 0 1";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
};

datablock ShapeBaseImageData(ironFullHelmetMountedImage : gc_KnightHelmMountedImage) 
{
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.015";
	eyeOffset = "0 0 10";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	correctMuzzleVector = true;
	doColorShift = false;
	colorShiftColor = "0.39 0.19 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Idle";

	stateName[1] = "Idle";
	stateAllowImageChange[1] = true;
};

function ironFullHelmetImage::onFire(%image, %player, %slot) 
{
	return false;
	if(isObject(%player)) 
	{
		if(%player.getMountedImage(2) $= nametoID(gc_KnightHelmMountedImage))
			%player.unmountImage(2);
		else 
		{
			%player.unmountImage(2);
			%player.mountImage(gc_KnightHelmMountedImage, 2);
		}
	}
}

function ironFullHelmetMountedImage::onMount(%image, %player, %slot) 
{
	//Hide all hat and accent nodes
	for(%i=0; $hat[%i] !$= ""; %i++)
		%player.hideNode($hat[%i]);

	for(%i=0; $accent[%i] !$= ""; %i++)
		%player.hideNode($accent[%i]);
		
	%player.client.helmet = %image.item;
}

function ironFullHelmetMountedImage::onUnmount(%image, %player, %slot) 
{
	if(isObject(%client = %player.client)) 
	{
		%client.helmet = false;
		%client.schedule(100, applyBodyParts);
		%client.schedule(100, applyBodyColors);
	}
}

//IRON END

//steel START

datablock ItemData(steelFullHelmetItem : gc_KnightHelmItem) 
{
	uiName = "steel Full Helmet";
	iconName = "add-ons/item_medievalArmor/icon_KnightHelm";
	image = steelFullHelmetImage;
	category = "Tools";
	className = "Weapon";
	colorShiftColor = "0.39 0.19 0 1";
	canDrop = true;
	isHelmet = 1;
	type = "steel";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	//shapeFile = "./Knight.dts";
};

datablock shapeBaseImageData(steelFullHelmetImage : gc_KnightHelmImage)
{
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	item = gc_KnightHelmItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "0.39 0.19 0 1";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
};

datablock ShapeBaseImageData(steelFullHelmetMountedImage : gc_KnightHelmMountedImage) 
{
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.015";
	eyeOffset = "0 0 10";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	correctMuzzleVector = true;
	doColorShift = false;
	colorShiftColor = "0.39 0.19 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Idle";

	stateName[1] = "Idle";
	stateAllowImageChange[1] = true;
};

function steelFullHelmetImage::onFire(%image, %player, %slot) 
{
	return false;
	if(isObject(%player)) 
	{
		if(%player.getMountedImage(2) $= nametoID(gc_KnightHelmMountedImage))
			%player.unmountImage(2);
		else 
		{
			%player.unmountImage(2);
			%player.mountImage(gc_KnightHelmMountedImage, 2);
		}
	}
}

function steelFullHelmetMountedImage::onMount(%image, %player, %slot) 
{
	//Hide all hat and accent nodes
	for(%i=0; $hat[%i] !$= ""; %i++)
		%player.hideNode($hat[%i]);

	for(%i=0; $accent[%i] !$= ""; %i++)
		%player.hideNode($accent[%i]);
		
	%player.client.helmet = %image.item;
}

function steelFullHelmetMountedImage::onUnmount(%image, %player, %slot) 
{
	if(isObject(%client = %player.client)) 
	{
		%client.helmet = false;
		%client.schedule(100, applyBodyParts);
		%client.schedule(100, applyBodyColors);
	}
}

//steel END

//mithril START

datablock ItemData(mithrilFullHelmetItem : gc_KnightHelmItem) 
{
	uiName = "mithril Full Helmet";
	iconName = "add-ons/item_medievalArmor/icon_KnightHelm";
	image = mithrilFullHelmetImage;
	category = "Tools";
	className = "Weapon";
	colorShiftColor = "0.39 0.19 0 1";
	canDrop = true;
	isHelmet = 1;
	type = "mithril";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	//shapeFile = "./Knight.dts";
};

datablock shapeBaseImageData(mithrilFullHelmetImage : gc_KnightHelmImage)
{
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	item = gc_KnightHelmItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "0.39 0.19 0 1";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
};

datablock ShapeBaseImageData(mithrilFullHelmetMountedImage : gc_KnightHelmMountedImage) 
{
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.015";
	eyeOffset = "0 0 10";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	correctMuzzleVector = true;
	doColorShift = false;
	colorShiftColor = "0.39 0.19 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Idle";

	stateName[1] = "Idle";
	stateAllowImageChange[1] = true;
};

function mithrilFullHelmetImage::onFire(%image, %player, %slot) 
{
	return false;
	if(isObject(%player)) 
	{
		if(%player.getMountedImage(2) $= nametoID(gc_KnightHelmMountedImage))
			%player.unmountImage(2);
		else 
		{
			%player.unmountImage(2);
			%player.mountImage(gc_KnightHelmMountedImage, 2);
		}
	}
}

function mithrilFullHelmetMountedImage::onMount(%image, %player, %slot) 
{
	//Hide all hat and accent nodes
	for(%i=0; $hat[%i] !$= ""; %i++)
		%player.hideNode($hat[%i]);

	for(%i=0; $accent[%i] !$= ""; %i++)
		%player.hideNode($accent[%i]);
		
	%player.client.helmet = %image.item;
}

function mithrilFullHelmetMountedImage::onUnmount(%image, %player, %slot) 
{
	if(isObject(%client = %player.client)) 
	{
		%client.helmet = false;
		%client.schedule(100, applyBodyParts);
		%client.schedule(100, applyBodyColors);
	}
}

//mithril END

//adamantite START

datablock ItemData(adamantiteFullHelmetItem : gc_KnightHelmItem) 
{
	uiName = "adamantite Full Helmet";
	iconName = "add-ons/item_medievalArmor/icon_KnightHelm";
	image = adamantiteFullHelmetImage;
	category = "Tools";
	className = "Weapon";
	colorShiftColor = "0.39 0.19 0 1";
	canDrop = true;
	isHelmet = 1;
	type = "adamantite";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	//shapeFile = "./Knight.dts";
};

datablock shapeBaseImageData(adamantiteFullHelmetImage : gc_KnightHelmImage)
{
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	item = gc_KnightHelmItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "0.39 0.19 0 1";
	shapefile = "add-ons/item_medievalArmor/knight.dts";
};

datablock ShapeBaseImageData(adamantiteFullHelmetMountedImage : gc_KnightHelmMountedImage) 
{
	shapefile = "add-ons/item_medievalArmor/knight.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.015";
	eyeOffset = "0 0 10";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	correctMuzzleVector = true;
	doColorShift = false;
	colorShiftColor = "0.39 0.19 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Idle";

	stateName[1] = "Idle";
	stateAllowImageChange[1] = true;
};

function adamantiteFullHelmetImage::onFire(%image, %player, %slot) 
{
	return false;
	if(isObject(%player)) 
	{
		if(%player.getMountedImage(2) $= nametoID(gc_KnightHelmMountedImage))
			%player.unmountImage(2);
		else 
		{
			%player.unmountImage(2);
			%player.mountImage(gc_KnightHelmMountedImage, 2);
		}
	}
}

function adamantiteFullHelmetMountedImage::onMount(%image, %player, %slot) 
{
	//Hide all hat and accent nodes
	for(%i=0; $hat[%i] !$= ""; %i++)
		%player.hideNode($hat[%i]);

	for(%i=0; $accent[%i] !$= ""; %i++)
		%player.hideNode($accent[%i]);
		
	%player.client.helmet = %image.item;
}

function adamantiteFullHelmetMountedImage::onUnmount(%image, %player, %slot) 
{
	if(isObject(%client = %player.client)) 
	{
		%client.helmet = false;
		%client.schedule(100, applyBodyParts);
		%client.schedule(100, applyBodyColors);
	}
}

//adamantite END