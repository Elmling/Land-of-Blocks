function lob_startQuest(%client,%questName)
{
	%m = setKeyWords("\c6" @ %client.name @ " has started the quest " @ %questName @ "!",%client.name SPC %questName,"\c6");
	messageAll('',%m);
}

function lob_finishQuest(%client,%questName)
{
	%m = setKeyWords("\c6" @ %client.name @ " has finished the quest " @ %questName @ "!",%client.name SPC %questName,"\c6");
	messageAll('',%m);
}

function serverCmdNpcQ1(%this)
{echo("yeeeeeee");
	%lastNpcN = %this.lastNpcTalkedTo;
	%lastNPC = %lastNpcN.getId();

	%q = %lastNpcN.quest_0;
	serverCmdNpcEvaluateDialogue(%this,%q);
}

function serverCmdNpcQ2(%this)
{
	%lastNpcN = %this.lastNpcTalkedTo;
	%lastNPC = %lastNpcN.getId();	

	%q = %lastNpcN.quest_1;
	serverCmdNpcEvaluateDialogue(%this,%q);
}

function serverCmdNpcQ3(%this)
{
	%lastNpcN = %this.lastNpcTalkedTo;
	%lastNPC = %lastNpcN.getId();	

	%q = %lastNpcN.quest_2;
	serverCmdNpcEvaluateDialogue(%this,%q);
}

function serverCmdNpcQ4(%this)
{
	%lastNpcN = %this.lastNpcTalkedTo;
	%lastNPC = %lastNpcN.getId();	

	%q = %lastNpcN.quest_3;
	serverCmdNpcEvaluateDialogue(%this,%q);
}

function serverCmdNpcQ5(%this)
{
	%lastNpcN = %this.lastNpcTalkedTo;
	%lastNPC = %lastNpcN.getId();	

	%q = %lastNpcN.quest_4;
	serverCmdNpcEvaluateDialogue(%this,%q);
}

function serverCmdNpcEvaluateDialogue(%this,%about)
{
	%lastNpcN = %this.lastNpcTalkedTo;
	%lastNPC = %lastNpcN.getId();	

	//for(%i=0;%i<50;%i++)
	//{
	//	eval("%lastRes = %this.slo." @ %lastNpc.name @ "quest" @ %i @";");
	//	echo(%lastRes);
	//	if(%lastRes $= "")
	//	{
	//		break;
	//	}
	//	echo("last resutl = " @ %lastRes);
	//	//if(%lastRes)
	//}

	//eval("%path = %lastNpc.questPath_" @ %i @";");
	
	if(%about $= "")
	{
		%npcname = %LastNpcN;
		%name = %this.name;
		%head = "Dialogue with " @ %npcname;
		%m = "Hi " @ %this.name;		
		for(%i=0;%i<5;%i++)
		{
			%q = %lastNpcN.quest_[%i];
			if(%q !$= "")
				%um[%i+1] = "#string " @ %q @ " #command npcq" @ %i+1;
		}
		commandToClient(%this,'setdlg',%head,%m,%um1,%um2,%um3,%um4,%um5);		
	}
	else
	{
		%index = %lastNpcN.quest[%about];
		echo(%lastnpcn @ "index = " @ %index);
		if(%index $= "")
			return;

		%path = %lastNpcn.questPath_[%index];

		%f = new fileObject();
		%f.openForRead(%path);

		while(!%f.isEof())
		{
			%line = %f.readLine();
			
			if(getSubStr(%line,0,4) $= "#REW")
			{
				echo("BREAKING");
				break;
			}
			else
			{
				echo("in condition");
				//condition
				%condition = %line;
				%condition = getSubStr(%condition,5,strStr(%condition," #CONEND")-5);
				%condition = strReplace(%condition,"(client)",%this);
				%condition = strReplace(%condition," ","&&");
				%condition = strReplace(%condition,"_","==");
				%stringCon = %condition;
				
				if(%condition !$= "")
					eval("%condition = " @ %condition @ ";");
				else
					%condition = 0;
			}
			
			if((%condition) $= "1")
			{	
				//echo(%stringCon @ " $= 1??? ");
				//action
				
				%action = %line;
				%action = getSubStr(%action,strStr(%action,"#ACT")+5,strLen(%action));		
				%action = getSubStr(%action,0,strStr(%action," #ACTEND"));
				
				if(getWord(%action,0) $= "(client).reward()")
				{
					eval("%doneQuest = %this.slo." @ %lastNpcN @ "Quest" @ %index @ ";");
					if(%doneQuest $= "")
					{
						%this.questReward(%path);
						//return true;
					}
					
					%action = trim(strReplace(%action,"(client).reward()",""));
					%action = striptrailingSpaces(%action);
					//return false;
				}
				
				%action = strReplace(%action,"_","=");
				%action = strReplace(%action," ","&&");
				echo(getWord(%action,0));
				%action = strReplace(%action,"(client)",%this);
				
				if(strStr(strLwr(%action),"null") == -1)
					eval("%action = " @ %action @ ";");
				
				//dialogue
				%dialogue = %line;
				%dialogue = getSubStr(%dialogue,strStr(%dialogue,"#DIA")+5,strLen(%dialogue));
				%dialogue = getSubStr(%dialogue,0,strStr(%dialogue,"#DIAEND"));

				%npcname = %LastNpcN;
				%name = %this.name;
				%head = "Dialogue with " @ %npcname;
				%m = %dialogue;		
				echo("index ===== " @ %index);
				%um1 = "#string Ok #command npcq" @ %index+1;
				%um2 = "#string Nevermind. #command";
				commandToClient(%this,'setdlg',%head,%m,%um1,%um2);
				break;
			}
			else
			{
			}
			//break;
			
		}
		%f.close();
		%f.delete();
	}
	
}

function gameConnection::questReward(%this,%path)
{
	%f = new fileObject();
	%f.openForRead(%path);
	
	while(!%f.isEof())
	{
		%line = %f.readLine();
		
		if(getSubStr(%line,0,4) $= "#REW")
		{
			%reward = getWords(%line,1,getWordCount(%line)-2);
			%reward = strReplace(%reward,"(client)",%this);
			%wc = getWordCount(%reward);
			
			for(%i=0;%i<%wc+1;%i++)
			{
				%r = getWord(%reward,%i);
				eval(%r);
			}
			
			//finished quest
			//eval("%this.slo." @ %lastNpcN @ "Quest" @ %index @ " = 1;");
			break;
		}	
	}
}