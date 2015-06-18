//Achievement Just_Starting_Out
//------------------------------
//IMPORTANT: When declaring a package it must follow this syntax: Achievement_AchievementNameOfThisFile without the extension of course (.cs)
//Here, you will write the code necessary to complete an achievement.
//You will use the achievements.onAchieved(%client,AchievementName); function when a user has successfully achieved a landmark / goal.
//You can either call this function with the first two arguments or use as the function like so:
//achievements.onAchieved(%client,%nameOfAchievement,%alertAll,"Message","SoundDataBlock");
//Any field you leave blank will use the default settings (besides the first and second argument which are required).
//--------- WRITE CODE BELOW---------

package Achievement_Just_Starting_Out
{
	function Robin_Complete_Q1(%client)
	{
		%m = setKeyWords("\c6"@%client.name@" has completed the quest Just Starting Out.",%client.name SPC "Just Starting Out","\c6");
		achievements.onAchieved(%client,"a gift to ingvar",1,%m);
	}
};

activatePackage(Achievement_Just_Starting_Out);