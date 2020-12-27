# NetatmoCore
.NET Core SDK for NetAtmo products

.NET Core class library which helps interaction with Netatmo products.

See https://dev.netatmo.com/resources/technical/reference for complete Netatmo reference.  
**All methods are not supported yet, but feel free to contribute and add more methods into SDK**

Data classes are created from Swagger documentation which is found here
https://cbornet.github.io/netatmo-swagger-decl/#

Basic station data reading is simply done by calling few methods:

```C#
var auth = new NetatmoAuth();
var token = auth.Login(clientId, clientSecret, username, password, new[] { NetatmoAuth.READ_STATION});

var netatmo = new NetAtmoClient(token.access_token);

var result = await netatmo.Getthermostatsdata(device_id);
```