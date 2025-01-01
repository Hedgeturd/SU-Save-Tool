namespace SonicUnleashedFCOConv {
    public static class Platform {
        static bool PS3 = false;

        public static void ReadSave(string path) {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            
            if (binaryReader.ReadInt64() != 144115188075888703) {
                Console.WriteLine("\nERROR: Exception occurred during parsing at: 0x" + unchecked((int)binaryReader.BaseStream.Position).ToString("X")  + ".");
                Console.WriteLine("Please make sure your Save file is Decrypted!");
                Console.WriteLine("\nPress Enter to Exit.");
                Console.Read();
                Environment.Exit(1);
            }

            switch (binaryReader.BaseStream.Length) {
                case 0x9150:
                    Console.WriteLine("PS3 Save Detected!");
                    PS3 = true;
                    break;
                case 0xA950:
                    Console.WriteLine("X360 Save Detected!");
                    break;
                default:
                    Console.WriteLine("\nERROR: Save file size: 0x" + unchecked((int)binaryReader.BaseStream.Length).ToString("X") + " does not match any Save file!");
                    Console.WriteLine("There is a structural abnormality within the Save file!");
                    Console.WriteLine("\nPress Enter to Exit.");
                    Console.Read();
                    Environment.Exit(1);
                    break;
            }
            
            fileStream.Close();
            binaryReader.Close();
            Console.WriteLine("Save Read!");
        }

        public static void ConvSave(string path) {
            string filePath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path);
            File.Delete(Path.Combine(filePath + "_CONV"));
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            BinaryWriter binaryWriter = new BinaryWriter(File.Open(filePath + "_CONV", FileMode.OpenOrCreate));
            
            for (int i = 0; i < 9300; i++) {
                binaryWriter.Write(binaryReader.ReadInt32());
            }
            if (PS3 == true) {
                binaryWriter.BaseStream.Position = 0x9150;
                for (int i = 0; i < 1536; i++) {
                    binaryWriter.Write(0x00000000);
                }
            }

            fileStream.Close();
            binaryReader.Close();
            binaryWriter.Close();
            Console.WriteLine("Save Converted!");
        }
    }
}