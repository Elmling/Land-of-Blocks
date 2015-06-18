if(!isObject(lob_execManager))
{
	new scriptGroup(lob_execManager);
}

function lobexec()
{
	lob_execManager.exec();
}

function lob_execManager::init(%this)
{
	if(%this.init)
		return 0;
		
	%this.count = -1;
	%this.init = true;
}

lob_execManager.init();

function lob_execManager::append(%this,%path)
{
	if(!isFile(%path) || isObject(%this.getObjByName(%path)))
		return 0;
	
	%o = new scriptObject()
	{
		path = %path;
	};
	
	%this.add(%o);
}

function lob_execManager::clear(%this,%path)
{
	if(%path $= "")
	{
		%this.clear();
	}
	else
	{
		if(isObject(%o = %this.getObjByPath(%path)))
		{
			%o.delete();
		}
	}
}

function lob_execManager::getObjByPath(%this,%path)
{
	%count = %this.getCount();
	
	for(%i=0;%i<%count;%i++)
	{
		%o = %this.getObject(%i);
		
		if(%o.path $= "path" || fileName(%o.path) $= %path)
		{
			return %o;
		}
	}	
	return 0;
}

function lob_execManager::exec(%this)
{
	%count = %this.getCount();
	
	for(%i=0;%i<%count;%i++)
	{
		%o = %this.getObject(%i);
		
		exec(%o.path);
	}
}