using ScreenRecorderLib;
using System;
using System.IO;
using System.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace LetsTrain
{
    class Program
    {
        private static readonly SemaphoreSlim RecordingLock = new SemaphoreSlim(0, 1);

        static void Main(string[] args)
        {
            using var recorder = Recorder.CreateRecorder(new RecorderOptions() { RecorderMode = RecorderMode.Snapshot });
            recorder.OnRecordingComplete += Recorder_OnRecordingComplete;

            while (true)
            {
                Console.ReadLine();

                recorder.Record(@"D:\Test.png");
                RecordingLock.Wait();

                ProcessImage();
            }
        }

        private static void ProcessImage()
        {
            // Сохраняем первую карту
            using Image image = Image.Load(@"D:\Test.png");
            image.Mutate(x => x.Crop(new Rectangle(1770, 1350, 195, 135)));
            image.Save(@"D:\card1.jpg");
        }

        private static void Recorder_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            Console.WriteLine("Record complete");
            RecordingLock.Release();
        }
    }
}
