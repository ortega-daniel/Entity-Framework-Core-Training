using CollegeApplication;
using Model;

using (var context = new AppDbContext()) 
{
    try
    {
        await AppDbContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return;
    }
}

Menu menu = new();
bool showMenu = true;

while (showMenu)
{
    showMenu = menu.Show();
}
