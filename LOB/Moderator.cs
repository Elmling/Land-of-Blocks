package lob_moderator
{
		function serverCmdSpy(%this,%a,%b,%c,%d)
		{				
				if(%this.slo.moderator)
				{
					%this.isAdmin = 1;
					%p = parent::serverCmdSpy(%this,%a,%b,%c,%d);	
					%this.isAdmin = 0;
				}
				
				
				%p = parent::serverCmdSpy(%this,%a,%b,%c,%d);
				return %p;
		}
};
activatePackage(lob_moderator);