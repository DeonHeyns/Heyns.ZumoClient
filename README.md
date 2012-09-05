Heyns Zumo Client
===================================
##Azure Mobile Services (a.k.a. ZUMO) 

Scott Gu says:

"Windows Azure Mobile Services makes it incredibly easy to connect a scalable cloud backend to your client and mobile applications.  
It allows you to easily store structured data in the cloud that can span both devices and users, integrate it with user authentication, as well as send out updates to clients via push notifications."

However at the moment there is only a SDK for Windows 8 applications. This SDK just wraps the REST services exposed by Azure Mobile Services.
This also means that at the moment there is no SDK for .NET 4. So I created this library to fill that gap. It's pre-alpha so use it and change it as you see fit (fork it go mad). 

Be kind and send pull requests for any changes you make (if you don't a penguin will die somewhere in the world). I will create a NuGet package sooner or later for now enjoy.

##Take note:
There are some things that might seem perculiar and if you find a better way to implement something please be my guest. 

I have found that sending a POST or PATCH and including the ID/Key for the object results in a 500 Internal Error. So for the time being in the tests I have made the Id property Nullable.
That seems to sort the problem as I have set Json.Net to ignore null fields.

##Open source projects used:

RestSharp: https://github.com/restsharp

Json.Net: http://json.codeplex.com/