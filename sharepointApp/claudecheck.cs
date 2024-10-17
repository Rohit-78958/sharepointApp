using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace sharepointApp
{
    internal class claudecheck
    {
        public static void RunCheck()
        {
            string siteUrl = "**********"; // Update with your site URL
            string userName = "**********"; // Your username
            string appPassword = "**********"; // The app password generated

            try
            {
                using (var context = new ClientContext(siteUrl))
                {
                    // Use SecureString for the password
                    SecureString securePassword = GetSecureString(appPassword);
                    context.Credentials = new SharePointOnlineCredentials(userName, securePassword);

                    // Ensure the context is authenticated before proceeding
                    context.ExecuteQuery();

                    List documentLibrary = context.Web.Lists.GetByTitle("Documents");
                    Folder rootFolder = documentLibrary.RootFolder;
                    context.Load(rootFolder);
                    context.Load(rootFolder.Folders);
                    context.ExecuteQuery();

                    Folder modelSpecificDocsFolder = rootFolder.Folders.GetByUrl("Model Specific Docs");
                    context.Load(modelSpecificDocsFolder);
                    context.ExecuteQuery();

                    bool check = false;
                    DownloadFilesFromFolder(context, modelSpecificDocsFolder, ref check);
                    Console.WriteLine("Download process completed. Press any key to exit.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.ReadKey();
            }
        }

        private static void DownloadFilesFromFolder(ClientContext context, Folder folder, ref bool fileDownloaded)
        {
            try
            {
                context.Load(folder.Files);
                context.Load(folder.Folders);
                context.ExecuteQuery();

                foreach (var file in folder.Files)
                {
                    if (fileDownloaded)
                        break;

                    Console.WriteLine($"Attempting to download: {file.Name}");
                    DownloadFile(context, file);
                    fileDownloaded = true;
                }

                if (!fileDownloaded)
                {
                    foreach (var subFolder in folder.Folders)
                    {
                        Console.WriteLine($"Entering folder: {subFolder.Name}");
                        DownloadFilesFromFolder(context, subFolder, ref fileDownloaded);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DownloadFilesFromFolder: {ex.Message}");
            }
        }


        private static SecureString GetSecureString(string str)
        {
            var secureStr = new SecureString();
            foreach (char c in str)
            {
                secureStr.AppendChar(c);
            }
            return secureStr;
        }

        private static void DownloadFile(ClientContext context, Microsoft.SharePoint.Client.File file)
        {
            try
            {
                context.Load(file);
                context.ExecuteQuery();

                var fileRef = file.ServerRelativeUrl;
                var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(context, fileRef);
                var fileName = Path.GetFileName(file.Name);

                string downloadPath = Path.Combine(@"C:\Users\Admin\Downloads", fileName);

                using (var fileStream = System.IO.File.Create(downloadPath))
                {
                    fileInfo.Stream.CopyTo(fileStream);
                }

                Console.WriteLine($"File downloaded successfully: {fileName}");
            }
            catch (ServerUnauthorizedAccessException)
            {
                Console.WriteLine($"Access denied for file: {file.Name}. Check permissions.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download file {file.Name}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}