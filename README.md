Heyns Zumo Client
===================================
#Azure Mobile Services (a.k.a. ZUMO)

Scott Gu says:

"Windows Azure Mobile Services makes it incredibly easy to connect a scalable cloud backend to your client and mobile applications.  
It allows you to easily store structured data in the cloud that can span both devices and users, integrate it with user authentication, as well as send out updates to clients via push notifications."

However at the moment there is only a SDK for Windows 8 applications. This SDK just wraps the REST services exposed by Azure Mobile Services.
This also means that at the moment there is no SDK for .NET 4. So I created this library to fill that gap. It's pre-alpha so use it and change it as you see fit (fork it go mad).

Be kind and send pull requests for any changes you make (if you don't a penguin will die somewhere in the world).

#Take note:
There are some things that might seem perculiar and if you find a better way to implement something please be my guest. 

#How to use it

* Clone the repository or download the source from GitHub. Or just install from [NuGet](https://nuget.org/packages/Heyns.ZumoClient).

##Crud operations

	public class Awesomeness
	{
		var client = new Heyns.ZumoClient.MobileServiceClient("https://[YOURS].azure-mobile.net/", "AzureApiKey");
        var table = client.GetTable<Item>();
		
		// get all 
        var items = table.Get();
		
		// get by id
		var item = table.Get(1);
		
		// insert
		var item = table.Insert(new Item { Text = "Random Text"});
		
		// update
		var item = table.Update(1, new Item { Text = "More Random Text"});
		
		// delete
		table.Delete(1);
	}

##Querying your table
	public class Awesomeness
	{
		var client = new Heyns.ZumoClient.MobileServiceClient("https://[YOURS].azure-mobile.net/", "AzureApiKey");
        var table = client.QueryTable<Item>();
		
		var items = table
						.Filter("text eq 'random'")
						.Top(10)
						.Skip(20)
                        .OrderBy("id")
                        .Select("text");
	}
	
#How to get hold of me
twitter: [@DeonHeyns](https://twitter.com/deonheyns)

website: [deonheyns.com](http://deonheyns.com/contact)

GitHub: [GitHub Profile](https://github.com/deonheyns)

#Contributors
[@MischaBoender](https://github.com/MischaBoender)

#Open source projects used:

RestSharp: https://github.com/restsharp

Json.Net: http://json.codeplex.com/