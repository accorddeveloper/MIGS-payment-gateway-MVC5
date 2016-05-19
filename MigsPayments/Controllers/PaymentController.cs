// MIGS payment gateway using Asp.Net MVC5
// https://github.com/mwd-au/MIGS-payment-gateway-MVC5
// Based off https://gist.github.com/samnaseri/2211309

using MigsPayments.Helpers;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MigsPayments.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {

            var PaymentStatus = "none";

            try
            {
                string hashSecret = ConfigurationManager.AppSettings["MigsSecureHashSecret"];
                var secureHash = Request.QueryString["vpc_SecureHash"];
                var txnResponseCode = Request.QueryString["vpc_TxnResponseCode"];
                if (!string.IsNullOrEmpty(secureHash))
                {
                    if (!string.IsNullOrEmpty(hashSecret))
                    {
                        var rawHashData = hashSecret + string.Join("", Request.QueryString.AllKeys.Where(k => k != "vpc_SecureHash").Select(k => Request.QueryString[k]));
                        var signature = PaymentHelperMethods.CreateMD5Signature(rawHashData);
                        if (signature != secureHash || txnResponseCode != "0")
                        {
                            PaymentStatus = "invalid";
                            //return View("Error", new ApplicationException("Invalid request."));
                        }
                        else
                        {
                            PaymentStatus = "approved";
                        }
                    }
                }

                ViewBag.PaymentStatus = PaymentStatus;

                var vpcResponse = new PaymentResponse(Request);
                return View(vpcResponse);

            }
            catch (Exception ex)
            {

                var message = "Exception encountered. " + ex.Message;
                return View("Error", ex);

            }

        }




        // POST: Payment/InitiatePayment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InitiatePayment([Bind(Include = "vpc_Amount, vpc_MerchTxnRef, vpc_OrderInfo, vpc_ReturnURL")] string vpc_Amount, string vpc_MerchTxnRef, string vpc_OrderInfo, string vpc_ReturnURL)
        {
            try
            {

                //region parameters
                var VPC_URL = "https://migs.mastercard.com.au/vpcpay";
                var paymentRequest = new PaymentRequest
                {
                    Amount = vpc_Amount,
                    MerchTxnRef = vpc_MerchTxnRef,
                    OrderInfo = vpc_OrderInfo,
                    ReturnUrl = vpc_ReturnURL
                };

                string hashSecret = ConfigurationManager.AppSettings["MigsSecureHashSecret"];
                //endregion


                //region redirect to payment gateway
                var transactionData = paymentRequest.GetParameters().OrderBy(t => t.Key, new VPCStringComparer()).ToList();
                // Add custom data, transactionData.Add(new KeyValuePair<string, string>("Title", title));
                var redirectUrl = VPC_URL + "?" + string.Join("&", transactionData.Select(item => HttpUtility.UrlEncode(item.Key) + "=" + HttpUtility.UrlEncode(item.Value)));
                if (!string.IsNullOrEmpty(hashSecret))
                    redirectUrl += "&vpc_SecureHash=" + PaymentHelperMethods.CreateMD5Signature(hashSecret + string.Join("", transactionData.Select(item => item.Value)));
                return Redirect(redirectUrl);
                //endregion

            }
            catch (Exception ex)
            {
                var message = "Exception encountered. " + ex.Message;
                return View("Error", ex);
            }
        }
    }
}