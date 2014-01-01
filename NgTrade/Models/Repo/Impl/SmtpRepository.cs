﻿using System;
using System.Configuration;
using System.Net.Mail;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Models.Repo.Impl
{
    public class SmtpRepository : ISmtpRepository
    {
        private readonly string _host = ConfigurationManager.AppSettings["smtpServer"];
        private readonly int _port = Int32.Parse(ConfigurationManager.AppSettings["smtpPort"]);
        private readonly string _userName = ConfigurationManager.AppSettings["smtpUsername"];
        private readonly string _password = ConfigurationManager.AppSettings["smtpPassword"];
        private readonly string _fromEmail = ConfigurationManager.AppSettings["smtpFromEmail"];

        public void SendContactEmail(ContactViewModel contact)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new System.Net.NetworkCredential(_userName, _password);
                client.EnableSsl = true;
                client.Send(_fromEmail, _fromEmail, contact.Name + " with email " + contact.Email + " contacted Ngtradeonline", contact.Message);
            }
        }

        public void SendReferralEmail(ReferViewModel referViewModel)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new System.Net.NetworkCredential(_userName, _password);
                client.EnableSsl = true;
                var message = "Hello " + referViewModel.Name + "<br /><br />" + referViewModel.ReferralName + " has invited you to join NgTradeOnline. <br /><br />";
                message = message + @"When you sign up for a free account, you get a chance to trade with virtual currency and see how your portfolio performs before you invest your hard earned money in the real stock exchange market. Again it is free to signup.
<br /><br />
Here is how it works:<br />
 1. Signup for a free account at http://www.ngtradeonline.com/Account/Register <br />
 2. Once you have registered, you get 1,000,000 Naira added to you NgTradeOnline account (This is NOT real money, it is virtual currency and do not worth anything. Mainly to be used for your investing education)<br />
 3. You can then start placing your trades, by buying stocks listed on the Nigerian stock exchange. The goal is for you to use the tools provided on ngtradeonline.com, to decide when to buy and ultimately when to sell your stocks and see how your portfolio performs. Do you feel like you can make money in the Nigerian stock market? <br />
<br /><br />
We really hope you take advantage of this free investing education and refer your friends and family to NgTradeOnline.com. If you have any questions, please send email to support@ngtradeonline.com. Our goal is that every Nigerian can take charge of their financial future and learn the ropes to stock trading.<br />
<br />Click the following link to signup for your free account - http://www.ngtradeonline.com/Account/Register
<br /><br />
CEO
<br />
Damilare Oladosu";

                var messageHtml = new MailMessage(_fromEmail, referViewModel.Email, referViewModel.Name + " sent you invitation to Ngtradeonline", message)
                {
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.GetEncoding("utf-8")
                };

                var plainView = AlternateView.CreateAlternateViewFromString
                    (System.Text.RegularExpressions.Regex.Replace(messageHtml.Body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(messageHtml.Body, null, "text/html");

                messageHtml.AlternateViews.Add(plainView);
                messageHtml.AlternateViews.Add(htmlView);
                client.Send(messageHtml);
            }
        }

        public void SendForgotPasswordEmail(string email, string body)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new System.Net.NetworkCredential(_userName, _password);
                client.EnableSsl = true;
                var messageHtml = new MailMessage(_fromEmail, email, "NgTradeOnline Password Reset", body)
                {
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.GetEncoding("utf-8")
                };

                var plainView = AlternateView.CreateAlternateViewFromString
                    (System.Text.RegularExpressions.Regex.Replace(messageHtml.Body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(messageHtml.Body, null, "text/html");

                messageHtml.AlternateViews.Add(plainView);
                messageHtml.AlternateViews.Add(htmlView);
                client.Send(messageHtml);
            }
        }
    }
}