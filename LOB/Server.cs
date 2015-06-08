function newFile(%this,%ext)
{
	if(%ext $= "")
		%ext = ".cs";
	%f = new fileObject();
	%f.openForWrite("base/Lob/"@%this@%ext);
	%f.close();
	%f.delete();
	
	echo("New file created at base/LOB/"@%this@%ext@".");
}

$LOB::path = "base/LOB/";

//exec($LOB::path@"client/client.cs");

exec($LOB::path@"events/dialouge.cs");

exec($LOB::path@"datablocks/tools.cs");
exec($LOB::path@"datablocks/weapons.cs");
exec($LOB::path@"datablocks/heal.cs");
exec($lob::path@"datablocks/dash.cs");
exec($lob::path@"datablocks/sounds.cs");
exec($lob::path@"datablocks/armor/helmet.cs");
exec($lob::path@"datablocks/armor/armor.cs");
exec($LOB::path@"scriptTools.cs");

exec($LOB::path@"Npcs/havelock.cs");
exec($LOB::path@"Npcs/garman.cs");
exec($LOB::path@"Npcs/Gandalf.cs");
exec($LOB::path@"Npcs/Worm.cs");
exec($LOB::path@"Npcs/Ingvar.cs");
exec($LOB::path@"Npcs/Bandit.cs");
exec($LOB::path@"Npcs/Robin.cs");
exec($LOB::path@"Npcs/Angus.cs");
exec($LOB::path@"Npcs/Banker.cs");
exec($LOB::path@"Npcs/Goblin.cs");
exec($LOB::path@"Npcs/Evelyn.cs");
exec($LOB::path@"Npcs/fairy.cs");
exec($LOB::path@"npcs/olympus.cs");
exec($lob::path@"npcs/Eldin.cs");
exec($lob::path@"/npcs/Herald.cs");
exec($lob::path@"/npcs/onyx.cs");
exec($lob::path@"/npcs/Belgard.cs");
exec($lob::path@"/npcs/ogre.cs");
exec($lob::path@"/npcs/horse.cs");
exec($lob::path@"/npcs/Dungeoneer.cs");
exec($lob::path@"/npcs/shopkeeper.cs");
exec($lob::path@"/npcs/yeti.cs");
exec($lob::path@"/npcs/crock.cs");
exec($lob::path@"/npcs/OgreGeneral.cs");
exec($lob::path@"/npcs/dragon.cs");
exec($lob::path@"/npcs/lilly.cs");
exec($lob::path@"/npcs/vitel.cs");
exec($lob::path@"/npcs/wizard.cs");
exec($lob::path@"/npcs/zil.cs");
exec($lob::path@"/npcs/arc.cs");
exec($lob::path@"/npcs/coyote.cs");

exec($LOB::path@"initialize.cs");
exec($LOB::path@"saveLoad.cs");
exec($LOB::path@"spawning.cs");
exec($lob::path@"npc_talk.cs");
exec($LOB::path@"Avatar.cs");
exec($LOB::path@"ServerCmd.cs");
exec($LOB::path@"Exp.cs");
exec($LOB::path@"Combat.cs");
exec($LOB::path@"Achievements.cs");
exec($LOB::path@"Building.cs");
exec($LOB::path@"GameConnection.cs");
exec($LOB::path@"Activity.cs");
exec($LOB::path@"Updates.cs");
exec($LOB::path@"Trade.cs");
exec($LOB::path@"Shop.cs");
exec($LOB::path@"TeleportScrolls.cs");
exec($lob::path@"horse.cs");
exec($lob::path@"zoneMagager.cs");
exec($lob::path@"quest.cs");
exec($lob::path@"antiBot.cs");
exec($LOB::path@"Npc.cs");
exec($lob::path@"Events.cs");
exec($lob::path@"suggestions.cs");
exec($lob::path@"dog.cs");
exec($lob::path@"duel.cs");

exec($LOB::path@"skills/firemaking.cs");
exec($LOB::path@"/skills/mining.cs");
exec($LOB::path@"/skills/woodcutting.cs");
exec($lob::path@"/skills/smithing.cs");
exec($LOB::path@"/skills/smelting.cs");
exec($LOB::path@"/skills/cooking.cs");
exec($LOB::path@"/skills/magic.cs");
exec($LOB::path@"/skills/climbing.cs");

exec($LOB::path@"/quests/quest.cs");

exec($LOB::path@"/dungeon/d_build.cs");

exec($LOB::path@"/donations/avatars/avatars.cs");

exec($LOB::path@"inventory.cs");