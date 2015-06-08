$lob::updateCount["client"] = 0;
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Gui Released";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Fixed the Dialouge GUI bug.\n\nFixed the bank bug. That's about it.";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Bank Gui bug popped up again, it is fixed now. New trading GUI has been made. Fixed a few other small bugs.";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Added support for new items. Will fix item descriptions with the next client update.";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Fixed up the bank only adding one item. Added in dash support, you'll get to use it when the server is updated to version 3.";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Bug Fixes.";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "1. New User interface. 2. Fairy gives info. 3. Other minor things.";
$lob::updateInfo["client",$lob::updateCount["client"]++] = "Check the forum topic, page 6.";

$lob::updateCount["server"] = 0;
$lob::updateInfo["Server",$lob::updateCount["server"]++] = "Smithing, smelting, mining, woodcutting, firemaking, and combat have all been coded. There is also a banking system. Some modifications to the AI system have been done.";
$lob::updateInfo["Server",$lob::updateCount["server"]++] = "Added in a trading system. Click on a player to bring up the interactive dialogue and click the trade button. When a user accepts the trade GUI will popup and you can trade.";
$lob::updateInfo["Server",$lob::updateCount["server"]++] = "1. Improvements to the AI.\n\n2. Inventory and bank glitches fixed.\n\n3. 2 New songs added.\n\n4. Your own Fairy, gives info.\n\n5. Special attacks now show up when a weapon is equipped.\n\n6.Made smithing and smelting a lot more friendly, less grinding.";
$lob::updateInfo["Server",$lob::updateCount["server"]++] = "Check the forum topic, page 6.";

function serverCmdPopulateUpdatesList(%client)
{
	commandToclient(%client,'populateUpdatesList',$lob::guiVersion,$lob::serverversion);
}

function serverCmdGetClientUpdateInfo(%client,%version)
{
	%info = $lob::updateInfo["client",%version];
	commandToClient(%client,'catchClientUpdateInfo',%info);
}

function serverCmdGetServerUpdateInfo(%client,%version)
{
	%info = $lob::updateInfo["server",%version];
	commandToClient(%client,'catchServerUpdateInfo',%info);
}