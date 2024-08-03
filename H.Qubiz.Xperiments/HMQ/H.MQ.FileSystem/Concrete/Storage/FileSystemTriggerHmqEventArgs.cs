using System;
using System.IO;

namespace H.MQ.FileSystem.Concrete.Storage
{
    internal class FileSystemTriggerHmqEventArgs : EventArgs
    {
        public FileSystemTriggerHmqEventArgs(FileInfo eventFile)
        {
            EventFile = eventFile;
        }

        public FileInfo EventFile { get; }
    }
}
