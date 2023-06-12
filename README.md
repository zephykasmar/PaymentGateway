

## Payment Gateway API

### Overview

This project implements a simple Payment Gateway API, providing merchants with the ability to process payments and retrieve details of previously made payments. The system is designed with simplicity, reliability, and scalability in mind, and is built to run on AWS EKS with Kubernetes.

### Environment

The project is currently deployed on an AWS EKS cluster and can be tested at: 
`k8s-default-ingress-02c0809f77-890764471.us-west-2.elb.amazonaws.com`

By default, both the hosted and local systems are seeded with one merchant for testing purposes. The ID of this merchant is `'2134a701-3e9c-4c54-993f-ff2c3b41797f'`.

For local testing, navigate to the root directory and run the following commands:

```bash
docker-compose build
docker-compose up
```
This will run the three services in docker, with the endpoints:
```
localhost:3030/process
localhost:3031/transactions
localhost:3032/payments
```

### API Endpoints
---
### Payment Processing


```bash
curl -X POST -H "Content-Type: application/json" -d "{\"merchantId\": \"2134a701-3e9c-4c54-993f-ff2c3b41797f\",\"pan\": \"5123-4567-8901-2346\",\"expiry\": \"12/25\",\"cvv\": \"123\",\"amount\": 59.99,\"currencyCode\": \"USD\",\"cardholdersName\": \"test name\"}" http://localhost:3030/process
```
*host - [localhost:3030 | k8s-default-ingress-02c0809f77-890764471.us-west-2.elb.amazonaws.com]*
#### `POST /process`
Accepts a JSON object for a payment attempt.

*Request:*

```json
{
	"merchantId": "2134a701-3e9c-4c54-993f-ff2c3b41797f",
	"pan": "5123-4567-8901-2346",
	"expiry": "12/25",
	"cvv": "123",
	"amount": 59.99,
	"currencyCode": "USD",
	"cardholdersName": "test name"
}
```

On success, it returns a similar JSON object with the PAN masked (\*\*\*\*-\*\*\*\*-\*\*\*\*-1234) and the CVV removed.
*Response:*

```json
{
	"transactionId": "1ec9a78f-0b0c-4098-a5c2-e49db2f96e07",
	"merchantId": "2134a701-3e9c-4c54-993f-ff2c3b41797f",
	"cardholdersName": "test name",
	"pan": "****-****-****-2346",
	"expiry": "12/25",
	"cvv": null,
	"amount": 59.99,
	"currencyCode": "USD"
}
```
---
### Payment Retrieval
```bash
curl -X GET http://localhost:3032/payments/1ec9a78f-0b0c-4098-a5c2-e49db2f96e07
```
*host - [localhost:3032 | k8s-default-ingress-02c0809f77-890764471.us-west-2.elb.amazonaws.com]*
#### `GET /payments/{id}`


Retrieves details of a previously made payment. The `{id}` should be replaced with the payment ID.
Example response:
```json
{
	"transactionId": "1ec9a78f-0b0c-4098-a5c2-e49db2f96e07",
	"merchantId": "2134a701-3e9c-4c54-993f-ff2c3b41797f",
	"cardholdersName": "test name",
	"pan": "****-****-****-2346",
	"expiry": "12/25",
	"cvv": null,
	"amount": 59.99,
	"currencyCode": "USD"
}
```

## High-Level API Design

![High level design](https://github.com/zephykasmar/PaymentGateway/blob/main/PaymentGateway/Images/hld.PNG)

## Sequence Diagrams
### Payment Processing
*Sequence diagram for the '/process' endpoint, detailing a typical request/response lifecycle*
![Payment Processing](https://github.com/zephykasmar/PaymentGateway/blob/main/PaymentGateway/Images/process.PNG)
### Payment Retrieval
*Sequence diagram for the '/payments/{id}' endpoint, detailing a typical request/response lifecycle*
![Payment Retrieval](https://github.com/zephykasmar/PaymentGateway/blob/main/PaymentGateway/Images/payments.PNG)


## Assumptions Made / Areas for Improvement / Additional Points

### 

Due to time constraints and a long absence from .NET API development, code/structure/pattern usage may fluctuate in quality. 

Here are some points that I would have liked to include, or improve upon.

-   Pre-emptive scaling based on heuristic (time of day, known future events (Black Friday), past data etc.).
-   Merchant ID validation to be moved to middleware to diminish IDOR attack space, and be used as an API key in the header.
-   Rate limiting and security measures to prevent DDoS attacks from malicious merchants, or actors via merchants.
-   Validation and testing are currently subpar; I would have liked to implement some in-memory database testing.
-  Acquiring Bank implementation is currently empty and always returns a successful response, as I believed it beyond the scope of this task.
- Support for requests failing mid-process.
- Adding logging, tracing and metrics.
- Improved compliancy to standards regarding storing and processing secure information.
