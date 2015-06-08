if(isObject(lob_activity))
	lob_activity.delete();
	
new scriptGroup(lob_activity);

function lob_activity::register(%this,%client)
{
	if(isObject(%client.activity))
		return 0;
		
	%cn = %client.name;
	%client.activity = new scriptGroup()
	{
		class = clientActivity;
		client = %client;
		name = %cn;
	};
}

function lob_activity::delete(%this,%client)
{
	%cn = %client.name;
	%activity = %this.findByName(%client.name);
	if(isObject(%activity))
	{
		%client.activity = 0;
		%activity.delete();
	}
}

function lob_Activity::findByName(%this,%name)
{
	for(%i=0;%i<%this.getCount();%i++)
	{
		%o = %this.getObject(%i);
		if(%o.name $= %name)
			return %o;
	}
	return false;
}

function clientActivity::newActivity(%this,%string)
{
	%client = %this.client;
	%time = getRealTime();
	
	if(%time - %this.lastActivityTime <= 1000 || %string $= %this.lastActivity)
		return 0;
	
	%activity = new scriptObject()
	{
		client = %client;
		String = %string;
		Time = %time;
	};
	
	%this.add(%activity);
	%this.lastActivityTime = %time;
	%this.lastActivity = %string;
	
	if(%this.getCount() >= 20)
		%this.getObject(0).delete();
}

function serverCmdGetActivity(%client,%name)
{
	if(%client.isAdmin)
	{
		%name = findClientByName(%name);
		if(isObject(%name))
		{
			if(isObject(%name.activity))
			{
				%activity = %name.activity;
				%ac = %activity.getCount();
				
				for(%i=0;%i<%ac;%i++)
				{
					%acc = %activity.getObject(%i);
					%time = %acc.time;
					%timeSub = getRealTime() - %time;
					%str = %timeSub @ " ago, " @ %name.name SPC %acc.string;
					%string = trim(%str);
					commandToClient(%client,'catchActivityData',%string);
				}
			}
		}
	}
}