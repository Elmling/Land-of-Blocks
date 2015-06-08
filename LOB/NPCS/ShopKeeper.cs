
$nodeColor["ShopKeeper","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["ShopKeeper","pants"] = "0.105882 0.458824 0.768627 1 1";
$nodeColor["ShopKeeper","larm"] = "0.200 0.200 0.200 1.000 1";
$nodeColor["ShopKeeper","lhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["ShopKeeper","lshoe"] = "0.200 0.200 0.200 1.000 1";
$nodeColor["ShopKeeper","pack"] = "0.500 0.500 0.500 1.000 1";
$nodeColor["ShopKeeper","rarm"] = "0.200 0.200 0.200 1.000 1";
$nodeColor["ShopKeeper","rhand"] = "1 0.878431 0.611765 1 1";
$nodeColor["ShopKeeper","rshoe"] = "0.200 0.200 0.200 1.000 1";
$nodeColor["ShopKeeper","chest"] = "0.900 0.900 0.900 1.000 1";
$decal["ShopKeeper"] = "worm_engineer";
$smiley["ShopKeeper"] = "ChefSmiley";
$pack["ShopKeeper"] = "";

$OnClickActionSet["ShopKeeper"] = "1";
$roam["ShopKeeper"] = 0;
$equip["ShopKeeper"] = "";
$LOB::ShopKeeper["ShopKeeper","Datablock"] = playerStandardArmor;
$lob::roamMsgCount["ShopKeeper"] = -1;
$lob::roamMsg["ShopKeeper",$lob::roamMsgCount["ShopKeeper"]++] = "Selling and buying goods!";
$lob::roamMsg["ShopKeeper",$lob::roamMsgCount["ShopKeeper"]++] = "Hey! Let's trade!";
$lob::roamMsg["ShopKeeper",$lob::roamMsgCount["ShopKeeper"]++] = "I've got what you need!.";
$lob::isShopShopKeeper["ShopKeeper"] = true;
$lob::vision["shopkeeper"] = 10;

if(isObject(ShopKeeper))
	ShopKeeper.delete();
	
new scriptObject(ShopKeeper);

function ShopKeeper::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%clientinv = %client.slo.inventory;
	
	if(!%ai.hasShop)
		%ai.lob_newShop();
	
}