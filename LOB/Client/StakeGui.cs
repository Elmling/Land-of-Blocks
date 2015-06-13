if(isObject(StakeGui))
	StakeGui.delete();

//--- OBJECT WRITE BEGIN ---
new GuiControl(StakeGui) {
   profile = "GuiDefaultProfile";
   horizSizing = "right";
   vertSizing = "bottom";
   position = "0 0";
   extent = "640 480";
   minExtent = "8 2";
   enabled = "1";
   visible = "1";
   clipToParent = "1";

   new GuiWindowCtrl() {
      profile = "GuiWindowProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "599 97";
      extent = "432 283";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      command = "commandtoserver('senddeclinestake');";
      accelerator = "escape";
      maxLength = "255";
      resizeWidth = "1";
      resizeHeight = "1";
      canMove = "1";
      canClose = "1";
      canMinimize = "1";
      canMaximize = "1";
      minSize = "50 50";
	  closeCommand="commandtoserver('senddeclinestake');";

      new GuiScrollCtrl() {
         profile = "GuiScrollProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "15 34";
         extent = "161 187";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         willFirstRespond = "0";
         hScrollBar = "alwaysOn";
         vScrollBar = "alwaysOn";
         constantThumbHeight = "0";
         childMargin = "0 0";
         rowHeight = "40";
         columnWidth = "30";

         new GuiTextListCtrl() {
            profile = "GuiTextListProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "1 1";
            extent = "144 2";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            enumerate = "0";
            resizeCell = "1";
            columns = "0";
            fitParentWidth = "1";
            clipColumnText = "0";
         };
      };
      new GuiScrollCtrl() {
         profile = "GuiScrollProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "251 34";
         extent = "156 187";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         willFirstRespond = "0";
         hScrollBar = "alwaysOn";
         vScrollBar = "alwaysOn";
         constantThumbHeight = "0";
         childMargin = "0 0";
         rowHeight = "40";
         columnWidth = "30";

         new GuiTextListCtrl() {
            profile = "GuiTextListProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "1 1";
            extent = "139 2";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            enumerate = "0";
            resizeCell = "1";
            columns = "0";
            fitParentWidth = "1";
            clipColumnText = "0";
         };
      };
      new GuiButtonCtrl() {
         profile = "GuiButtonProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "250 229";
         extent = "75 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         text = "Accept";
         groupNum = "-1";
         buttonType = "PushButton";
		 command="commandtoserver('sendAcceptStake');";
      };
      new GuiButtonCtrl() {
         profile = "GuiButtonProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "336 229";
         extent = "72 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         command = "commandtoserver(\'senddeclinestake\');";
         text = "Decline";
         groupNum = "-1";
         buttonType = "PushButton";
      };
      new GuiBitmapCtrl(MeleeButton) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "196 35";
         extent = "30 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "./melee.png";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
            activated = "1";

         new GuiMouseEventCtrl(ButtonMouseCtrl) {
            profile = "GuiDefaultProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "30 30";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            lockMouse = "0";
               bodyCtrl = "MeleeButton";
         };
      };
      new GuiBitmapCtrl(RangeButton) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "196 75";
         extent = "30 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "./range.png";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
            activated = "1";

         new GuiMouseEventCtrl(ButtonMouseCtrl) {
            profile = "GuiDefaultProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "30 30";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            lockMouse = "0";
               bodyCtrl = "RangeButton";
         };
      };
      new GuiBitmapCtrl(MagicButton) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "196 115";
         extent = "30 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "./magic.png";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
            activated = "1";

         new GuiMouseEventCtrl(ButtonMouseCtrl) {
            profile = "GuiDefaultProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "30 30";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            lockMouse = "0";
               bodyCtrl = "MagicButton";
         };
      };
      new GuiBitmapCtrl(FoodButton) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "196 155";
         extent = "30 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "./food.png";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
		 activated = "1";

         new GuiMouseEventCtrl(ButtonMouseCtrl) {
            profile = "GuiDefaultProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "30 30";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            lockMouse = "0";
               bodyCtrl = "FoodButton";
         };
      };
      new GuiBitmapCtrl(DashButton) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "196 195";
         extent = "30 30";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "./dash.png";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
            activated = "1";

         new GuiMouseEventCtrl(ButtonMouseCtrl) {
            profile = "GuiDefaultProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "30 30";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            lockMouse = "0";
               bodyCtrl = "DashButton";
         };
      };
      new GuiTextCtrl(StakeStatus) {
         profile = "GuiTextProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "251 260";
         extent = "22 18";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         maxLength = "255";
      };
   };
};
//--- OBJECT WRITE END ---


function clientCmdCloseStakeGui()
{
	canvas.popdialog(StakeGui);
}

function clientCmdUpdateStakeGui(%serverData)
{
	echo("Update entered");
	%type = getField(%serverData, 0);
	
	if(%type $= "rules")
	{
		%obj = getField(%serverData, 1);
		%val = getField(%serverData, 2);
		%name2 = getSubStr(%obj, 0, strstr(%obj, "Button"));
		
		%obj.activated = %val;
		%obj.setBitmap(%obj.activated ? "base/landofblocks/stakegui/"@ %name2 @ ".png" : "base/landofblocks/stakegui/no" @ %name2 @ ".png");
	}
	else if(%type $= "item")
	{
		
	}
}

function clientCmdUpdateStakeStatus(%text)
{
	StakeStatus.setText(%text);
}

function clientCmdOpenStakeGui(%title)
{
	StakeGui.getobject(0).setText(%title);
	canvas.pushdialog(StakeGui);
}

function ButtonMouseCtrl::onMouseDown(%this, %mod, %pos, %click)
{
	parent::onMouseDown(%this, %mod, %pos, %click);
	
	commandToServer('stakeGuiSendData', "rules" TAB %this.bodyctrl.getName() TAB %this.bodyctrl.activated);
}
