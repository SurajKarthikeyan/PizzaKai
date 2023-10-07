________ XnBugReporter Tools by Jeremy Bond of ExNinja Interactive ________
This is a simple bug report tool that you can add to any project. It will allow your players
to report bugs from within the game that will be posted to a flexible Google Sheet. One of 
the very cool aspects of this is that and field added to the WWWForm class that is posted to
the Sheet will be automatically added as a column in the Sheet itself.

Instructions and the Google Apps Script to make this work in the XnBugReporter.cs file.
You must follow those instructions, set up a Google Sheet, and set the XnBugReporter.sheetsUrl
field on the BugReportPanel for this to work.

You must install TextMeshPro for the text fields to work, and Unity should ask you to do so.

You must install the Naughty Attributes asset to use some of the nice features of this tool. Naughty
Attributes is an Editor extension very similar to Odin Inspector but free. It can be downloaded at:
    https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996

Several scripts are based on other people's work. I've tried to credit them in comments throughout.

If you need to allow iOS players to type text into an input field in WebGL, I highly recommend
the WebGLInput project by keo-yeung: https://github.com/kou-yeung/WebGLInput

Fonts Included:
    Highway Gothic Wide: https://www.dafont.com/highway-gothic.font
    m+ 1m: https://fonts.google.com/specimen/M+PLUS+1 or https://www.fontsquirrel.com/fonts/m-1m