namespace AdventOfCode._2024.Day12;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        var (garden, size) = ctx.GetInputIterator("Small.txt")
            .GridLike(data => new GardenItem(new Point(data.x, data.y), data.val));

        var lines = garden.Chunk(size.Width);
        
        List<ContiguousArea> areas = [];
        foreach (var gardenItems in lines)
        {
            Dictionary<char, ContiguousArea> areasInLine = new();

            foreach (var gardenItem in gardenItems)
            {
                if (areasInLine.ContainsKey(gardenItem.Plant))
                {
                    areasInLine[gardenItem.Plant].Add(gardenItem);
                }
                else
                {
                    areasInLine.Add(gardenItem.Plant, new ContiguousArea(gardenItem.Plant));
                }
            }
        }
        
        
            
        
        
        var pointLookup = garden.ToDictionary(k => k.Location);
        var fencesNeeded = garden.Select(item => item.RequiredFencing(size, pointLookup)).Sum();
        return $"{fencesNeeded}";
    }
    
}

public readonly record struct ContiguousArea(char Plant)
{
    private readonly List<GardenItem> items = [];
    public void Add(GardenItem gardenItem) => items.Add(gardenItem);
}

public readonly record struct GardenItem(Point Location, char Plant)
{
    public int RequiredFencing(Size size, Dictionary<Point, GardenItem> lookup)
    {
        var item = this;
        return Location.YieldNeighbours()
            .Select(p => size.IsOutside(p) ? new GardenItem(p, '-') : lookup[p])
            .Count(gardenItem => gardenItem.Plant != item.Plant);
    }
}