using System.Collections;

namespace AdventOfCode.Day7;

public class Challenge : IAdventDay
{
    public static string Day => "Day7";

    public static string Run(Context ctx)
    {
        var root = new Directory();
        var currentDirectory = root;
        foreach (var line in ctx.GetInputIterator())
            switch (line.Split(" "))
            {
                case ["$", "cd", "/"]:
                    currentDirectory = root;
                    break;
                case ["$", "cd", ".."]:
                    currentDirectory = currentDirectory.Parent ??
                                       throw new InvalidOperationException("Trying to go up on root");
                    break;
                case ["$", "cd", var child]:
                    currentDirectory = currentDirectory.Find(child);
                    break;
                case ["$", "ls"]:
                    // The "output" of ls we'll deal with on the fly
                    break;
                case ["dir", { } name]:
                    currentDirectory.Directories.Add(new Directory(currentDirectory, name));
                    break;
                case [{ } size, { } name] when int.TryParse(size, out var numSize):
                    currentDirectory.Files.Add(new File(name, numSize));
                    break;
            }

        var sumOfAllSmaller100K = root.EnumerateRecursively().Select(d => d.Size).Where(s => s <= 100_000).Sum();
        const int sizeOfDisk = 70_000_000;
        const int freeSpaceRequired = 30_000_000;
        var toDeleteAtLeast = root.Size - (70_000_000 - 30_000_000);

        var sizeOfDirToDelete = root.EnumerateRecursively().Select(d => d.Size).Where(size => size >= toDeleteAtLeast).Min();
        
        return $"{sumOfAllSmaller100K}, {sizeOfDirToDelete} (totalSizeBeforeDelete: {root.Size})";
    }

    private record Directory(Directory? Parent, string Name, ICollection<Directory> Directories,
        ICollection<File> Files) : IEnumerable<Directory>
    {
        private long? _size;

        public Directory() : this(null, "/", new List<Directory>(), new List<File>())
        {
        }

        public Directory(Directory parent, string name) : this(parent, name, new List<Directory>(), new List<File>())
        {
        }

        public long Size
        {
            get
            {
                if (_size.HasValue) return _size.Value;
                _size = Files.Select(f => f.Size).Sum() + Directories.Select(d => d.Size).Sum();
                return _size.Value;
            }
        }

        public IEnumerator<Directory> GetEnumerator() => Directories.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Directory Find(string childName) => Directories.Single(d => d.Name == childName);

        /// <summary>
        ///     Will not yield itself!
        /// </summary>
        public IEnumerable<Directory> EnumerateRecursively()
        {
            foreach (var directory in Directories)
            {
                yield return directory;
                if (directory.Directories.Count <= 0) continue;
                foreach (var child in directory.EnumerateRecursively())
                    yield return child;
            }
        }
    };

    private record File(string Name, int Size);
}