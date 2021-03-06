﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silverpop.Client.Extensions;
using Silverpop.Core;
using Silverpop.Core.XML;

namespace Silverpop.Client
{
    public class TransactClient : IDisposable
    {
        public static readonly string ErrorMissingPodNumber =
            "A valid PodNumber must be provided.";

        public static readonly string ErrorExceededNonBatchRecipients = string.Format(
            "Number of recipients exceeds the max of {0} recipients permitted. " +
            "Use SendMessageBatch or SendMessageBatchAsync instead.",
            TransactClientConfiguration.MaxRecipientsPerNonBatchRequest);

        private readonly TransactMessageEncoder _encoder;
        private readonly TransactMessageResponseDecoder _decoder;
        private readonly AddRecipientEncoder _addRecipientEncoder;
        private readonly AddRecipientResponseDecoder _addRecipientDecoder;
        private readonly SelectRecipientDataEncoder _selectRecipientEncoder;
        private readonly SelectRecipientDataResponseDecoder _selectRecipientDecoder;
        private readonly LoginEncoder _loginEncoder;
        private readonly LoginResponseDecoder _loginDecoder;
        private readonly SendMailingEncoder _sendMailingEncoder;
        private readonly SendMailingResponseDecoder _sendMailingDecoder;
        private readonly Func<ISilverpopCommunicationsClient> _silverpopFactory;

        public TransactClient(TransactClientConfiguration configuration)
            : this(
                configuration,
                new TransactMessageEncoder(),
                new TransactMessageResponseDecoder(),
                new AddRecipientEncoder(),
                new AddRecipientResponseDecoder(),
                new SelectRecipientDataEncoder(),
                new SelectRecipientDataResponseDecoder(),
                new LoginEncoder(),
                new LoginResponseDecoder(),
                new SendMailingEncoder(),
                new SendMailingResponseDecoder(),
                () => new SilverpopCommunicationsClient(configuration))
        {
        }

        public TransactClient(
            TransactClientConfiguration configuration,
            TransactMessageEncoder encoder,
            TransactMessageResponseDecoder decoder,
            AddRecipientEncoder addRecipientEncoder,
            AddRecipientResponseDecoder addRecipientDecoder,
            SelectRecipientDataEncoder selectRecipientEncoder,
            SelectRecipientDataResponseDecoder selectRecipientDecoder,
            LoginEncoder loginEncoder,
            LoginResponseDecoder loginDecoder,
            SendMailingEncoder sendMailingEncoder,
            SendMailingResponseDecoder sendMailingDecoder,
            Func<ISilverpopCommunicationsClient> silverpopFactory)
        {
            Configuration = configuration;
            _encoder = encoder;
            _decoder = decoder;
            _addRecipientEncoder = addRecipientEncoder;
            _addRecipientDecoder = addRecipientDecoder;
            _selectRecipientEncoder = selectRecipientEncoder;
            _selectRecipientDecoder = selectRecipientDecoder;
            _loginEncoder = loginEncoder;
            _loginDecoder = loginDecoder;
            _sendMailingEncoder = sendMailingEncoder;
            _sendMailingDecoder = sendMailingDecoder;
            _silverpopFactory = silverpopFactory;
        }

        public void Dispose()
        {
            var silverpop = _silverpopFactory();
            silverpop.Dispose();
        }

        public TransactClientConfiguration Configuration { get; private set; }

        public string XMLAPISession { get; set; }

        private void CheckLogin()
        {
            if (XMLAPISession == null)
            {
                var login = new Login() { Username = Configuration.Username, Password = Configuration.Password };

                var encodedRecipient = _loginEncoder.Encode(login);

                string response;
                using (var silverpop = _silverpopFactory())
                {
                    response = silverpop.HttpUpload(encodedRecipient, true, true);
                }

                var decodedResponse = _loginDecoder.Decode(response);

                XMLAPISession = decodedResponse.Success ? decodedResponse.SessionEncoding : null;
            }
        }

        public virtual SendMailingResponse SendMailing(SendMailing mailing)
        {
            if (mailing == null) throw new ArgumentNullException("mailing");

            var encodedMessage = _sendMailingEncoder.Encode(mailing);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = silverpop.HttpUpload(encodedMessage, true, true, XMLAPISession);
            }

