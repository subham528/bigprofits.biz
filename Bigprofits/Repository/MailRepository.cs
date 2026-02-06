using Newtonsoft.Json.Linq;
using System.Text;

namespace Bigprofits.Repository
{
    public class MailRepository
    {
        //[HttpPost]
        //public async Task<IActionResult> Mailcontent(InfoMember model)
        //{
        //    if (homeRepository.IsCookieExpired(HttpContext))
        //    {
        //        return RedirectToAction("PYCAdminLogin", "Admin");
        //    }

        //    if (model.MailContent != null && model.MailSubject != null)
        //    {
        //        var data = await context.InfoMembers.ToListAsync();

        //        if (data.Count > 0)
        //        {
        //            for (int i = 0; i < data.Count; i++)
        //            {
        //                string Mailid = data[i].Email;
        //                string msg = model.MailContent;
        //                string sub = model.MailSubject;

        //                string get = SendMail(Mailid, msg, sub);

        //                //data[i].IsStatus = Convert.ToInt32(get);
        //                //context.Update(data);
        //                //await context.SaveChangesAsync();
        //            }
        //        }
        //    }
        //    TempData["suck"] = "Mail has been send to all Members..!!";
        //    return RedirectToAction("Mailcontent");
        //}


        //public string MailSend0(string mailid, string strmsg, string subject)
        //{
        //    string text = "0";  // test email = jakirak126@pelung.com
        //    try
        //    {
        //        SmtpClient smtpClient = new SmtpClient();
        //        smtpClient.Host = "smtp-relay.sendinblue.com";
        //        smtpClient.Port = 587;
        //        //smtpClient.UseDefaultCredentials = false;
        //        //smtpClient.EnableSsl = true;
        //        smtpClient.Credentials = new NetworkCredential("jakirak126@pelung.com", "1dEKAVpOTkXBZb7h");
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.From = new MailAddress("appmobipe@gmail.com", "appmobipe@gmail.com");
        //        mailMessage.To.Add(new MailAddress(mailid));
        //        string content = strmsg.ToString();
        //        AlternateView item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
        //        mailMessage.AlternateViews.Add(item);
        //        mailMessage.Subject = subject;
        //        mailMessage.Body = "Mailtesting";
        //        smtpClient.Send(mailMessage);
        //        return "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public string MailSend1(string mailid, string strmsg, string subject)
        //{
        //    string text = "0";
        //    try
        //    {
        //        SmtpClient smtpClient = new SmtpClient();
        //        smtpClient.Host = "smtp-relay.sendinblue.com";
        //        smtpClient.Port = 587;
        //        //smtpClient.UseDefaultCredentials = false;
        //        //smtpClient.EnableSsl = true;
        //        smtpClient.Credentials = new NetworkCredential("jakirak126@pelung.com", "1dEKAVpOTkXBZb7h");
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.From = new MailAddress("jakirak126@pelung.com", "jakirak126@pelung.com");
        //        mailMessage.To.Add(new MailAddress(mailid));
        //        string content = strmsg.ToString();
        //        AlternateView item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
        //        mailMessage.AlternateViews.Add(item);
        //        mailMessage.Subject = subject;
        //        mailMessage.Body = "Rishabh";
        //        smtpClient.Send(mailMessage);
        //        return "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public string MailSend2(string Mailid, string Msg, string Subject)
        //{
        //    string text = "0";
        //    try
        //    {
        //        SmtpClient smtp = new SmtpClient("81.17.57.241", 25);
        //        smtp.Credentials = new NetworkCredential("supportTeam@bcsworld.io", "GoPu@@2022#@");
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        MailMessage email = new MailMessage();
        //        email.From = new MailAddress("supportTeam@bcsworld.io", "bscworld.com");
        //        email.To.Add(new MailAddress(Mailid));
        //        string content = Msg.ToString();
        //        AlternateView item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
        //        email.AlternateViews.Add(item);
        //        email.Subject = Subject;
        //        email.Body = "bscworld";
        //        smtp.Send(email);
        //        return "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public string SendMail3(string mailid, string strmsg, string subject)
        //{
        //    string text = "0";
        //    try
        //    {
        //        SmtpClient smtpClient = new SmtpClient("212.95.50.79", 25);
        //        smtpClient.Credentials = new NetworkCredential("teamtech@eagleearn.world", "FiLr@@2022");
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.From = new MailAddress("teamtech@eagleearn.world", "eagleearn.world");
        //        mailMessage.To.Add(new MailAddress(mailid));
        //        string content = strmsg.ToString();
        //        AlternateView item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
        //        mailMessage.AlternateViews.Add(item);
        //        mailMessage.Subject = subject;
        //        mailMessage.Body = "Eagle Earn";
        //        smtpClient.Send(mailMessage);
        //        return "1";
        //    }
        //    catch (Exception)
        //    {
        //        return "Exception message, please contact to admin";
        //    }
        //}

        public async Task<string> SendMail(string mailid, string strmsg, string subject)
        {
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //var baseAddress = "https://api.zeptomail.in/v1.1/email";

                //var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                //http.Accept = "application/json";
                //http.ContentType = "application/json";
                //http.Method = "POST";
                //http.PreAuthenticate = true;
                //http.Headers.Add("Authorization", "Zoho-enczapikey PHtE6r1bQe29jWUnpEQE56WwEMOnPYh89b9vLQQSsIlGWKcGSk0Dr9h4kT+0qh17VPZGQPKeyY5v5LOVsOqHdGrlMm1OVWqyqK3sx/VYSPOZsbq6x00bslQbcUTcVIDvd95p3SPTuNmX");
                //JObject parsedContent = JObject.Parse("{'from': { 'address': 'noreply@coinpaywallet.io'},'to': [{'email_address': {'address': '" + mailid + "','name': 'Coinpaywallet'}}],'subject':'" + subject + "','htmlbody':'<div>" + strmsg + "</div>'}");
                //Console.WriteLine(parsedContent.ToString());
                //ASCIIEncoding encoding = new ASCIIEncoding();
                //Byte[] bytes = encoding.GetBytes(parsedContent.ToString());

                //Stream newStream = http.GetRequestStream();
                //newStream.Write(bytes, 0, bytes.Length);
                //newStream.Close();

                //var response = http.GetResponse();

                //var stream = response.GetResponseStream();
                //var sr = new StreamReader(stream);
                //var content = sr.ReadToEnd();
                //Console.WriteLine(content);

                var url = "https://api.zeptomail.in/v1.1/email";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Zoho-enczapikey PHtE6r1bQe29jWUnpEQE56WwEMOnPYh89b9vLQQSsIlGWKcGSk0Dr9h4kT+0qh17VPZGQPKeyY5v5LOVsOqHdGrlMm1OVWqyqK3sx/VYSPOZsbq6x00bslQbcUTcVIDvd95p3SPTuNmX");
                var payload = new JObject
                {
                    ["from"] = new JObject
                    {
                        ["address"] = "noreply@coinpaywallet.io"
                    },
                    ["to"] = new JArray
                    {
                        new JObject
                        {
                            ["email_address"] = new JObject
                            {
                                ["address"] = mailid,
                                ["name"] = "Coinpaywallet"
                            }
                        }
                    },
                    ["subject"] = subject,
                    ["htmlbody"] = $"<div>{strmsg}</div>"
                };

                var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                using var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());

                return "1";
            }
            catch (Exception)
            {
                return "Exception message, please contact to admin";
            }
        }





    }
}
