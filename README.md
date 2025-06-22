# FluentNewsApp
## Authorization
This application uses [News API](https://newsapi.org) to get news data. The application won't work unless the user's News API api key is set in `app.config`:
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="userApiKey" value="[api key here]"/>
	</appSettings>
</configuration>
```

## Simulating network behavior
To simulate latency and errors, uncomment lines 29-34 from `NewsApiClient.cs`
