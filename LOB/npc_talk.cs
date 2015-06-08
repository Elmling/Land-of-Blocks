
$lob::npcTalk["citizen"] = 1;
$lob::npctalkcount["citizen","interpass"] = -1;
$lob::npctalk["citizen","interpass",$lob::npctalkcount["citizen","interpass"]++] = "It's quite lovely here";
$lob::npctalk["citizen","interpass",$lob::npctalkcount["citizen","interpass"]++] = "Beware of the Yetis traveler";

$lob::npcTalk["citizen"] = 1;
$lob::npctalkcount["citizen","ordunia"] = -1;
$lob::npctalk["citizen","ordunia",$lob::npctalkcount["citizen","ordunia"]++] = "Oi there, did ya know jumping on another players horse will get you wanted?";
$lob::npctalk["citizen","ordunia",$lob::npctalkcount["citizen","ordunia"]++] = "The meat dropped from the worms are great, you should find some.";
$lob::npctalk["citizen","ordunia",$lob::npctalkcount["citizen","ordunia"]++] = "I've been working on my wood cutting skills!";

$lob::npcTalk["citizen"] = 1;
$lob::npctalkcount["citizen","Eldria"] = -1;
$lob::npctalk["citizen","eldria",$lob::npctalkcount["citizen","eldria"]++] = "Legend has it, the Yetis are the fastest creature in the lands, beware.";
$lob::npctalk["citizen","eldria",$lob::npctalkcount["citizen","eldria"]++] = "Every town has a shop, bank, and other things you can do in it.";
$lob::npctalk["citizen","eldria",$lob::npctalkcount["citizen","eldria"]++] = "If you haven't heard or seen of the legendary SpeedBlade, consider yourself lucky.";

function doNpcTalk(%object,%client)
{
	%name = %object.name;
	%area = %object.brick;
	%area = %area.getName();
	%area = strReplace(strlwr(%area),"_npcspawn_","");
	%area = strReplace(%area,"_"," ");
	%area = trim(%area);
	%area = stripTrailingSpaces(%area);
	%area = getWords(%area,0,getWordCount(%area)-2);
	
	%object.aimAt(%client.player.position);

	if($lob::npctalkcount[%name,%area] !$= "")
	{
		%ran = getRandom(0,$lob::npctalkcount[%name,%area]);
		%m = $lob::npctalk[%name,%area,%ran];
		//commandToClient(%client,'messageBoxOk',%name @ " Says,",%m);
		commandToClient(%client,'setdlg',%name @ " says,",%m,"#string Okay.. #command");
		
	}
	
	//%area = strReplace
}