            var decodedResponse = _sendMailingDecoder.Decode(response, encodedMessage);

            if (!decodedResponse.Success)
            {
                var errorMessage = decodedResponse.ErrorString;
                throw new TransactClientException(errorMessage, encodedMessage, decodedResponse.RawResponse);
            }

            return decodedResponse;
        }

        public virtual async Task<SendMailingResponse> SendMailingAsync(SendMailing mailing)
        {
            if (mailing == null) throw new ArgumentNullException("mailing");

            var encodedMessage = _sendMailingEncoder.Encode(mailing);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = await silverpop.HttpUploadAsync(encodedMessage, true, true, XMLAPISession);
            }

            var decodedResponse = _sendMailingDecoder.Decode(response, encodedMessage);

            if (decodedResponse.Success)
            {
                var errorMessage = decodedResponse.ErrorString;
                throw new TransactClientException(
                    errorMessage, encodedMessage, decodedResponse.RawResponse);
            }

            return decodedResponse;
        }

        public virtual AddRecipientResponse AddRecipient(AddRecipient contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");

            var encodedRecipient = _addRecipientEncoder.Encode(contact);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = silverpop.HttpUpload(encodedRecipient, true, true, XMLAPISession);
            }

            var decodedResponse = _addRecipientDecoder.Decode(response);

            return decodedResponse;
        }

        public virtual async Task<AddRecipientResponse> addRecipientAsync(AddRecipient contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");

            var encodedRecipient = _addRecipientEncoder.Encode(contact);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = await silverpop.HttpUploadAsync(encodedRecipient, true, true, XMLAPISession).ConfigureAwait(false);
            }

            var decodedResponse = _addRecipientDecoder.Decode(response);

            return decodedResponse;
        }

        public virtual SelectRecipientDataResponse SelectRecipientData(SelectRecipientData contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");

            var encodedRecipient = _selectRecipientEncoder.Encode(contact);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = silverpop.HttpUpload(encodedRecipient, true, true, XMLAPISession);
            }

            var decodedResponse = _selectRecipientDecoder.Decode(response);

            return decodedResponse;
        }

        public virtual async Task<SelectRecipientDataResponse> SelectRecipientDataAsync(SelectRecipientData contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");

            var encodedRecipient = _selectRecipientEncoder.Encode(contact);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = await silverpop.HttpUploadAsync(encodedRecipient, true, true, XMLAPISession).ConfigureAwait(false);
            }

            var decodedResponse = _selectRecipientDecoder.Decode(response);

            return decodedResponse;
        }

        public virtual TransactMessageResponse SendMessage(TransactMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            SendMessagePreCommunicationVerification(message);

            var encodedMessage = _encoder.Encode(message);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = silverpop.HttpUpload(encodedMessage);
            }

            var decodedResponse = _decoder.Decode(response, encodedMessage);

            if (decodedResponse.Status == TransactMessageResponseStatus.EncounteredErrorsNoMessagesSent)
            {
                var errorMessage = decodedResponse.Error.Value;

                if (string.IsNullOrWhiteSpace(errorMessage) && decodedResponse.RecipientDetails.Any())
                {
                    errorMessage = string.Join(
                        "; ",
                        decodedResponse.RecipientDetails
                            .Where(x => x.SendStatus == TransactMessageResponseRecipientSendStatus.ErrorEncounteredWillNotRetry)
                            .Select(x => $"{x.Email}: {(string.IsNullOrWhiteSpace(x.Error.Value) ? "Unknown error" : x.Error.Value)}"));
                }

                throw new TransactClientException(
                    errorMessage, encodedMessage, decodedResponse.RawResponse);
            }

            return decodedResponse;
        }

        public virtual async Task<TransactMessageResponse> SendMessageAsync(TransactMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            SendMessagePreCommunicationVerification(message);

            var encodedMessage = _encoder.Encode(message);

            string response;
            using (var silverpop = _silverpopFactory())
            {
                response = await silverpop.HttpUploadAsync(encodedMessage).ConfigureAwait(false);
            }

            var decodedResponse = _decoder.Decode(response, encodedMessage);

            if (decodedResponse.Status == TransactMessageResponseStatus.EncounteredErrorsNoMessagesSent)
            {
                var errorMessage = decodedResponse.Error.Value;

                if (string.IsNullOrWhiteSpace(errorMessage) && decodedResponse.RecipientDetails.Any())
                {
                    errorMessage = string.Join(
                        "; ",
                        decodedResponse.RecipientDetails
                            .Where(x => x.SendStatus == TransactMessageResponseRecipientSendStatus.ErrorEncounteredWillNotRetry)
                            .Select(x => $"{x.Email}: {(string.IsNullOrWhiteSpace(x.Error.Value) ? "Unknown error" : x.Error.Value)}"));
                }

                throw new TransactClientException(
                    errorMessage, encodedMessage, decodedResponse.RawResponse);
            }

            return decodedResponse;
        }

        /// <returns>Filenames that can be used for checking status of batches.</returns>
        public virtual IEnumerable<string> SendMessageBatch(TransactMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            MessageBatchPreCommunicationVerification();

            var filenames = new List<string>();

            using (var silverpop = _silverpopFactory())
            {
                var batchedMessages = message.SelectRecipientDataBatchedMessages(
                    TransactClientConfiguration.MaxRecipientsPerBatchRequest);

                var identifier = Guid.NewGuid().ToString();

                foreach (var batchMessage in batchedMessages)
                {
                    var encodedMessage = _encoder.Encode(batchMessage);

                    var filename = string.Format(
                        "{0}.{1}.xml.gz",
                        identifier,
                        filenames.Count() + 1);

                    silverpop.SftpCreateDirectoryIfNotExists("transact/temp");

                    silverpop.SftpGzipUpload(
                        encodedMessage,
                        "transact/temp/" + filename);

                    silverpop.SftpMove(
                        "transact/temp/" + filename,
                        "transact/inbound/" + filename);

                    filenames.Add(filename + ".status");
                }
            }

            return filenames;
        }

        /// <returns>Filenames that can be used for checking status of batches.</returns>
        public virtual async Task<IEnumerable<string>> SendMessageBatchAsync(TransactMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            MessageBatchPreCommunicationVerification();

            var filenames = new List<string>();

            using (var silverpop = _silverpopFactory())
            {
                var batchedMessages = message.SelectRecipientDataBatchedMessages(
                    TransactClientConfiguration.MaxRecipientsPerBatchRequest);

                var identifier = Guid.NewGuid().ToString();

                foreach (var batchMessage in batchedMessages)
                {
                    var encodedMessage = _encoder.Encode(batchMessage);

                    var filename = string.Format(
                        "{0}.{1}.xml.gz",
                        identifier,
                        filenames.Count() + 1);

                    silverpop.SftpCreateDirectoryIfNotExists("transact/temp");

                    await silverpop.SftpGzipUploadAsync(
                        encodedMessage,
                        "transact/temp/" + filename).ConfigureAwait(false);

                    await silverpop.SftpMoveAsync(
                        "transact/temp/" + filename,
                        "transact/inbound/" + filename).ConfigureAwait(false);

                    filenames.Add(filename + ".status");
                }
            }

            return filenames;
        }

        public virtual TransactMessageResponse GetStatusOfMessageBatch(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            MessageBatchPreCommunicationVerification();

            var filePath = "transact/status/" + filename;

            Stream stream;
            using (var silverpop = _silverpopFactory())
            {
                stream = silverpop.SftpDownload(filePath);
            }

            if (stream == null)
                throw new TransactClientException(
                    string.Format("Requested file {0} does not currently exist.", filePath),
                    filePath);

            var decodedResponse = _decoder.Decode(stream.ToContentString(Encoding.UTF8));

            return decodedResponse;
        }

        public virtual async Task<TransactMessageResponse> GetStatusOfMessageBatchAsync(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            MessageBatchPreCommunicationVerification();

            var filePath = "transact/status/" + filename;

            Stream stream;
            using (var silverpop = _silverpopFactory())
            {
                stream = await silverpop.SftpDownloadAsync(filePath).ConfigureAwait(false);
            }

            if (stream == null)
                throw new TransactClientException(
                    string.Format("Requested file {0} does not currently exist.", filePath),
                    filePath);

            var decodedResponse = _decoder.Decode(stream.ToContentString(Encoding.UTF8));

            return decodedResponse;
        }

        private void SendMessagePreCommunicationVerification(TransactMessage message)
        {
            if (message.Recipients.Count() > TransactClientConfiguration.MaxRecipientsPerNonBatchRequest)
                throw new ArgumentException(ErrorExceededNonBatchRecipients);

            if (!Configuration.PodNumber.HasValue)
                throw new ApplicationException(ErrorMissingPodNumber);
        }

        private void MessageBatchPreCommunicationVerification()
        {
            if (!Configuration.PodNumber.HasValue)
                throw new ApplicationException(ErrorMissingPodNumber);
        }

        /// <summary>
        /// Create the TransactClient with standard authentication only.
        /// Note: This does not enable OAuth scenarios.
        /// OAuth is typically used when an application is hosted
        /// somewhere with a non-static IP address (Azure Websites, etc.),
        /// or when you don't want to specify IP address(es) with Silverpop.
        /// </summary>
        public static TransactClient Create(int podNumber, string username, string password)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");

            return new TransactClient(new TransactClientConfiguration()
            {
                PodNumber = podNumber,
                Username = username,
                Password = password
            });
        }

        /// <summary>
        /// Create the TransactClient enabling all features.
        /// OAuth will be used for non-batch message scenarios.
        /// </summary>
        public static TransactClient CreateIncludingOAuth(
            int podNumber,
            string username,
            string password,
            string oAuthClientId,
            string oAuthClientSecret,
            string oAuthRefreshToken)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (oAuthClientId == null) throw new ArgumentNullException("oAuthClientId");
            if (oAuthClientSecret == null) throw new ArgumentNullException("oAuthClientSecret");
            if (oAuthRefreshToken == null) throw new ArgumentNullException("oAuthRefreshToken");

            return new TransactClient(new TransactClientConfiguration()
            {
                PodNumber = podNumber,
                Username = username,
                Password = password,
                OAuthClientId = oAuthClientId,
                OAuthClientSecret = oAuthClientSecret,
                OAuthRefreshToken = oAuthRefreshToken
            });
        }

        /// <summary>
        /// Create the TransactClient with OAuth authentication only.
        /// Note: This cannot be used with batch sending.
        /// </summary>
        public static TransactClient CreateOAuthOnly(
            int podNumber,
            string oAuthClientId,
            string oAuthClientSecret,
            string oAuthRefreshToken)
        {
            if (oAuthClientId == null) throw new ArgumentNullException("oAuthClientId");
            if (oAuthClientSecret == null) throw new ArgumentNullException("oAuthClientSecret");
            if (oAuthRefreshToken == null) throw new ArgumentNullException("oAuthRefreshToken");

            return new TransactClient(new TransactClientConfiguration()
            {
                PodNumber = podNumber,
                OAuthClientId = oAuthClientId,
                OAuthClientSecret = oAuthClientSecret,
                OAuthRefreshToken = oAuthRefreshToken
            });
        }

        /// <summary>
        /// Create the TransactClient using application configuration.
        /// This will attempt to read both the TransactClientConfigurationSection
        /// and appSettings from configuration. When a setting is set in both
        /// TransactClientConfigurationSection and appSettings the appSettings value is used.
        /// </summary>
        /// <param name="configProvider">
        /// Optional override for specifying a custom TransactClientConfigurationProvider.
        /// </param>
        public static TransactClient CreateUsingConfiguration(
            TransactClientConfigurationProvider configProvider = null)
        {
            var config = new TransactClientConfiguration();

            if (configProvider == null)
            {
                configProvider = new TransactClientConfigurationProvider();
            }

            var configSectionConfig = configProvider.GetFromConfigurationSection();
            var appSettingsConfig = configProvider.GetFromAppSettings();

            if (configSectionConfig == null && appSettingsConfig == null)
            {
                throw new InvalidOperationException(
                    "Unable to create TransactClient using configuration. " +
                    "No configuration was provided.");
            }

            if (configSectionConfig != null)
            {
                config.HydrateUsing(configSectionConfig);
            }

            if (appSettingsConfig != null)
            {
                config.HydrateUsing(appSettingsConfig);
            }

            return new TransactClient(config);
        }
    }
}