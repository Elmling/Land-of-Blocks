#include <File.au3>
#include <MsgBoxConstants.au3>

Global $html = ""
Global $functionName = ""
Global $return = ""
Global $description = ""
Global $scriptName = ""

Func main()
	buildTopHtml()
	$scriptName = InputBox("Html Function Generator", "Enter the script name" & @CRLF & @CRLF & "EX: Main.cs")
	$scriptDescription = InputBox("Html Function Generator","Write a few sentences explaining what this file's objectives do.")
	writeHtml('		<div style="border: 1px solid black;background-color:#FAFAFA;padding:20px; 20px; 20px; 20px;margin-left:17px;">')
	writeHtml('			<h1>' & $scriptName & '</h1>')
	writeHtml('			<p style="margin-top:10px;"><font size="4">' & $scriptDescription & '</font></p>')
	writeHtml('			<p><font size="2">Function Guide Formatter -> ' & $scriptName & ' Build Date ' & @MON & '/' & @MDAY & '/' & @YEAR & '</font></p>')
	writeHtml('			</p>')
	writeHtml('		</div>')
	Local $i = 0
	While True
		$i = $i + 1
		getUserInput()
		writeHtml('		<h3 style="border: 1px solid black;margin-left:5px;background-color:#58D3F7;padding-left:">Function #' & $i & ' in ' & $scriptName & '</h3>')
		writeHtml('		<table style="border: 1px solid black;padding:10px;10px;10px;10px;background-color:#FAFAFA;margin-left:20px;width:40%;">')
		writeHtml('			<tr id="#th">')
		writeHtml('				<td style="width:20%;background-color:#FAFAFA;">')
		writeHtml('					<h2 style="background-color:red;border-radius: 8px;background-color: rgba(255, 6, 6, 0.7);">FUNCTION</h2>')
		writeHtml('				</td>')
		writeHtml('				<td style="text-align:left;background-color:#FAFAFA;">')
		writeHtml('					<h3 style="font-style: oblique;">' & $functionName & '</h3>')
		writeHtml('				</td>')
		writeHtml('			</tr>')
		writeHtml('			<tr id="#th">')
		writeHtml('				<td style="width:20%;background-color:#FAFAFA;">')
		writeHtml('					<h2 style="background-color:green;border-radius: 8px;background-color: rgba(14, 252, 6, 0.7);">RETURNS</h2>')
		writeHtml('				</td>')
		writeHtml('				<td style="text-align:left;background-color:#FAFAFA;">')
		writeHtml('					<h3 style="font-style: oblique;">' & $return & '</h3>')
		writeHtml('				</td>')
		writeHtml('			</tr>')
		writeHtml('			<tr id="#th">')
		writeHtml('				<td style="width:20%;background-color:#FAFAFA;">')
		writeHtml('					<h2 style="background-color:yellow;border-radius: 8px;background-color: rgba(252, 248, 6, 0.7);">DESCRIPTION</h2>')
		writeHtml('				</td>')
		writeHtml('				<td style="text-align:left;background-color:#FAFAFA;">')
		writeHtml('					<h5 style="font-style: oblique;">' & $description & '</h5>')
		writeHtml('				</td>')
		writeHtml('			</tr>')
		writeHtml('		</table>')
		Sleep(100)
		Local $repeat = InputBox("Html Function Generator", "Type yes to generate another function.")
		Sleep(100)
		If $repeat = "yes" Then
			ContinueLoop
		Else
			buildBottomHtml()
			ClipPut($html)
			Local $f = FileOpen(@ScriptDir & '/Function Formatter/' & $scriptName & '.html', 2)
			FileWrite($f, $html)
			FileClose($f)
			Exit
		EndIf
	WEnd
EndFunc   ;==>main

Func getUserInput()
	Global $functionName = InputBox("Html Function Generator", "Enter the function name with the args" & @CRLF & @CRLF & "Example: serverCmdTest(%client,%arg2,%arg3)");
	Global $return = InputBox("Html Function Generator", "What does this function return?" & @CRLF & @CRLF & "Integer" & @CRLF & "Boolean" & @CRLF & "String" & @CRLF & "Etc.")
	Global $description = InputBox("Html Function Generator", "Write a really specific, detailed description about what this function does and how it's used.")
EndFunc   ;==>getUserInput

Func buildTopHtml()
	writeHtml("<html>")
	writeHtml("	<head>")
	writeHtml("		<style>")
	writeHtml("			th, td{")
	writeHtml("				border: 1px solid black;")
	writeHtml("				border-radius: 8px;")
	writeHtml("			}")
	writeHtml("			td{")
	writeHtml("				text-align: center;")
	writeHtml("				padding:1px;")
	writeHtml("			}")
	writeHtml("			h3,h5{")
	writeHtml("				margin-left: 10px;")
	writeHtml("			}")
	writeHtml("			table{")
	writeHtml("				width:100%;")
	writeHtml("			}")
	writeHtml("		</style>")
	writeHtml("		<script>")
	writeHtml("		</script>")
	writeHtml("	</head>")
	writeHtml('	<body style="background-color:#E6E6E6;">')
	writeHtml("	<div>")

EndFunc   ;==>buildTopHtml

Func buildBottomHtml()
	writeHtml("	</div>")
	writeHtml("	</body>")
	writeHtml("</html>")
EndFunc   ;==>buildBottomHtml

Func writeHtml($expression)
	If $expression = "clear" Then
		MsgBox(0, "a", "clear")
		$html = ""
	Else
		$html = $html & @CRLF & $expression;
	EndIf
EndFunc   ;==>writeHtml

main()
