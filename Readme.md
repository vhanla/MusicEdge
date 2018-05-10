GMusicEdge
------------------------
Author: @vhanla - Victor Alberto Gil 


Description:
-----------
GMusicEdge is another Google Play Music client but only for Windows 10.

It is based on Edge's WebView (edgehtml.dll) and it means, it doesn't require to use Chromium (big bunch of dll files) nor Electron in order to wrap Google Play Music.

As previous work was written using DCEF3 (Delphi Chromium Embedded Framework) this on the contrary doesn't require it, resulting in a very small executable, now written in C#.

![](https://lh3.googleusercontent.com/3zbz7pzsXrD69cSMPf_QGaTHg23Kawk02wz3dMEgkzXMj4ZP-Hd2LHBs5jjmFwbKUbaya3vLdfwYTg=w1366-h768-no)

_This is a proof of concept, using the new WebView from Microsoft Edge instead of Chromium neither IE11 old WebBrowser ActiveX._


Features:
--------

- HTML5 audio 
- Native system notification (WebNotification, activate on settings and refresh)
- Custom Styles using minified CSS files (don't use quote symbols)
- Media Key controls
- Small program
- Portable

Requirements:
------------

- Windows 10 April 2018 Update
- It might work on all Windows with Edge (not tested)

Changelog:
---------

- 2018-05-09:
  Written from scratch 
  
  
No affiliation with Alphabet's Google. Google Play is a trademark of Alphabet Google Inc.  
  
License:
-------

The MIT License (MIT)

Copyright (c) 2011 Victor Alberto Gil

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.