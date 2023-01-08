# README #

Endpoints helper for Postman:

1. Login:
* https://localhost:44303/api/Login
* Body:
		{
			"UserName": "admin",
   			"Password": "adminpass",
    			"UserRole": "Admin"
		}
2. CreateCardAsync:
* URL: https://localhost:44303/api/CardManagement/CreateCardAsync
* Body: 
{
   		 "Number": xxxxxxxxxxxxxxx
		}
* Authorization Type: Bearer Token + token
3. PayAsync:
* URL: https://localhost:44303/api/CardManagement/PayAsync
* Body: 
{
    		"CardNumber": xxxxxxxxxxxxxxx,
    		"Amount": xxx
		}
* Authorization Type: Bearer Token + token

4. GetBalanceAsync:
* URL: https://localhost:44303/api/CardManagement/GetBalanceAsync?cardNumber=xxxxxxxxxxxxxxx
* Authorization Type: Bearer Token + token

