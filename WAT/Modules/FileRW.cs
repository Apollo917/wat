using System;
using System.IO;

namespace WAT.Modules
{
    public class FileRW
    {
        public static bool TryRead(string path, out string result)
        {
            bool status = false;
            result = string.Empty;

            try
            {
                if (File.Exists(path))
                {
                    using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read))
                    using (var myReader = new StreamReader(stream))

                    {
                        result = myReader.ReadToEnd();
                    }

                    status = result != null && result.Length > 0;
                }
                else
                {
                    createDirFile(path);
                }
            }
            catch (Exception ex)
            {
                ex.Handle();
            }

            return status;
        }

        public static void Write(string path, string data, bool append = true)
        {
            if (File.Exists(path))
            {
                writeToFile(path, data, append);
            }
            else
            {
                createDirFile(path);
                writeToFile(path, data, append);
            }
        }



        private static void writeToFile(string path, string data, bool append)
        {
            try
            {
                using (Stream stream = File.Open(path, append ? FileMode.Append: FileMode.Create, FileAccess.Write))
                using (var myWriter = new StreamWriter(stream))
                {
                    myWriter.Write($"{data}{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                ex.Handle();
            }
        }
        private static void createDirFile(string path)
        {
            try
            {
                if (Path.GetDirectoryName(path) is string dir && dir.Trim().Length > 0)
                {
                    Directory.CreateDirectory(dir);
                }

                writeToFile(path, string.Empty, false);
            }
            catch (Exception ex)
            {
                ex.Handle();
            }
        }
    }
}
