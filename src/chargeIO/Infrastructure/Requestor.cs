﻿using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace ChargeIO
{
	internal static class Requestor
	{
        public static string GetString(string url, string secretKey = null)
		{
            secretKey = secretKey ?? Configuration.GetSecretKey();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";           
            request.Headers.Add("Authorization", GetAuthorizationHeaderValue(secretKey));
            request.UserAgent = GetUserAgent();
            request.Timeout = Configuration.GetHTTPTimeout();
			return ExecuteWebRequest(request);
		}

        public static string PostString(string url, string postData, string secretKey = null)
        {
            return Post(url, postData, "application/x-www-form-urlencoded", secretKey);
        }

        public static string PostJson(string url, string postData, string secretKey = null)
        {
            return Post(url, postData, "application/json", secretKey);
        }

        public static string Post(string url, string postData, string contentType = null, string secretKey = null)
		{
            secretKey = secretKey ?? Configuration.GetSecretKey();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            request.Headers.Add("Authorization", GetAuthorizationHeaderValue(secretKey));
            request.UserAgent = GetUserAgent();
            request.Timeout = Configuration.GetHTTPTimeout();

            request.ContentType = contentType;

            if (postData != null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            return ExecuteWebRequest(request);
		}

        public static string PutString(string url, string putData, string secretKey = null)
        {
            return Put(url, putData, "application/x-www-form-urlencoded", secretKey);
        }

        public static string PutJson(string url, string putData, string secretKey = null)
        {
            return Put(url, putData, "application/json", secretKey);
        }

        public static string Put(string url, string putData, string contentType = null, string secretKey = null)
        {
            secretKey = secretKey ?? Configuration.GetSecretKey();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";

            request.Headers.Add("Authorization", GetAuthorizationHeaderValue(secretKey));
            request.UserAgent = GetUserAgent();
            request.Timeout = Configuration.GetHTTPTimeout();

            request.ContentType = contentType;

            if (putData != null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(putData);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            return ExecuteWebRequest(request);
        }

        public static string Delete(string url, string secretKey = null)
		{
            var wr = GetWebRequest(url, "DELETE", secretKey);

			return ExecuteWebRequest(wr);
		}

        private static WebRequest GetWebRequest(string url, string method, string secretKey = null)
		{
			secretKey = secretKey ?? Configuration.GetSecretKey();

			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = method;

			request.Headers.Add("Authorization", GetAuthorizationHeaderValue(secretKey));
            request.UserAgent = GetUserAgent();
            request.Timeout = Configuration.GetHTTPTimeout();

			return request;
		}

        private static string GetUserAgent()
        {
            return "ChargeIO .net Client (" + Urls.Merchant.GetType().Assembly.GetName().Version.ToString() + ")";
        }

		private static string GetAuthorizationHeaderValue(string secretKey)
		{
			var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:", secretKey)));
			return string.Format("Basic {0}", token);
		}

		private static string ExecuteWebRequest(WebRequest webRequest)
		{
			try
			{
				using (var response = webRequest.GetResponse())
				{
					return ReadStream(response.GetResponseStream());
				}
			}
			catch (WebException webException)
			{
                if (webException.Response != null)
                {
                    try
                    {
                        var statusCode = ((HttpWebResponse)webException.Response).StatusCode;
                        string json_response = ReadStream(webException.Response.GetResponseStream());
                        List<ChargeIOError> errors = Mapper<List<ChargeIOError>>.MapFromJson(json_response, "messages");
                        throw new ChargeIOException(statusCode, errors, errors[0].Message);
                    }
                    catch (ChargeIOException chargeioException)
                    {
                        throw chargeioException;
                    }
                    catch
                    {
                        throw webException;
                    }
                }
                throw webException;
			}
		}

		private static string ReadStream(Stream stream)
		{
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
		}

    }
}