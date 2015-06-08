$nodeColor["Garman","headskin"] = "1 0.7 0.6 1";
$nodeColor["Garman","chest"] = "1 0.7 0.6 1";
$nodeColor["Garman","pants"] = "0 0 0 1";
$nodeColor["Garman","larm"] = "1 0.7 0.6 1";
$nodeColor["Garman","rarm"] = "1 0.7 0.6 1";
$nodeColor["Garman","lhand"] = "1 0.7 0.6 1";
$nodeColor["Garman","rhand"] = "1 0.7 0.6 1";
$nodeColor["Garman","lshoe"] = "0 0 0 1";
$nodeColor["Garman","rshoe"] = $nodeColor["Garman","lshoe"];
//$nodeColor["Garman","pack"] = $nodeColor["Garman","pants"];
//$pack["Garman"] = "pack";
$smiley["Garman"] = "";
$decal["Garman"] = "";
$task["Garman"] = "woodcut";
$taskInner["Garman"] = "Pine";
$equip["garman"] = "bronzeAxeImage";
$OnClickActionSet["Garman"] = true;
$LOB::NPC["Garman","Datablock"] = playerStandardArmor;

while(isObject(Garman))Garman.delete();

new scriptObject(Garman);

function Garman::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	cancel(%ai.woodcutloop);
	cancel(%ai.onClickLoop);
	
	%ai.setimageTrigger(1,0);
	
	%client = %player.client;
	%pp = %player.position;
	
	%mp = %ai.position;
	
	%vd = vectorDist(%mp,%pp);
	
	if(%vd >= 5)
	{
		%ai.messageBoxPlayer = "";
		%ai.woodcut();
		return true;
	}
	
	if(%ai.getAimLocation() != %pp)
		%ai.setAimLocation(%pp);
	
	if(%ai.messageBoxPlayer != %player)
	{
		%ai.messageBoxPlayer = %player;
		commandtoClient(%client,'messageBoxOk',"Dialogue with "@%ai.name,"Argg, hello there "@%client.name@". Cutting down these Acacia trees is hard work, but I'm a smither. Bring me 10 iron and I will make you an Iron Sword. Now leave me be.. Argg..");
	}
	
	
	%ai.onClickLoop = %this.schedule(1000,onclick,%ai,%player);
}