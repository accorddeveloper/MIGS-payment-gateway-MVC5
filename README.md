# MIGS payment gateway for Asp.Net MVC5

An example of Mastercards MIGS payment gateway using Asp.Net MVC5.

Screenshots: https://github.com/mwd-au/MIGS-payment-gateway-MVC5/wiki/Screenshots

## Important Files:

Controllers\PaymentController.cs

Helpers\PaymentHelper.cs

Views\Payment\Index.cshtml

Views\Shared\\_Layout.cshtml

Web.config

## Get Started:

Look for the following code in the web.config file and replace the values with the information given to you by your bank or log into your MIGS portal and find them there. 

```xml
<!-- MIGS SETTINGS -->
<add key="MigsMerchantID" value="" />
<add key="MigsAccessCode" value="" />
<add key="MigsSecureHashSecret" value="" />
```
## Important Notes

1. You must publish this project to a live domain so that Mastercard can return a result to your website after they have attempted to process your payment.
2. All payment amounts are in cents (eg: $5.00 = 500 cents).

## Test Mode

To use MIGS test mode, log into your banks MIGS portal using the word TEST in-front of your Merchant ID. Update your web.config file to use the values found in the test portal.

Live merchant id example: BBL1234567

Test merchant id example: TESTBBL1234567

## Test Card Numbers

You can do transactions against MIGS in test mode using the following test card numbers.

**Mastercard**

card number = 5123456789012346

expiry date = 0517 (MMYY)

csc/cvv = 100


**Visa**

card number = 4987654321098769

expiry date = 0517 (MMYY)

csc/cvv = 100


**To return the appropriately mapped response you must append each purchase amount with a cents amount that correlates as below. The dollar amount is not relevant.**

.00 - Approved (Response Code 0)

.51- Insufficient Funds (Response Code 5)

.57 - Invalid Transaction (Response Code 8)

.54 - Expired Card (Response Code 4)

.91 - Error communicating with Bank (Response Code 6)


**So to test an approved Mastercard you would enter details as below:**

card number = 5123456789012346

expiry date = 0517 (MMYY)

purchase amount = $1.00


**And to test an insufficient funds Mastercard response you would enter details as below:**

card number = 5123456789012346

expiry date = 0517 (MMYY)

purchase amount = $1.51

### Credits:

Based on the payment controller from https://gist.github.com/samnaseri/2211309
