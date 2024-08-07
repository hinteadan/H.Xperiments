namespace H.Xperiments.AspNetCore.StaticWebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseFileServer(enableDirectoryBrowsing: true);

            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
