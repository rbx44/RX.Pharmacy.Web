# RX.Pharmacy.Web
Pharmacy Api allows user to find closest pharmacy from given gps coordinates.


## Site Url
* http://pharmt01wfe.azurewebsites.net/swagger/index.html

## How to?
* Get the JWT token see below from user login api (site url + api endpoint) **Ask for valid user and password**.
* Pass the JWT token as Authorization header to pharmacy api and provide coordinates as `application/json` content type to find the closest pharmacy store (stored in DB) from the provided coordinates.


## Implementation Details  
* Uses AWS Cognito for authenticating username and password on the login request.
* Uses Redis in Azure for caching
* Uses Azure App Service
* Uses Azure Keyvault secrets for app settings 
* Uses Azure SQL Server
* Response is cached for 2 hours for future request(s) provided same gps coordinates
* Returns closest pharmacy name, address and distance in miles


## Api Endpoint
| Endpoint  | Status | Response | Type | Verb | Request Payload
| ------------- | ------------- | ------------- |------------- | ------------- | ------------- |
| api/v1/user/login  | | | REST | POST | $Ref: `UserInputModel`
| | Success 200 | string JWT Id token | | |
| | Failure 400  | Error Message for Bad Request | | |
| | Failure 401  | Error Message for invalid credentials passed | | |

## Request Payload

### UserInputModel

  | Name | Type | Description
  | ------------- | ------------- | ------------- |
  | Username  | String | valid AWS Cognito username |
  | Password  | String  | valid AWS Cognito user's password |


## Api Endpoint
| Endpoint  | Status | Response | Type | Verb | Request Payload | Request Header
| ------------- | ------------- | ------------- |------------- | ------------- | ------------- | ------------- |
| api/v1/pharmacy  | | | REST | POST | $Ref: `PharmacyInputModel` | Key - `Authorization` Value - `Bearer {JWT id token}`
| | Success 200 | $Ref: `PharmacyModel` cloest pharmacy response payload | | | |
| | Failure 400  | Error Message for Bad Request | | | |
| | Failure 401  | Error Message for invalid authorization header | | | |

## Request Payload

### PharmacyInputModel

  | Name | Type | Description
  | ------------- | ------------- | ------------- |
  | Latitude  | double | latitude decimal value of the provided gps coordinate   |
  | Longitude  | double  | longitude decimal value of the provided gps coordinate |
  
  
## Response Payload

### PharmacyModel
  
  | Name | Type | Description
  | ------------- | ------------- | ------------- |
  | Name  | String | Name of the closest pharmacy store from the provided gps coordinates |
  | Address  | String  | Address of the closest pharmacy store from the provided gps coordinates |
  | Distance  | decimal  | Distance (in miles) from the provided gps coordinates to the closest pharmacy in db  |
  
