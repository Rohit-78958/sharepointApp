using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client; // Ensure you have this namespace

namespace sharepointApp
{
    internal class SharePointIter2
    {
        public static void Main1(string[] args)
        {
            string siteUrl = "**********"; // Update with your site URL
            string userName = "**********"; // Your username
            string appPassword = "**********"; // The app password generated

            try
            {
                // Connect to SharePoint
                using (var context = new ClientContext(siteUrl))
                {
                    context.Credentials = new SharePointOnlineCredentials(userName, GetSecureString(appPassword));

                    // Specify the document library name
                    List documentLibrary = context.Web.Lists.GetByTitle("Documents");

                    // Specify the folder path you want to download files from (e.g., "Shared Documents/Model Specific Docs")
                    string folderPath = "/sites/MDLACE/Shared Documents/Model Specific Docs/Apollo/Apollo Fanuc"; // Replace with your folder path
                    Folder folder = context.Web.GetFolderByServerRelativeUrl(folderPath);

                    //for downloading specific file in the provided folder path
                    string targetFileName = "109A_Apollo_Oi_TF_IMPM_New PM.pdf";

                    // Download all files in the specified folder
                    DownloadFilesFromFolder(context, folder, @"C:\Users\devteam\Downloads\", targetFileName); // Replace with your local directory path
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        private static void DownloadFilesFromFolder(ClientContext context, Folder folder, string localPath, string targetFileName="")
        {
            try
            {
                // Load files in the current folder
                context.Load(folder.Files);
                context.ExecuteQuery();


                if (!string.IsNullOrEmpty(targetFileName))
                {
                    // Check if the specific file exists in the folder and download it
                    foreach (var file in folder.Files)
                    {
                        if (file.Name == targetFileName) // Check if the file matches the target file name
                        {
                            Console.WriteLine("File Found: " + file.Name);
                            string fileUrl = file.ServerRelativeUrl; // SharePoint file URL
                            string localFilePath = Path.Combine(localPath, file.Name); // Local path to save the file

                            // Download the file
                            DownloadFile(context, file, localFilePath);
                            return; // Exit after downloading the specific file
                        }
                    }

                    // If the file was not found
                    Console.WriteLine($"File '{targetFileName}' not found in the specified folder.");
                    return;
                }

                // Download each file in the folder
                foreach (var file in folder.Files)
                {
                    Console.WriteLine("   File: " + file.Name);
                    string fileUrl = file.ServerRelativeUrl; // SharePoint file URL
                    string localFilePath = Path.Combine(localPath, file.Name); // Local path to save the file

                    // Download the file
                    DownloadFile(context, file, localFilePath);
                }

                // Load subfolders
                context.Load(folder.Folders);
                context.ExecuteQuery();

                // Recursively download files from subfolders
                foreach (var subFolder in folder.Folders)
                {
                    Console.WriteLine("Folder: " + subFolder.Name);
                    //string subFolderPath = Path.Combine(localPath, subFolder.Name);

                    //// Ensure the directory exists locally
                    //if (!Directory.Exists(subFolderPath))
                    //{
                    //    Directory.CreateDirectory(subFolderPath);
                    //}

                    // Recursively download from the subfolder
                    //DownloadFilesFromFolder(context, subFolder, subFolderPath);
                    
                    DownloadFilesFromFolder(context, subFolder, localPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download files: {ex.Message}");
            }
        }

        private static void DownloadFile(ClientContext context, Microsoft.SharePoint.Client.File file, string localFilePath)
        {
            try
            {
                // Open the binary stream for the file
                ClientResult<Stream> data = file.OpenBinaryStream();
                context.ExecuteQuery(); // Execute query to load the file

                // Create a memory stream to hold the data
                using (MemoryStream memStream = new MemoryStream())
                {
                    data.Value.CopyTo(memStream);

                    // Write the file bytes to the local path
                    System.IO.File.WriteAllBytes(localFilePath, memStream.ToArray());
                    Console.WriteLine("*************************************************************************");
                    Console.WriteLine($"File downloaded successfully to {localFilePath}!");
                    Console.WriteLine("*************************************************************************");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download file: {ex.Message}");
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
    }
}
