﻿
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualControl.Debugging
{
    /// <summary>
    /// Extensions to Work with Embedded Resouce Files
    /// </summary>
    public class EmbeddedResources
    {

        /// <summary>
        /// Converts Embedded Resource File to Actual File On Processor
        /// </summary>
        /// <param name="assembly">Calling Assembly (Use Reflection)</param>
        /// <param name="resourceDirectory">.Syntax Project Resource Directory</param>
        /// <param name="fileName">Embedded Resource File in Project</param>
        /// <param name="outputDir">File Directory on Processor to Place Reconstructed File</param>
        /// <example>
        /// <code>
        /// EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "Cenero.Hardware.Cameras.Factory.HTML", "index.html", "HTML\\CeneroApk\\Cameras\\htmlfile");
        /// </code>
        /// </example>
        public static void ExtractEmbeddedResource(Assembly assembly, string resourceDirectory, string fileName, string outputDir)
        {
            try
            {  
                using (Stream stream = assembly.GetManifestResourceStream(resourceDirectory + @"." + fileName))
                {
                    using (FileStream fileStream = new FileStream(Path.Combine(outputDir, fileName), FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Exception("Failed to Convert Embedded Resource to Files: {0}", e);
            }
        }

        /// <summary>
        /// Converts Embedded Resource Files From Entire Directory to Actual Files On Processor
        /// </summary>
        /// <param name="assembly">Calling Assembly (Use Reflection)</param>
        /// <param name="resourceDirectory">.Syntax Project Resource Directory</param>
        /// <param name="outputDir">File Directory on Processor to Place Reconstructed File</param>
        /// <example>
        /// <code>
        /// EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "Cenero.Hardware.Cameras.Factory.HTML", "HTML\\CeneroApk\\Cameras\\htmlDir");
        /// </code>
        /// </example>
        public static void ExtractEmbeddedResource(Assembly assembly, string resourceDirectory, string outputDir)
        {
            try
            {
                var files = assembly.GetManifestResourceNames();

                foreach (string file in files)
                {
                    var fileName = file.Remove(0, resourceDirectory.Length + 1);

                    if (file.StartsWith(resourceDirectory))
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(file))
                        {
                            using (FileStream fileStream = new FileStream(Path.Combine(outputDir, fileName), FileMode.Create))
                            {
                                for (int i = 0; i < stream.Length; i++)
                                {
                                    fileStream.WriteByte((byte)stream.ReadByte());
                                }
                                fileStream.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Exception("Failed to Convert Embedded Resources to Files: {0}", e);
            }
        }

        /// <summary>
        /// Converts Embedded Resource Files From List to Actual Files On Processor
        /// </summary>
        /// <param name="assembly">Calling Assembly (Use Reflection)</param>
        /// <param name="resourceDirectory">.Syntax Project Resource Directory</param>
        /// <param name="files">List of File Names</param>
        /// <param name="outputDir">File Directory on Processor to Place Reconstructed File</param>
        /// <example>
        /// <code>
        /// EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "Cenero.Hardware.Cameras.Factory.HTML", new List(){"index.html", "app.js", "style.css"}, "HTML\\CeneroApk\\Cameras\\htmllist");
        /// </code>
        /// </example>
        public static void ExtractEmbeddedResource(Assembly assembly, string resourceDirectory, List<string> files, string outputDir)
        {
            try
            {
                foreach (string file in files)
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceDirectory + @"." + file))
                    {
                        using (FileStream fileStream = new FileStream(Path.Combine(outputDir, file), FileMode.Create))
                        {
                            for (int i = 0; i < stream.Length; i++)
                            {
                                fileStream.WriteByte((byte)stream.ReadByte());
                            }
                            fileStream.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Exception("Failed to Convert Embedded Resources to Files: {0}", e);
            }
        }
        /// <summary>
        /// Loads embedded resources from DLL and converts the String Contents to an Object
        /// </summary>
        /// <typeparam name="T">The Expected Returned Object</typeparam>
        /// <param name="assembly">The assembly to load object from</param>
        /// <param name="file">Embedded Resource File Name</param>
        /// <returns>Deserialized JSON Object</returns>
        public static T ConvertJson<T>(Assembly assembly, string file) where T : class, new()
        {
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Exception("Failed to Convert Embedded Resource to Object: {0}", e);
                return null;
            }
        }
    }
}