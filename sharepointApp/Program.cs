using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client; // Make sure you have this namespace

namespace sharepointApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string siteUrl = "**********"; // Update with your site URL
            string userName = "**********"; // Your username
            string appPassword = "**********"; // The app password generated

            //claudecheck.RunCheck();
            SharePointIter2.Main1(args);

            //try
            //{
            //    // Connect to SharePoint
            //    using (var context = new ClientContext(siteUrl))
            //    {
            //        context.Credentials = new SharePointOnlineCredentials(userName, GetSecureString(appPassword));

            //        // Download a file
            //        //DownloadFile(context, "/Shared Documents/yourfile.docx", @"C:/MyDocuments/"); // Update with your file path and local filename

            //        // Assign Read permission to the user
            //        // Specify the document library name (e.g., "Documents")
            //        List documentLibrary = context.Web.Lists.GetByTitle("Documents");

            //        // Load the folders from the root folder
            //        Folder rootFolder = documentLibrary.RootFolder;
            //        context.Load(rootFolder);
            //        context.Load(rootFolder.Folders); // Load all the folders
            //        context.ExecuteQuery();



            //        //Assign read permission
            //        //Web web = context.Web;
            //        //List list = web.Lists.GetByTitle("Documents");

            //        //User user = web.EnsureUser("amgmiot@acemicromatic.com");
            //        //RoleDefinitionBindingCollection roleDefBinding = new RoleDefinitionBindingCollection(context);
            //        //roleDefBinding.Add(web.RoleDefinitions.GetByType(RoleType.Reader));

            //        //list.RoleAssignments.Add(user, roleDefBinding);
            //        //context.ExecuteQuery();



            //        //Iterate over the folders and print their names
            //        foreach (Folder folder in rootFolder.Folders)
            //        {
            //            Console.WriteLine("Folder: " + folder.Name);
            //            // Optional: To list files inside this folder, uncomment the following lines

            //            context.Load(folder.Files);
            //            context.ExecuteQuery();
            //            //foreach (var file in folder.Files)
            //            //{
            //            //    Console.WriteLine("   File: " + file.Name);
            //            //}

            //            // Load subfolders
            //            context.Load(folder.Folders);
            //            context.ExecuteQuery();
            //            foreach (var subfolders in folder.Folders)
            //            {
            //                Console.WriteLine("  Sub Folder: " + subfolders.Name);
            //                // Load subfolders
            //                context.Load(subfolders.Folders);
            //                context.ExecuteQuery();
            //                foreach (var subfolders1 in folder.Folders)
            //                {
            //                    Console.WriteLine("     Sub Folder1: " + subfolders1.Name);
            //                }
            //                Console.WriteLine();
            //            }
            //            Console.WriteLine() ;
            //            //if (folder.Name == "Model Specific Docs") // Check for "SOP" folder
            //            //{
            //            //    Console.WriteLine("Folder: " + folder.Name);

            //            //    // Load the files in the "SOP" folder
            //            //    context.Load(folder.Files);
            //            //    context.ExecuteQuery();

            //            //    // Download specific files
            //            //    foreach (var file in folder.Files)
            //            //    {
            //            //        Console.WriteLine("   File: " + file.Name);

            //            //        // Check for specific file names
            //            //        //if (file.Name == "00103A003F JXL Built in type.pdf" ||
            //            //        //file.Name == "94104A024- TAIL STOCK BUILT IN ASSY(100 MM).pdf")
            //            //        //{
            //            //        // Construct file URLs and local paths
            //            //        string fileUrl = file.ServerRelativeUrl; // SharePoint file URL
            //            //        string localPath = Path.Combine(@"C:\MyDocuments", file.Name); // Local path to download to

            //            //        // Download the file
            //            //        DownloadFile(context, fileUrl, localPath);
            //            //        //}
            //            //    }
            //            //}
            //        }

            //        // Load the "Model Specific Docs" folder
            //        //Folder modelSpecificDocsFolder = documentLibrary.RootFolder.Folders.GetByUrl("Model Specific Docs");
            //        //context.Load(modelSpecificDocsFolder);
            //        //context.ExecuteQuery();

            //        //// Start recursive download
            //        //DownloadFilesFromFolder(context, modelSpecificDocsFolder);
            //        Console.ReadKey();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //    Console.ReadKey();
            //}
        }

        private static void DownloadFilesFromFolder(ClientContext context, Folder folder)
        {
            // Load files in the current folder
            context.Load(folder.Files);
            context.ExecuteQuery();

            // Download files in the current folder
            foreach (var file in folder.Files)
            {
                Console.WriteLine("   File: " + file.Name);
                string fileUrl = file.ServerRelativeUrl; // SharePoint file URL
                string localPath = Path.Combine(@"C:\MyDocuments", file.Name); // Local path to download to

                // Download the file
                //DownloadFile(context, fileUrl, localPath);
                DownloadFile(context, file);
            }

            // Load subfolders
            context.Load(folder.Folders);
            context.ExecuteQuery();

            // Recursively download files from subfolders
            foreach (var subFolder in folder.Folders)
            {
                Console.WriteLine("Folder: " + subFolder.Name);
                DownloadFilesFromFolder(context, subFolder);
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
                //// Use Microsoft.SharePoint.Client.File for SharePoint file operations
                //FileInformation fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(context, fileUrl);

                //// Use System.IO for local file handling
                //using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                //{
                //    fileInfo.Stream.CopyTo(fileStream);
                //    Console.WriteLine("File downloaded successfully!");
                //}


                // Retrieve the file by its server-relative URL
                //var file = context.Web.GetFileByServerRelativeUrl(fileUrl);
                //context.Load(file);
                //context.ExecuteQuery();

                //// Open binary stream for the file
                //ClientResult<Stream> streamResult = file.OpenBinaryStream();
                //context.ExecuteQuery();

                //using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                //{
                //    // Copy the stream to a file
                //    streamResult.Value.CopyTo(fileStream);
                //    Console.WriteLine($"File downloaded successfully to {localFilePath}!");
                //}


                // Open the binary stream for the file
                ClientResult<Stream> data = file.OpenBinaryStream();
                context.ExecuteQuery(); // Execute query to load the file

                // Create a memory stream to hold the data
                using (MemoryStream memStream = new MemoryStream())
                {
                    data.Value.CopyTo(memStream);

                    // Define the local path where the file will be saved
                    string downloadedFile = Path.Combine(@"C:\Users\devteam\Downloads", file.Name); // Change as needed

                    // Write the file bytes to the local path
                    System.IO.File.WriteAllBytes(downloadedFile, memStream.ToArray());
                    Console.WriteLine($"File downloaded successfully to {downloadedFile}!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download file: {ex.Message}");
            }
        }

    }
}
