using Microsoft.EntityFrameworkCore;
using StudentManagementApplication.Context;
using System.Runtime.CompilerServices;

namespace StudentManagementApplication.Services;

public static class DatabaseUpdate
{
    public static async Task UpdateMigration(this WebApplication app)
    {
        //var scope = services.CreateScope();
        var scope = app.Services.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<StudentManagementDbContext>();

        await dbContext.Database.MigrateAsync();
        
    }
}
