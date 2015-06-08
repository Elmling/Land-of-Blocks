
//self explanatory
//--
$nodeColor["Wizard","headskin"] = "1 0.878431 0.611765 1 1";
$nodeColor["Wizard","pants"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","larm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","lhand"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","lshoe"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","pack"] = "0.560784 0.929412 0.960784 1 1";
$nodeColor["Wizard","rarm"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","rhand"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","rshoe"] = "0.0784314 0.0784314 0.0784314 1 1";
$nodeColor["Wizard","chest"] = "0.0784314 0.0784314 0.0784314 1 1";
$smiley["Wizard"] = "smileyEvil2";
$decal["Wizard"] = "Mod-DareDevil";
$lob::vision["Wizard"] = 10;
//--

//function the Wizard will use
$task["Wizard"] = "roam";

//for some task, there is an inner task
//$taskInner["Wizard"] = "iron";

//if we click the Wizard, are they ready for dialogue?
$OnClickActionSet["Wizard"] = "1";

//roam range
$roam["Wizard"] = 5;

//item to be heald
$equip["Wizard"] = "windWallImage";

//datablock
$LOB::Wizard["Wizard","Datablock"] = playerStandardArmor;

//Stuff they say that users can see above the Wizard's head
//--
$lob::roamMsgCount["Wizard"] = -1;
$lob::roamMsg["Wizard",$lob::roamMsgCount["Wizard"]++] = "Interested in the art of magic?";
$lob::roamMsg["Wizard",$lob::roamMsgCount["Wizard"]++] = "Only the few are able to cast devistating spells.";
$lob::roamMsgCount++;
//--

while(isObject(Wizard))Wizard.delete();

//SO
new scriptObject(Wizard);
Wizard.quest1 = "Wizards Payback";
Wizard.quest2 = "Wizards Plight";

function Wizard::onObjectSpawned(%this,%Wizard)
{
	//callback for when they first spawn
	
	//nothing
}

function Wizard::onClick(%this,%ai,%player)
{
	if(%player.client.slo.hasGui $= "")
	{
		lob_playerNeedsGui(%player.client);
		return false;
	}
	
	%client = %player.client;
	%slo = %client.slo;
	%client.LastNpcTalkedTo = %ai;
	
	%m = "I'm busy.";

	if(!%client.slo.hasGui)
	{
		commandToClient(%client,'messageBoxOk',"Attention","You'll need the client to talk to Wizard.");
		return false;
	}	

	if(%client.slo.vitel_q1_complete $= "")
	{
		%m = "You should go help Vitel before I decide to sell you any Magic Material.";
		%um1 = "#string Alright.. #command";
	}
	else
	{
		%m = "So you want to buy some Magic Material?";
		%um1 = "#string Yes. #command WizardBuyMagicMaterial";
	}

	%name = %c.name;
	%head = "Dialogue with Wizard";
		
	commandToClient(%client,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
	
	//commandToClient(%client,'messageBoxOk',"Dialouge with Wizard",%m);
}

function serverCmdWizardBuyMagicMaterial(%this)
{
	if(%this.slo.vitel_q1_complete $= "")
		return false;
		
	
	%name = %c.name;
	%head = "Dialogue with Wizard";
	%m = "I'll sell you magic material for 10 gold each.";
	%um1 = "#string Buy 100 Magic Material (1k) #command wizardBuy100";
	%um2 = "#string Buy 500 Magic Material (5k) #command wizardBuy500";
	%um3 = "#string Buy 1000 Magic Material (10k) #command wizardBuy1000";
	%um4 = "#string Buy 5000 Magic Material (50k) #command wizardBuy5000";
	%um5 = "#string Buy 10000 Magic Material (100k) #command wizardBuy10000";
	commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
}

function serverCmdwizardBuy100(%this)
{
	%vd = vectorDist(%this.player.position,%this.lastNpcTalkedTo.position);
	if(%this.lastNpcTalkedTo.name $= "Wizard" && %vd <= 10)
	{
		%mmPrice = 10;
		%total = %mmprice * 100;
		if(%this.slo.inventory.itemCountGold >= %total)
		{
			%this.slo.inventory.itemCountGold-=%total;
			%this.slo.inventory.itemCountMagicMaterial+=100;
			
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You've bought 100 Magic Material, come again.";
			%um1 = "#string Bye. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
		}
		else
		{
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You don't have enough gold!";
			%um1 = "#string Oops. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);		
		}
	}
}

function serverCmdwizardBuy500(%this)
{
	%vd = vectorDist(%this.player.position,%this.lastNpcTalkedTo.position);
	if(%this.lastNpcTalkedTo.name $= "Wizard" && %vd <= 10)
	{
		%mmPrice = 10;
		%total = %mmprice * 500;
		if(%this.slo.inventory.itemCountGold >= %total)
		{
			%this.slo.inventory.itemCountGold-=%total;
			%this.slo.inventory.itemCountMagicMaterial+=500;
			
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You've bought 500 Magic Material, come again.";
			%um1 = "#string Bye. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
		}
		else
		{
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You don't have enough gold!";
			%um1 = "#string Oops. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);		
		}
	}
}

function serverCmdwizardBuy1000(%this)
{
	%vd = vectorDist(%this.player.position,%this.lastNpcTalkedTo.position);
	if(%this.lastNpcTalkedTo.name $= "Wizard" && %vd <= 10)
	{
		%mmPrice = 10;
		%total = %mmprice * 1000;
		if(%this.slo.inventory.itemCountGold >= %total)
		{
			%this.slo.inventory.itemCountGold-=%total;
			%this.slo.inventory.itemCountMagicMaterial+=1000;
			
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You've bought 1000 Magic Material, come again.";
			%um1 = "#string Bye. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
		}
		else
		{
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You don't have enough gold!";
			%um1 = "#string Oops. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);		
		}
	}
}

function serverCmdwizardBuy5000(%this)
{
	%vd = vectorDist(%this.player.position,%this.lastNpcTalkedTo.position);
	if(%this.lastNpcTalkedTo.name $= "Wizard" && %vd <= 10)
	{
		%mmPrice = 10;
		%total = %mmprice * 5000;
		if(%this.slo.inventory.itemCountGold >= %total)
		{
			%this.slo.inventory.itemCountGold-=%total;
			%this.slo.inventory.itemCountMagicMaterial+=5000;
			
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You've bought 5000 Magic Material, come again.";
			%um1 = "#string Bye. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
		}
		else
		{
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You don't have enough gold!";
			%um1 = "#string Oops. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);		
		}
	}
}

function serverCmdwizardBuy10000(%this)
{
	%vd = vectorDist(%this.player.position,%this.lastNpcTalkedTo.position);
	if(%this.lastNpcTalkedTo.name $= "Wizard" && %vd <= 10)
	{
		%mmPrice = 10;
		%total = %mmprice * 10000;
		if(%this.slo.inventory.itemCountGold >= %total)
		{
			%this.slo.inventory.itemCountGold-=%total;
			%this.slo.inventory.itemCountMagicMaterial+=10000;
			
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You've bought 10,000 Magic Material, come again.";
			%um1 = "#string Bye. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);
		}
		else
		{
			%name = %c.name;
			%head = "Dialogue with Wizard";
			%m = "You don't have enough gold!";
			%um1 = "#string Oops. #command";
			commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5,%um6);		
		}
	}
}