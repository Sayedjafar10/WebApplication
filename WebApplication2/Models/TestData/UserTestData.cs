using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebApplication2.Models;

public class UserTestData
{
    public static List<User> GetTestData()
    {
        var passwordHasher = new PasswordHasher<User>();

        var testData = new List<User>
        {
            new User
            {
                Id = "1",
                UserName = "testuser1@example.com",
                Email = "testuser1@example.com",
                NormalizedUserName = "TESTUSER1",
                NormalizedEmail = "TESTUSER1@EXAMPLE.COM",
                PasswordHash = passwordHasher.HashPassword(null, "TestPassword1"),
                Name = "Test User 1",
                ProfileType = ProfileType.Public,
                ProfilePictureUrl = "url1",
                // Andra egenskaper och relationer
            },
            new User
            {
                Id = "2",
                UserName = "testuser2",
                Email = "testuser2@example.com",
                NormalizedUserName = "TESTUSER2",
                NormalizedEmail = "TESTUSER2@EXAMPLE.COM",
                PasswordHash = passwordHasher.HashPassword(null, "TestPassword2"),
                Name = "Test User 2",
                ProfileType = ProfileType.Private,
                ProfilePictureUrl = "url2",
                // Andra egenskaper och relationer
            },
    };

        return testData;
    }
}
