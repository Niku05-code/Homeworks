// Să se implementeze un manager de fişiere simplu, care să suporte următoarele funcţii: afişarea
// conţinutului unui director (folder), ştergere director/fişier, redenumire director/fişier,
// copiere/mutare fişiere/directoare, editarea conţinutului fişierelor text (asemănător comenzii F4 Edit
// din Total Commander). Orice funcţionalitate suplimentară este binevenită şi apreciată.

using System;
using System.IO;
using System.Text;

namespace SimpleFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simple File Manager");
            Console.WriteLine("Available commands:");
            Console.WriteLine("list [path] - List directory contents");
            Console.WriteLine("del <path> - Delete file or directory");
            Console.WriteLine("rename <old> <new> - Rename file or directory");
            Console.WriteLine("copy <source> <dest> - Copy file or directory");
            Console.WriteLine("move <source> <dest> - Move file or directory");
            Console.WriteLine("edit <file> - Edit text file");
            Console.WriteLine("exit - Exit the program");
            
            string currentDirectory = Directory.GetCurrentDirectory();
            
            while (true)
            {
                Console.Write($"{currentDirectory}> ");
                string input = Console.ReadLine();
                string[] parts = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                
                if (parts.Length == 0) continue;
                
                try
                {
                    switch (parts[0].ToLower())
                    {
                        case "list":
                            ListDirectory(parts.Length > 1 ? parts[1] : currentDirectory);
                            break;
                        case "del":
                            if (parts.Length < 2) throw new ArgumentException("Missing path");
                            DeleteItem(parts[1]);
                            break;
                        case "rename":
                            if (parts.Length < 3) throw new ArgumentException("Missing old and new names");
                            RenameItem(parts[1], parts[2]);
                            break;
                        case "copy":
                            if (parts.Length < 3) throw new ArgumentException("Missing source and destination");
                            CopyItem(parts[1], parts[2]);
                            break;
                        case "move":
                            if (parts.Length < 3) throw new ArgumentException("Missing source and destination");
                            MoveItem(parts[1], parts[2]);
                            break;
                        case "edit":
                            if (parts.Length < 2) throw new ArgumentException("Missing file name");
                            EditFile(parts[1]);
                            break;
                        case "cd":
                            if (parts.Length < 2) throw new ArgumentException("Missing directory");
                            currentDirectory = ChangeDirectory(currentDirectory, parts[1]);
                            break;
                        case "exit":
                            return;
                        default:
                            Console.WriteLine("Unknown command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void ListDirectory(string path)
        {
            path = Path.GetFullPath(path);
            
            Console.WriteLine($"Contents of {path}:");
            Console.WriteLine("Directories:");
            foreach (var dir in Directory.GetDirectories(path))
            {
                Console.WriteLine($"  {Path.GetFileName(dir)}/");
            }
            
            Console.WriteLine("Files:");
            foreach (var file in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(file);
                Console.WriteLine($"  {Path.GetFileName(file)} ({fi.Length} bytes)");
            }
        }

        static void DeleteItem(string path)
        {
            path = Path.GetFullPath(path);
            
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Console.WriteLine($"Directory '{path}' deleted.");
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"File '{path}' deleted.");
            }
            else
            {
                throw new FileNotFoundException($"Path '{path}' not found.");
            }
        }

        static void RenameItem(string oldPath, string newName)
        {
            oldPath = Path.GetFullPath(oldPath);
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);
            
            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
                Console.WriteLine($"Directory renamed to '{newPath}'.");
            }
            else if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
                Console.WriteLine($"File renamed to '{newPath}'.");
            }
            else
            {
                throw new FileNotFoundException($"Path '{oldPath}' not found.");
            }
        }

        static void CopyItem(string source, string destination)
        {
            source = Path.GetFullPath(source);
            destination = Path.GetFullPath(destination);
            
            if (Directory.Exists(source))
            {
                CopyDirectory(source, destination);
                Console.WriteLine($"Directory copied to '{destination}'.");
            }
            else if (File.Exists(source))
            {
                File.Copy(source, destination, true);
                Console.WriteLine($"File copied to '{destination}'.");
            }
            else
            {
                throw new FileNotFoundException($"Source path '{source}' not found.");
            }
        }

        static void MoveItem(string source, string destination)
        {
            source = Path.GetFullPath(source);
            destination = Path.GetFullPath(destination);
            
            if (Directory.Exists(source))
            {
                Directory.Move(source, destination);
                Console.WriteLine($"Directory moved to '{destination}'.");
            }
            else if (File.Exists(source))
            {
                File.Move(source, destination);
                Console.WriteLine($"File moved to '{destination}'.");
            }
            else
            {
                throw new FileNotFoundException($"Source path '{source}' not found.");
            }
        }

        static void EditFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File doesn't exist. Create new file? (Y/N)");
                if (Console.ReadLine().ToUpper() != "Y") return;
            }
            
            string content = File.Exists(filePath) ? File.ReadAllText(filePath) : "";
            
            Console.WriteLine($"Editing {filePath} (Press Ctrl+Z then Enter to save, or Ctrl+C to cancel)");
            Console.WriteLine("Current content:");
            Console.WriteLine(content);
            Console.WriteLine("Enter new content:");
            
            StringBuilder newContent = new StringBuilder();
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                newContent.AppendLine(line);
            }
            
            File.WriteAllText(filePath, newContent.ToString());
            Console.WriteLine("File saved.");
        }

        static string ChangeDirectory(string current, string newDir)
        {
            if (newDir == "..")
            {
                return Path.GetDirectoryName(current) ?? current;
            }
            
            string fullPath = Path.GetFullPath(Path.Combine(current, newDir));
            
            if (!Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException($"Directory '{fullPath}' not found.");
            }
            
            return fullPath;
        }

        static void CopyDirectory(string sourceDir, string destDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            }
            
            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDir);
            
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                file.CopyTo(targetFilePath);
            }
            
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestDir = Path.Combine(destDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestDir);
            }
        }
    }
}