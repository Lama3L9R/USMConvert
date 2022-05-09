using System.ComponentModel;
using System.Diagnostics;
using VGMToolbox.format;

namespace USMCovert {
    class USMCovert
    {
        [STAThreadAttribute]
        public static void Main(string[] Args)
        {
            if (Args.Length == 0)
            {
                Console.WriteLine("ERROR: No input file was specificated!");
                return;
            }

            var ExtractOnly = false;
            string? Input = null;

            foreach (string s in Args)
            {
                if (s.StartsWith("--"))
                {
                    switch (s.Substring(2))
                    {
                        case "Help":
                            Console.WriteLine("USMCovert -- A tool for covert USM files to MKV files");
                            Console.WriteLine($"Usage: {Path.GetFileName(Environment.ProcessPath)} <Input.usm> [--Help] [--ExtractOnly]"); 
                            return;
                        case "ExtractOnly":
                            ExtractOnly = true;
                            break;
                        default:
                            Console.WriteLine($"Usage: {Path.GetFileName(Environment.ProcessPath)} <Input.usm> [--Help] [--ExtractOnly]"); 
                            break;
                    }
                }
                else
                {
                    Input = s;
                }
            }

            if (Input == null || !File.Exists(Input))
            {
                Console.WriteLine("ERROR: Input file is missing!");
                return;
            }
            
            Console.WriteLine($"INFO: Unpacking file {Input}");
            
            var stream = new USMStream(Input);
            stream.DemultiplexStreams(
                new MpegStream.DemuxOptionsStruct
                {
                    ExtractAudio = true,
                    ExtractVideo = true
                }
            );
            Console.WriteLine("INFO: Extraction Complete!");

            if (ExtractOnly)
            {
                return;
            }
            
            var fileName = Path.GetFileNameWithoutExtension(stream.FilePath);
            var arguments = "";
            stream.OutputFiles.ForEach(file =>
            {
                arguments += $"-i {file} ";
            });

            arguments += fileName + ".mkv";
            
            Console.WriteLine($"INFO: Executing ffmpeg command with args: {arguments}");
            
            try
            {
                var ffmpeg = Process.Start("ffmpeg");

                ffmpeg.StartInfo.Arguments = arguments
;
                ffmpeg.StartInfo.RedirectStandardOutput = true;
                ffmpeg.Start();
                ffmpeg.WaitForExit();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("ERROR: Failed to execute ffmpeg command! Please download and install ffmpeg then execute the following command by your self: ");
                Console.WriteLine($"  $ ffmpeg {arguments}");
                return;
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("ERROR: Failed to execute ffmpeg command! Please download and install ffmpeg then execute the following command by your self: ");
                Console.WriteLine($"  $ ffmpeg {arguments}");
                return;
            } 

            Console.WriteLine($"INFO: Done! Output file written to: {fileName}.mkv");
        }
    }

    class USMStream : CriUsmStream
    {
        public List<string> OutputFiles = new();
        
        public USMStream(string file) : base(file)
        {
        }

        protected override void DoFinalTasks(FileStream sourceFileStream, Dictionary<uint, FileStream> outputFiles, bool addHeader)
        {
            base.DoFinalTasks(sourceFileStream, outputFiles, addHeader);

            
            foreach (var fs in outputFiles.Values)
            {
                OutputFiles.Add(fs.Name);
            }
        }
    }
}