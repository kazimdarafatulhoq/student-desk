using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Security;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Application.Services;

public class AuthService
{
    private readonly IUnitOfWork _uow;

    public AuthService(IUnitOfWork uow) => _uow = uow;

    public async Task<AuthSession> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new UnauthorizedAccessException("Username and password are required.");

        var user = await _uow.Users.GetByUsernameAsync(request.Username.Trim(), ct);
        if (user is null || !user.IsActive || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        user.LastLoginAt = DateTime.Now;
        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return new AuthSession
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            AcademicSession = "2025-2026",
            LoginAt = DateTime.Now
        };
    }

    public async Task<UserAccountDto> CreateUserAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Username and password are required.");

        var existing = await _uow.Users.GetByUsernameAsync(request.Username.Trim(), ct);
        if (existing is not null)
            throw new InvalidOperationException("Username already exists.");

        var user = new UserAccount
        {
            Username = request.Username.Trim(),
            FullName = request.FullName.Trim(),
            PasswordHash = PasswordHasher.Hash(request.Password),
            Email = request.Email,
            Phone = request.Phone,
            Role = request.Role,
            IsActive = true,
            CreatedBy = request.CreatedBy
        };

        user = await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
        return Map(user);
    }

    public async Task<IReadOnlyList<UserAccountDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _uow.Users.GetAllAsync(ct);
        return users.Select(Map).ToList();
    }

    public async Task SetActiveAsync(int userId, bool isActive, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(userId, ct)
                   ?? throw new InvalidOperationException("User not found.");
        user.IsActive = isActive;
        user.UpdatedAt = DateTime.UtcNow;
        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public static bool CanAccess(UserRole role, string module)
    {
        return module switch
        {
            "Dashboard" => true,
            "Admission" => role is UserRole.SuperAdmin or UserRole.AdmissionOfficer,
            "Search" => true,
            "FeeCollection" => role is UserRole.SuperAdmin or UserRole.AccountsManager,
            "Ledger" => role is UserRole.SuperAdmin or UserRole.AccountsManager or UserRole.AdmissionOfficer,
            "ExamClearance" => role is UserRole.SuperAdmin or UserRole.AccountsManager or UserRole.Teacher,
            "AdmitCard" => role is UserRole.SuperAdmin or UserRole.AccountsManager or UserRole.Teacher,
            "Users" => role is UserRole.SuperAdmin,
            _ => role == UserRole.SuperAdmin
        };
    }

    private static UserAccountDto Map(UserAccount u) => new()
    {
        UserId = u.UserId,
        Username = u.Username,
        FullName = u.FullName,
        Email = u.Email,
        Phone = u.Phone,
        Role = u.Role,
        IsActive = u.IsActive,
        LastLoginAt = u.LastLoginAt
    };
}
