using SonicUnleashedFCOConv;

namespace SUSaveConv {
    class Program {
        static void Main(string[] args) {
            if(args.Length == 0) {
                Console.WriteLine("SU Save Converter v1.1\nUsage: Drag and drop your Save file.");
                return;
            }
            else {
                Platform.ReadSave(args[0]);
                Platform.ConvSave(args[0]);
            }
        }
    }
}