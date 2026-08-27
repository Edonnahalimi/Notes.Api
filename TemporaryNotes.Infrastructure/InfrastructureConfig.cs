using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Domain.Entities;
using TemporaryNotes.Infrastructure.Repositories;

namespace TemporaryNotes.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<INoteRepository, NoteRepository>();

        services.AddScoped<
            IPasswordHasher<Notes>,
            PasswordHasher<Notes>>();

        return services;
    }
}