//Achievement A_gift_to_Ingvar
//------------------------------
//IMPORTANT: When declaring a package it must follow this syntax: Achievement_AchievementNameOfThisFile without the extension of course (.cs)
//Here, you will write the code necessary to complete an achievement.
//You will use the achievements.onAchieved(%client,AchievementName); function when a user has successfully achieved a landmark / goal.
//You can either call this function with the first two arguments or use as the function like so:
//achievements.onAchieved(%client,%nameOfAchievement,%alertAll,"Message","SoundDataBlock");
//Any field you leave blank will use the default settings (besides the first and second argument which are required).
//--------- WRITE CODE BELOW---------

package Achievement_a_gift_to_Ingvar
{
	function ingvar_complete_q1(%client)
	{
		%m = setKeyWords("\c6"@%client.name@" has completed the quest Ingvar's Favor.",%client.name SPC "Ingvar's Favor","\c6");
		achievements.onAchieved(%client,"a gift to ingvar",1,%m);
	}
};

activatePackage("Achievement_a_gift_to_ingvar");

