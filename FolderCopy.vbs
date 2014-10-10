Option Explicit


Function CopyFolder( strSouceFolder, strDestinationFolder, blnOverWrite)
	Dim ObjFso
	Dim objFolder


	'Use below for debugging
	'WScript.Echo strSouceFolder & vbcrlf & strDestinationFolder & vbcrlf & blnOverWrite

	'Creating the file system object
	Set ObjFso = CreateObject("Scripting.FileSystemObject")

	'Deleting the destination folder
	On Error Resume Next 
	Set objFolder = ObjFso.GetFolder(strDestinationFolder)
	objFolder.Delete

	'Copying the file
	ObjFso.CopyFolder strSouceFolder, strDestinationFolder, blnOverWrite

	if ObjFso.FolderExists(strDestinationFolder) Then
		CopyFolder = True
	Else
		CopyFolder = False
	End If

End Function


Dim intResult1, intResult2


'intResult1 = CopyFolder("C:\Users\jakel\Desktop\OffSitePHP\Project4E\Client", "C:\wamp\www\Client", True)
'intResult2 = CopyFolder("C:\Users\jakel\Desktop\OffSitePHP\Project4E\Server", "C:\wamp\www\Server", True)

intResult1 = CopyFolder("C:\Users\jakel\Desktop\OffSiteCS\MoveTicketBarcodes\MoveTicketBarcodes\bin\Debug", "\\pink\c$\Documents and Settings\administrator\Desktop\Barcode", True)


If (intResult1 = True) And (intResult2 = True) Then
	msgbox("Copied")
End If