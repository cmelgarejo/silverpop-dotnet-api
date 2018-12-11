using System;
using System.IO;
using System.Threading.Tasks;

namespace Silverpop.Client
{
    public interface ISilverpopCommunicationsClient : IDisposable
    {
        string HttpUpload(string data, bool tryRefreshingOAuthAccessToken = true, bool useXMLAPI = false, string XMLAPISession = "");

        Task<string> HttpUploadAsync(string data, bool tryRefreshingOAuthAccessToken = true, bool useXMLAPI = false, string XMLAPISession = "");

        void SftpCreateDirectoryIfNotExists(string path);

        void SftpGzipUpload(string data, string destinationPath);

        Task SftpGzipUploadAsync(string data, string destinationPath);

        void SftpMove(string fromPath, string toPath);

        Task SftpMoveAsync(string fromPath, string toPath);

        Stream SftpDownload(string filePath);

        Task<Stream> SftpDownloadAsync(string filePath);
    }
}