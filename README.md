# BillingAPI
Created using .Net 6 and Mediatr CQRS

## Swagger
Run the API in Visual Studio or run from terminal and go to https://localhost:7213/swagger/index.html to see the swagger docs

## Entities

### Gateway
Gateways that all payments get paid to. All payments are related to a payment, and all orders are related to a payment

### User
Client user

### Balance
User balance. Can be increased by user. It also acts as log of balance changes, the last added balance ( one with the max id ) is the current balance.

### Order
Orders placed and paid by the user.

### Payment
Payments, can be succesfull or unseccfull when creating an order.

### Account
Account for managing users, gateways, balances, etc.

## Order creation process
When creating an order, if the user has enough balance, a new succesfull payment will be created and an order related to that payment will also be created. Also a new balance log will be created ( which will be the new currect balance ) and the amount in it will be "last current balance amount - new order payment amount".
If the user does not have enough balance, no order or balance will be created, and a new unsuccessfull payment will be created.

## Authorization
Here the authorization is only for managing Gateways, orders, seeing all the data, and etc. It is not for processing orders or adding to user balance, orders can be processed and user balance can be increased without authorization.
(since this is a toy api, user balance can be increased willy nilly. Normally there would be some kind of bank api that would verify user before adding balance or processing orders ).

## Testing
I have only written tests for 2 controller handlers, since the rest of handlers have more or less same testing scenarios, i didn't want to copy paste the same thing over and over since this is a toy api ( there would be more logic to each handler and it would be very critical to test each handler, even if the logic was same ).

