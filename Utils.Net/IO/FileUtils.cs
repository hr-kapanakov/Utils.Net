using System.IO;
using System.Threading.Tasks;

namespace Utils.Net.IO
{
    /// <summary>
    /// Provides functions to help working with Windows files.
    /// </summary>
    public static class FileUtils
    {
        #region Constants
        /// Default buffer size used for operations requiring one
        internal const int DefaultBufferSize = 4096;

        #endregion

        #region Static Methods
        /// <summary>
        /// Check that two files content are the same.
        /// FileInfo objects should be up to date. Refresh them before if they're is a doubt.
        /// </summary>
        /// <param name="firstFile">First file</param>
        /// <param name="secondFile">Second file</param>
        /// <param name="bufferSize">Buffer size used for file reading</param>
        public static bool ContentEquals(this FileInfo firstFile, FileInfo secondFile, int bufferSize = DefaultBufferSize)
        {
            if (firstFile.Length != secondFile.Length)
                return false;

            using (FileStream firstStream = firstFile.OpenRead())
            {
                using (FileStream secondStream = secondFile.OpenRead())
                {
                    return firstStream.ContentEquals(secondStream, bufferSize);
                }
            }
        }

        /// <summary>
        /// Check that two streams have the same content.
        /// </summary>
        /// <param name="firstStream">First stream</param>
        /// <param name="secondStream">Second stream</param>
        /// <param name="bufferSize">Buffer size used for stream reading</param>
        public static bool ContentEquals(this Stream firstStream, Stream secondStream, int bufferSize = DefaultBufferSize)
        {
            var firstBuffer = new byte[bufferSize];
            var secondBuffer = new byte[bufferSize];

            int firstReadBytes;
            do
            {
                firstReadBytes = firstStream.Read(firstBuffer, 0, bufferSize);
                int secondReadBytes = secondStream.Read(secondBuffer, 0, bufferSize);

                if (firstReadBytes != secondReadBytes)
                    return false;

                for (int i = 0; i < firstReadBytes; i++)
                {
                    if (firstBuffer[i] != secondBuffer[i])
                        return false;
                }
            }
            while (firstReadBytes == bufferSize);

            return true;
        }

        /// <summary>
        /// Check that two files have the same content.
        /// FileInfo objects should be up to date. Refresh them before if they're is a doubt.
        /// </summary>
        /// <param name="firstFile">First file</param>
        /// <param name="secondFile">Second file</param>
        /// <param name="bufferSize">Buffer size used for file reading</param>
        public static async Task<bool> ContentEqualsAsync(this FileInfo firstFile, FileInfo secondFile, int bufferSize = DefaultBufferSize)
        {
            if (firstFile.Length != secondFile.Length)
                return false;

            using (FileStream firstStream = firstFile.OpenRead())
            {
                using (FileStream secondStream = secondFile.OpenRead())
                {
                    return await firstStream.ContentEqualsAsync(secondStream, bufferSize);
                }
            }
        }

        /// <summary>
        /// Check that two streams have the same content.
        /// </summary>
        /// <param name="firstStream">First stream</param>
        /// <param name="secondStream">Second stream</param>
        /// <param name="bufferSize">Buffer size used for stream reading</param>
        public static async Task<bool> ContentEqualsAsync(this Stream firstStream, Stream secondStream, int bufferSize = DefaultBufferSize)
        {
            var firstBuffer = new byte[bufferSize];
            var secondBuffer = new byte[bufferSize];

            int firstReadBytes;
            do
            {
                Task<int> firstReadTask = firstStream.ReadAsync(firstBuffer, 0, bufferSize);
                Task<int> secondReadTask = secondStream.ReadAsync(secondBuffer, 0, bufferSize);

                firstReadBytes = await firstReadTask;
                int secondReadBytes = await secondReadTask;

                if (firstReadBytes != secondReadBytes)
                    return false;

                for (int i = 0; i < firstReadBytes; i++)
                    if (firstBuffer[i] != secondBuffer[i])
                        return false;
            }
            while (firstReadBytes == bufferSize);

            return true;
        }

        /// <summary>
        /// Delete a file. Do not throw an exception if the file does not exist.
        /// Offer the possibility to delete read-only files.
        /// </summary>
        /// <param name="filePath">Path of the file to delete.</param>
        /// <param name="force">true if you want to delete the file even when it is read-only or hidden.</param>
        public static void DeleteFile(string filePath, bool force = true)
        {
            if (!File.Exists(filePath))
                return;

            if (force)
            {
                RemoveReadOnly(filePath);
                RemoveHidden(filePath);
            }

            File.Delete(filePath);
        }

        /// <summary>
        /// Copy a file by replacing the existing one.
        /// </summary>
        /// <param name="sourcePath">Path of the file to copy.</param>
        /// <param name="destPath">Path of the file to create.</param>
        /// <param name="createFolder">If true, the folder of <see cref="destPath"/> 
        /// will be created if necessary.</param>
        /// <param name="removeReadOnly">Remove the read-only flag on the newly
        /// created file. Also remove the read-only flag on the destination file
        /// before the copy to make sure the operation succeeds.</param>
        /// <param name="removeHidden">Remove the hidden flag on the newly
        /// created file. Also remove the hidden flag on the destination file
        /// before the copy to make sure the operation succeeds.</param>
        public static void CopyFile(
            string sourcePath,
            string destPath,
            bool createFolder = false,
            bool removeReadOnly = false,
            bool removeHidden = false)
        {
            if (!File.Exists(sourcePath))
                return;

            if (createFolder)
            {
                string dir = Path.GetDirectoryName(destPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);
            }

            if (removeReadOnly)
            {
                RemoveReadOnly(destPath);
            }

            if (removeHidden)
            {
                RemoveHidden(destPath);
            }

            File.Copy(sourcePath, destPath, true);

            if (removeReadOnly)
            {
                RemoveReadOnly(destPath);
            }

            if (removeHidden)
            {
                RemoveHidden(destPath);
            }
        }

        #endregion

        #region ReadOnly attribute
        /// <summary>
        /// Check if file is read-only
        /// </summary>
        public static bool IsReadOnly(string filePath)
        {
            return (File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        /// <summary>
        /// Set the read-only flag of a file and returns true if read-only attribute was set.
        /// </summary>
        /// <returns>False if the operation failed.</returns>
        public static bool SetReadOnly(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
            {
                File.SetAttributes(filePath, attributes | FileAttributes.ReadOnly);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove the read-only flag of a file and returns true if read-only attribute was removed.
        /// </summary>
        /// <returns>False if the operation failed.</returns>
        public static bool RemoveReadOnly(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(filePath, attributes & ~FileAttributes.ReadOnly);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the read-only flag of a file.
        /// </summary>
        /// <returns>False if the operation failed.</returns>
        public static bool SafeRemoveReadOnly(string filePath)
        {
            try
            {
                RemoveReadOnly(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Hidden attribute
        /// <summary>
        /// Check if file is hidden
        /// </summary>
        public static bool IsHidden(string filePath)
        {
            return (File.GetAttributes(filePath) & FileAttributes.Hidden) == FileAttributes.Hidden;
        }

        /// <summary>
        /// Set the hidden flag of a file.
        /// </summary>
        public static void SetHidden(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                File.SetAttributes(filePath, attributes | FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// Remove the hidden flag of a file.
        /// </summary>
        public static void RemoveHidden(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                File.SetAttributes(filePath, attributes & ~FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// Remove the hidden flag of a file.
        /// </summary>
        /// <returns>False if the operation failed.</returns>
        public static bool SafeRemoveHidden(string filePath)
        {
            try
            {
                RemoveHidden(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
