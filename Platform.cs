namespace SonicUnleashedFCOConv {
    public static class Platform {
        public static bool PS3 = false;
        public static long address;

        public static void ReadSave(string path) {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            try
            {
                //long header = binaryReader.ReadInt64();
                if (binaryReader.ReadInt64() != 144115188075888703) {
                    //structureError = true;
                    //address = binaryReader.BaseStream.Position;
                    //return;
                
                    Console.WriteLine("\nERROR: Exception occurred during parsing at: 0x" + unchecked((int)binaryReader.BaseStream.Position).ToString("X")  + ".");
                    Console.WriteLine("Please make sure your Save file is Decrypted!");
                    Console.WriteLine("\nPress Enter to Exit.");
                    Console.Read();
                    
                    return;
                }

                if (binaryReader.BaseStream.Length == 0x9150) {
                    Console.WriteLine("PS3 Save Detected!");
                    PS3 = true;
                }
                if (binaryReader.BaseStream.Length == 0xA950) {
                    Console.WriteLine("X360 Save Detected!");
                }
            }
            catch (EndOfStreamException e) {
                Console.WriteLine(e);
                
                Console.WriteLine("\nERROR: Exception occurred during parsing at: 0x" + unchecked((int)binaryReader.BaseStream.Position).ToString("X")  + ".");
                Console.WriteLine("There is a structural abnormality within the Save file!");
                Console.WriteLine("\nPress Enter to Exit.");
                Console.Read();
                
                throw;
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


            if (PS3 == true) {
                for (int i = 0; i < 9300; i++) {
                    binaryWriter.Write(binaryReader.ReadInt32());
                }
                binaryWriter.BaseStream.Position = 0x9150;
                for (int i = 0; i < 1536; i++) {
                    binaryWriter.Write(0x00000000);
                }
            }
            else {
                for (int i = 0; i < 9300; i++) {
                    binaryWriter.Write(binaryReader.ReadInt32());
                }
            }

            fileStream.Close();
            binaryReader.Close();
            binaryWriter.Close();
            Console.WriteLine("Save Converted!");
        }
    }
}